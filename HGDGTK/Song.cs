using System;
using Gtk;
namespace HGDGTK
{

	[TreeNode(ListOnly = true)]
	public class Song : Gtk.TreeNode
	{

		[Gtk.TreeNodeValue(Column = 1)]
		public string user { get; set; }

		[Gtk.TreeNodeValue(Column = 2)]
		public string name { get; set; }

		[Gtk.TreeNodeValue(Column = 0)]
		public string id { get; set; }

		public Song (string user, string name, string id)
		{
			this.user = user;
			this.name = name;
			this.id = id;
			Console.WriteLine (this);
			
		}

		public override string ToString ()
		{
			return "{" + id + "," + user + "," + name + "}";
			
		}

		public static Song parseSong1 (String songstring)
		{
			string[] np_parts = songstring.Split ('|');
			if (np_parts[1] == "1") {
				
				return new Song (np_parts[4], np_parts[3], np_parts[2]);
			} else {
				return null;
			}
		}


		//Song with no ok.
		public static Song parseSong2 (String songstring)
		{
			string[] np_parts = songstring.Split ('|');
			
			return new Song (np_parts[2], np_parts[1], np_parts[0]);
			
		}
		
	}
}


