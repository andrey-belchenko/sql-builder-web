//using System;
//using System.Drawing;
//using System.Linq;
////using System.Windows.Forms;
//using DevExpress.XtraBars;
//using infoenergo.app.common;
//using infoenergo.Context;
//using MenuItem = infoenergo.app.common.MainRibbon.MenuItem;
//using infoenergo.ui.win.Forms;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.Controls;
//using sql.builder.UI.CommandItems;
//using sql.builder.Test;
//using sql.builder.UI.WinForms;

//namespace sql.builder.WinForms
//{
//    internal partial class frmDynamicEditor : frmBaseSqlBuilder, IContextForm
//    {
//        private string _form_name;
//        private string _inner_group_name;

//        BarManager bm = null;
//        PopupMenu menu = null;

//        internal frmDynamicEditor(string form_name = null, string inner_group_name = null)
//        {
//            _form_name = form_name;
//            _inner_group_name = inner_group_name;

//            InitializeComponent();

//            UpdateSize();
//        }

//        void UpdateSize()
//        {
//            if (_form_name == null) return;

//            string size = null;
//            if (!XmlReports.IsDeveloperMode())
//            {
//                string path = TextConst.RegPath.Forms + "\\" + _form_name;
//                if(_inner_group_name != null) path += "\\" + _inner_group_name;

//                size = Cmn.ReadStringFromRegistry(path, TextConst.RegVal.Size);
//            }

//            if (size == null)
//            {
//                var vform = XmlReports.Environment.GetForm(_form_name);
//                size = (_inner_group_name != null) 
//                    ? vform.GetDescedantsP(EName.fieldgroup).First(g => g.P_Alias == _inner_group_name).P_FormSize
//                    : vform.P_FormSize;
//            }

//            if (string.IsNullOrEmpty(size)) return;

//            SuspendLayout();
//            var sizes = size.Split(';');
//            Width = int.Parse(sizes[0]);
//            Height = int.Parse(sizes[1]);
//            ResumeLayout();
//        }
//        void SaveSize()
//        {
//            if (_form_name == null) return;

//            string size = Width + ";" + Height;
//            string path = TextConst.RegPath.Forms + "\\" + _form_name;
//            if (_inner_group_name != null) path += "\\" + _inner_group_name;

//            Cmn.WriteStringToRegistry(path, TextConst.RegVal.Size, size);
//        }
//        private void frmDynamicEditor_Load(object sender, EventArgs e)
//        {
//            if (XmlReports.IsDeveloperMode()) {
//                bm = new BarManager();
//                bm.Form = this;
//                menu = new PopupMenu(bm);
//                menu.MenuCaption = "Отладка";
//                menu.ShowCaption = true;
//                var btnSaveFormSize = new BarButtonItem(bm, 
//                    "Сохранить размеры формы " + _form_name + ((_inner_group_name != null) ? ":" + _inner_group_name : ""));
//                btnSaveFormSize.ItemClick += btnSaveFormSize_ItemClick;
//                menu.AddItem(btnSaveFormSize);
//                //var ctrls = Cmn.GetChildControls(this).ToArray();
//                //foreach (var grid in ctrls.OfType<ucReportGridNew>())
//                //{
//                //    var btn = new BarButtonItem(bm, string.Format("Сохранить параметры колонок для грида \"{0}\"", grid.ReportTitle));
//                //    menu.AddItem(btn);
//                //}
//                foreach (Control c in Cmn.GetChildControls(this)) {
//                    c.MouseClick += frmDynamicEditor_MouseClick;
//                }
//            }
//        }
//        private void frmDynamicEditor_OnFormClosed(object sender, FormClosedEventArgs args)
//        {
//            sql.builder.UI.WinForms.UIFormControl uiform_control = Controls[0] as sql.builder.UI.WinForms.UIFormControl;
//            if (uiform_control == null) return;

//            UIFormC uiform = uiform_control.GetVForm();

//            if (uiform.GroupName == null)
//            {
//                (uiform_control as Control).Dispose();
//            }
//            else
//            {
//                Controls.Remove(uiform_control as Control);
//                if (uiform.ClearDataOnClose)
//                {
//                    uiform.DataSource.ClearData();
//                }
//                UIFormsPool.Free(uiform);
//            }
//            //Cmn.GetChildControlsOfType<UIFormC>(this).ForEach(f => f.Dispose());
//        }
//        private void frmDynamicEditor_OnFormClosing(object sender, FormClosingEventArgs args)
//        {
//            // наивно полагаю, что ближайший контрол и будет самой верхней формой и достаточно проверить ее
//            // старые контролы форм
//            var uiform_unsaved = Controls.OfType<UIFormControl>().FirstOrDefault(f => f.GetVForm().FormUseType == UIFormC.UseType.DataEditor && f.GetVForm().IsModifiedSelfOrSub());
//            //if (uiform_unsaved == null)
//            //{
//            //    // новые контролы форм
//            //    var control = Controls.OfType<UIFormC2Control>().FirstOrDefault(f => f.Form.FormUseType == UIFormC.UseType.DataEditor && f.Form.IsModifiedSelfOrSub());
//            //    if (control != null) uiform_unsaved = control.Form;
//            //}
//            //var uiforms = Controls.OfType<UIFormC>().Where(f => f.GetCheckTableName() == null && f.IsModifiedSelfOrSub()).ToArray();
//            if (uiform_unsaved != null)
//            {
//                var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                if (result == DialogResult.Cancel) args.Cancel = true;
//                else if (result == DialogResult.Yes)
//                {
//                    if (!uiform_unsaved.GetVForm().SaveData(false))
//                    {
//                        args.Cancel = true;
//                        return;
//                    }
//                }
//            }

