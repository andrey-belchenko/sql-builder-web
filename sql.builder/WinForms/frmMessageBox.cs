//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using infoenergo.ui.win.Forms;

//namespace sql.builder.WinForms
//{
//	internal partial class frmMessageBox : FormBase
//	{
//		public frmMessageBox()
//		{
//			InitializeComponent();
//		}

//		public void SetHeader(string header)
//		{
//			this.SuspendLayout();
//			this.Text = header;
//			this.ResumeLayout(false);
//		}
		
//		public void SetText(string text)
//		{
//			this.mainMemoEdit.EditValue = text;
//			this.okButton.Select();
//			this.CenterToParent();
//			//this.ShowDialog();
//		}

//		private void okButton_Click(object sender, EventArgs e)
//		{
//			this.Close();
//		}
//	}
//}
