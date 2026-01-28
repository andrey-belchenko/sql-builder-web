using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
//using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
////using System.Windows.Forms; // FolderBrowserDialog
using System.Xml;
using System.Xml.Linq;
//using DevExpress.LookAndFeel;
//using DevExpress.Utils.Taskbar.Core;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using infoenergo.framework.Extensions.Oracle;
//using Microsoft.Office.Interop.Word;
//using Microsoft.Win32;
using sql.builder.DataApi;
//using sql.builder.FieldInfo;
using sql.builder.Print.Xlsx;
using sql.builder.UI;
using sql.builder.XmlHelpers;
using SqlBuilderLib.DevTools;

namespace sql.builder.WinForms
{
    internal sealed partial class CleanFrmExpressReport
    {
        /// <summary>
        ///          
        /// </summary>
        internal event EventHandler<CleanExpressReportEventArgs> ReportOpening;
        /// <summary>
        ///     
        /// </summary>
        internal event EventHandler<CleanExpressReportEventArgs> CustomPrint;

        private bool _openDocumentAfterPrint = true;
        /// <summary>
        ///     
        /// </summary>
        internal bool OpenDocumentAfterPrint
        {
            get
            {
                return this._openDocumentAfterPrint;
            }
            set
            {
                this._openDocumentAfterPrint = value;
            }
        }
        private bool _showMessages = true;
        internal bool ShowMessages
        {
            get
            {
                return this._showMessages;
            }
            set
            {
                this._showMessages = value;
            }
        }

        internal DataSet GetParamsData()
        {
            //  ,  
            return (DataSet)this._uIForm.DataSource;
        }
        /// <summary>
        ///   ResultData   CustomPrint
        /// </summary>
        /// <param name="param"></param>
        internal void DoCustomPrint(string param)
        {
            if (this.ValidateParams() != string.Empty)
            {
                return;
            }
            VDataSet ds = this.RefreshData();
            this.TryCustomPrint(ds, param);
        }
        #region  
        private string _report_name;
        private VReport _report;
        private VDataSet _dataSet;
        private UIFormC _uIForm;
        private DataTable _dt_params;
        private DataTable _dt_repository_info;
        private DataTable _dt_print_forms;
        private bool _isWithBehavior;
        internal static object lockObj = new Object();
        private CancellationTokenSource cts;
        private bool in_process;
        private Dictionary<string, object> _user_settings;
        private string _reg_path;
        #endregion
        #region 

        private XElement ParamValues
        {
            get
            {
                if (this._uIForm == null)
                {
                    return null;
                }
                else
                {
                    return this._uIForm.GetValue();
                }
            }
        }

        public XmlNode GetTemplateInfo(string name = null)
        {
            if (this._dt_print_forms == null)
            {
                return null;
            }
            else if (this._dt_print_forms.Rows.Count == 0)
            {
                return null;
            }
            else if (name == null)
            {
                return (XmlNode)this._dt_print_forms.Rows[0]["node"];
            }
            else
            {
                //return (XmlNode)this._dt_print_forms.AsEnumerable().First(row => row["name"].Equals(bePrintForm.EditValue))["node"];
                for (int index = 0; index < this._dt_print_forms.Rows.Count; index++)
                {
                    DataRow row = this._dt_print_forms.Rows[index];
                    if (row["name"].Equals(name))
                    {
                        return (XmlNode)row["node"];
                    }
                }
                return null;
            }
        }
        #endregion
        #region 
        //
        #endregion
        #region  

