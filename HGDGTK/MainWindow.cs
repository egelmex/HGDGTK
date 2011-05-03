using System;
using Gtk;
namespace HGDGTK
{
	public partial class MainWindow : Gtk.Window
	{
		//new TargetEntry()
		private static TargetEntry[] targetEntry = new TargetEntry[] {  };



		public MainWindow () : base(Gtk.WindowType.Toplevel)
		{
			Build ();
			Gtk.Drag.SourceSet (filepane, Gdk.ModifierType.Button1Mask, targetEntry, Gdk.DragAction.Ask);	
			
			HGDLogin win2 = new HGDLogin("test", this, DialogFlags.Modal);
			win2.Run();
			if (win2.ok == true) {
				this.Show();
			} else {
				this.Hide();
				Console.WriteLine("Should close there");
			}
			
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
		protected virtual void OnNodeview1DragEnd (object o, Gtk.DragEndArgs args)
		{
			Console.WriteLine (o);
			Console.WriteLine (args);
			
		}

		protected virtual void OnNodeview1DragDrop (object o, Gtk.DragDropArgs args)
		{
			Console.WriteLine (o);
			Console.WriteLine (args);
		}
		
		protected virtual void OnRealized (object sender, System.EventArgs e)
		{

		}
		
		protected virtual void OnShown (object sender, System.EventArgs e)
		{

		}
		
	
	}
}

