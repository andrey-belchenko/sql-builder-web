//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
////using DevExpress.XtraEditors.Controls;
//namespace sql.builder.UI.WinForms
//{
//    class VLinkEdit : HyperLinkEdit, IVLinkEdit
//    {
//
//        public VLinkEdit()
//        {
//            BeginInit();
//            setControlsProperties();
//        }
//        public void BeginInit()
//        {
//
//
//
//            VControl.BeginInitObject(this.Properties);
//
//        }
//
//        public void EndInit()
//        {
//
//            VControl.EndInitObject(this.Properties);
//        }
//
//
//        private void setControlsProperties()
//        {
//            this.CausesValidation = false;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.Location = new System.Drawing.Point(0, 0);
//            this.Name = "heControl";
//            this.Properties.StartKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None);
//            this.Properties.StartLinkOnClickingEmptySpace = false;
//            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
//            this.Size = new System.Drawing.Size(200, 20);
//            this.TabIndex = 2;
//            this.OpenLink += heControl_OpenLink;
//            //this.Enter += new System.EventHandler(this.teControl_Enter);
//            this.MouseEnter += heControl_MouseEnter;
//            this.MouseLeave += heControl_MouseLeave;
//            this.MouseMove += heControl_MouseMove;
//            this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            this.EditValueChanged += VTextEdit_EditValueChanged;
//            this.ToolTip = UILink.TooltipText;
//        }
//
//
//        public static void heControl_MouseMove(object sender, MouseEventArgs e)
//        {
//            UpdateCursor(sender as HyperLinkEdit);
//        }
//
//         public static void heControl_MouseEnter(object sender, EventArgs e)
//        {
//            UpdateCursor(sender as HyperLinkEdit);
//        }
//
//         public static void heControl_MouseLeave(object sender, EventArgs e)
//        {
//            UpdateCursor(sender as HyperLinkEdit);
//        }
//
//        public static void UpdateCursor(HyperLinkEdit edit)
//        {
//            if ((Control.ModifierKeys & Keys.Control) != 0 && edit.Text != "") edit.Controls[0].Cursor = Cursors.Hand;
//            else edit.Controls[0].Cursor = Cursors.Default;
//        }
//
//         public static void heControl_OpenLink(object sender, OpenLinkEventArgs e)
//        {
//            if ((Control.ModifierKeys & Keys.Control) == 0)
//            {
//
//                e.Handled = true;
//            }
//        }
//
//       //  public event ValueChangeEventHandler ValueChanged;
//         void VTextEdit_EditValueChanged(object sender, EventArgs e)
//         {
//             if (ValueChanged != null)
//             {
//                 ValueChanged(this.EditValue);
//             }
//         }
//
//        //void VTextEdit_Enter(object sender, EventArgs e)
//        //{
//        //    if (Entered != null)
//        //    {
//        //        Entered();
//        //    }
//        //}
//
//
//        public void SetValue(object value)
//        {
//            this.EditValue = value;
//        }
//
//        public event ValueChangeEventHandler ValueChanged;
//
//
//        //public event SimpleEventHandler Entered;
//    }
//}
