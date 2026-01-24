
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
// Cross-platform: System.Windows.Input (WPF) is Windows-only, commented out
//using System.Windows.Input;
using System.Xml.Linq;
using sql.builder.DataApi;
using sql.builder.UI.CommandItems;
using infoenergo.sys;
using sql.builder.Exceptions;
//using sql.builder.Test;
using sql.builder.XmlHelpers;
namespace sql.builder.UI
{
    public static partial class UIStatic
    {
        //private static IVControlsFactory _factory = new sql.builder.UI.WinForms.VControlsFactoryWinForms();
        public static bool IsMpep = false;//костыль дя кастомизации главного окна, вынести в xml если понадобится
        //public static IVControlsFactory GetControlsfactory()
        //{
        //    return _factory;
        //}
        public static object RibbonParent = null;
        public static void FormsPoolReset()
        {
            UIFormsPool.Reset();
        }
        //public static void SetControlsfactory(IVControlsFactory factory)
        //{
        //    _factory = factory;
        //}


        public static bool IsWeb() // выременный метод для исключения некоторых кусков для web
        {
            return false;
            //return !(_factory is sql.builder.UI.WinForms.VControlsFactoryWinForms);
        }

        public static string ClipboardDummy = "вставка данных возможна только в приложении " + XmlReports.GetProductName();
        public static XElement MyClipboard = null;
        public static bool _one_form_mode = true;
        public static bool _show_wait_forms = false;
        public static bool IsAsync = true;
        private static bool _showErrorsInSchemeEditor = true;
        public static bool ShowErrorsInSchemeEditor
        {
            get
            {
                if (!XmlReports.IsDeveloperMode() || !XmlReports.IsNative)
                {
                    return false;
                }
                return _showErrorsInSchemeEditor;
            }
            set
            {
                _showErrorsInSchemeEditor = value;
            }
        }
        public static UIFormC CreateForm(string formName, object[] pars, bool isCreation, bool isDialog, bool useOneFormMode,
            Func<UIFormC, XElement> need_report_delegate = null, XElement xparams = null, string project = null)
        {
            if (!string.IsNullOrEmpty(project)) XmlReports.Environment.Manager.LoadProjectIfNeed(project);

            UIFormC form = GetForm(formName, isDialog, useOneFormMode, need_report_delegate);
            if (xparams != null) form.SetDefaultParams(xparams); // чтобы загружалось из сохраненного в базе шаблона
            if (form.Init) UpdateForm(form, pars, isCreation);

            return form;
        }


        public static UIFormC CreateSystemForm(string formName, bool isDialog)
        {
            var form = UIStatic.GetForm(formName, isDialog, true);
            form.DataSource.ClearData();
         
            form.ApplyVisibitlity();
            return form;
        }

        public static void LoadProject(string name)
        {
            XmlReports.Environment.LoadProject( name);
        }

