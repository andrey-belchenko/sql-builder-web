//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
//namespace sql.builder.UI.WinForms
//{
//    class VMemoEdit : MemoExEdit, IVMemoEdit
//    {
//
//        public VMemoEdit()
//        {
//           
//            BeginInit();
//            setControlsProperties();
//        
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
//            //base.InitializeComponent();
//
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
//            // 
//            // pEditors
//            // 
//
//
//
//            //SetProperiesForTextEx();
//
//
//            
//            // 
//            this.CausesValidation = false;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.Location = new System.Drawing.Point(3, 3);
//            this.Name = "meControl";
//            this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
//            this.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None);
//            this.Properties.ShowIcon = false;
//            this.Size = new System.Drawing.Size(194, 20);
//            this.TabIndex = 1;
//            this.EditValueChanged += VTextEdit_EditValueChanged;
//            this.CustomDisplayText += VMemoEdit_CustomDisplayText;
//            this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//        }
//
//        void VMemoEdit_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
//        {
//            var text = (Cmn.Nvl(e.Value, null)) != null
//                   ? e.Value.ToString().Replace(Environment.NewLine, " ")
//                   : "";
//
//            e.DisplayText = text;
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
//
//        //public event SimpleEventHandler Entered;
//    }
//}
