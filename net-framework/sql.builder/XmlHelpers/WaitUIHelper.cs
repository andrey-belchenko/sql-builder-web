using System;
using System.Collections.Generic;
using System.Linq;
////using System.Windows.Forms;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraWaitForm;
//using Point = System.Drawing.Point;
//using WaitForm = infoenergo.ui.win.WaitForm;

namespace sql.builder
{
    internal class WaitUIHelper : IDisposable
    {
        #region static
        private static WaitUIHelper last_used_ui_helper;
        internal static void ShowCursor() // оборачивает эти странные вызовы через LastUsedUIHelper потом разобраться в чем их смысл, переделать
        {
            //LastUsedUIHelper.Show("", WaitUIMode.WaitCursor);
        }
        internal static void HideCursor()
        {
           //LastUsedUIHelper.Hide();
        }
        internal static WaitUIHelper LastUsedUIHelper {
            get {
                if (last_used_ui_helper == null) {
                    last_used_ui_helper = new WaitUIHelper(null);
                }
                return last_used_ui_helper;
            }
        }
        #endregion
        internal const string DESCRIPTION_DEFAULT = "Пожалуйста, подождите...";
        const string PROGRESS_PANEL_NAME = "pProgress__";
        #region поля
        //private Form _form;
        //private Dictionary<Control, bool> _formControlStates;
        private bool _waitPanelShown;
        private bool _waitCursorShown;
        private object _lockObj = new object();
        private Stack<WaitUIInfo> _stack = new Stack<WaitUIInfo>();
        #endregion
        // панелька "подождите" встраивается прямо в форму
        // влючил только для ExpressForm, т.к. нормально отображается только если не заблокирован основной поток
        internal bool CanEmbed { get; set; }
        internal WaitUIHelper(Object form = null)
        {
            //_form = form;
            ////_formControlStates = new Dictionary<Control, bool>();

            //_waitPanelShown = false;
            //_waitCursorShown = false;
            //CanEmbed = false;
            //ChangeLastUsedUIHelperIfCan();
        }
        internal void Show(string caption, WaitUIMode mode, bool overlap = false, string description = DESCRIPTION_DEFAULT)
        {

            //Console.WriteLine("start " + caption + ":" + description);
            //if (Wait.Waiting()) return;
            //ChangeLastUsedUIHelperIfCan();

            //if (!SqlBuilder.ShowPopupWaitForms) return;

            //var wi = new WaitUIInfo()
            //{
            //    Caption = caption,
            //    Mode = mode,
            //    Overlap = overlap,
            //    Description = description
            //};

            //// выполняем в потоке UI формы
            //InvokeIfNeed(() => ShowNext(wi));
        }
        internal void SetDescription(string description)
        {
            //if (Wait.Waiting()) {
            //    return;
            //}
            //this.ChangeLastUsedUIHelperIfCan();
            //if (!SqlBuilder.ShowPopupWaitForms) {
            //    return;
            //}
            //// выполняем в потоке UI формы
            //this.InvokeIfNeed(() =>
            //{
            //    if (WaitForm.IsShown) {
            //        WaitForm.SetCurrentDescription(description);
            //    }
            //});
        }
        internal void Hide()
        {
            //if (Wait.Waiting()) return;
            //ChangeLastUsedUIHelperIfCan();

            //if (!SqlBuilder.ShowPopupWaitForms) return;

            //// выполняем в потоке UI формы
            //InvokeIfNeed(ShowPrev);
        }
        internal void ForceHide()
        {
            //if (Wait.Waiting()) return;
            //ChangeLastUsedUIHelperIfCan();

            //// выполняем в потоке UI формы
            //InvokeIfNeed(HideAll);
        }
        private void ShowNext(WaitUIInfo wi)
        {
            //lock (_lockObj)
            //{
            //    if (_stack.Count == 0)
            //    {
            //        switch (wi.Mode)
            //        {
            //            case WaitUIMode.WaitPanel:
            //                ShowWaitPanel(wi.Caption, wi.Description);
            //                break;
            //            case WaitUIMode.WaitCursor:
            //                ShowWaitCursor();
            //                break;
            //        }
            //    }
            //    else if (wi.Overlap)
            //    {
            //        switch (wi.Mode)
            //        {
            //            case WaitUIMode.WaitPanel:
            //                if (_waitPanelShown) ChangeWaitPanel(wi.Caption, wi.Description);
            //                else ShowWaitPanel(wi.Caption, wi.Description);
            //                break;
            //            case WaitUIMode.WaitCursor:
            //                if (!_waitCursorShown) ShowWaitCursor();
            //                break;
            //        }
            //    }

            //    _stack.Push(wi);   
            //}
        }

        private void ShowPrev()
        {
            //lock (_lockObj)
            //{
            //    if (_stack.Count != 0)
            //    {
            //        var wi = _stack.Pop();

            //        if (_stack.Count == 0)
            //        {
            //            if (_waitPanelShown) HideWaitPanel();
            //            if (_waitCursorShown) HideWaitCursor();
            //        }
            //        else
            //        {
            //            // не укладывается в концепцию стека
            //            IList<WaitUIInfo> wiOverlapped = _stack.Where(w => w.Overlap || w == _stack.First()).ToList();
            //            if (_waitPanelShown) {
            //                WaitUIInfo wiWaitPanelPrev = wiOverlapped.LastOrDefault(WaitUIInfo.IsWaitPanel);
            //                if (wiWaitPanelPrev == null) {
            //                    HideWaitPanel();
            //                } else {
            //                    ChangeWaitPanel(wiWaitPanelPrev.Caption, wiWaitPanelPrev.Description);
            //                }
            //            }
            //            if (_waitCursorShown) {
            //                WaitUIInfo wiWaitCursorPrev = wiOverlapped.LastOrDefault(WaitUIInfo.IsWaitCursor);
            //                if (wiWaitCursorPrev == null) {
            //                    HideWaitCursor();
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        HideAll();
            //    }
            //}
        }
        private void HideAll()
        {
            //if (Wait.Waiting()) return;
            //lock (_lockObj)
            //{
            //    _stack.Clear();
            //    if (_waitPanelShown) HideWaitPanel();
            //    if (_waitCursorShown) HideWaitCursor();
            //}
        }