        internal static UIFormC GetForm(string formName, bool isDialog, bool useOneFormMode,
            Func<UIFormC, XElement> need_report_delegate = null)
        {
            VCashUtils.ClearCashNotErrors();// Нужно чтобы периодически кеш зачищался наверняка есть утечка поставлю зачистку на открытие формы 

            UIFormC form = null;
            XElement xform = null;
            XElement xparams = null;
            VDataSet ds = null;

            string group_name = null;
            if (_one_form_mode && useOneFormMode && Cache.UseFormsCache)
            {
                group_name = GenerateFormGroupName(formName, null, isDialog);
                form = UIFormsPool.Get(group_name);


                if (form != null)
                {
                    // в режиме отладки при зажатом ctrl+shift форма генерируется заново всегда
                    //if (XmlReports.IsDeveloperMode() && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    //{
                    //    UIFormsPool.Release(form);
                    //    form = null;
                    //}
                    //else
                    //{
                        form.IsNew = false;
                        return form;
                    //}
                }
            }

            try
            {
                //Wait.Show("Построение формы", only_cursor: !_show_wait_forms);
                var mode = (UIStatic._show_wait_forms) ? WaitUIMode.WaitPanel : WaitUIMode.WaitCursor;
                WaitUIHelper.LastUsedUIHelper.Show("Построение формы", mode);
                //VForm vform = XmlReports.Environment.GetFormOrQueryAsForm(formName).GetPreprocessed();

                //VExceptionController.BeginProcessingElement(vform);

                //var fds = vform.GetFormXelementAndDataSet();
                var fds = VForm.GetFormXelementAndDataSet(formName);
                xform = fds.Item1;
                ds = fds.Item2;
                xparams = fds.Item3;
                //ds = vform.ProcessAndCreateDataSet();
                //xform = vform.GetFormXElement();
                //VExceptionController.EndProcessingElement(vform);
            }
            finally
            {
                //Wait.Hide();
                WaitUIHelper.LastUsedUIHelper.Hide();
            }

            ds.Connection = XmlReports.Environment.Connection;

            try
            {
                //Wait.Show("Загрузка формы", only_cursor: !_show_wait_forms);
                var mode = (UIStatic._show_wait_forms) ? WaitUIMode.WaitPanel : WaitUIMode.WaitCursor;
                WaitUIHelper.LastUsedUIHelper.Show("Загрузка формы", mode);
                //form = (XmlReports.UseNewForms) ? (UIFormC)new UIFormC2(ds, UIFormC.UseType.DataEditor, false) : (UIFormC)new UIFormC(ds, UIFormC.UseType.DataEditor, false);
                form = (UIFormC)new UIFormC(ds, UIFormC.UseType.DataEditor, false);
                form.XParams = xparams;
                // Емцов - падали формы с colsets
                if (need_report_delegate != null) form.NeedReportScheme += need_report_delegate;

                //form = new UIFormC(ds, UIFormC.UseType.DataEditor, false);
                //(form.TmpGetControlAsWinFormCtrl() as Control).Dock = DockStyle.Fill;
                form.Initialize(xform, isDialog);
                form.GroupName = group_name;

                if (group_name != null) UIFormsPool.Add(form);
            }
            finally
            {
                //Wait.Hide();
                WaitUIHelper.LastUsedUIHelper.Hide();
            }


            //   if (_one_form_mode) UIFormsPool.Add(form); // Бельченко: вроде это лишнее , дает ошибку, может при слиянии так получилось

            return form;
        }

       

        public static bool UpdateForm(UIFormC form, object[] pars, bool isCreation=false, bool checkModified = true)
        {
            try
            {
                //Wait.Show("Загрузка данных", only_cursor: !_show_wait_forms);
                if (form.Layout != null)
                {
                    form.Layout.ResetTabs();
                }
                
                var mode = (UIStatic._show_wait_forms) ? WaitUIMode.WaitPanel : WaitUIMode.WaitCursor;
                WaitUIHelper.LastUsedUIHelper.Show("Загрузка данных", mode);
                //VForm vform = XmlReports.Environment.GetFormOrQueryAsForm(form.FormName);

                if (pars == null) pars = CreatePars(form.XParams);
                if (pars != null) form.DataSource.SetParamsValues(pars);

                if (checkModified)
                {
                    bool success = form.RefreshSourceWithCheckModified(pars, isCreation);
                    return success;
                }
                else
                {
                    form.RefreshSource(pars, isCreation);
                    return true;
                }
            }
            finally
            {
                //Wait.Hide();
                WaitUIHelper.LastUsedUIHelper.Hide();
            }
        }
        internal static object[] CreatePars(XElement xpars)
        {

            var parsList = new List<object>();
            if (xpars != null)
            {
                foreach (XElement par in xpars.Elements())
                {
                    object val = null;
                    var cnst = par.Elements(TextConst.EName.Const).FirstOrDefault();
                    if (cnst != null)
                    {
                        val = VConst.ValueToObject(cnst.Value);
                    }
                    parsList.Add(val);
                }
            }
            return parsList.ToArray();
        }
        public static string GenerateFormGroupName(string form_name, object[] pars, bool is_dialog)
        {
            return (form_name + (pars != null ? "_" + string.Join("_", pars) : "_") + ((is_dialog) ? ":dialog" : ""));
        }

        public static void UpdateConnection()// пока так
        {
            db.Connection = Global.Connection;
        }

        public static DataSet GetReportData(string report_name)
        {
            var data = XmlReports.Environment.GetPrecompiledReport(report_name).Result(null, 2);
            if (!data.IsRefreshed) data.Refresh();

            return data;
        }
    }
}
