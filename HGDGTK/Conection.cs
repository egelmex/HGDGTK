using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HGDGTK
{
	// Event declerations
	public delegate void SendChangedEvent (object sender, EventArgs e);
	
	public class SendChangedArgs : EventArgs {
		string filename_;
		int percentage_;
		public string filename {get{return filename_;}}
		public int percentage {get{return percentage_;}}
		
		public SendChangedArgs(string filename, int percentage) {
			this.percentage_ = percentage;
			this.filename_ = filename;
		}
		
	}

	public class Connection
	{

		//Log logwin = Log.getLog ();

		public TcpClient client { get; set; }

		private Stream stream { get; set; }

		public NetworkStream networkStream { get; set; }

		public StreamReader r { get; set; }

		public StreamWriter w { get; set; }

		public string hostname { get; set; }

		public SslStream sslStream { get; set; }

		private string username { get; set; }

		private string password { get; set; }
		
		private int port {get; set; }

		public Connection (string hostname, string username, string password)
		{
			this.hostname = hostname;
			this.username = username;
			this.password = password;
		}

		public void encrypt ()
		{
			//stream.
			//sslStream = new SslStream(stream);
		}

		public void connect ()
		{
			lock (this) {
				//logwin.logInfo ("trying to connect to: " + hostname);
				client = new TcpClient (hostname, port);
				
				networkStream = client.GetStream ();
				stream = client.GetStream ();
				
				r = new StreamReader (stream);
				w = new StreamWriter (stream);
				
				w.NewLine = "\r\n";
				
				string x = r.ReadLine ();
				//logwin.logInfo (x);
			}
		}



		public void disconnect ()
		{
			lock (this) {
				
				w.WriteLine ("bye");
				w.Flush ();
				String s = r.ReadLine ();
				//logwin.logInfo (s);
				
				try {
					w.Close ();
				} finally {
					
					r.Close ();
					stream.Close ();
					client.Close ();
				}
			}
		}

		public void sendFile (string[] filenames)
		{
			
			
			lock (this) {
				
				foreach (string filename in filenames) {
					//logwin.logInfo ("trying to send \"" + filename + "\"");
					
					if (File.Exists (filename)) {
						
						FileStream fs = File.Open (filename, FileMode.Open);
						
						
						
						String cmd = "q|" + Path.GetFileName (filename) + "|" + fs.Length;
						//logwin.logInfo (cmd);
						w.WriteLine (cmd);
						w.Flush ();
						
						string x = r.ReadLine ();
						//logwin.logInfo (x);
						
						if (x.Contains ("ok")) {
							
							long blocksize = 512;
							
							for (long i = 0; i < fs.Length; i += blocksize) {
								long size = (fs.Length - i > blocksize) ? blocksize : (fs.Length - i);
								
								byte[] buffer = new byte[size];
								fs.Read (buffer, 0, (int)size);
								stream.Write (buffer, 0, (int)size);
								OnChanged(new SendChangedArgs(filename, (int) ((double)i / (double)fs.Length)));
								//win.updateProgress ((double)i / (double)fs.Length);
							}
							stream.Flush ();
							
							x = r.ReadLine ();
							//logwin.logInfo (x);
							
						} else {
							//logwin.logInfo ("Failed to send file !");
						}
					} else {
						
					}
				}
			}
		}

		public void login ()
		{
			lock (this) {
				string cmd = "user|" + username + "|" + password;
				//logwin.logInfo (cmd);
				w.WriteLine (cmd);
				w.Flush ();
				string x = r.ReadLine ();
				//logwin.logInfo (x);
			}
		}

		public Song getNowPlaying ()
		{
			
			try {
				lock (this) {
					string cmd = "np";
					//logwin.logInfo (cmd);
					w.WriteLine (cmd);
					w.Flush ();
					string np = r.ReadLine ();
					//logwin.logInfo (np);
					if (np.StartsWith ("ok|0")) {
						return null;
					} else {
						return Song.parseSong1 (np);
					}
				}
			} catch (Exception ex) {
				throw ex;
			}
		}

		public Song[] getPlaylist ()
		{
			lock (this) {
				string cmd = "ls";
				//logwin.logInfo (cmd);
				w.WriteLine (cmd);
				w.Flush ();
				string ls = r.ReadLine ();
				//logwin.logInfo (ls);
				
				string[] ls_p = ls.Split ('|');
				
				
				if (ls_p[0].Equals ("ok")) {
					Song[] songs = new Song[int.Parse (ls_p[1])];
					for (int i = 0; i < int.Parse (ls_p[1]); ++i) {
						ls = r.ReadLine ();
						//logwin.logInfo (ls);
						songs[i] = Song.parseSong2 (ls);
					}
					
					return songs;
				} else {
					//logwin.logInfo ("error getting playlist");
					return new Song[0];
				}
			}
			
		}

		public bool crapsong (string songId)
		{
			lock (this) {
				string cmd = "vo|" + songId;
				//logwin.logInfo (cmd);
				w.WriteLine (cmd);
				w.Flush ();
				string ls = r.ReadLine ();
				//logwin.logInfo (ls);
				string[] ls_p = ls.Split ('|');
				
				
				if (ls_p[0].Equals ("ok")) {
					//logwin.logInfo ("Voted off song");
					return true;
				} else {
					//logwin.logInfo ("error crapping on song");
					return false;
				}
			}
		}

		public event SendChangedEvent Changed;

		protected virtual void OnChanged (EventArgs e)
		{
			if (Changed != null)
				Changed (this, e);
		}
	}
}

