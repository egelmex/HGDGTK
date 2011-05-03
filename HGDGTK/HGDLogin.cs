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