        internal UIFormC GetUIForm()
        {
            return this._uIForm;
        }
        /// <summary>
        ///     Initialize(string report_name)
        /// </summary>
        internal CleanFrmExpressReport()
        {
            //this.InitializeComponent();
            this._user_settings = new Dictionary<string, object>();
        }
        internal static string GetProjectNameFromNavigator(string reportName) //  
        {
            XElement xusereport = XmlReports.GetUseReport(reportName);
            if (xusereport != null)
            {
                return xusereport.Attribute(AName.project).Value;
            }
            else
            {
                return null;
            }
        }
        public bool Initialize(string report_name)
        {
            try
            {

                //  report_name
                string project;
                int n_pos = report_name.IndexOf('.');
                if (n_pos >= 0)
                {
                    project = report_name.Substring(0, n_pos);
                    report_name = report_name.Substring(n_pos + 1);
                }
                else
                {
                    project = GetProjectNameFromNavigator(report_name);
                }
                this._report = XmlReports.Environment.GetPrecompiledReport(report_name, project);
                //    
                this._dt_print_forms = new DataTable();
                DataColumn col_name = this._dt_print_forms.Columns.Add("name", typeof(string));
                DataColumn col_title = this._dt_print_forms.Columns.Add("title", typeof(string));
                DataColumn col_node = this._dt_print_forms.Columns.Add("node", typeof(object));
                this._dt_print_forms.PrimaryKey = new DataColumn[1] { col_name };
                XElement xreport = XmlReports.GetReport(report_name);
                var xmlreport = new XmlDocument();
                xmlreport.LoadXml(xreport.ToString());
                var names = new HashSet<string>();
                foreach (XmlNode printFormNode in xmlreport.FirstChild.SelectNodes("print-templates//template"))
                {

                   //TODO: в основном решении ошибки при дублях нет, тут была
                    var name = printFormNode.Attributes["name"].Value;
                    if (names.Contains(name)) continue;
                    names.Add(name);
                 

                    DataRow row = this._dt_print_forms.NewRow();
                    row[col_name] = printFormNode.Attributes["name"].Value;
                    row[col_title] = printFormNode.Attributes["title"].Value;
                    row[col_node] = printFormNode;
                    this._dt_print_forms.Rows.Add(row);
                }
                //    
                XElement xform = XmlReports.GetForm(this._report.P_Form, report_name);
                this._isWithBehavior = !xform.AttrOrDefault(TextConst.AName.WithBehavior, true);
                if (this._isWithBehavior)
                {
                    //  -  -, ..    colsets
                    this._uIForm = UIStatic.CreateForm(xform.Attribute(AName.name).Value, null, false, false, false, this.GetReportScheme);
                    if (this._uIForm.Init)
                    {
                        UIStatic.UpdateForm(this._uIForm, null, false);
                    }
                }
                else
                {
                    this._uIForm = new UIFormC();
                    this._uIForm.NeedReportScheme += this.GetReportScheme;
                }
                this._uIForm.NeedReport += this.GetReport;
                this._uIForm.SpecialTypeChanged += this.UIFormC_SpecialTypeChanged;
                if (!this._isWithBehavior)
                {
                    this._uIForm.Initialize(xform);
                }
                XElement xparams = Cmn.LoadDefaultReportParams(report_name);
                if (xparams != null)
                {
                    //this.ceStoreDefaultParams.EditValue = true;
                    this._uIForm.SetDefaultParams(xparams);
                }
                else
                {
                    //this.ceStoreDefaultParams.EditValue = false;
                }

                this._dt_params = new DataTable();
                DataColumn col_check = this._dt_params.Columns.Add("check", typeof(bool));
                col_name = this._dt_params.Columns.Add("name", typeof(string));
                col_title = this._dt_params.Columns.Add("title", typeof(string));
                DataColumn col_parent_name = this._dt_params.Columns.Add("parent_name", typeof(string));
                this._dt_params.PrimaryKey = new[] { col_name };
                XElement content = xform.Element(EName.content);
                if (content == null)
                {
                    content = xform;
                }
                foreach (XElement xfield in content.Elements(EName.field))
                {
                    DataRow row = this._dt_params.NewRow();
                    row[col_check] = Cmn.BOOLEAN_TRUE;
                    row[col_name] = xfield.AttrOrEmpty(AName.name);
                    row[col_title] = xfield.AttrOrEmpty(AName.title);
                    row[col_parent_name] = DBNull.Value;
                    this._dt_params.Rows.Add(row);
                }
                this._dt_params.AcceptChanges();

                //Control ctrl = this._uIForm.TmpGetControlAsWinFormCtrl() as Control;
                //ctrl.Dock = DockStyle.Fill;
                //pParams.Controls.Add(ctrl);
                this._uIForm.ApplyVisibitlity();
                //   
                //this.Text = _report.P_SelfTitle;
                this._report_name = report_name;
                //if (this._report.AttrOrDefault(AName.use_repository, false))
                //{
                //    this.tlRepositories.DataSource = this._dt_repository_info = RepositoriesHelper.GetQueryRepositories(this._report);
                //    if (this._dt_repository_info.Rows.Count > 0)
                //    {
                //        this.gcRepositories.Visible = true;
                //    }
                //}
                if (!DevAnalyzer.PrepareOnly)
                {
                    this._uIForm.RefreshData();
                }
                
                //_no_params_mode = (!_uIForm.controls.Any() && (_dt_repository_info == null || _dt_repository_info.Rows.Count == 0));
                //this.FillWorkFolder();
            }
            finally
            {
                WaitUIHelper.LastUsedUIHelper.Hide();
            }
            return true;
        }
        #endregion
        #region  
        private VDataSet RefreshData(bool async = false)
        {
            lock (lockObj)
            {
                decimal kod_log = 0M;
                VDataSet ds = null;
                try
                {
                    XElement rep_params = this.ParamValues ?? new XElement(EName.@params);
                    XElement rep_params2 = new XElement(EName.root);
                    Parser.SaveReportParamsToXml(rep_params2, GetUIForm());
                    kod_log = Logger.ReportStart(this._report_name, rep_params2);
                    // 
                    ds = _report.Result(rep_params, 2, null, true, this._dataSet != null ? _dataSet.SchemePreset : null);
                    if (ds.Connection == null)
                    {
                        ds.Connection = XmlReports.Environment.Connection;
                    }
                    if (async)
                    {
                        ds.Connection = XmlReports.Environment.Connection.Clone();
                        //ds.Connection.Open(useGlobalSettings: true);
                        ds.Connection.Open();
                    }
                    if (this._report.IsSimpleParams)
                    {
                        ds.Refresh(rep_params);
                    }
                    else
                    {
                        ds.Refresh();
                    }
                    sql.builder.Controls.ucMainReports.CopyParsToResult(this.GetUIForm(), ds);
                    Logger.ReportFinish(kod_log);
                    return ds;
                }
                catch (ThreadAbortException)
                {
                    // 
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.ReportError(kod_log, ex.Message, ex.StackTrace);
                    throw;
                }
            }
        }
        private string PrintData(VDataSet ds, XmlNode template)
        {
            if (template == null)
            {
                template = this.GetTemplateInfo();
            }
            var data = new XDocument();
            data.Add(new XElement(EName.root, new XElement(ds.Scheme)));
            if (template.Attributes["print-proc"] == null)
            { //  dataSet  xml       excel
                Parser.SaveReportDataToXml(data.Root, ds);
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data.ToString());
            string path = null;
            if (template != null && template.Attributes["print-xlsx"] != null && template.Attributes["print-xlsx"].Value == "1")
            {
                string template_path = Path.Combine(Printing.templatesFolder, "excel", template.Attributes["name"].Value);
                string output_path = Printing.GetFreeName(Printing.outputFolder, template.Attributes["title"].Value, "xlsx");
                ExcelPrintOptions options = new ExcelPrintOptions(XElement.Parse(template.OuterXml));
                options.UseDataReader = (this._report.P_UseDataReader == TextConst.AVBool.True);
                if (options.DeleteUnusedColumns)
                {
                    options.UsedVariables = ds.Tables.Cast<DataTable>().SelectMany(ExcelUtils.GetVariablesNames).ToArray();
                }
                options.NeedConvert = (Path.GetExtension(template_path) != ".xlsx");
                options.NeedPostProcess = (template.Attributes[TextConst.AName.PostProcess] == null || template.Attributes["post-process"].Value != "0");
                if (template.Attributes["output-format"] != null)
                {
                    options.OutputFormat = (ExcelPrintOptions.FileFormat)Enum.Parse(typeof(ExcelPrintOptions.FileFormat), template.Attributes["output-format"].Value, true);
                }
                if (template.Attributes[TextConst.AName.FormatSource] != null)
                {
                    options.FormatSource = Path.Combine(Printing.templatesFolder, "excel", template.Attributes[TextConst.AName.FormatSource].Value);
                }
                options.CopyTemplate = XmlReports.IsDeveloperMode();
                ExcelPrintDocument.ExcelPrintErrors err = ExcelPrintDocument.PrintNew(template_path, output_path, ds, options);
                //if (err == ExcelPrintDocument.ExcelPrintErrors.NoData)
                //{
                //    if (ShowMessages)
                //    {
                //        XtraMessageBox.Show("      ");
                //    }
                //    output_path = string.Empty;
                //}
                path = output_path;
                if (template.Attributes["output-format"] != null)
                {
                    if (ExcelPrintDocument.LastPrintedFilePath != null)
                    {
                        path = ExcelPrintDocument.LastPrintedFilePath;
                        ExcelPrintDocument.LastPrintedFilePath = null;
                    }
                    else
                    {
                        path = Path.ChangeExtension(output_path, template.Attributes["output-format"].Value);
                    }
                }
            }
            else
            {
                path = Printing.Print(xmlDoc, ds, template, show_messages: ShowMessages);
            }
            return path;
        }
        //private void OpenFile(string path)
        //{
        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        if (XtraMessageBox.Show(" " + path + "?", " ", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            Process.Start(path);
        //        }
        //    }
        //}
        private void OnEndedReport(CleanExpressReportEventArgs e)
        {
            if (this.ReportOpening != null)
            {
                this.ReportOpening(this, e);
            }
        }
        private string TryCustomPrint(VDataSet ds, string param = null)
        {
            if (this.CustomPrint != null)
            {
                var ee = new CleanExpressReportEventArgs();
                ee.ResultData = ds;
                ee.Folder = "C:\\Temp";
                //ee.Folder = this.GetWorkFolder();
                ee.ParamsData = this.GetParamsData();
                ee.Param = param;
                this.CustomPrint(this, ee);
                return ee.Path;
            }
            else
            {
                return null;
            }
        }
        private void BeginForming()
        {
            this.in_process = true;
            //this.TaskBarAssistent.ProgressMode = TaskbarButtonProgressMode.Indeterminate;
            WaitUIHelper.LastUsedUIHelper.Show("  ", WaitUIMode.WaitPanel, true);
        }
        private void EndForming()
        {
            WaitUIHelper.LastUsedUIHelper.Hide();
            //this.TaskBarAssistent.ProgressMode = TaskbarButtonProgressMode.NoProgress;
            this.in_process = false;
        }
        internal string ExecuteReport(XmlNode templateInfo)
        {
            if (this.ValidateParams() != string.Empty)
            {
                return string.Empty;
            }
            //  ,    ExpressReport
            if (templateInfo == null)
            {
                this.ShowMessages = false;
            }
            this.BeginForming();
            string path = null;
            try
            {

                VDataSet ds = this.RefreshData();
                if (!DevAnalyzer.PrepareOnly)
                {
                    path = this.TryCustomPrint(ds);
                    if (path == null)
                    {
                        path = this.PrintData(ds, templateInfo);
                    }
                }
                var args = new CleanExpressReportEventArgs();
                args.Path = path;
                args.ParamsData = GetParamsData();
                this.OnEndedReport(args);
                //if (this.OpenDocumentAfterPrint)
                //{
                //    this.OpenFile(path);
                //}
            }
            finally
            {
                this.EndForming();
            }
            return path;
        }
        internal DataSet GetExecuteReportResult()
        {
            this.ShowMessages = false;
            this.BeginForming();
            try
            {
                return (DataSet)this.RefreshData();
            }
            finally
            {
                this.EndForming();
            }
        }
        private void ExecuteReportAsync(XmlNode templateInfo)
        {
            if (this.in_process || this.ValidateParams() != string.Empty)
            {
                return;
            }
            this.BeginForming();
            TaskScheduler context = TaskScheduler.FromCurrentSynchronizationContext();
            var scheduler = new CleanCustomTaskScheduler();
            this.cts = new CancellationTokenSource();
            Task.Factory.StartNew<string>(() =>
            {
                VDataSet ds = null;
                try
                {
                    using (cts.Token.Register(Thread.CurrentThread.Abort))
                    {
                        ds = RefreshData(false);

                        var path = TryCustomPrint(ds);

                        if (path == null)
                        {
                            path = PrintData(ds, templateInfo);
                        }

                        return path;
                    }
                }
                finally
                {
                    //   connection     RefreshData
                    if (ds != null && ds.Connection != null && ds.Connection != XmlReports.Environment.Connection)
                    {
                        ds.Connection.Close();
                    }
                }

            }, cts.Token, TaskCreationOptions.None, scheduler)
            .ContinueWith((t) =>
            {
                if (t.Exception == null)
                {
                    var args = new CleanExpressReportEventArgs
                    {
                        Path = t.Result,
                        ParamsData = GetParamsData()
                    };
                    OnEndedReport(args);
                }
                EndForming();
                if (t.Exception == null && OpenDocumentAfterPrint)
                {
                    //OpenFile(t.Result);
                }
                //if (autoClose)
                //{
                //    this.Close();
                //}
            }, cts.Token, TaskContinuationOptions.None, context);
        }
        private XElement GetReportScheme(UIFormC sender)
        {
            if (this._dataSet == null)
            {
                this._dataSet = this._report.Result(2, true);
            }
            return this._report.Scheme;
        }
        private VReport GetReport(UIFormC sender)
        {
            return this._report;
        }
        protected void Dispose(bool disposing)
        {
            //SaveStateToRegistry();
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //base.Dispose(disposing);
        }
        #endregion
        #region  
        private void start()
        {
            XmlNode templateInfo = this.GetTemplateInfo();
            //    (2    )
            if (this.changeReportName != null && this._report.P_IdName != this.changeReportName)
            {
                this._report = XmlReports.Environment.GetPrecompiledReport(this.changeReportName);
            }
            if (this.changeReportName != null)
            {
                var xmlreport = new XmlDocument();
                xmlreport.LoadXml(_report.ToString());
                templateInfo = xmlreport.FirstChild.SelectSingleNode("print-templates//template");
            }
            this.ExecuteReportAsync(templateInfo);
        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.start();
        }
        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}
        #endregion
        private void UIFormC_SpecialTypeChanged(UIFormC sender, string special_type, object data)
        {
            switch (special_type)
            {
                case TextConst.AVSpecType.ColSets:
                    if (this._dataSet != null)
                    {
                        IEnumerable<string> visible_colset_names = (IEnumerable<string>)data;
                        XmlReports.SetColsetsVisible(this._dataSet.Scheme, this._dataSet.SchemePreset, visible_colset_names);
                    }
                    return;
                case TextConst.AVSpecType.SelectRep:
                    this.ChangeReport(data.ToString());
                    return;
            }
        }
        #region WorkFolder
        private const string WORK_FOLDER_PROMPT = " ,     ";
        private bool _folder_button_pressed;
        #endregion
        private string changeReportName;
        private void ChangeReport(string newReportName)
        {
            this.changeReportName = newReportName;
        }
        private void SaveDefaultParams()
        {
            XElement xroot = new XElement(EName.root);
            Parser.SaveReportParamsToXml(xroot, this._uIForm);
            db.MergeDefaultReportSetting(this._report_name, xroot.ToString());
        }
        internal string ValidateParams()
        {
            if (!this._isWithBehavior)
            {
                return string.Empty;
            }
            return this._uIForm.GetValidation() ?? string.Empty;
        }
    }
    internal sealed class CleanCustomTaskScheduler : TaskScheduler
    {
        #region Fields

