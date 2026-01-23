//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors.Mask;
////using DevExpress.XtraEditors.Repository;
//
////using DevExpress.XtraTreeList;
//using sql.builder.UI;
//namespace sql.builder.UI.WinForms
//{
//    public partial class VLayoutLabel : XtraUserControl, IVLayoutLabel
//    {
//        public VLayoutLabel()
//        {
//           
//            InitializeComponent();
//        }
//
//        public void SetText(string text)
//        {
//            labelControl1.Text = text;
//			//this.SetHint("sasasasasasasasasasasasaasaasasasas");
//           
//        }
//
//        public void SetTextAlignment(bool isLeft)
//        {
//            if (isLeft)
//            {
//                labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
//            }
//            else
//            {
//                labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
//            }
//
//        }
//
//        public void SetBold(bool value)
//        {
//            if (value)
//            {
//                labelControl1.Font = new Font(labelControl1.Font, FontStyle.Bold);
//            }
//            else
//            {
//                labelControl1.Font = new Font(labelControl1.Font, FontStyle.Regular);
//            }
//         
//           
//        }
//
//        public int GetTextWidth()
//        {
//            var v = 0;
//            if (hintControl != null)
//            {
//
//                v = 18;
//            }
//            return labelControl1.CalcBestSize().Width +v; //+16;
//		}
//
//		private DevExpress.XtraEditors.LabelControl hintControl;
//
//		public void SetHint(string hint)
//		{
//			this.hintControl = new DevExpress.XtraEditors.LabelControl();
//			this.SuspendLayout();
//
//			this.hintControl.Dock = System.Windows.Forms.DockStyle.Right;
//			//this.hintControl.Location = new System.Drawing.Point(319, 0);
//			//this.hintControl.Name = "hintControl";
//			this.hintControl.Size = new System.Drawing.Size(18, 150);
//			this.hintControl.TabStop = false;
//			this.hintControl.TabIndex = 2;
//            this.hintControl.Padding = new Padding(0,1,0,0);
//			this.hintControl.Text = "";
//			this.hintControl.ToolTip = hint;
//			this.hintControl.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
//			this.hintControl.Appearance.Image = global::sql.builder.Properties.Resources.info_16x16;
//			this.hintControl.Click += new System.EventHandler(this.hint_Click);
//			this.Controls.Add(this.hintControl);
//
//			this.ResumeLayout(false);
//		}
//
//		private void hint_Click(object sender, EventArgs e)
//		{
//			sql.builder.WinForms.ShowMessage.ShowAdvancedMessage(((DevExpress.XtraEditors.LabelControl)sender).ToolTip, this.labelControl1.Text);
//		}
//    }
//}
