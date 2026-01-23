//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
////using DevExpress.Utils;
//namespace sql.builder.UI.WinForms
//{
//    class VDateEdit : DateEdit, IVDateEdit
//    {
//        public VDateEdit()
//        {
//           
//            BeginInit();
//            setControlsProperties();
//        
//        }
//        public void BeginInit()
//        {
//            VControl.BeginInitObject(this.Properties);
//
//        }
//
//        public void SetMask(string value)
//        {
//            //if (value.StartsWith("n", true, System.Globalization.CultureInfo.CurrentCulture))
//            //{
//            
//            //}
//            //this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
//            //this.Properties.Mask.EditMask = value;
//        }
//
//        public void EndInit()
//        {
//            VControl.EndInitObject(this.Properties);
//        }
//
//
//        private void setControlsProperties()
//        {
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
//            this.CausesValidation = false;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.EditValue = null;
//            this.Location = new System.Drawing.Point(0, 0);
//            this.Name = "deControl";
//            this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
//            this.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None);
//            this.Properties.DisplayFormat.FormatString = "";
//            this.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
//            this.Properties.EditFormat.FormatString = "";
//            this.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
//            this.Properties.Mask.UseMaskAsDisplayFormat = true;
//            SetShowTime(false);
//           // this.Properties.Mask.EditMask = "([012]?[1-9]|[123]0|31)\\.(0?[1-9]|1[012])\\.([123][0-9])?[0-9][0-9]";
//            this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
//            this.Size = new System.Drawing.Size(200, 20);
//            this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            this.TabIndex = 1;
//            //////
//            //this.CausesValidation = false;
//            //this.Dock = System.Windows.Forms.DockStyle.Fill;
//            //this.Location = new System.Drawing.Point(0, 0);
//            //this.Name = "beControl";
//            //this.ize = new System.Drawing.Size(200, 20);
//            //this.ЕabIndex = 2;
//            //this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            //this.ErrorIcon = Cmn.ImageWarning14;
//            //this.Enter += VTextEdit_Enter;
//            this.EditValueChanged += VTextEdit_EditValueChanged;
//            this.LostFocus += VDateEdit_LostFocus;
//        }
//
//        
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
//         public void SetCanClear(bool value)
//        {
//            if (value)
//            {
//                this.Properties.ShowClear = true;
//                this.Properties.AllowNullInput = DefaultBoolean.True;
//            }
//            else
//            {
//                this.Properties.ShowClear = false;
//                this.Properties.AllowNullInput = DefaultBoolean.False;
//            }
//        
//        }
//         public void SetShowTime(bool value)
//         {
//             if (value)
//             {
//                 this.Properties.Mask.EditMask = "([012]?[1-9]|[123]0|31)\\.(0?[1-9]|1[012])\\.([123][0-9])?[0-9][0-9] (0?[0-9]|1[0-9]|2[0-4]):[0-5][0-9]";
//             }
//             else
//             {
//                 this.Properties.Mask.EditMask = "([012]?[1-9]|[123]0|31)\\.(0?[1-9]|1[012])\\.([123][0-9])?[0-9][0-9]";
//             }
//         }
//         public event SimpleEventHandler FocusLost;
//         void VDateEdit_LostFocus(object sender, EventArgs e)
//         {
//             if (FocusLost != null)
//             {
//                 FocusLost();
//             }
//         }
//
//        public event ValueChangeEventHandler ValueChanged;
//
//
//        public event SimpleEventHandler Entered;
//
//
//
//
//        public void SetMaxLength(int value)
//        {
//            this.Properties.MaxLength = value;
//        }
//    }
//}
