using System;
using Gtk;
namespace HGDGTK
{
	public partial class HGDLogin : Gtk.Dialog
	{
		public bool ok {
			get;
			set;
		}
		
		public string hostname {
			get { return hostname_cmd.ActiveText; }
		}
		
		public string username {
			get { return username_cmd.ActiveText; }
			
		}
		
		public string password {
			get { return password_txt.Text; }
		}
		
		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			ok = false;
			this.Hide();
		}
		
		
		public HGDLogin (string title, Window parent, DialogFlags flags, params object[] button_data) :
		
			base(title, parent, flags, button_data)
		{
			this.Build ();
		}
		
		
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			ok=true;
			this.Hide();
		}
		
		
	}
}

