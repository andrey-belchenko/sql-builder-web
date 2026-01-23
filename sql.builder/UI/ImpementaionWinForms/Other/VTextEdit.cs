//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
////using DevExpress.Utils.Menu;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
////using DevExpress.XtraEditors.Controls;
//
//namespace sql.builder.UI.WinForms
//{
//    class VTextEdit : ButtonEdit, IVTextEdit
//    {
//
//        public VTextEdit()
//        {
//           
//            BeginInit();
//            setControlsProperties();
//        
//        }
//        public void SetMask(string value)
//        {
//            if (value.StartsWith("n", true, System.Globalization.CultureInfo.CurrentCulture))
//            {
//                this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//            }
//            else if (value.StartsWith("s")) // заплатка
//            {
//                value = value.Substring(1, value.Length - 1);
//                this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
//                this.Properties.Mask.SaveLiteral = true;
//
//            }
//
//            this.Properties.Mask.EditMask = value;
//        }
//        public void SetMaxLength(int value)
//        {
//            this.Properties.MaxLength = value;
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
//           
//            this.CausesValidation = false;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.Location = new System.Drawing.Point(0, 0);
//            this.Name = "beControl";
//            this.Size = new System.Drawing.Size(200, 20);
//            this.TabIndex = 2;
//            this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            this.ErrorIcon = Cmn.ImageWarning14;
//            this.Properties.Mask.UseMaskAsDisplayFormat = true;
//            //this.Enter += VTextEdit_Enter;
//            this.EditValueChanged += VTextEdit_EditValueChanged;
//            
//        }
//
//        public void SetUsePlaneString() // всегда нельза т.к. криво работает вставка если уже есть текст
//        {
//            this.KeyDown += edit_KeyDown;
//            this.Properties.BeforeShowMenu += Properties_BeforeShowMenu;
//        }
//
//        void VTextEdit_EditValueChanged(object sender, EventArgs e)
//        {
//            if (ValueChanged != null)
//            {
//                ValueChanged(this.EditValue);
//            }
//        }
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
//        #region ButtonEditClickHelper Q499983
//        //public class ButtonEditClickHelper
//        //{
//
//            
//            // NEW
//           
//            void Properties_BeforeShowMenu(object sender, BeforeShowMenuEventArgs e)
//            {
//                
//                for (int i = 0; i < e.Menu.Items.Count; i++)
//                    if (e.Menu.Items[i].Caption == Localizer.GetLocalizedString("&Paste") || e.Menu.Items[i].Caption == Localizer.GetLocalizedString("Вст&авить"))
//                    {
//                        Image img = e.Menu.Items[i].Image;
//                        DXMenuItem item = null;
//                        if (e.Menu.Items.Count > i && e.Menu.Items[i + 1].Caption == "Вставить")
//                        {
//                            item = e.Menu.Items[i + 1];
//
//                        }
//                        if (Cmn.Nvle(this.EditValue, null) == null)  // т.к. если не пусто-заменяет все, поэтому отключаем наворот в этом случае
//                        {
//                            e.Menu.Items[i].Visible = false;
//                            if (item != null)
//                            {
//                                item.Visible = true;
//                            }
//                            else
//                            {
//                                e.Menu.Items.Insert(i + 1, new DXMenuItem(Localizer.GetLocalizedString("Вставить"), item_Click, img));
//                            }
//                        }
//                        else
//                        {
//                            e.Menu.Items[i].Visible = true;
//                            if (item != null)
//                            {
//                                item.Visible = false;
//                            }
//                        }
//                       
//                      //  e.Menu.Items.RemoveAt(i);
//                        
//                        
//                    }
//            }
//            // NEW
//            void item_Click(object sender, EventArgs e)
//            {
//                this.EditValue = PlaneString(Clipboard.GetText());
//            }
//
//            // NEW
//            string PlaneString(string s)
//            {
//                string result = String.Empty;
//                result = s.Replace("\r\n", " ");
//                return result;
//            }
//            // NEW
//            void edit_KeyDown(object sender, KeyEventArgs e)
//            {
//                if ((e.KeyCode == Keys.V && e.Modifiers == Keys.Control) || (e.KeyCode == Keys.Insert && e.Modifiers == Keys.Shift))
//                {
//                    if (Cmn.Nvle(this.EditValue, null) != null) return;
//                    ButtonEdit editor = sender as ButtonEdit;
//                    editor.EditValue = PlaneString(Clipboard.GetText());
//                    e.SuppressKeyPress = true;
//                }
//            }
//
//            
//            //private readonly ButtonEdit _Edit;
//
//
//            //public void DetachEvents()
//            //{
//            //      _Edit.KeyDown -=  edit_KeyDown;
//            //    _Edit.Properties.BeforeShowMenu -= Properties_BeforeShowMenu;
//            //}
//
//        //}
//        #endregion
//        //public event SimpleEventHandler Entered;
//    }
//}
