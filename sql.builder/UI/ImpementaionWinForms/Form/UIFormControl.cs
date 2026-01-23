//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.DashboardCommon.Native;
////using DevExpress.XtraEditors;
////using DevExpress.XtraGrid.Views.Grid;
//
////using DevExpress.XtraBars;
////using DevExpress.XtraBars.Docking2010.Views;
////using DevExpress.XtraEditors.Controls;
//using infoenergo.core.Extensions;
//using sql.builder.Controls;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.WinForms;
//using infoenergo.ui.win.Base;
//using sql.builder.Controls.FormFields;
//using sql.builder.Controls.Grids;
//using sql.builder.XmlHelpers;
//using sql.builder.Exceptions;
//namespace sql.builder.UI.WinForms
//{
//    internal partial class UIFormControl : XtraUserControl,IVForm
//    {
//
//        public UIFormControl()
//        {
//            if (XmlReports.IsDeveloperMode())
//            {
//                DevExpress.XtraEditors.WindowsFormsSettings.DefaultSettingsCompatibilityMode = DevExpress.XtraEditors.SettingsCompatibilityMode.v17_1;
//
//            }// InitializeComponent();
//            this.Dock = DockStyle.Fill;
//       }
//
//        #region Обработчики событий
//        //private void ButtonRefresh_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//        //   // RefreshSourceWithCheckModified();
//        //}
//        //private void UIFormC_Load(object sender, EventArgs e)
//        //{
//        //    //_loaded = true;
//        //    //ApplyVisibitlity();
//        //}
//        //private void UIFormC_Paint(object sender, PaintEventArgs e)
//        //{
//        //    //Equip();
//        //}
//
//       
//        //private void ButtonSave_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//            
//        //}
//        //private void ButtonSaveAndClose_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//            
//        //}
//        //private void ButtonChoice_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//           
//        //}
//        //private void ButtonTest_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//            
//           
//        //}
//
//        
//        #endregion
//
//       
//       
//
//
//
//
//        //private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//           
//        //}
//
//        //private void btnExtParams_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//           
//        //}
//
//        //private void btnSaveSettings_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//            
//        //}
//
//        //private void btnLoadSettings_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//            
//        //}
//
//        
//        //private void buttonDelete_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//           
//        //}
//
//
//
//        public void AddChild(IVControl control)
//        {
//          
//            var tc = (Control)control;
//            tc.Dock = DockStyle.Fill;
//            Controls.Add(tc);
//
//        }
//
//        public void ClearChilds()
//        {
//          
//            Controls.Clear();
//
//        }
//       
//        #region временная реализация
//
//        private UIFormC _vform;
//        public UIFormC GetVForm()
//        {
//            return _vform;
//        }
//        public void SetVForm(UIFormC vform)
//        {
//             _vform= vform;
//        }
//
//
//        public BarManager TmpGetBarManager()
//        {
//            return barManager;
//        }
//        public Bar TmpGetToolBar()
//        {
//            return tbMain;
//        }
//
//
//        
//
//        public IVBar GetToolBar()
//        {
//            return tbMain;
//        }
//#endregion
//
//        //public BarButtonItem TmpGetBarButton(string btn)
//        //{
//        //    return this.specialButtons[btn];
//        //    //switch (btn)
//        //    //{
//        //    //    case FormBarButtonType.Refresh: return this.specialButtons[FormBarButtonType.Refresh];// ButtonRefresh;
//        //    //    case FormBarButtonType.Save: return this.specialButtons[FormBarButtonType.Save];// ButtonSave;
//        //    //    case FormBarButtonType.SaveAndClose: return ButtonSaveAndClose;
//        //    //    case FormBarButtonType.Delete: return ButtonDelete;
//        //    //    case FormBarButtonType.Choice: return ButtonChoice;
//        //    //    case FormBarButtonType.Test: return ButtonTest;
//        //    //    case FormBarButtonType.ExtParams: return btnExtParams;
//        //    //    case FormBarButtonType.LoadSettings: return btnLoadSettings;
//        //    //    case FormBarButtonType.SaveSettings: return btnSaveSettings;
//        //    //    case FormBarButtonType.ViewTemp: return barButtonItem4;
//        //    //    default: return null;
//        //    //}
//        //}
//
//        //public  BarItem GetToolBarItem(string name)
//        //{
//        //    //return (BarItem) Cmn.GetProperty(this, name);
//        //    return this.specialButtons[name];
//        //}
//
//
//
//
//
//
//        public void SetTitle(string title)
//        {
//            if (XmlReports.IsInfoenergo && this.ParentForm != null)
//            {
//                this.ParentForm.Text = title;
//            }
//            else if ( GetVForm().DockDocument != null)
//            {
//                 GetVForm().DockDocument.Caption = title;
//            }
//            else if (this.ParentForm != null)
//            {
//                this.ParentForm.Text = title;
//            }
//            else
//            {
//                var form = this.FindForm();
//                if (form != null)
//                {
//                    form.Text = title;
//                }
//            }
//        }
//
//        private static int _dialogcount;
//
//        public static bool IsDialogOpen()
//        {
//            return _dialogcount > 0;
//        }
//        public void ShowDialog(object owner)
//        {
//
//
//            _controller.LayoutSuspend();
//            var dform = _controller.GetDialogContainer() as frmDynamicEditor ?? new frmDynamicEditor(_controller.GetFormName());
//            dform.Controls.Add((Control)this);
//
//            dform.Text = _controller.GetTitle();
//            dform.Name = _controller.GetFormName();
//
//
//            dform.MdiParent = null;
//            dform.StartPosition = FormStartPosition.CenterParent;
//
//            _controller.SetDialogContainer(dform);
//            _controller.LayoutResume();
//            _dialogcount++;
//            dform.ShowDialog((Control)owner);
//            _dialogcount--;
//
//        }
//
//
//        
//
//
//        public void ShowForm()
//        {
//            if ( ucDataEditorMain.GetInstance() != null)
//            {
//                ucDataEditorMain.GetInstance().ShowForm(_controller);
//            }
//            else
//            {
//                var dform = _controller.GetDialogContainer() as frmDynamicEditor ?? new frmDynamicEditor(_controller.GetFormName());
//                dform.Controls.Add((Control)this);
//
//                dform.Text = _controller.GetTitle();
//                dform.Name = _controller.GetFormName();
//
//                _controller.SetDialogContainer(dform);
//
//                dform.Show();
//          
//
//               
//
//            }
//        }
//        IForm _controller = null;
//        public void SetController(IForm controller)
//        {
//            _controller = controller;
//        }
//    }
//
//   
//}
