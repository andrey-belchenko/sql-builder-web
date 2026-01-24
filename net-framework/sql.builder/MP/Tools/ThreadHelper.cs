using System;

namespace sql.builder.MP.Tools
{
    public static class ThreadHelper
    {
        public static void RunInUIThread(Action action)
        {
            throw new NotImplementedException();
            //var form = System.Windows.Forms.Application.OpenForms[0];
            //if (form.InvokeRequired)
            //{
            //    form.Invoke(action);
            //}
            //else
            //{
            //    action();
            //}
        }
    }
}