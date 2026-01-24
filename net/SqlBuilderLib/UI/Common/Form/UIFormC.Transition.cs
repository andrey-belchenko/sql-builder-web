using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
//using System.Windows.Forms;
using System.Xml.Linq;
// Cross-platform: Drawing2D and Imaging are Windows-only, commented out
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using Microsoft.Win32;
using sql.builder.DataApi;
//using sql.builder.Properties;
using sql.builder.XmlHelpers;
//using sql.builder.Controls.Grids;
using sql.builder.WinForms;

namespace sql.builder.UI
{
    // В этом файле временная реализация , в процессе перехода на класс не привязанный к winforms
    public partial class UIFormC : IForm
    {
        private void TmpBarManagetInitialize()
        {
            //var ctrl = TmpGetControlAsWinFormCtrl();
            //if (ctrl != null)
            //{
            //    ctrl.TmpGetBarManager().ForceLinkCreate();
            //    ctrl.TmpGetBarManager().ForceInitialize();
            //}
        }
        //internal BarManager TmpGetBarManager()
        //{
        //    var ctrl = TmpGetControlAsWinFormCtrl();
        //    if (ctrl == null)
        //    {
        //        return null;
        //    }
        //    return ctrl.TmpGetBarManager();
        //}
        internal IVBar GetToolBar()
        {
            //var ctrl = TmpGetControlAsWinFormCtrl();
            //if (ctrl == null)
            //{
            //    return null;
            //}
            //return GetControl().GetToolBar();
            return null;
        }
        //internal sql.builder.UI.WinForms.UIFormControl TmpGetControlAsWinFormCtrl()
        //{
        //    return GetControl() as sql.builder.UI.WinForms.UIFormControl;
        //}
        //internal BarButtonItem TmpGetBarButton(string btn)
        //{
        //    return TmpGetControlAsWinFormCtrl().TmpGetBarButton(btn);
        //}
    }
}
