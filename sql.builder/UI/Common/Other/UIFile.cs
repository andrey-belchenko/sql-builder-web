//using System;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using System.Linq;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors.Repository;
//using System.IO;
//using System.Diagnostics;
//namespace sql.builder.UI
//{
//    internal partial class UIFile : UIBase
//    {
//        public UIFile()
//        {
//           // InitializeComponent();
//            //openfile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//        }

//        public override void Initialize(XElement xfield, UIFormC form)
//        {
//            BaseInitialize(xfield, form, ReturnType.Simple, typeof(string), true);
//            InitControl();
//        }
//        protected override void InitControl()
//        {
//        }
//        public override void RefreshData()
//        {
//            if (this.UseDefaultQuery && this.data_set_default != null) {
//                XElement master_values = this.OnNeedMasterValues(this);
//                this.data_set_default.Refresh(master_values);
//                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
//                if (rows.Count > 0) {
//                    setValue(rows[0][0].ToString());
//                    //beControl.EditValue = DataTableDefault.Rows[0][0].ToString();
//                }
//            }
//            base.RefreshData();
//        }
//        private object _value = null;
//        private void setValue(object value)
//        {
//            _value = value;
//            beControl.SetValue(_value);
//        }
//        public override string GetText()
//        {
//            return _value.ToString();
//        }

//        public override RepositoryItem GetRepositoryItem()
//        {
//            var rep = new RepositoryItemButtonEdit()
//            {
//                ReadOnly = true,
//                UseReadOnlyAppearance = false
//            };
//            addRepositoryButtons(rep.Buttons);
//            rep.Buttons[0].Caption = "Выбрать";
//            rep.Buttons[0].ToolTip = "Выбрать файл";
//            rep.Buttons.Add(new EditorButton()
//            {
//                //Caption = "Открыть",
//                ToolTip = "Открыть файл",
//                Kind = ButtonPredefines.Search,

//                //Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/zoom/zoom_16x16.png")
//            });
//            rep.ButtonPressed += rep_ButtonPressed;
//            rep.KeyDown += beControl_KeyDown;
//            //rep.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
           
//            //_repository = rep;

//            return rep;
//        }

//        void rep_ButtonPressed(object sender, ButtonPressedEventArgs e)
//        {
//            (beControl as sql.builder.UI.WinForms.VFileEdit).VFileEdit_ButtonPressed(sender, e);
//        }

//        private void SaveFile(string filename)
//        {
//			//to mark row as modified
//			SetSourceValue(-1);
//			SetSourceValue(DBNull.Value);

//            var col = GetBoundColumn();

//            var getter = new DataApi.VFileGetter(filename);
//            col.AddFileGetter(getter);
//            col.SetFieldValueName(getter.Name());
//			beControl.SetValue(getter.Name());
           
//            //var ms = Form.DataSource.ParamsTable.files.ContainsKey(FieldName) 
//            //    ? Form.DataSource.ParamsTable.files[FieldName] 
//            //    : new MemoryStream();

//            //using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
//            //{
//            //    ms.SetLength(0);
//            //    fs.CopyTo(ms);
//            //}

//            //Form.DataSource.ParamsTable.files[FieldName] = ms;
//        }
//        private void LoadFile()
//        {
//            //if (string.IsNullOrEmpty(filename)) return;
//            //if (!Form.DataSource.ParamsTable.files.ContainsKey(FieldName)) return;
//            //Form.DataSource.ParamsTable.files[FieldName];
//            var col = GetBoundColumn();
//            var getter = col.GetFileGetter();
//            string filepath = null;
//            if (getter != null)
//            {
//                filepath = getter.FileName;
//            }
//            else
//            {
//               // if(col.FileGetters == null) return;
//                var ms = col.LoadFile();
//                if (ms != null)
//                {
//                    filepath = Path.Combine(Path.GetTempPath(), col.GetFieldValueName());

//                    using (var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
//                    {
//                        ms.Seek(0, SeekOrigin.Begin);
//                        ms.CopyTo(fs);
//                    }
//                }
//            }
//            if (filepath != null)
//            {
//                Process.Start(filepath);
//            }
//        }
//        private void ClearFile()
//        {
//            SetSourceValue(DBNull.Value);
//            var col = GetBoundColumn();
//            col.SetFieldValueName(null);
//            col.RemoveFileGetter();
//            //beControl.EditValue = DBNull.Value;
//           // Form.ShowMessage("Должна быть очистка, но нет");
//        }

//        //private void beControl_ButtonPressed(object sender, ButtonPressedEventArgs e)
//        //{
//        //    if (e.Button.Caption == "Выбрать")
//        //    {
//        //        var result = openfile.ShowDialog();
//        //        if (result != DialogResult.OK) return;

//        //        beControl.EditValue = openfile.SafeFileName;
//        //        SaveFile(openfile.FileName);

//        //        openfile.InitialDirectory = Path.GetDirectoryName(openfile.FileName);
//        //        openfile.FileName = openfile.SafeFileName;
//        //    }
//        //    else //if (e.Button.Caption == "Открыть")
//        //    {
//        //        LoadFile();
//        //    }
//        //}


//        //public override void SetDisplayValue(object value, int index = 1)
//        //{
//        //    beControl.EditValue = value;
//        //}

//        //private void beControl_EditValueChanged(object sender, EventArgs e)
//        //{
//        //    //if (_repository_edit != null)
//        //    //{
//        //    //    _repository_edit.EditValue = beControl.EditValue;
//        //    //}
//        //}


        

//        void beControl_FileSelected(object value)
//        {
//            SaveFile(value.ToString());
//        }

//        void beControl_FileOpenPressed()
//        {
//            LoadFile();
//        }
//        void beControl_Cleared()
//        {
//            var read_only = GetSourceReadOnly();
//            if (read_only) return;
//            if (!this.mandatory)
//            {
//                ClearFile();
//            }
//        }
//        private void beControl_KeyDown(object sender, KeyEventArgs e)
//        {
//            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back))
//            {
//                beControl_Cleared();
//            }
//        }

//        public override void SetDisplayValue(object text, int index = 1)
//        {
//            setValue((text != null) ? text.ToString() : null);
//        }
//    }
//}
