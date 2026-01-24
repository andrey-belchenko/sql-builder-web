using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVDialogManager 
    {
        void ShowQuestion(string title, string text, Action yesAction = null, Action noAction = null, Action cancelAction = null);

    }
}
