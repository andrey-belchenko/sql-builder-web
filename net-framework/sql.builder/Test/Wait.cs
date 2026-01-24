using System;
using System.Collections.Generic;
//using System.Windows.Forms;
//using infoenergo.ui.win;

namespace sql.builder.Test
{
    /// <summary>
    /// Старый вариант!! Использовать WaitUIHelper
    /// </summary>
    internal class Wait
    {
        private static int _cursor_cnt = 0;
        private static object syncObj = new object();
        private static Stack<WaitInfo> stack = new Stack<WaitInfo>();
        public static void Show(string caption, string description = "Пожалуйста, подождите...", bool over = false
            , object form = null, bool only_cursor = false)
        {
            //if (!SqlBuilder.ShowPopupWaitForms) return;

            //var waitInfo = new WaitInfo
            //{
            //    Caption = caption,
            //    Decrtiption = description,
            //    Over = over,
            //    WinForm = form,
            //    IsMainThread = true,
            //    OnlyCursor = only_cursor
            //};

            //// текущий поток не отличается от потока первой открытой формы
            //waitInfo.IsMainThread = (Application.OpenForms.Count == 0) || !Application.OpenForms[0].InvokeRequired;

            //show(waitInfo);
        }

        //private static void show(WaitInfo waitInfo, bool prev = false)
        //{
        //    lock (syncObj)
        //    {
        //        stack.Push(waitInfo);
        //        if (waitInfo.IsMainThread && !waitInfo.OnlyCursor)
        //        {
        //            if (stack.Count == 0 || waitInfo.Over)
        //            {
        //                if (WaitForm.IsShown)
        //                {
        //                    WaitForm.Close();
        //                }
        //            }
        //            WaitForm.Show(waitInfo.Caption, waitInfo.Decrtiption);
        //        }
        //        else if (waitInfo.WinForm != null)
        //        {
        //            if (waitInfo.WinForm.InvokeRequired)
        //            {
        //                waitInfo.WinForm.Invoke((Action)(() => waitInfo.WinForm.Cursor = Cursors.WaitCursor));
        //            }
        //            else
        //            {
        //                waitInfo.WinForm.Cursor = Cursors.WaitCursor;
        //            }
        //        }
        //        else if(waitInfo.IsMainThread && !prev)
        //        {
        //            if (_cursor_cnt == 0)
        //            {
        //                Cursor.Current = Cursors.WaitCursor;
        //            }
        //            _cursor_cnt += 1;
        //        }

        //        // использование NoModal выглядит стремно
        //        //stack.Push(waitInfo);

        //        //var form_base = waitInfo.WinForm as FormBase;

        //        //if (form_base != null)
        //        //{
        //        //    form_base.CloseNoModalDialog(null, DialogResult.OK);
        //        //    form_base.ShowNoModalDialog(waitInfo.Caption, waitInfo.Decrtiption);
        //        //}
        //        //else if (waitInfo.WinForm != null)
        //        //{
        //        //    if (waitInfo.WinForm.InvokeRequired)
        //        //    {
        //        //        waitInfo.WinForm.Invoke((Action)(() => waitInfo.WinForm.Cursor = Cursors.WaitCursor));
        //        //    }
        //        //    else
        //        //    {
        //        //        waitInfo.WinForm.Cursor = Cursors.WaitCursor;
        //        //    }
        //        //}
        //        //else if (waitInfo.IsMainThread)
        //        //{
        //        //    if (stack.Count == 0 || waitInfo.Over)
        //        //    {
        //        //        if (WaitForm.IsShown) WaitForm.Close();
        //        //        WaitForm.Show(waitInfo.Caption, waitInfo.Decrtiption);
        //        //    }
        //        //}
        //    }
        //}


        public static void Hide()
        {
            //if (!SqlBuilder.ShowPopupWaitForms) return;

            //bool show_prev = false;
            //WaitInfo waitInfo = null;

            //lock (syncObj)
            //{
            //    waitInfo = stack.Pop();

            //    // использование NoModal выглядит стремно
            //    //var form_base = waitInfo.WinForm as FormBase;

            //    //if (form_base != null)
            //    //{
            //    //    form_base.CloseNoModalDialog(null, DialogResult.OK);
            //    //}
            //    //else if (waitInfo.WinForm != null)
            //    //{
            //    //    if (waitInfo.WinForm.InvokeRequired)
            //    //    {
            //    //        waitInfo.WinForm.Invoke((Action)(() => waitInfo.WinForm.Cursor = Cursors.Default));
            //    //    }
            //    //    else
            //    //    {
            //    //        waitInfo.WinForm.Cursor = Cursors.Default;
            //    //    }
            //    //}
            //    //else if (waitInfo.IsMainThread)
            //    //{
            //    //    WaitForm.Close();
            //    //}

            //    if (waitInfo.IsMainThread && !waitInfo.OnlyCursor)
            //    {
            //        WaitForm.Close();
            //    }
            //    else if (waitInfo.WinForm != null)
            //    {
            //        if (waitInfo.WinForm.InvokeRequired)
            //        {
            //            waitInfo.WinForm.Invoke((Action)(() => waitInfo.WinForm.Cursor = Cursors.Default));
            //        }
            //        else
            //        {
            //            waitInfo.WinForm.Cursor = Cursors.Default;
            //        }
            //    }
            //    else if (waitInfo.IsMainThread)
            //    {
            //        _cursor_cnt -= 1;
            //        if (_cursor_cnt == 0)
            //        {
            //            Cursor.Current = Cursors.Default;
            //        }
            //    }

            //    if (stack.Count > 0)
            //    {
            //        show_prev = true;
            //        waitInfo = stack.Pop();
            //    }
            //}

            //if (show_prev) show(waitInfo, true);
        }
        public static void ForceHide()
        {
            while (stack.Count != 0)
            {
                Hide();
            }
        }

        private class WaitInfo
        {
            public string Caption;
            public string Decrtiption;
            public bool Over;
            //public Form WinForm;
            public bool IsMainThread;
            public bool OnlyCursor;
        }
    }
}
