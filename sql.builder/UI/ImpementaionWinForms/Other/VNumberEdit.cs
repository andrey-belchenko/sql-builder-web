//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
//namespace sql.builder.UI.WinForms
//{
//    class VNumberEdit : SpinEdit, IVNumberEdit
//    {
//
//        public VNumberEdit()
//        {
//           
//            BeginInit();
//            setControlsProperties();
//        
//        }
//
//        public void SetMask(string value)
//        {
//            if (value.StartsWith("n", true, System.Globalization.CultureInfo.CurrentCulture))
//            {
//                this.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//            }
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
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
//
//
//            this.Properties.Mask.UseMaskAsDisplayFormat = true;
//            this.CausesValidation = false;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.EditValue = new decimal(new int[] {
//            0,
//            0,
//            0,
//            0});
//            this.Location = new System.Drawing.Point(0, 0);
//            this.Name = "seControl";
//            this.Properties.Appearance.Options.UseTextOptions = true;
//            this.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
//            this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
//            this.Properties.Mask.UseMaskAsDisplayFormat = true;
//            this.Size = new System.Drawing.Size(200, 20);
//            this.TabIndex = 0;
//            this.KeyDown += new System.Windows.Forms.KeyEventHandler(VNumberEdit_KeyDown);
//            this.LostFocus += VNumberEdit_LostFocus;
//            this.EditValue = null;
//            this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            //this.CausesValidation = false;
//            //this.Dock = System.Windows.Forms.DockStyle.Fill;
//            //this.Location = new System.Drawing.Point(0, 0);
//            //this.Name = "beControl";
//            //this.Size = new System.Drawing.Size(200, 20);
//            //this.TabIndex = 2;
//            //this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            //this.ErrorIcon = Cmn.ImageWarning14;
//           
//            //this.Enter += VTextEdit_Enter;
//            this.EditValueChanged += VTextEdit_EditValueChanged;
//        }
//        public event SimpleEventHandler FocusLost;
//        void VNumberEdit_LostFocus(object sender, EventArgs e)
//        {
//            if (FocusLost != null)
//            {
//                FocusLost();
//            }
//        }
//
//        void VNumberEdit_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Delete)
//            {
//                if (Cleared != null)
//                {
//                    Cleared();
//                }
//            }
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
//        public void SetStep(decimal value)
//        {
//            if (value > 0) this.Properties.Increment = value;
//            else this.Properties.Buttons[0].Visible = false;
//            this.Properties.Increment = value;
//           
//        }
//        public event ValueChangeEventHandler ValueChanged;
//        public event SimpleEventHandler Cleared;
//     
//
//        //public event SimpleEventHandler Entered;
//    }
//}
