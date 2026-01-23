//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.XtraBars;
//using sql.builder.DataApi;
//using sql.builder.Test;
//using sql.builder.UI;
//using sql.builder.WinForms;

//namespace sql.builder
//{
//    internal partial class ucFormDesigner : ucBase
//    {
//        private VForm _vform;
//        private VQuery _vquery;
//        private VDataSet _ds;

//        private bool _no_data;
//        private bool _load_default;
//        private bool _loaded;

//        private frmFieldsSettings _frmFieldsSettings;

//        public UIFormC FormC { get; private set; }

//        public ucFormDesigner()
//        {
//            InitializeComponent();
//        }
//        /*
//        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().Test1();
//        }
//        private void Test1()
//        {
//            object[] pars = new object[1];
//            if (db.Connection.DataSource == "asuse") {
//                pars[0] = 10006396;
//            } else {
//                pars[0] = 202;
//            }
//            OpenFormVForm("test-editor-ur_folders", pars);
//        }
//        */
//        /*public void OpenFormVForm(string formName, object[] pars, bool noData = false, bool loadDefault = false)
//        {

//            ////// Бельченко 12.05.2017 убираю тк функционал не используется - убираю.
//            ////// если что для восстановления организовать работу по новому  новый варианту _vform.GetFormXelementAndDataSet() вмесо  _vform.ProcessAndCreateDataSet()

//            //////////////var fds = _vform.GetFormXelementAndDataSet();
//            //////////////var XForm = fds.Item1;
//            //////////////_ds = fds.Item2;
//            ////////////////var xForm = _vform.GetFormXElement();
//            ////////////////_ds = _vform.ProcessAndCreateDataSet();
//            //_no_data = noData;
//            //_load_default = loadDefault;

//            //_vform = XmlReports.Environment.GetForm(formName);
//            //XElement xpars = VDataSet.ParsObjectArrayToXelement(pars, _vform.ParamsElement());

//            //if (_vform.WithData)
//            //{
//            //    _ds = _vform.ProcessAndCreateDataSet();
//            //    _ds.Connection = XmlReports.Environment.Connection;

//            //    VReport.ApplySimpleParams(xpars, _ds, _vform.Element(TextConst.EName.Params));

//            //    //if (!noData) _ds.Refresh(false, xpars);

//            //    if (!noData)
//            //    {
//            //        _ds.SetParams(xpars);
//            //    }
//            //    var vtable = _ds.Tables[0] as VDataTable;
//            //    if (vtable.Rows.Count > 0) vtable.CurrentRow = _ds.Tables[0].Rows[0];
//            //}

//            //if (_loaded)
//            //{
//            //    UpdateFormVForm(noData, loadDefault);

//            //    _frmFieldsSettings = new frmFieldsSettings();
//            //    _frmFieldsSettings.Initialize(_vform);
//            //}
//        }*/
//        public void OpenFormVQuery(string queryName, object[] pars, bool noData = false, bool loadDefault = false)
//        {
//            _no_data = noData;
//            _load_default = loadDefault;

//            _vquery = XmlReports.Environment.GetQuery(queryName);

//            if (_loaded)
//            {
//                UpdateFormVQuery(noData, loadDefault);

//                _frmFieldsSettings = new frmFieldsSettings();
//                _frmFieldsSettings.Initialize(_vquery);
//            }
//        }
//        private static bool IsLayoutAttribute(XAttribute attr)
//        {
//            XName name = attr.Name;
//            return (name == AName.size) || (name == AName.min_size) || (name == AName.max_size) || (name == AName.position) || (name == TextConst.AName.TextLocation) || (name == AName.text_visible) || (name == AName.is_layout_block) || (name == AName.width_perc) || (name == AName.width_fixed) || (name == AName.fixed_side) || (name == AName.fill_height);
//        }
//        private void UpdateFormVForm(bool loadDefault = false, bool noData = false, XElement xForm = null)
//        {
//            if (_vform == null) return;

//            if (xForm == null)
//            {
//                xForm = _vform.GetFormXElement();
//            }

//            if (loadDefault)
//            {
//                xForm.Descendants().Attributes().Where(IsLayoutAttribute).Remove();
//                xForm.Descendants(TextConst.EName.LayoutColumns).Remove();
//                _vform.Descendants().Attributes().Where(IsLayoutAttribute).Remove();
//                _vform.Descendants(TextConst.EName.LayoutColumns).Remove();
//                _vform.Descendants().Attributes("custom-layout").Remove();
//            }


//            FormC = new UIFormC(_ds, (_vform.WithData) ? UIFormC.UseType.DataEditor : UIFormC.UseType.ParamEditor, noData);

//            // если создан в форме
//            _ds = FormC.DataSource;

//            FormC.Initialize(xForm);

//            panelControl1.Controls.Clear();
//            panelControl1.Controls.Add(FormC.TmpGetControlAsWinFormCtrl());

