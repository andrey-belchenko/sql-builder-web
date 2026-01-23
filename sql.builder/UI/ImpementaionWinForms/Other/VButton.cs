//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
////using DevExpress.XtraEditors;
//namespace sql.builder.UI.WinForms
//{
//    class VButton : SimpleButton,IVButton
//    {
//
//        public VButton()
//        {
//
//            
//            this.Name = "a" + this.GetHashCode().ToString();
//           
//            
//
//            this.AutoSize = true;
//            
//           
//         
//
//            
//
//            this.Click += VButton_Click;
//
//            this.MouseDown += VButton_MouseDown;
//        }
//
//        void VButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (this.Menu != null)
//            {
//                (this.Menu as PopupMenu).ShowPopup(new Point( System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y));
//            }
//        }
//
//        void VButton_Click(object sender, EventArgs e)
//        {
//
//            if (this.Menu != null)
//            {
//                return;
//            }
//            if (ButtonClick != null)
//            {
//                ButtonClick(this.Tag);
//            }
//        }
//
//
//        public object Menu
//        {
//            get;
//            set;
//        }
//        public void SetCaption(string value)
//        {
//            this.Text = value;
//            this.MaximumSize = this.CalcBestSize();
//        }
//
//        public void SetImage(Image value)
//        {
//            this.Image = value;
//            this.MaximumSize = this.CalcBestSize();
//            //this.Glyph = value;
//        }
//        public void SetVisible(bool value)
//        {
//
//            //this.Visibility = (value) ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//
//        public void SetEnabled(bool value)
//        {
//            this.Enabled= (value);
//        }
//
//       
//
//
//        public void SetToolTip(string value)
//        {
//            this.ToolTip = value;
//        }
//
//
//        public event ValueChangeEventHandler ButtonClick;
//
//
//        public int GetTextWith()
//        {
//           return this.CalcBestSize().Width;
//        }
//    }
//}
