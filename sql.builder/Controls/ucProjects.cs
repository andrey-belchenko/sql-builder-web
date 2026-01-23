//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Xml.Linq;
////using System.Windows.Forms;
//using System.Data;
//using System.Linq;
//using System.IO;
//using DevExpress.XtraEditors;
//using sql.builder.Core;
//using sql.builder.UI;
//using sql.builder.DataApi;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//    internal partial class ucProjects : XtraUserControl
//    {
//        private ProjectsController controller;
//        private bool changes;
//        internal ucProjects()
//        {
//            this.InitializeComponent();
//            this.rleStatus.DataSource = new KeyValuePair<ProjectStatus, string>[4] {
//                new KeyValuePair<ProjectStatus, string>(ProjectStatus.NotLoaded, "Не загружен"),
//                new KeyValuePair<ProjectStatus, string>(ProjectStatus.Loaded, "Загружен"),
//                new KeyValuePair<ProjectStatus, string>(ProjectStatus.ReadyToLoad, "Загрузится"),
//                new KeyValuePair<ProjectStatus, string>(ProjectStatus.ReadyToUnload, "Выгрузится")
//            };
//        }
//        internal void Initialize(ProjectsController controller)
//        {
//            this.controller = controller;
//            this.controller.HasMessage += OnHasMessage;
//            this.LoadData();
//        }
//        private void LoadData()
//        {
//            grid.DataSource = this.controller.Projects.Values;
//        }
//        private void ReLoadData()
//        {
//            XmlReports.Environment.Manager.ReloadProjects();
//            this.controller.LoadState();
//            this.LoadData();
//        }
//        private ProjectRecord GetRecord(int rowHandle)
//        {
//            return this.view.GetRow(rowHandle) as ProjectRecord;
//        }
//        private ProjectRecord GetCurrentRecord()
//        {
//            return this.view.GetFocusedRow() as ProjectRecord;
//        }
//        private void view_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
//        {
//            if (e.Column.Name == "colStatus") {
//                ProjectRecord record = this.GetRecord(e.RowHandle);
//                if (record == null) {
//                    return;
//                }
//                Color color;
//                if (record.Status == ProjectStatus.NotLoaded) {
//                    color = Color.AliceBlue;
//                } else if (record.Status == ProjectStatus.ReadyToLoad) {
//                    color = Color.Khaki;
//                } else if (record.Status == ProjectStatus.Loaded) {
//                    color = Color.LightGreen;
//                } else if (record.Status == ProjectStatus.ReadyToUnload) {
//                    color = Color.LightPink;
//                } else {
//                    color = Color.AliceBlue;
//                }
//                e.Appearance.BackColor = color;
//            }
//        }
//        private void rceCheck_EditValueChanged(object sender, EventArgs e)
//        {
//            this.view.CloseEditor();
//            this.view.RefreshData();
//        }
//        private void btnAcceptChanges_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.controller.AcceptChanges();
//            this.controller.SaveState();
//            this.view.RefreshData();
//        }
//        private void btnCreateProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.openProjEditor(true);
//        }
//        private void btnEditProject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.openProjEditor(false);
//        }
//        private void openProjEditor(bool isCreation)
//        {
//            this.changes = false;
//            UIStatic.LoadProject("system");
//            UIFormC form = UIStatic.CreateSystemForm("sys_project", true);
//            string name;
//            IList<string> references;
//            if (isCreation) {
//                form.SetProp("is_creation", null);
//                name = string.Empty;
//                references = null;
//            } else {
//                ProjectRecord project = this.GetCurrentRecord();
//                project = this.GetCurrentRecord();
//                name = project.Name;
//                form.SetProp("project_name", name);
//                references = project.Project.ReferencesNames;
//            }
//            if (form.IsNew) {
//                //form.OnButtonClick += form_OnButtonClick;
//                form.CustomSave += this.form_CustomSave;
//            }
//            //form.LoadData(null);
//            form.GetParamField("project").SetValue(name);
//            DataTable dt = form.DataSource.Tables["refs"];
//            foreach (ProjectRecord p in this.controller.Projects.Values) {
//                string reference = p.Name;
//                DataRow row = dt.NewRow();
//                row["check"] = (references != null && references.Contains(reference)) ? Cmn.DECIMAL_ONE : Cmn.DECIMAL_ZERO;
//                row["name"] = reference;
//                dt.Rows.Add(row);
//            }
//            dt.AcceptChanges();
//            //
//            form.ShowDialog();
//            if (changes) {
//                this.ReLoadData();
//            }
//        }
//        private void form_CustomSave(UIFormC form)
//        {
//            string projName = (string)form.GetParamField("project").GetValue();
//            bool isCreation = form.HasProp("is_creation");
//            VSXElement element = null;
//            if (isCreation) {
//                XElement xproj = new XElement(EName.project);
//                xproj.Add(new XAttribute(AName.name, projName));
//                xproj.Add(new XElement(EName.references));
//                string fileName = XmlReports.Environment.GetElement<VSXElement>(EName.projects, AName.name, "system").AttrOrEmpty(AName.file);
//                element = XmlReports.Environment.CreateElement(TextConst.EName.Project, xproj, fileName);
//                string rootPath = Path.Combine(XmlReports.GetDefaultSourceFolder(), projName);
//                Directory.CreateDirectory(rootPath);
//                string path1 = Path.Combine(rootPath, projName + ".xml");
//                string path2 = Path.Combine(rootPath, projName + ".native.xml");
//                XElement root = new XElement(EName.root);
//                Cmn.SaveXmlWithCheckOut(root, path1);
//                Cmn.SaveXmlWithCheckOut(root, path2);
//                // добавление в проект VS
//                VSProjectHelper.AddContentToProject(path1);
//                VSProjectHelper.AddContentToProject(path2);
//            } else {
//                string project_name = (string)form.GetPropVal("project_name");
//                element = XmlReports.Environment.GetElement<VSXElement>(EName.projects, AName.name, project_name);
//                if (project_name != projName) {
//                    element.Attribute(AName.name).Value = projName;
//                }
//            }
//            XElement references = element.Element(EName.references);
//            if (references == null) {
//                references = new XElement(EName.references);
//                element.Add(references);
//            } else {
//                references.RemoveNodes();
//            }
//            DataRowCollection rows = form.DataSource.Tables["refs"].Rows;
//            for (int index = 0; index < rows.Count; index++) {
//                DataRow row = rows[index];
//                if (Cmn.DECIMAL_ONE.Equals(row["check"])) {
//                    references.Add(new XElement(EName.reference, new XAttribute(AName.project, row["name"].ToString())));
//                }
//            }
//            element.SaveInDefSourceFile();
//            form.RemoveProp("is_creation");
//            this.changes = true;
//        }
//        /*private static void sys_m_sel_a_CustomRowRefresh(System.Data.DataRow row)
//        {
//            row["name"] = row["ref"];
//        }*/
//        /*private void openSelectReferences(UIFormC form)
//        {
//            UIFormC formSel = UIStatic.CreateSystemForm("sys_multiselect", true);
//            if (formSel.IsNew) {
//                formSel.DataSource.GetTable("a").CustomFill += sys_m_sel_a_CustomFill;
//                formSel.DataSource.GetTable("m").CustomFill += NewRow_CustomFill;
//                formSel.DataSource.GetTable("b").CustomFill += (table) =>
//                {
//                    foreach (DataRow r in form.DataSource.GetTable("refs").Rows) {
//                        string name = r[TextConst.AName.Name].ToString();
//                        DataRow row = table.NewRow();
//                        row[TextConst.AName.Id] = name;
//                        row["ref"] = name;
//                        row[TextConst.AName.Name] = name;
//                        table.Rows.Add(row);
//                    }
//                };
//                formSel.DataSource.GetTable("b").CustomRowRefresh += sys_m_sel_a_CustomRowRefresh;
//            }
//            formSel.LoadData(null);
//            formSel.ClearDataOnClose = false;
//            DialogResult res = formSel.ShowDialog();
//            if (res == DialogResult.OK) {
//                VDataTable tbl1 = form.DataSource.GetTable("refs");
//                tbl1.ClearData();
//                foreach (DataRow r in formSel.DataSource.GetTable("b").Rows) {
//                    //string name = r["ref"].ToString();
//                    DataRow row = tbl1.NewRow();
//                    //row[TextConst.AName.Id] = name;
//                    row[TextConst.AName.Name] = r["ref"].ToString();
//                    tbl1.Rows.Add(row);
//                }
//                tbl1.RaiseUserChangedData(null,null);
//            }
//        }*/
//        /*private void sys_m_sel_a_CustomFill(VDataTable table)
//        {
//            foreach (ProjectRecord p in this.controller.Projects.Values) {
//                string name = p.Name;
//                DataRow row = table.NewRow();
//                row[TextConst.AName.Id] = name;
//                row[TextConst.AName.Name] = name;
//                table.Rows.Add(row);
//            }
//        }*/
//        /*private static void NewRow_CustomFill(VDataTable table)
//        {
//            DataRow row = table.NewRow();
//            table.Rows.Add(row);
//        }*/
//        /*private void form_OnButtonClick(UIFormC form, XElement value)
//        {
//            if (value.AttrOrEmpty(AName.name) == "select") {
//                this.openSelectReferences(form);
//            }
//        }*/
//        private void OnHasMessage(object sender, Core.HasMessageArgs hasMessageArgs)
//        {
//            this.tooltip.ShowHint(hasMessageArgs.Message);
//        }
//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (this.components != null)) {
//                this.components.Dispose();
//            }
//            if (this.controller != null) {
//                this.controller.HasMessage -= OnHasMessage;    
//            }
//            base.Dispose(disposing);
//        }
//        private void btnUnloadAllProjects_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.controller.UncheckAll();
//            this.view.RefreshData();
//        }
//        private void btnCheckSelected_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            int[] row_handles = view.GetSelectedRows();
//            for (int index = 0; index < row_handles.Length; index++) {
//                ProjectRecord project_record = this.GetRecord(row_handles[index]);
//                project_record.Checked = true;
//            }
//            this.view.RefreshData();
//        }
//    }
//}