//            if (!args.Cancel && XmlReports.IsDeveloperMode())
//            {
//                foreach (var c in Cmn.GetChildControls(this)) c.MouseClick -= frmDynamicEditor_MouseClick;
//            }
//        }
//        private void frmDynamicEditor_ResizeBegin(object sender, EventArgs e)
//        {
//            SuspendLayout();
//        }
//        private void frmDynamicEditor_ResizeEnd(object sender, EventArgs e)
//        {
//            ResumeLayout();

//            if (WindowState != FormWindowState.Normal) return;

//            SaveSize();
//        }
//        private void frmDynamicEditor_MouseClick(object sender, MouseEventArgs e)
//        {
//            if (e.Button != MouseButtons.Right) return;

//            menu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//        }

//        private void btnSaveFormSize_ItemClick(object sender, EventArgs e)
//        {
//            if (_form_name == null) return;

//            var vform = XmlReports.Environment.GetForm(_form_name);
//            if (_inner_group_name == null)
//            {
//                vform.P_FormSize = Width + ";" + Height;
//            }
//            else
//            {
//                var vgroup = vform.GetDescedantsP(EName.fieldgroup).First(g => g.P_Alias == _inner_group_name);
//                vgroup.P_FormSize = Width + ";" + Height;
//            }

//            XmlReports.UpdateElementInCompiledScheme(vform);

//            vform.SourceFileName = ucQueryEditor.AddPathToFilename(Cmn.GetAttrValue(vform, "file"));
//            vform.ParentName = TextConst.EName.Forms;
//            vform.SaveInSourceFile();
//        }

//        #region Реализация IContextForm
//        public string UserText { get; set; }
//        public class MenuButtonFoldersPP : MenuItem
//        {
//            // чтобы открывалось не более одного раза
//            class frmDynamicEditorFoldersPP : frmDynamicEditor
//            {
//            }
//            public bool BeginGroup()
//            {
//                return false;
//            }
//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmDynamicEditorFoldersPP), false) as frmDynamicEditorFoldersPP;
//                string action_name;
//                if ((infoenergo.sys.Global.RS_ESYS.KOD_ESYS == decimal.One)) {
//                    action_name = "open_ur_folders_list_te";
//                } else {
//                    action_name = "open_ur_folders_list";
//                }
//                DataEditor.ExecuteAction("asuse2", action_name, null, frm);
//                frm.Show();
//                frm.Activate();
//                base.OnClick(link);
//            }
//        }
//        public class MenuButtonFoldersISP : MenuItem
//        {
//            // чтобы открывалось не более одного раза
//            class frmDynamicEditorFoldersIsp : frmDynamicEditor
//            {
//            }
//            public bool BeginGroup()
//            {
//                return false;
//            }
//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmDynamicEditorFoldersIsp), false) as frmDynamicEditorFoldersIsp;
//				DataEditor.ExecuteAction("asuse2", "open_ur_folders_isp_list", null, frm);
//                frm.Show();
//                frm.Activate();
//                base.OnClick(link);
//            }
//        }
//        public class MenuButtonFoldersBA : MenuItem
//        {
//            // чтобы открывалось не более одного раза
//            class frmDynamicEditorFoldersBa : frmDynamicEditor
//            {
//            }
//            public bool BeginGroup()
//            {
//                return false;
//            }
//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmDynamicEditorFoldersBa), false) as frmDynamicEditorFoldersBa;
//				DataEditor.ExecuteAction("asuse2", "open_ur_folders_bankrot_list", null, frm);
//                frm.Show();
//                frm.Activate();
//            }
//        }
//		public class MenuButtonUsersLKK : MenuItem
//		{
//			// чтобы открывалось не более одного раза
//			class frmDynamicEditorUsersLKK : frmDynamicEditor
//			{
//			}

//			public bool BeginGroup()
//			{
//				return false;
//			}
//			protected override void OnClick(BarItemLink link)
//			{
//				var frm = Run(typeof(frmDynamicEditorUsersLKK), false) as frmDynamicEditorUsersLKK;
//				DataEditor.ExecuteAction("kido_lkk", "open_vc_user_login_list", null, frm);
//				frm.Show();
//				frm.Activate();
//			}
//		}
//        public ContextGroupBase ContextGroup { get; set; }
//        public GlobalContextGroupBase GlobalContextGroup
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }
//        #endregion

//        private void frmDynamicEditor_Shown(object sender, EventArgs e)
//        {
//            if (this.Controls.Count > 0)
//            {
                
//            }

//            if (this.Controls.Count > 0)
//            {
//                var fromcComtrol = this.Controls[0] as sql.builder.UI.WinForms.UIFormControl;
//                if (fromcComtrol != null)
//                {
//                    fromcComtrol.GetVForm().LayoutRefresh();
//                }
//                else
//                {
//                    var grControl = this.Controls[0] as IVLayoutGroup;
//                    if (grControl != null)
//                    {
//                        var gr = grControl.GetInfo();
//                        gr.GetLayoutController().RefreshLayout();
//                    }
//                }
//            }
            
//        }
//    }
//}