        private void ShowWaitCursor()
        {
            //if (_form == null)
            //{
            //    Cursor.Current = Cursors.WaitCursor;
            //}
            //else
            //{
            //    _form.Cursor = Cursors.WaitCursor;
            //}

            //_waitCursorShown = true;
        }
        private void HideWaitCursor()
        {
            //if (_form == null)
            //{
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            //    _form.Cursor = Cursors.Default;
            //}

            //_waitCursorShown = false;
        }

        private void ShowWaitPanel(string caption, string description)
        {
            //if (_form == null || !CanEmbed)
            //{
            //    WaitForm.Show(caption, description);
            //}
            //else
            //{
            //    // включил только для ExpressForm
            //    _form.SuspendLayout();
            //    foreach (Control control in _form.Controls)
            //    {
            //        _formControlStates.Add(control, control.Enabled);
            //        control.Enabled = false;
            //    }

            //    var wait = new ProgressPanel()
            //    {
            //        BorderStyle = BorderStyles.Simple, 
            //        Name = PROGRESS_PANEL_NAME, 
            //        Caption = caption, 
            //        Description = description,
            //        Width = 400,
            //        Height = 70,
            //        ImageHorzOffset = 30
            //    };

            //    wait.Location = new Point((_form.Width - wait.Width) / 2, (_form.Height - wait.Height) / 2);
            //    _form.Controls.Add(wait);
            //    _form.Controls.SetChildIndex(wait, 0);
            //    _form.Resize += form_Resize;
            //    _form.ResumeLayout(true);
            //}

            //_waitPanelShown = true;
        }
        private void form_Resize(object sender, EventArgs e)
        {
            //var wait = _form.Controls[PROGRESS_PANEL_NAME];
            //if (wait == null) return;
            //wait.Location = new Point((_form.Width - wait.Width) / 2, (_form.Height - wait.Height) / 2);
        }
        private void HideWaitPanel()
        {
            //if (_form == null  || !CanEmbed)
            //{
            //    WaitForm.Close();
            //}
            //else
            //{
            //    _form.SuspendLayout();
            //    // они же не успели задиспозиться, правда?
            //    foreach (var cs in _formControlStates)
            //    {
            //        cs.Key.Enabled = cs.Value;
            //    }
            //    _formControlStates.Clear();
            //    _form.Controls.RemoveByKey(PROGRESS_PANEL_NAME);
            //    _form.ResumeLayout(true);
            //}

            //_waitPanelShown = false;
        }
        private void ChangeWaitPanel(string caption, string description)
        {
            //if (_form == null || !CanEmbed)
            //{
            //    WaitForm.Show(caption, description);
            //}
            //else
            //{
            //    var wait = (ProgressPanel)_form.Controls[PROGRESS_PANEL_NAME];
            //    wait.Caption = caption;
            //    wait.Description = description;
            //}
        }

        private void InvokeIfNeed(Object action)
        {
            //if(_form == null)
            //{
            //    action();
            //    return;
            //}

            //if (_form.IsDisposed) return;

            //if (_form.InvokeRequired) _form.Invoke(action);
            //else action();
        }

        private void ChangeLastUsedUIHelperIfCan()
        {
            //WaitUIHelper helper = last_used_ui_helper;
            //if (helper != null && helper != this) { //Бельченко 20180510: Заплатка, чтобы не возникала проблема ниже. "Логика нарушена - возможно одна форма с WaitUIHelper вызвала другую". Почему так нельзя? что - то непродумано
            //    helper.HideAll();
            //}
            //if (helper != null && helper != this && helper._stack.Count > 0) {
            //    try {
            //        throw new InvalidOperationException("Логика нарушена - возможно одна форма с WaitUIHelper вызвала другую");
            //    } catch (InvalidOperationException ex) {
            //        //
            //    }
            //}
            //last_used_ui_helper = this;
        }
        public void Dispose()
        {
            //// от утечек, чтобы не держать form
            //if (last_used_ui_helper == this) {
            //    last_used_ui_helper = new WaitUIHelper();
            //}
            //if (this._form != null) {
            //    this._form.Resize -= form_Resize;
            //    this._form = null;
            //}
            //this._formControlStates.Clear();
        }
       
        private class WaitUIInfo
        {
            internal string Caption {get;set;}
            internal WaitUIMode Mode {get;set;}
            internal bool Overlap {get;set;}
            internal string Description {get;set;}
            internal static bool IsWaitPanel(WaitUIInfo info)
            {
                return info.Mode == WaitUIMode.WaitPanel;
            }
            internal static bool IsWaitCursor(WaitUIInfo info)
            {
                return info.Mode == WaitUIMode.WaitCursor;
            }
        }
    }

    internal enum WaitUIMode
    {
        WaitPanel,
        WaitCursor
    }
}