//            FormC.ApplyVisibitlity();

//            // не нужно
//            //FormC.InitData();
//            if (noData)
//            {
//                //FormC.CustomizeMode(true);
//            }
//            else
//            {
//                _ds.RefreshTopTable(false);
//                FormC.RefreshData();
//            }
//        }
//        public void UpdateFormVQuery(bool loadDefault = false, bool noData = false, XElement xForm = null)
//        {
//            if (_vquery == null) return;

//            if (xForm == null)
//            {
//                xForm = _vquery.GetFormXElement();
//            }

//            if (loadDefault)
//            {
//                xForm.Descendants().Attributes().Where(APredicate.IsLayoutOptions).Remove();
//                xForm.Descendants(TextConst.EName.LayoutColumns).Remove();
//                _vquery.Descendants().Attributes().Where(APredicate.IsLayoutOptions).Remove();
//                _vquery.Descendants(TextConst.EName.LayoutColumns).Remove();
//                _vquery.Descendants().Attributes("custom-layout").Remove();
//            }

//            FormC = new UIFormC(_ds, UIFormC.UseType.ParamEditor, noData);
//            // если создан в форме
//            _ds = FormC.DataSource;

//            FormC.Initialize(xForm);

//            panelControl1.Controls.Clear();
//            panelControl1.Controls.Add(FormC.TmpGetControlAsWinFormCtrl());

//            FormC.ApplyVisibitlity();
//            // не нужно
//            //FormC.InitData();
//        }


//        //public void OpenFormForSelect(string formName, object[] pars)
//        //{

//        //    var grid = (VGrid)XmlReports.Environment.GetForm("test-editor-ur_folders").GetDescedantsApplyingParts(TextConst.EName.Grid).First(e => e.P_MultiSelectColumn != "");
//        //    var col = grid.MultiSelectColumn();
//        //    _vform = XmlReports.Environment.GetForm(formName);

//        //    var formalParams = VForm.ExtendParamsForSelection(_vform.ParamsElement(), col);
//        //    XElement xpars = VDataSet.ParsObjectArrayToXelement(pars, formalParams);
//        //    _ds = _vform.CreateDataSetSelectorUse(col);
//        //    _ds.Connection = XmlReports.Environment.Connection;

//        //    VReport.ApplySimpleParams(xpars, _ds, formalParams);

//        //    //_ds.Refresh(false, xpars);
//        //    _ds.SetParams(xpars);
//        //    var vtable = _ds.Tables[0] as VDataTable;
//        //    if (vtable.Rows.Count > 0) vtable.CurrentRow = _ds.Tables[0].Rows[0];

//        //    var xForm = _vform.AsProcessedXElementForSelect();
//        //    UpdateFormVForm(false, false, xForm);

//        //}

//        public void OpenFormForSimpleSelect(string formName, object[] pars, VDataTable selectionTarget)
//        {
            
//            //// Бельченко 12.05.2017 убираю тк функционал не используется - убираю.
//            //// если что для восстановления организовать работу по новому  новый варианту _vform.GetFormXelementAndDataSet() вмесо  _vform.ProcessAndCreateDataSet()

//            ////////////var fds = _vform.GetFormXelementAndDataSet();
//            ////////////var XForm = fds.Item1;
//            ////////////_ds = fds.Item2;
//            //////////////var xForm = _vform.GetFormXElement();
//            //////////////_ds = _vform.ProcessAndCreateDataSet();
//            //_vform = XmlReports.Environment.GetForm(formName);
//            //XElement xpars = VDataSet.ParsObjectArrayToXelement(pars, _vform.ParamsElement());
//            //_ds = _vform.ProcessAndCreateDataSet();
//            //_ds.Connection = XmlReports.Environment.Connection;


//            //VReport.ApplySimpleParams(xpars, _ds, _vform.Element(TextConst.EName.Params));

//            //_ds.SetParams(xpars);

//            //var vtable = _ds.Tables[0] as VDataTable;
//            //if (vtable.Rows.Count > 0) vtable.CurrentRow = _ds.Tables[0].Rows[0];

//            //FormC = new UIFormC(_ds, UIFormC.UseType.DataEditor, false, selectionTarget);

//            //var xForm = _vform.GetFormXElement();

//            //FormC.Initialize(xForm);

//            //panelControl1.Controls.Clear();
//            //panelControl1.Controls.Add(FormC.TmpGetControlAsWinFormCtrl());


//            //_ds.RefreshTopTable(false);
//            //FormC.RefreshData();
//            //_loaded = true;

//        }


//        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //var grid=  new infoenergo.ui.win.Grid.GridControl();


//            //grid.GetType().GetProperties().Where(e=>e.PropertyType.Name==typeof());
//            //layoutControl1.BeginUpdate();
//            //var textEdit1 = new DevExpress.XtraEditors.TextEdit();
//            //var layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
//            //layoutControl1.Controls.Add(textEdit1);
//            //layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            //layoutControlItem1});

