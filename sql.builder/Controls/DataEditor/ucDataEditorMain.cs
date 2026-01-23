//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
////using System.Windows.Forms;
//using System.Linq;
//using System.Xml.Linq;
//using DevExpress.Skins;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;

//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using DevExpress.XtraBars.Docking2010.Views.Tabbed;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using sql.builder.UI.CommandItems;
//using Document = DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;

//namespace sql.builder
//{
//    internal partial class ucDataEditorMain : ucBase
//    {
//        public ucDataEditorMain()
//        {
//            InitializeComponent();
//        }
//        internal static List<VSXElement> GetFormsList()
//        {
//          //var forms=  XmlReports.Environment.GetElements(TextConst.EName.Forms).Where(e => e.Elements(TextConst.EName.From).Any()).OrderBy(e => e.P_Name).ToList();
//          // по косвенным признакам - придумать получше
//          List<VSXElement> forms = XmlReports.Environment.GetElements(TextConst.EName.Forms).Where(e => e.Elements(EName.from).Any() || e.AttrOrDefault(TextConst.AName.WithBehavior, true)).ToList();
//          List<VSXElement> qrys = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(e => e.P_IsReport == TextConst.AVBool.True && e.P_Title != "" && e.P_Form == "" && Cmn.GetAttrValue(e, TextConst.AName.WithBehavior) != TextConst.AVBool.False).ToList();
//          forms.AddRange(qrys);
//          forms = forms.OrderBy(e => e.P_Name).ToList();
//          return forms;
//        }
//        private void RefreshFormsList()
//        {
//            IList<VSXElement> forms = GetFormsList();
//            DataTable dt = Cmn.XElementsToDataTable(forms, false, true);
//            DataColumn col_name = dt.Columns["name"];
//            DataColumn col_favorite = new DataColumn("favorite", typeof(bool));
//            col_favorite.DefaultValue = Cmn.BOOLEAN_FALSE;
//            dt.Columns.Add(col_favorite);
//            //
//            string val = Cmn.ReadStringFromRegistry("all", "favorite_nodes");
//            if (!string.IsNullOrEmpty(val)) {
//                HashSet<string> favorites = new HashSet<string>(val.Split(','));
//                for (int index = 0; index < dt.Rows.Count; index++) {
//                    DataRow row = dt.Rows[index];
//                    if (favorites.Contains(row.Field<string>(col_name))) {
//                        row[col_favorite] = Cmn.BOOLEAN_TRUE;
//                    }
//                }
//            }
//            gcForms.DataSource = dt;
//        }
//        private void ucDataEditorMain_Load(object sender, EventArgs e)
//        {
//            XmlReports.Environment.Manager.GetController().LoadState();
//            RefreshFormsList();
//            GetRibbonSource<ucDataEditorMain>().CheckOneFormMode.EditValue = UIStatic._one_form_mode;
//            GetRibbonSource<ucDataEditorMain>().CheckShowWaitForms.EditValue = UIStatic._show_wait_forms;
//            GetRibbonSource<ucDataEditorMain>().seMaxFormsInGroup.EditValue = UIFormsPool.MaxInGroup;
//            Cache.UseFormsCache = SettingsHelper.UseFormCache;
//            GetRibbonSource<ucDataEditorMain>().CheckUseCache.EditValue = !Cache.UseFormsCache;
//        }
//        /// <summary>
//        /// Отладочный режим, каждая форма исп в одном экземпляре
//        /// </summary>
//        private void OpenForm(string formName, object[] pars)
//        {
//            UIFormC form = UIStatic.GetForm(formName, false, true);
//            // при первом создании сначала заполняем потом показываем, при повторном - сразу показываем, потом перезаполняем
//            var form_ctrl = form.TmpGetControlAsWinFormCtrl() as Control;
//            if (form_ctrl.Parent == null) {
//                if (form.Init) UIStatic.UpdateForm(form, pars, false);
//                ShowForm(form);
//            } else {
//                ShowForm(form);
//                if (form.Init) UIStatic.UpdateForm(form, pars, false);
//            }
//        }
//        private static ucDataEditorMain _instance = null;
//        internal static ucDataEditorMain GetInstance()
//        {
//            return _instance;
//        }
//        internal void ShowForm(IForm form)
//        {
//            var form1 = (UIFormC)form;
//            var form_ctrl = form1.TmpGetControlAsWinFormCtrl() as Control;
//            var doc = documentManager1.GetDocument(form_ctrl.Parent);
//            if (doc == null) {
//                var panel = new PanelControl();
//                panel.Dock = DockStyle.Fill;
//                panel.BorderStyle = BorderStyles.NoBorder;
//                panel.Controls.Add(form_ctrl);
//                doc = documentManager1.View.AddDocument(panel);
//                doc.Caption = form1.GetTitle();
//                form1.DockDocument = doc;
//                // form.DataEditorMain = this;
//                _instance = this;
//                // гридам кресса нужна форма
//                //form.ApplyVisibitlityForce();
//            } else {
//                var tview = (doc.Manager.View as TabbedView);
//                tview.Controller.Move(doc as Document, tview.Documents.Count - 1);
//                tview.ActivateDocument(form_ctrl.Parent);
//                //if (form is UIFormC2)
//                //{
//                //    Cmn.GetChildControlsOfType<UIFormC2Control>(form_ctrl.Parent)
//                //      .SelectMany(c => c.Form.GetRelativeFormsAndSelf())
//                //      .Where(f => f.AutoRefresh)
//                //      .Distinct()
//                //      .ForEach(f => f.RefreshData());   
//                //}
//                //else
//                //{
//                foreach (var f in Cmn.GetChildControlsOfType<sql.builder.UI.WinForms.UIFormControl>(form_ctrl.Parent) 
//                    .SelectMany(f => f.GetVForm().GetRelativeFormsAndSelf())
//                    .Where(f => f.AutoRefresh)
//                    .Distinct()) f.RefreshData();   
//                //}
//            }
//            if (form1.Layout != null) {
//                form1.Layout.RefreshLayout();
//            }
//            documentManager1.View.ActivateDocument(form_ctrl.Parent);
//        }
//        internal void CloseForm(UIFormC form)
//        {
//            documentManager1.View.RemoveDocument((form.TmpGetControlAsWinFormCtrl() as Control).Parent);
//        }
//        private void gridView1_Click(object sender, EventArgs e)
//        {
//            var gv = (GridView)sender;
//            if (gv.GetFocusedRow() != null) {
//                DataRow dr = gv.GetDataRow(gv.FocusedRowHandle);
//                OpenForm(dr.Field<string>("name"), null);
//            }
//        }
//        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucDataEditorMain>().RefreshFormsList();
//        }
//        private void UseCacheChanged()
//        {
//            Cache.UseFormsCache = !(bool)GetRibbonSource<ucDataEditorMain>().CheckUseCache.EditValue;
//            SettingsHelper.UseFormCache = Cache.UseFormsCache;
//        }
//        private void OneFormModeChanged()
//        {
//            UIStatic._one_form_mode = (bool)GetRibbonSource<ucDataEditorMain>().CheckOneFormMode.EditValue;
//        }
//        private void ShowWaitFormsChanged()
//        {
//            UIStatic._show_wait_forms = (bool)GetRibbonSource<ucDataEditorMain>().CheckShowWaitForms.EditValue;
//        }
//        private void riCheckUseCash_CheckedChanged(object sender, EventArgs e)
//        {
//            GetCurrentControl<ucDataEditorMain>().UseCacheChanged();
//        }
//        private void rceOneFormMode_CheckedChanged(object sender, EventArgs e)
//        {
//            GetCurrentControl<ucDataEditorMain>().OneFormModeChanged();
//        }
//        private void rceShowWaitForms_CheckedChanged(object sender, EventArgs e)
//        {
//            GetCurrentControl<ucDataEditorMain>().ShowWaitFormsChanged();
//        }
//        private void seMaxFormsInGroup_EditValueChanged(object sender, EventArgs e)
//        {
//            UIFormsPool.MaxInGroup = Convert.ToInt32(GetRibbonSource<ucDataEditorMain>().seMaxFormsInGroup.EditValue);
//        }
//        private void tabbedView1_DocumentClosing(object sender, DocumentCancelEventArgs args)
//        {
//            UIFormC uiform = (args.Document.Control.Controls[0] as UI.WinForms.UIFormControl).GetVForm();
//            if (uiform.IsModifiedSelfOrSub()) {
//                var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                if (result == DialogResult.Cancel) {
//                    args.Cancel = true;
//                    return;
//                } else if (result == DialogResult.Yes) {
//                    if (!uiform.SaveData(false)) {
//                        args.Cancel = true;
//                        return;
//                    }
//                } 
//            }
//            if (uiform.GroupName != null) {
//                args.Document.Control.Controls.Remove(uiform.TmpGetControlAsWinFormCtrl() as Control);
//                uiform.DataSource.ClearData();
//                UIFormsPool.Free(uiform);
//            }
//        }
//        /*private void tabbedView1_DocumentActivated(object sender, DocumentEventArgs args)
//        {
//            //if (args.Document.Control != null)
//            //{
//            //    Cmn.GetChildControlsOfType<UIFormC>(args.Document.Control).Where(f => f.FormUseType == UIFormC.UseType.DataEditor).ForEach(f => f.RefreshData());
//            //}
//        }*/
//        /// <summary>
//        /// Реакция на кнопку "Очистить пул форм"
//        /// </summary>
//        private void btnClearUIFormsPool_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            UIFormsPool.Reset();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Перекомпиляция форм"
//        /// </summary>
//        private void btnCompileForms_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            using (var frm = new frmCompileFormsCache(false)) {
//                UIStatic.ShowErrorsInSchemeEditor = false;
//                frm.ShowDialog();
//                UIStatic.ShowErrorsInSchemeEditor = true;
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Перезагрузить роли"
//        /// </summary>
//        private void btnReloadUserRoles_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            VSecurityUtils.ReloadUserRoles();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Добавить в избранное"
//        /// </summary>
//        private void btnToFavorites_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucDataEditorMain ctrl = GetCurrentControl<ucDataEditorMain>();
//            DataRow row = ctrl.gridView1.GetFocusedDataRow();
//            if (row == null) {
//                return;
//            }
//            if (row.Field<bool>("favorite")) {
//                row["favorite"] = Cmn.BOOLEAN_FALSE;
//            } else {
//                row["favorite"] = Cmn.BOOLEAN_TRUE;
//            }
//            string val = null;
//            DataRowCollection rows = row.Table.Rows;
//            for (int index = 0; index < rows.Count; index++) {
//                row = rows[index];
//                if (row.Field<bool>("favorite")) {
//                    string name = row.Field<string>("name");
//                    if (val == null) {
//                        val = name;
//                    } else {
//                        val = val + "," + name;
//                    }
//                }
//            }
//            Cmn.WriteStringToRegistry("all", "favorite_nodes", val);
//            ctrl.gridView1.RefreshData();
//        }
//        private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
//        {
//            DataRow row = this.gridView1.GetDataRow(e.RowHandle);
//            if (row == null) {
//                return;
//            }
//            if (row.Field<bool>("favorite")) {
//                Color c = Color.Khaki;
//                if (row == this.gridView1.GetFocusedDataRow()) {
//                    var focused_color = CommonSkins.GetSkin(LookAndFeel)[CommonSkins.SkinSelection].Color.BackColor;
//                    e.Appearance.BackColor = focused_color.MixColors(Color.FromArgb(c.R, c.G, c.B), 0.5F);
//                } else {
//                    e.Appearance.BackColor = c;
//                }
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Просмотр Temp (ctrl+T)"
//        /// </summary>
//        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            UIFormC.ActiveForm.viewTemp();
//        }
//    }
//}