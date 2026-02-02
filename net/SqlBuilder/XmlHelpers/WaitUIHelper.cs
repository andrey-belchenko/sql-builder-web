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
    public class WaitUIHelper : IDisposable
    {
        #region static
        private static WaitUIHelper last_used_ui_helper;
        public static void ShowCursor() // оборачивает эти странные вызовы через LastUsedUIHelper потом разобраться в чем их смысл, переделать
        {
            //LastUsedUIHelper.Show("", WaitUIMode.WaitCursor);
        }
        public static void HideCursor()
        {
           //LastUsedUIHelper.Hide();
        }
        public static WaitUIHelper LastUsedUIHelper {
            get {
                if (last_used_ui_helper == null) {
                    last_used_ui_helper = new WaitUIHelper(null);
                }
                return last_used_ui_helper;
            }
        }
        #endregion
        public const string DESCRIPTION_DEFAULT = "Пожалуйста, подождите...";
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
        public bool CanEmbed { get; set; }
        public WaitUIHelper(Object form = null)
        {
            //_form = form;
            ////_formControlStates = new Dictionary<Control, bool>();

            //_waitPanelShown = false;
            //_waitCursorShown = false;
            //CanEmbed = false;
            //ChangeLastUsedUIHelperIfCan();
        }
        public void Show(string caption, WaitUIMode mode, bool overlap = false, string description = DESCRIPTION_DEFAULT)
        {
        }
        public void SetDescription(string description)
        {
        }
        public void Hide()
        {
            //if (Wait.Waiting()) return;
            //ChangeLastUsedUIHelperIfCan();

            //if (!SqlBuilder.ShowPopupWaitForms) return;

            //// выполняем в потоке UI формы
            //InvokeIfNeed(ShowPrev);
        }
        public void ForceHide()
        {
            //if (Wait.Waiting()) return;
            //ChangeLastUsedUIHelperIfCan();

            //// выполняем в потоке UI формы
            //InvokeIfNeed(HideAll);
        }
        private void ShowNext(WaitUIInfo wi)
        {
        }

        private void ShowPrev()
        {
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
        }
        private void form_Resize(object sender, EventArgs e)
        {
            //var wait = _form.Controls[PROGRESS_PANEL_NAME];
            //if (wait == null) return;
            //wait.Location = new Point((_form.Width - wait.Width) / 2, (_form.Height - wait.Height) / 2);
        }
        private void HideWaitPanel()
        {
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
            public string Caption {get;set;}
            public WaitUIMode Mode {get;set;}
            public bool Overlap {get;set;}
            public string Description {get;set;}
            public static bool IsWaitPanel(WaitUIInfo info)
            {
                return info.Mode == WaitUIMode.WaitPanel;
            }
            public static bool IsWaitCursor(WaitUIInfo info)
            {
                return info.Mode == WaitUIMode.WaitCursor;
            }
        }
    }

    public enum WaitUIMode
    {
        WaitPanel,
        WaitCursor
    }
}