//            //textEdit1.Location = new System.Drawing.Point(108, 12);
//            //textEdit1.MenuManager = ribbonControl1;
//            //textEdit1.Name = "textEdit2";
//            //textEdit1.Size = new System.Drawing.Size(149, 20);
//            //textEdit1.StyleController = layoutControl1;
//            //textEdit1.TabIndex = 4;
//            //// 
//            //// layoutControlItem1
//            //// 
//            //layoutControlItem1.Control = textEdit1;
//            //layoutControlItem1.CustomizationFormText = "layoutControlItem2";
//            //layoutControlItem1.Location = new System.Drawing.Point(0, 0);
//            //layoutControlItem1.Name = "layoutControlItem1";
//            //layoutControlItem1.Size = new System.Drawing.Size(249, 100);
//            //layoutControlItem1.Text = "layoutControlItem1";
//            //layoutControlItem1.TextSize = new System.Drawing.Size(93, 13);
//            //layoutControl1.EndUpdate();

//            //this.layoutControl1.ResumeLayout(false);
//        }
//        /*
//        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            (new TestLayout()).Show();
//        }*/
//        /*private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().OpenFormVForm("editor-test", Array.Empty<object>());
//        }*/
//        /*private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var pars = new List<object>();
//            pars.Add(118);
//            pars.Add(118);
//            // pars.Add(10006396);
//            //pars.Add(10006225);
//           // thisControl<ucFormDesigner>().OpenFormForSelect("editor-ur_mat", pars.ToArray());
//        }
//        */
//        //private void ucDataEditorMain_Load(object sender, EventArgs e)
//        //{
//        //    //if (FormC == null) return;

//        //    AfterLoad();
//        //}

//        private void btnAccept_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucFormDesigner>();
//            //ctrl._frmFieldsSettings.UpdateFieldsSettings();
//            ctrl.FindForm().DialogResult = DialogResult.OK;
//        }

//        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucFormDesigner>().FindForm().DialogResult = DialogResult.Cancel;
//        }

//        private void btnLoadDefault_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (_vform != null)
//            {
//                UpdateFormVForm(true, true);
//                _frmFieldsSettings.Initialize(_vform);
//            }
//            else if (_vquery != null)
//            {
//                UpdateFormVQuery(true, true);
//                _frmFieldsSettings.Initialize(_vquery);
//            }
//        }
//        /*
//        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var pars = new List<object>();
//            pars.Add(227);


//            thisControl<ucFormDesigner>().OpenFormVForm("editor-ur_graf_opl", pars.ToArray());
//        }
//        */
//        /*private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().CallAction("fill_graf");

//        }*/
//        /*private void CallAction(string actionName)
//        {
//            //  _vform.ExecuteAction(actionName, _ds,null);
//        }*/
//        /*private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().CallAction("recalc_dolg");
//        }*/
//        /*private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().CallAction("set_sum_v");
//        }*/
//        private void ucDataEditorMain_Load(object sender, System.EventArgs e)
//        {
//            if (_loaded) return;

//            _frmFieldsSettings = new frmFieldsSettings();
//            if (_vquery != null)
//            {
//                UpdateFormVQuery(_load_default, _no_data);
//                _frmFieldsSettings.Initialize(_vquery);
//            }
//            else if (_vform != null)
//            {
//                UpdateFormVForm(_load_default, _no_data);
//                _frmFieldsSettings.Initialize(_vform);
//            }

//            _loaded = true;
//        }
//        /*
//        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var pars = new List<object>();
//            pars.Add(118);
//            //      pars.Add(118);
//            // pars.Add(10006396);
//            //pars.Add(10006225);
//            thisControl<ucFormDesigner>().OpenFormVForm("editor-ur_mat", pars.ToArray());
//        }
//        */
//        /*private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().CallAction("select_fin_doc");
//        }*/
//        /*private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            thisControl<ucFormDesigner>().CallAction("test_open_ur_mat");
//        }*/
//        /*private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            object[] pars = new object[1] { 118 };
//            thisControl<ucFormDesigner>().OpenFormVForm("test-editor-ur_mat", pars);
//        }*/
//        private void btnFieldsSettings_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var result = _frmFieldsSettings.ShowDialog();
//            if (result == DialogResult.OK)
//            {
//                if (_vform != null)
//                {
//                    Cmn.SyncWithVForm(FormC.XForm, VSXElement.GetDescedantsP(_vform));
//                    UpdateFormVForm(false, _no_data, FormC.XForm);
//                }
//                else if (_vquery != null)
//                {
//                    Cmn.SyncWithVForm(FormC.XForm, _vquery.Params().Cast<VSXElement>().ToList());
//                    UpdateFormVQuery(false, _no_data, FormC.XForm);
//                }
//            }
//        }
//    }
//}
