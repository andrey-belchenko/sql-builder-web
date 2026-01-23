//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
//using System.IO;
//namespace sql.builder.UI.WinForms
//{
//    class VFileEdit : ButtonEdit, IVFileEdit
//    {
//
//        public VFileEdit()
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
//        private System.Windows.Forms.OpenFileDialog openfile;
//        private void setControlsProperties()
//        {
//           
//            //base.InitializeComponent();
//            this.openfile = new System.Windows.Forms.OpenFileDialog();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
//            
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.Location = new System.Drawing.Point(0, 0);
//            this.Name = "beControl";
//            this.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Открыть файл", null, null, true),
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "Выбрать", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "Выбрать файл", null, null, true)});
//            this.Properties.ReadOnly = true;
//            this.Properties.UseReadOnlyAppearance = false;
//            this.Size = new System.Drawing.Size(201, 20);
//            this.TabIndex = 0;
//            this.ButtonPressed += VFileEdit_ButtonPressed;
//            //this.EditValueChanged += VFileEdit_EditValueChanged;
//            this.KeyDown += new System.Windows.Forms.KeyEventHandler(VFileEdit_KeyDown);
//
//            this.openfile.RestoreDirectory = true;
//           
//
//
//            //this.CausesValidation = false;
//            //this.Dock = System.Windows.Forms.DockStyle.Fill;
//            //this.Location = new System.Drawing.Point(0, 0);
//            //this.Name = "beControl";
//            //this.Size = new System.Drawing.Size(200, 20);
//            //this.TabIndex = 2;
//            //this.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            //this.ErrorIcon = Cmn.ImageWarning14;
//           
//            //this.EditValueChanged += VTextEdit_EditValueChanged;
//        }
//
//        //void VFileEdit_EditValueChanged(object sender, EventArgs e)
//        //{
//        //    if (ValueChanged != null)
//        //    {
//        //        ValueChanged(this.EditValue);
//        //    }
//        //}
//
//        public /*временно*/ void VFileEdit_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
//        {
//            if (e.Button.Caption == "Выбрать")
//            {
//                var result = openfile.ShowDialog();
//                if (result != DialogResult.OK) return;
//
//                this.EditValue = openfile.SafeFileName;
//                if (FileSelected != null)
//                {
//                    FileSelected(openfile.FileName);
//                }
//                //SaveFile(openfile.FileName);
//
//                openfile.InitialDirectory = Path.GetDirectoryName(openfile.FileName);
//                openfile.FileName = openfile.SafeFileName;
//            }
//            else //if (e.Button.Caption == "Открыть")
//            {
//                if (FileOpenPressed != null)
//                {
//                    FileOpenPressed();
//                }
//                //LoadFile();
//            }
//        }
//
//
//
//        private void VFileEdit_KeyDown(object sender, KeyEventArgs e)
//        {
//            // режим readonly
//            //var read_only = GetSourceReadOnly();
//            //if (read_only) return;
//
//            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back))
//            {
//                if (Cleared != null)
//                {
//                    Cleared();
//                }
//            }
//        }
//
//
//        
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
//        //public event ValueChangeEventHandler ValueChanged;
//
//
//        //public event SimpleEventHandler Entered;
//
//
//        public event ValueChangeEventHandler FileSelected;
//
//        public event SimpleEventHandler FileOpenPressed;
//
//
//        public event SimpleEventHandler Cleared;
//    }
//}
