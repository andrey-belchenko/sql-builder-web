using System;
using System.Xml.Linq;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
//using DevExpress.XtraLayout;
//using DevExpress.XtraLayout.Utils;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;
using sql.builder.WinForms;
//using sql.builder.Test;

namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        //internal Dictionary<string, LayoutControl> innerSubForms = new Dictionary<string, LayoutControl>();
        internal Dictionary<string, InnerSubFormInfo> innerSubFormsNew = new Dictionary<string, InnerSubFormInfo>();
        //frmDynamicEditor activeSubForm = null;
        internal void ShowInnerSubformNew(string name)
        {
            throw new NotImplementedException();
            //var info = innerSubFormsNew[name];
            //var mainGroup = info.Layout.GetMainGroup().GetControl() as Control;
            //var dform = (mainGroup.FindForm() as frmDynamicEditor);
            //if (dform == null)
            //{
            //    dform = new frmDynamicEditor(this.GetFormName(), name);
            //    dform.Controls.Add(mainGroup);
            //    dform.Text = info.Layout.GetMainGroup().GetText();
            //    dform.Name = name;
            //    mainGroup.Dock = DockStyle.Fill;
            //    info.Parent = dform;
            //    //  dform.Controls.Add(layout);
            //    // ContainerVisibleChanged(innerSubForms[name].Root, update_immediately: true, recursive: true);
            //    //RefreshData();
            //}
            //activeSubForm = dform;
            //dform.MdiParent = null;
            //dform.ShowDialog();
        }
        internal void HideInnerSubform()
        {
            //if (activeSubForm != null)
            //{
            //    activeSubForm.Hide();
            //}
        }
        internal class InnerSubFormInfo
        {
            public VLayout Layout { get; set; }
            public IDisposable Parent { get; set; }
        }
        //frmDynamicEditor _dform = null;
        //DialogResult LastDialogResult = DialogResult.None; // возможно тоже самое уже реализовано другим сособом
        //internal DialogResult ShowDialog()
        //{
        //    LastDialogResult = DialogResult.None;
        //    ApplyVisibitlity();
        //    if (_dform == null)
        //    {
        //        _dform = new frmDynamicEditor(this.GetFormName());
        //    }
        //    _dform.Controls.Add(TmpGetControlAsWinFormCtrl() as Control);
        //    _dform.Text = GetTitle();
        //    _dform.Name = GetFormName();
        //    _dform.MdiParent = null;
        //    _dform.StartPosition = FormStartPosition.CenterParent;
        //     LayoutResume();
        //     //UpdateTitle();
        //    // uiform.LayoutRefresh();
        //     _dform.ShowDialog(null);
        //     return this.LastDialogResult;
        //}
    }
}
