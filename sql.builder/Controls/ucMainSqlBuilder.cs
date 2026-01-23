//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.XtraBars.Ribbon;
//using sql.builder.Controls.Testing;
//using sql.builder.Exceptions;

//namespace sql.builder.Controls
//{
//    internal partial class ucMainSqlBuilder : ucBase
//    {
//        private static ucMainSqlBuilder _object;
//        private Dictionary<string, ucBase> openControls;
//        public ucMainSqlBuilder()
//        {
//            this.InitializeComponent();
//            this.Multiple = true;
//            this.openControls = new Dictionary<string, ucBase>(4);
//            _object = this;
//        }
//        public override void Initialize()
//        {
//            Cursor tmp = Cursor.Current;
//            if (XmlReports.IsNative) {
//                this.OnBaseEvent(ucBaseEventType.ExecuteStart, "Загрузка приложения");
//            } else {
//                Cursor.Current = Cursors.WaitCursor;
//            }
//            try {
//                var env = XmlReports.Environment;
//                this.ribbonPageReports.Text = sql.builder.Controls.ucMainReports.MainPanelName;
//                #if DEBUG
//                if (XmlReports.IsDeveloperMode()) {
//                    this.ribbonPageScheme.Visible = true;
//                    // ribbonPageData.Visible = true;
//                    this.ribbonPageDataEditor.Visible = true;
//                    //Вкладка Тестирование
//                    //ribbonPageTesting.Visible = true; - не работает. где-то глубже она повторно отключается.
//                }
//                #endif
//                //отключаю вкладку Тестирование в релиз ветке
//                if (XmlReports.IsNative) {
//                    this.ribbonPageTesting.Visible = false;
//                }
//                // Перенес сюда вместо Load чтобы не показывалась форма до загрузки
//                this.OpenControl<ucMainReports>();
//            } finally {
//                if (XmlReports.IsNative) {
//                    this.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//                } else {
//                    Cursor.Current = tmp;
//                }
//            }
//        }
//        private void OpenControl<T>()
//            where T : ucBase, new()
//        {
//            string type_name = typeof(T).Name;
//            ucBase ctrl;
//            if (!this.openControls.TryGetValue(type_name, out ctrl)) {
//                ctrl = new T();
//                ctrl.Initialize();
//                this.openControls.Add(type_name, ctrl);
//            }
//            this.mainPanel.SuspendLayout();
//            if (ctrl.Parent == null) {
//                ctrl.Visible = false;
//                ctrl.Dock = DockStyle.Fill;
//                this.mainPanel.Controls.Add(ctrl);
//            }
//            if (!ctrl.Visible) {
//                for (int index = 0; index < this.mainPanel.Controls.Count; index++) {
//                    ucBase otherCtrl = (ucBase)this.mainPanel.Controls[index];
//                    if (otherCtrl != ctrl) {
//                        otherCtrl.SaveState();
//                        otherCtrl.Visible = false;
//                    }
//                }
//                ctrl.Visible = true;
//                ctrl.RestoreState();
//            }
//            this.mainPanel.ResumeLayout();
//        }
//        internal static void ShowErrorInSchemeEditor(VErrorInfo errorInfo)
//        {
//            if (_object == null) {
//                return;
//            }
//            _object.ribbonControl1.SelectedPage = _object.ribbonPageScheme;
//            ucQueriesEditor ctrl = (ucQueriesEditor)_object.openControls[typeof(ucQueriesEditor).Name];
//            ctrl.OpenElementByErrorInfo(errorInfo);            
//        }
//        private void ribbonControl1_SelectedPageChanging(object sender, RibbonPageChangingEventArgs e)
//        {
//            if (e.Page == this.ribbonPageReports) {
//                this.OpenControl<ucMainReports>();
//            } else if (e.Page == this.ribbonPageScheme) {
//                this.OpenControl<ucQueriesEditor>();
//            } else if (e.Page == this.ribbonPageDataEditor) {
//                this.OpenControl<ucDataEditorMain>();
//            } else if (e.Page == this.ribbonPageTesting) {
//                this.OpenControl<ucTestReports>();
//            }
//        }
//    }
//}