        private SynchronizationContext synchronizationContext;
        private ConcurrentQueue<Task> taskQueue = new ConcurrentQueue<Task>();

        #endregion

        #region Constructors

        public CleanCustomTaskScheduler()
            : this(SynchronizationContext.Current)
        {
        }

        public CleanCustomTaskScheduler(SynchronizationContext synchronizationContext)
        {
            this.synchronizationContext = synchronizationContext;
        }

        #endregion

        #region Base class overrides

        protected override void QueueTask(Task task)
        {
            // Add a continuation to the task that will only execute if faulted and then post the exception back to the synchronization context
            task.ContinueWith(antecedent =>
            {
                this.synchronizationContext.Post(sendState =>
                {
                    var ex = (Exception)sendState;
                    if (ex.InnerException is ThreadAbortException) return;
                    throw ex;
                },
                antecedent.Exception);
            },
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            // Enqueue this task
            this.taskQueue.Enqueue(task);

            // Make sure we're processing all queued tasks
            this.EnsureTasksAreBeingExecuted();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // Excercise for the reader
            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return this.taskQueue.ToArray();
        }

        #endregion

        #region Helper methods

        private void EnsureTasksAreBeingExecuted()
        {
            // Check if there's actually any tasks left at this point as it may have already been picked up by a previously executing thread pool thread (avoids queueing something up to the thread pool that will do nothing)
            if (this.taskQueue.Count > 0)
            {
                ThreadPool.UnsafeQueueUserWorkItem(_ =>
                {
                    Task nextTask;

                    // This thread pool thread will be used to drain the queue for as long as there are tasks in it
                    while (this.taskQueue.TryDequeue(out nextTask))
                    {
                        base.TryExecuteTask(nextTask);
                    }
                },
                null);
            }
        }

        #endregion
    }
    public class CleanExpressReportEventArgs : EventArgs
    {
        private string path;
        private string folder;
        private DataSet result_data;
        private DataSet params_data;
        private string param;
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }
        public string Folder
        {
            get
            {
                return this.folder;
            }
            set
            {
                this.folder = value;
            }
        }
        public DataSet ResultData
        {
            get
            {
                return this.result_data;
            }
            set
            {
                this.result_data = value;
            }
        }
        public DataSet ParamsData
        {
            get
            {
                return this.params_data;
            }
            set
            {
                this.params_data = value;
            }
        }
        /// <summary>
        ///    DoCustomPrint
        /// </summary>
        public string Param
        {
            get
            {
                return this.param;
            }
            set
            {
                this.param = value;
            }
        }
    }
}