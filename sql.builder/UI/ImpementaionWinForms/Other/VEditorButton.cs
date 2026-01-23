//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
//namespace sql.builder.UI.WinForms
//{
//    class VEditorButton : EditorButton, IVEditorButton
//    {
//
//        public VEditorButton()
//        {
//            this.Click += VEditorButton_Click;
//            
//           
//        }
//
//        void VEditorButton_Click(object sender, EventArgs e)
//        {
//            if (ButtonClick != null)
//            {
//                ButtonClick(this);
//            }
//        }
//
//        public IVEditorButton Copy()
//        {
//            var btn1 = new VEditorButton();
//
//            btn1.ToolTip = btn1.Caption = this.Caption;
//
//            btn1.IsLeft = this.IsLeft;
//            btn1.Kind = this.Kind;
//            btn1.Image = this.Image;
//
//            return btn1;
//        }
//
//        //void VButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        //{
//        //    if (this.Menu != null)
//        //    {
//        //        (this.Menu as PopupMenu).ShowPopup(new Point( System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y));
//        //    }
//        //}
//
//        
//
//
//        //public object Menu
//        //{
//        //    get;
//        //    set;
//        //}
//        public void SetCaption(string value)
//        {
//            this.Caption = value;
//            //this.Text = value;
//            //this.MaximumSize = this.CalcBestSize();
//        }
//
//        public void SetImage(Image value)
//        {
//            this.Image = value;
//            //this.MaximumSize = this.CalcBestSize();
//            //this.Glyph = value;
//        }
//        //public void SetVisible(bool value)
//        //{
//
//        //    //this.Visibility = (value) ? BarItemVisibility.Always : BarItemVisibility.Never;
//        //}
//
//        public void SetEnabled(bool value)
//        {
//            this.Enabled = (value);
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
//        //public int GetTextWith()
//        //{
//        //    return 0;
//        //   //return this.CalcBestSize().Width;
//        //}
//
//
//        public void SetIsLeft(bool value)
//        {
//            this.IsLeft = value;
//           
//        }
//
//        public void SetKind(string value)
//        {
//            ButtonPredefines kind;
//            if (!Enum.TryParse<ButtonPredefines>(value, true, out kind)) {
//                throw new ArgumentOutOfRangeException("value");
//            }
//            this.Kind = kind;
//        }
//    }
//}
