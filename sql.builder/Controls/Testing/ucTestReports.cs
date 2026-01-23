//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
////using System.Windows.Forms;
//using System.Xml;
//using System.Xml.Linq;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid.Views.Grid;

//using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Data;
//using infoenergo.core.Extensions;
//using infoenergo.sys;
//using Microsoft.WindowsAPICodePack.Dialogs;
//using sql.builder.DataApi;
//using sql.builder.Print.Xlsx;
//using sql.builder.Test;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using infoenergo.framework.Extensions.Oracle;

//namespace sql.builder.Controls.Testing
//{
//    internal partial class ucTestReports : ucBase, IWCFServer
//    {
//        bool _inProcess;
//        DataTable _reportsTable;
//        DataTable _logTable;
//        string _lastTestFolder;
//        CancellationTokenSource _token;
//        string _compareFolderPath;
//        TreeListNode _currentNode;

//        Process _clientProcess;
//        ClientState _clientState;
//        ServerCommand _clientCommand;
//        string _lastClientMessage;
//        bool _compare;
//        bool _compilation_only;
//        StreamWriter _logFileStream;
//        int _errCount;
//        bool _isFullLog = true;
//        bool _spTemp;
//        int _progressFactor;

//        public ucTestReports()
//        {
//            InitializeComponent();
//        }

//        public void Start()
//        {
//            if (XmlReports.TestCompare != null) GetRibbonSource<ucTestReports>().ceCompareEnable.EditValue = XmlReports.TestCompare.Value;
//            if (XmlReports.TestFile != null) LoadScript(XmlReports.TestFile);
//            StartTesting();
//        }

//        private new void Initialize()
//        {
//            var env = XmlReports.Environment;

//            _compareFolderPath = SettingsHelper.CompareFolder;
//            GetRibbonSource<ucTestReports>().beCompareFolderPath.EditValue = string.IsNullOrEmpty(_compareFolderPath)
//                ? _compareFolderPath
//                : Cmn.CutString(_compareFolderPath, 40);

//            // загружаем информацию об отчетах
//            _reportsTable = SqlBuilder.GetReportsDataTable(XmlReports.GetInputFolderNames());
//            if (Cmn.IsAdministrator()) GetRibbonSource<ucTestReports>().ceCompareEnable.EditValue = true;

//            var rows = _reportsTable.AsEnumerable()
//                .Where(r => r["item_type"].Equals("useform") || r["item_type"].Equals("setting"))
//                .ToArray();

//            foreach (var r in rows) _reportsTable.Rows.Remove(r);

//            _reportsTable.Columns.AddRange(new[]
//            {
//                new DataColumn("params", typeof(UIFormC)), 
//                new DataColumn("params_changed", typeof(bool)) { DefaultValue = Cmn.BOOLEAN_FALSE }, 
//                new DataColumn("vreport", typeof(VReport)), 
//                new DataColumn("vds", typeof(VDataSet)), 
//                new DataColumn("time_new", typeof(string)), 
//                new DataColumn("time_old", typeof(string)), 
//                new DataColumn("result_new", typeof(int)) { DefaultValue = Cmn.INT32_ZERO }, 
//                new DataColumn("result_old", typeof(int)) { DefaultValue = Cmn.INT32_ZERO }, 
//                new DataColumn("compare_file", typeof(string)),
//                new DataColumn("params_default", typeof(XElement))
//            });


//            ucReports.TreeControl.AfterFocusNode += TreeControl_AfterFocusNode;
//            ucReports.TreeControl.BeforeCheckNode += TreeControl_BeforeCheckNode;
//            ucReports.TreeControl.CustomDrawNodeCell += TreeControl_CustomDrawNodeCell;
//            ucReports.TreeControl.GetNodeDisplayValue += TreeControl_GetNodeDisplayValue;
//            ucReports.TreeControl.DataSource = _reportsTable;
//            ucReports.TreeControl.ForceInitialize();
//            ucReports.TreeControl.ExpandAll();
//            ucReports.TreeControl.FocusedNode = ucReports.TreeControl.Nodes.FirstNode;

//            _logTable = new DataTable();
//            _logTable.Columns.Add("status", typeof(int));
//            _logTable.Columns.Add("time", typeof(DateTime));
//            _logTable.Columns.Add("text", typeof(string));
//            _logTable.Columns.Add("name", typeof(string));
//            _logTable.Columns.Add("title", typeof(string));

//            grLog.DataSource = _logTable;

//            foreach (var sb in grLog.Controls.OfType<DevExpress.XtraEditors.VScrollBar>()) {
//                sb.ValueChanged += OnGridVerticalScroll;
//            }

//            bool visible = SettingsHelper.ShowTestParamsPanel;
//            GetRibbonSource<ucTestReports>().ceParamsVisible.EditValue = visible;
//            GetRibbonSource<ucTestReports>().ucReports.ShowParamsForm = visible;
//        }

//        private void OnGridVerticalScroll(object sender, EventArgs eventArgs)
//        {
//            GetRibbonSource<ucTestReports>().ceAutoScrollLog.EditValue = false;
//        }
//        private static bool IsUseReport(TreeListNode node)
//        {
//            return node.Field<string>("item_type") == "usereport";
//        }
//        private void StartTesting()
//        {
//            if (_inProcess) {
//                return;
//            }
//            IEnumerable<TreeListNode> nodes = ucReports.TreeControl.GetAllCheckedNodes().Where(IsUseReport);
//            if (nodes.Any()) {
//                _token = new CancellationTokenSource();
//                Task.Factory.StartNew(TestingAsync, nodes, _token.Token);
//            }
//        }
//        private void StartComparing()
//        {
//            if (_inProcess || _lastTestFolder == null) {
//                return;
//            }
//            IEnumerable<TreeListNode> nodes = ucReports.TreeControl.GetAllCheckedNodes().Where(IsUseReport);
//            if (nodes.Any()) {
//                _token = new CancellationTokenSource();
//                Task.Factory.StartNew(ComparingAsync, nodes, _token.Token);
//            }
//        }
//        private void TestingAsync(object nodes)
//        {
//            int nodes_count = ((IEnumerable<TreeListNode>)nodes).Count();
//            bool success = TryStartTestAsync(nodes_count);
//            if (!success) return;

//            if (_compare)
//            {
//                success = TryStartCompareAppAsync();
//                if (!success) return;
//            }

//            int cnt = -1;
//            foreach (var checkedNode in (IEnumerable<TreeListNode>)nodes)
//            {
//                if (_token.Token.IsCancellationRequested) break;

//                cnt++;

//                InvokeIfNeed(() =>
//                {
//                    lStatus.Caption = "Идет процесс тестирования...";
//                    pbProgress.EditValue = cnt * _progressFactor;
//                });

//                _currentNode = checkedNode;
//                ucReports.TreeControl.Invalidate();

//                success = TryLoadReportAsync();
//                if (!success) continue;

//                InvokeIfNeed(() => pbProgress.EditValue = cnt * _progressFactor + 1);

//                if(_compilation_only) continue;

//                success = TryExecuteReportAsync();
//                if (!success) continue;

//                InvokeIfNeed(() => pbProgress.EditValue = cnt * _progressFactor + 1);

//                // 2 печать excel файлов

//                XElement[] xtemplates = GetReportByNode(_currentNode).Elements(EName.print_templates).Elements(EName.excel).Elements(EName.template).ToArray();
//                if (xtemplates.Length != 0) {
//                    foreach (XElement xtemplate in xtemplates) {
//                        success = TryPrintByTemplateAsync(xtemplate);
//                        if (!success) break;
//                    }

//                    if (!success) continue;
//                }
//                else
//                {
//                    success = TryExportToExcelAsync();
//                    if (!success) continue;
//                }

//                InvokeIfNeed(() =>
//                {
//                    pbProgress.EditValue = cnt * _progressFactor + 2;
//                    _currentNode["result_new"] = Cmn.INT32_ONE; // = (int)TestResult.Success;
//                });

//                // 3 сравнение excel файлов

//                if (_compare)
//                {
//                    success = TryWaitFilesForCompareAsync();
//                    if (!success) continue;

//                    if (_token.Token.IsCancellationRequested) continue;

//                    InvokeIfNeed(() =>
//                    {
//                        pbProgress.EditValue = cnt * _progressFactor + 3;
//                        lStatus.Caption = "Сравнение Excel-файлов...";
//                    });
//                    if (xtemplates.Length != 0) {
//                        foreach (var xtemplate in xtemplates)
//                        {
//                            success = TryCompareExcelFilesAsync(xtemplate.Attribute(AName.title).Value);
//                            if (!success) break;
//                        }

//                        if (!success) continue;
//                    }
//                    else
//                    {
//                        success = TryCompareExcelFilesAsync(GetReportByNode(_currentNode).P_SelfTitle);
//                        if (!success) continue;
//                    }

//                    InvokeIfNeed(() => pbProgress.EditValue = cnt * _progressFactor + 4);
//                }

//                InvokeIfNeed(() =>
//                {
//                    AppendAnyText("Тестирование отчёта прошло успешно", report_name: (string)_currentNode["name"], report_title: (string)_currentNode["title"]);
//                });
//            }

//            InvokeIfNeed(() => pbProgress.EditValue = ++cnt * _progressFactor);

//            CompleteTestingAsync();
//        }

//        private bool TryStartTestAsync(int reports_count, bool new_session = true)
//        {
//            _spTemp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;


//            _errCount = 0;

//            InvokeIfNeed(() =>
//            {
//                if (new_session) _logTable.Rows.Clear();

//                var rib = GetRibbonSource<ucTestReports>();

//                var text = "Инициализация...";
//                lStatus.Caption = text;
//                AppendAnyText(text);

//                // число шагов прогресса при проверке одного отчёта
//                _progressFactor = 3;
//                _compilation_only = rib.ceCompilationOnly.EditValue.Equals(true);
//                if (_compilation_only) _progressFactor = 1;
//                else
//                {
//                    _compare = rib.ceCompareEnable.EditValue.Equals(true);
//                    if (_compare) _progressFactor = 5;
//                }

//                rpbProgress.Maximum = reports_count * _progressFactor;
//                pbProgress.EditValue = 0;
//            });

//            _inProcess = true;

//            Logger.BeginSession();
//            Logger.LogEvent += Log;

//            if(new_session)
//            {
//                _lastTestFolder = null;

//                // генерируем имя папки для файлов и создаём её на диске
//                _lastTestFolder = Path.Combine(SettingsHelper.WorkFolder,
//                    string.Format("{0}_{1}", XmlReports.TestName ?? db.Connection.GetAlias().ToLower(), DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss")));

//                Directory.CreateDirectory(_lastTestFolder);
//            }

//            InvokeIfNeed(() =>
//            {
//                OnBaseEvent(ucBaseEventType.ExecuteStart);
//                UpdateUI();
//            });

//            var fs = new FileStream(Path.Combine(_lastTestFolder, "log.txt"), FileMode.Append);
//            _logFileStream = new StreamWriter(fs, Encoding.Unicode);

//            return true;
//        }
//        private bool TryStartCompareAppAsync()
//        {
//            int attempts_max = 3;
//            int attempt = 0;

//            for (attempt = 1; attempt <= attempts_max; attempt++)
//            {
//                _clientState = ClientState.Unknown;

//                InvokeIfNeed(() =>
//                {
//                    var text = string.Format("Запуск приложения для сравнения ({0})...", attempt);
//                    lStatus.Caption = text;
//                    AppendAnyText(text);
//                });

//                WCFHelper.StartServer(this);

//                _clientCommand = ServerCommand.Wait;

//                try
//                {
//                    _clientProcess = new Process();
//                    _clientProcess.StartInfo.FileName = Path.Combine(_compareFolderPath, "sql.builder.exe");
//                    _clientProcess.StartInfo.Arguments = string.Format("-client -sn=\"{0}\"", WCFHelper.ServiceName);
//                    _clientProcess.Start();
//                }
//                catch (Exception ex)
//                {
//                    InvokeIfNeed(() => AppendAnyText("Не удалось запустить приложение для сравнения\r\n" + ex.Message, success: false));
//                    continue;
//                }


//                // ждем когда запустится клиент
//                while (_clientState != ClientState.Ready && _clientState != ClientState.Fault && !_token.Token.IsCancellationRequested)
//                {
//                    //
//                }

//                InvokeIfNeed(() => ParentForm.Activate());

//                if (_clientState == ClientState.Fault)
//                {
//                    InvokeIfNeed(() => AppendAnyText("Не удалось запустить приложение для сравнения\r\n" + _lastClientMessage, success: false));
//                    continue;
//                }

//                break;
//            }

//            // не смогли запустить
//            if (_clientState != ClientState.Ready || _token.Token.IsCancellationRequested)
//            {
//                if (!_token.Token.IsCancellationRequested) _token.Cancel();
//                CompleteTestingAsync();
//                return false;
//            }

//            InvokeIfNeed(() => AppendAnyText("Приложение для сравнения запущено"));

//            return true;
//        }
//        private bool TryLoadReportAsync()
//        {
//            string report_title = _currentNode.Field<string>("title");
//            string report_name = _currentNode.Field<string>("name");
//            InvokeIfNeed(() =>
//            {
//                _currentNode["result_new"] = Cmn.INT32_ZERO; // = (int)TestResult.None;
//                _currentNode["result_old"] = Cmn.INT32_ZERO; // = (int)TestResult.None;
//                _currentNode["time_new"] = DBNull.Value;
//                _currentNode["time_old"] = DBNull.Value;
//            });

//            if (_compare) _clientCommand = ServerCommand.ExecuteReport;

//            // 1 формирование отчёта

//            InvokeIfNeed(() => AppendAnyText("Компиляция отчёта...", report_name: report_name, report_title: report_title));

//            VReport vreport = null;
//            try
//            {
//                InvokeIfNeed(() => vreport = GetReportByNode(_currentNode));
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: report_name, report_title: report_title);
//                });

//                return false;
//            }

//            InvokeIfNeed(() =>
//            {
//                AppendAnyText("Отчёт успешно скомпилирован", report_name: report_name, report_title: report_title);
//                AppendAnyText("Загрузка формы с параметрами...", report_name: report_name, report_title: report_title);
//            });

//            UIFormC form = null;
//            try
//            {
//                InvokeIfNeed(() => form = GetFormByNode(_currentNode));
//                if(form.WithBehavior)
//                {
//                    foreach (var uilist in form.controls.Values.OfType<UIList>())
//                    {
//                        InvokeIfNeed(() =>
//                        {
//                            AppendAnyText(string.Format("Заполнение списка \"{0}\"...", uilist.Caption), report_name: report_name, report_title: report_title);
//                            uilist.PrepareListSource();
//                            AppendAnyText(string.Format("Список \"{0}\" успешно заполнен", uilist.Caption), report_name: report_name, report_title: report_title);
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: report_name, report_title: report_title);
//                });

//                return false;
//            }

//            InvokeIfNeed(() =>
//            {
//                AppendAnyText("Форма с параметрами загружена", report_name: report_name, report_title: report_title);
//            });

//            return true;
//        }
//        private bool TryExecuteReportAsync()
//        {
//            string report_title = _currentNode.Field<string>("title");
//            string report_name = _currentNode.Field<string>("name");

//            VDataSet vds = _currentNode["vds"] as VDataSet;
//            VReport vreport = _currentNode["vreport"] as VReport;
//            UIFormC form = _currentNode["params"] as UIFormC;

//            try
//            {
//                InvokeIfNeed(() =>
//                {
//                    var text = "Начало формирования отчёта...";
//                    var text2 = Cmn.GetAvgReportFormingTime(report_name);
//                    if (text2 != null) text = text + " (ожидаемое время ~" + text2 + ")";
//                    AppendAnyText(text, report_name: report_name, report_title: report_title);
//                });

//                var xparams = form.GetValue();

//                InvokeIfNeed(() => AppendAnyText("Выполнение запроса...", report_name: report_name, report_title: report_title));
//                var timer = new Stopwatch();
//                timer.Start();
//                Compiler.ResetAfterError();
//                vds = vreport.Result(xparams, vreport.P_UseRepository != "" ? 1 : 2, null, true, vds != null ? vds.SchemePreset : null);

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["vds"] = vds;
//                });


//                if (vreport.IsSimpleParams)
//                {
//                    vds.Refresh(xparams, vreport.P_UseRepository != "" ? 1 : 2);
//                }
//                else
//                {
//                    vds.Refresh(vreport.P_UseRepository != "" ? 1 : 2);
//                }

//                timer.Stop();
//                TimeSpan ts = timer.Elapsed;
//                string forming_time = String.Format("{0}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["time_new"] = forming_time;
//                    AppendAnyText("Запрос выполнен успешно", report_name: report_name, report_title: report_title);
//                });
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: report_name, report_title: report_title);
//                });

//                return false;
//            }

//            return true;
//        }
//        private bool TryPrintByTemplateAsync(XElement xtemplate)
//        {
//            string report_name = _currentNode.Field<string>("name");
//            string report_title = _currentNode.Field<string>("title");

//            string path = Path.Combine(_lastTestFolder, report_name);
//            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

//            string temp = Printing.outputFolder;
//            Printing.outputFolder = path;

//            var vds = GetVDataSetByNode(_currentNode);

//            try
//            {
//                InvokeIfNeed(() => AppendAnyText(string.Format("Формирование печатной формы \"{0}\"...", xtemplate.Attribute(AName.title).Value), report_name: report_name, report_title: report_title));

//                string template_path = Path.Combine(Printing.templatesFolder, "excel", xtemplate.Attribute(AName.name).Value);

//                if (xtemplate.AttrOrDef("print-xlsx", null) == "1") {
//                    var options = new ExcelPrintOptions(xtemplate);
//                    options.UseDataReader = (GetReportByNode(_currentNode).P_UseDataReader == TextConst.AVBool.True);
//                    options.UsedVariables = vds.Tables.Cast<DataTable>().SelectMany(ExcelUtils.GetVariablesNames).ToArray();
//                    options.NeedConvert = (Path.GetExtension(template_path) != ".xlsx");
//                    options.NeedPostProcess = xtemplate.AttrOrDefault(AName.post_process, true);
//                    string output_path = Printing.GetFreeName(path, xtemplate.Attribute(AName.title).Value, "xlsx");
//                    ExcelPrintDocument.PrintNew(template_path, output_path, vds, options);
//                } else {
//                    var data = new XDocument();
//                    data.Add(new XElement(EName.root, new XElement(vds.Scheme)));
//                    if (xtemplate.AttrOrDefault("print-proc", null) == null) {
//                        Parser.SaveReportDataToXml(data.Root, vds);
//                    }
//                    XmlDocument xmlDoc = new XmlDocument();
//                    xmlDoc.LoadXml(data.ToString());
//                    var template = new XmlDocument();
//                    template.LoadXml(xtemplate.ToString());
//                    Printing.printExcel(xmlDoc, vds, template_path, xtemplate.Attribute(AName.title).Value, template.FirstChild, null, false);
//                }
//                InvokeIfNeed(() => AppendAnyText(string.Format("Печатная форма \"{0}\" успешно сформирована", xtemplate.Attribute(AName.title).Value), report_name: report_name, report_title: report_title));
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: _currentNode.Field<string>("name"), report_title: _currentNode.Field<string>("title"));
//                });

//                return false;
//            }
//            finally
//            {
//                Printing.outputFolder = temp;
//            }

//            return true;
//        }
//        private bool TryExportToExcelAsync()
//        {
//            var vds = GetVDataSetByNode(_currentNode);
//            var vreport = GetReportByNode(_currentNode);
//            string report_name = _currentNode.Field<string>("name");
//            string report_title = _currentNode.Field<string>("title");

//            string path = Path.Combine(_lastTestFolder, report_name);
//            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

//            try
//            {
//                InvokeIfNeed(() => AppendAnyText("Экспорт в Excel...", report_name: report_name, report_title: report_title));
//                string file_path = Printing.GetFreeName(path, GetReportByNode(_currentNode).P_SelfTitle, "xlsx");

//                var grid = new ucTableViewerContainer(ControlMode.Report);
//                grid.Initialize(XmlReports.GetReportInfo(report_name), false);
//                grid.BeginUpdate();
//                var xreport = new XDocument(new XElement("root", new XElement(vds.Scheme)));
//                grid.LoadSchemeSettingsFromXml(xreport.Root,null);
//                grid.EndUpdate();
//                grid.DataSource = vds;

//                if (vreport.P_DxExport == "1")
//                {
//                    DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
//                    grid.ExportToXlsx(file_path, vreport.P_SelfTitle);
//                }
//                else
//                {
//                    //var view = grid.CurrentView;
//                    GridView view = null;
//                    if (grid.GetGridControl()!=null)
//                    {
//                        view = (grid.GetGridControl() as sql.builder.Controls.Grids.ReportViewModes.ucGridWF).GetMainView();
//                    }
//                    // отключаем колонки, по которым нет данных для печати
//                    var existed = vds.Tables[0].Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
//                    var columns = view.Columns.Where(c => c.Visible && !existed.Contains(c.FieldName)).ToArray();
//                    foreach (var c in columns) c.Visible = false;

//                    Printing.Print(view, vreport.P_SelfTitle, file_path);

//                    foreach (var c in columns) c.Visible = true;
//                }

//                InvokeIfNeed(() => AppendAnyText("Экспорт в Excel прошёл успешно", report_name: report_name, report_title: report_title));
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: report_name, report_title: report_title);
//                });

//                return false;
//            }

//            return true;
//        }
//        private bool TryWaitFilesForCompareAsync()
//        {
//            InvokeIfNeed(() => lStatus.Caption = "Ожидание файлов для сравнения...");

//            _clientCommand = ServerCommand.PrintExcel;

//            while (_clientState != ClientState.PrintingExcel && !_token.Token.IsCancellationRequested)
//            {
//                //
//            }

//            while (_clientState != ClientState.Ready && _clientState != ClientState.Fault && !_token.Token.IsCancellationRequested)
//            {
//                //
//            }

//            if (_clientState == ClientState.Fault)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_old"] = Cmn.INT32_THREE; // = (int)TestResult.ExecuteError;
//                    AppendAnyText("Ошибка в приложении для сравнения:\r\n" + _lastClientMessage, success: false, report_name: (string)_currentNode["name"], report_title: (string)_currentNode["title"]);
//                });

//                return false;
//            }
//            else
//            {
//                InvokeIfNeed(() => { _currentNode["result_old"] = Cmn.INT32_ONE; }); // = (int)TestResult.Success);
//            }

//            return true;
//        }
//        private bool TryCompareExcelFilesAsync(string template_title)
//        {
//            string report_name = _currentNode.Field<string>("name");
//            string report_title = _currentNode.Field<string>("title");

//            InvokeIfNeed(() => AppendAnyText(string.Format("Сравнение Excel-файлов \"{0}\"...", template_title), report_name: report_name, report_title: report_title));

//            try
//            {
//                var files = Directory.EnumerateFiles(Path.Combine(_lastTestFolder, report_name))
//                .OrderByDescending(f => f)
//                .ToArray();

//                if (files.Length == 2)
//                {
//                    string output_path = Path.Combine(_lastTestFolder, report_name, report_title + "_res.xlsx");
//                    bool result = Printing.CompareExcelFiles(files[0], files[1], output_path);

//                    InvokeIfNeed(() =>
//                    {
//                        _currentNode["compare_file"] = output_path;
//                        if (_currentNode == ucReports.TreeControl.FocusedNode) UpdateUI();
//                    });

//                    if (result)
//                    {
//                        InvokeIfNeed(() => AppendAnyText("Файлы совпадают", report_name: report_name, report_title: report_title));
//                    }
//                    else
//                    {
//                        _errCount++;

//                        InvokeIfNeed(() =>
//                        {
//                            _currentNode["result_new"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                            _currentNode["result_old"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                            AppendAnyText("Файлы отличаются", success: false, report_name: report_name, report_title: report_title);
//                        });

//                        return false;
//                    }

//                    // удаляем все файлы кроме результирующего
//                    //files.ForEach(File.Delete);
//                }
//                else
//                {
//                    _errCount++;

//                    InvokeIfNeed(() =>
//                    {
//                        _currentNode["result_new"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                        _currentNode["result_old"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                        AppendAnyText("Неверное количество файлов для сравнения - ожидается 2, найдено - " + files.Length, success: false, report_name: report_name, report_title: report_title);
//                    });

//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                _errCount++;

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                    _currentNode["result_old"] = Cmn.INT32_TWO; // = (int)TestResult.CompareFault;
//                    AppendAnyText(ex.Message + "\r\n" + ex.StackTrace, success: false, report_name: report_name, report_title: report_title);
//                });

//                return false;
//            }

//            return true;
//        }
//        private void CompleteTestingAsync(bool new_session = true)
//        {
//            _inProcess = false;

//            _currentNode = null;

//            Logger.EndSession();

//            string text = null;

//            // пока завязался на признак new_session, но вообще это не дело
//            if(new_session) text = (_token.IsCancellationRequested) ? "Тестирование прервано" : "Тестирование завершено";
//            else text = (_token.IsCancellationRequested) ? "Сравнение прервано" : "Сравнение завершено";

//            text += string.Format(" (ошибок {0})", _errCount);
//            InvokeIfNeed(() => AppendAnyText(text));

//            if (_logFileStream != null)
//            {
//                _logFileStream.Close();
//                _logFileStream = null;
//            }

//            if (_compare && new_session)
//            {
//                _clientCommand = ServerCommand.CloseApplication;

//                // не всегда срабатывает
//                //while (_clientCommand != ServerCommand.Wait && _clientCommand != ServerCommand.CloseApplication && !_token.Token.IsCancellationRequested)
//                //{
//                //    //
//                //}


//                try
//                {
//                    if (_clientProcess != null && !_clientProcess.HasExited) _clientProcess.Kill();
//                }
//                catch (InvalidOperationException)
//                {
//                    //
//                }

//                _clientProcess = null;

//                WCFHelper.ShutdownServer();
//            }

//            InvokeIfNeed(() =>
//            {
//                lStatus.Caption = text;

//                OnBaseEvent(ucBaseEventType.ExecuteComplete);
//                UpdateUI();
//            });

//            SqlBuilder.ShowPopupWaitForms = _spTemp;
//        }

//        private void ComparingAsync(object nodes)
//        {
//            int nodes_count = ((IEnumerable<TreeListNode>)nodes).Count();
//            bool success = TryStartTestAsync(nodes_count, false);
//            if (!success) return;

//            int cnt = -1;
//            foreach (var checkedNode in (IEnumerable<TreeListNode>)nodes)
//            {
//                _currentNode = checkedNode;

//                if (_token.Token.IsCancellationRequested) break;

//                cnt++;

//                InvokeIfNeed(() =>
//                {
//                    lStatus.Caption = "Идет процесс сравнения...";
//                    pbProgress.EditValue = cnt * _progressFactor;
//                });

//                XElement[] xtemplates = GetReportByNode(_currentNode).Elements(EName.print_templates).Elements(EName.excel).Elements(EName.template).ToArray();
//                if (xtemplates.Length != 0) {
//                    foreach (XElement xtemplate in xtemplates) {
//                        success = TryCompareExcelFilesAsync(xtemplate.Attribute(AName.title).Value);
//                        if (!success) break;
//                    }

//                    if (!success) continue;
//                }
//                else
//                {
//                    success = TryCompareExcelFilesAsync(GetReportByNode(_currentNode).P_SelfTitle);
//                    if (!success) continue;
//                }

//                InvokeIfNeed(() =>
//                {
//                    _currentNode["result_new"] = Cmn.INT32_ONE; // = (int)TestResult.Success;
//                    _currentNode["result_old"] = Cmn.INT32_ONE; // = (int)TestResult.Success;
//                });
//            }

//            InvokeIfNeed(() => pbProgress.EditValue = ++cnt * _progressFactor);

//            CompleteTestingAsync(false);
//        }

//        private VReport GetReportByNode(TreeListNode node)
//        {
//            var vreport = node["vreport"] as VReport;
//            if (vreport != null) return vreport;

//            vreport = XmlReports.Environment.GetPrecompiledReport(node.Field<string>("name"), node.Field<string>("project"));

//            node["vreport"] = vreport;
//            return vreport;
//        }

//        private VDataSet GetVDataSetByNode(TreeListNode node)
//        {
//            var vds = node["vds"] as VDataSet;
//            if (vds != null) return vds;

//            var vreport = GetReportByNode(node);
//            vds = vreport.Result(vreport.P_UseRepository != "" ? 1 : 2, true);
//            vds.SchemePreset = new VXElement(vreport.Scheme);

//            node["vds"] = vds;
//            return vds;
//        }

//        private UIFormC GetFormByNode(TreeListNode node)
//        {
//            var form = node["params"] as UIFormC;
//            if (form != null) return form;

//            var vreport = GetReportByNode(node);

//            form = GetUIFormC(vreport.P_Form, node.Field<string>("name"), node);

//            return form;
//        }
//        private void ClearSession()
//        {
//            // чтобы не дергались события
//            ucReports.TreeControl.DataSource = null;
//            foreach (DataRow row in _reportsTable.Rows) {
//                row["params"] = DBNull.Value;
//                row["params_changed"] = Cmn.BOOLEAN_FALSE;
//                row["result_new"] = Cmn.INT32_ZERO; // = (int)TestResult.None;
//                row["result_old"] = Cmn.INT32_ZERO; // = (int)TestResult.None;
//                row["time_new"] = DBNull.Value;
//                row["time_old"] = DBNull.Value;
//                row["compare_file"] = DBNull.Value;
//                row["params_default"] = DBNull.Value;
//            }
//            ucReports.TreeControl.DataSource = _reportsTable;
//            ucReports.TreeControl.ForceInitialize();
//            ucReports.TreeControl.ExpandAll();
//            _logTable.Rows.Clear();
//            lStatus.Caption = string.Empty;
//            pbProgress.EditValue = 0;
//            _clientProcess = null;
//            _lastTestFolder = null;
//            CurrentNodeChanged();
//        }
//        private void OpenTestFolder()
//        {
//            if (_lastTestFolder != null) {
//                Process.Start(_lastTestFolder);
//            }
//        }
//        private void OpenCompareFile()
//        {
//            Process.Start(ucReports.TreeControl.FocusedNode.Field<string>("compare_file"));
//        }
//        private void SelectCompareFolder()
//        {
//            //using (var dlg = new FolderBrowserDialog())
//            //{
//            //    dlg.SelectedPath = _compareFolderPath;
//            //    if (dlg.ShowDialog() == DialogResult.OK)
//            //    {
//            //        _compareFolderPath = dlg.SelectedPath;
//            //        SettingsHelper.CompareFolder = _compareFolderPath;
//            //        ribSrcControl<ucTestReports>().beCompareFolderPath.EditValue = Cmn.CutString(_compareFolderPath, 40);
//            //    }
//            //}
//            // говорят на WIN XP не работает, но у нас же продвинутые заказчики
//            using (var dlg = new CommonOpenFileDialog()) {
//                dlg.IsFolderPicker = true;
//                dlg.InitialDirectory = _compareFolderPath;
//                if (dlg.ShowDialog() == CommonFileDialogResult.Ok) {
//                    _compareFolderPath = dlg.FileName;
//                    SettingsHelper.CompareFolder = _compareFolderPath;
//                    GetRibbonSource<ucTestReports>().beCompareFolderPath.EditValue = Cmn.CutString(_compareFolderPath, 40);
//                }
//            }
//        }
//        private void UpdateUI()
//        {
//            ucReports.EnableParamsForm = !_inProcess;
//            var rib = GetRibbonSource<ucTestReports>();
//            rib.btnStart.Enabled = !_inProcess;
//            rib.btnStop.Enabled = _inProcess;
//            rib.btnClearSession.Enabled = !_inProcess;
//            rib.btnCheckAllReport.Enabled = !_inProcess;
//            rib.btnCheckReportsXlsx.Enabled = !_inProcess;
//            rib.btnUncheckAllReports.Enabled = !_inProcess;
//            rib.ceCompilationOnly.Enabled = !_inProcess;
//            rib.btnOpenTestFolder.Enabled = (_lastTestFolder != null);
//            rib.ceCompareEnable.Enabled = !_inProcess;
//            rib.beCompareFolderPath.Enabled = !_inProcess;
//            rib.btnOpenCompareFile.Enabled = (ucReports.TreeControl.FocusedNode != null && ucReports.TreeControl.FocusedNode["compare_file"] != DBNull.Value);
//            rib.btnSaveScript.Enabled = !_inProcess;
//            rib.btnLoadScript.Enabled = !_inProcess;
//            rib.btnScriptingTest.Enabled = !_inProcess;
//            rib.btnStartComparing.Enabled = !_inProcess && (_lastTestFolder != null);
//            rib.btnShowFullLog.Enabled = !_isFullLog;
//        }
//        private UIFormC GetUIFormC(string form_name, string report_name, TreeListNode node)
//        {
//            UIFormC form = null;
//            var xform = XmlReports.GetForm(form_name, report_name);
//            var isWithBehavior = (Cmn.GetAttrValue(xform, TextConst.AName.WithBehavior) != TextConst.AVBool.False);
//            if (isWithBehavior) {
//                // Емцов - добавил параметр-делегат, т.к. падали формы с colsets
//                form = UIStatic.CreateForm(xform.Attribute(AName.name).Value, null, false, false, false, UIFormC_NeedReportScheme);
//                node["params"] = form;
//                if (form.Init) UIStatic.UpdateForm(form, null, false);

//            } else {
//                form = new UIFormC();
//                node["params"] = form;
//                form.NeedReportScheme += UIFormC_NeedReportScheme;
//            }
//            form.NeedReport += UIFormC_NeedReport;
//            form.SpecialTypeChanged += UIFormC_SpecialTypeChanged;
//            if (!isWithBehavior) {
//                form.Initialize(xform);
//            }
//            //form.DataSource.Changed += OnDataSourceOnChanged;
//            var xparams = Cmn.LoadDefaultReportParams(report_name);
//            if (xparams != null) {
//                form.SetDefaultParams(xparams);
//            }
//            node["params"] = form;
//            form.RefreshData();
//            // чтобы отследить, что параметры менялись
//            form.AnyValueChanged += UIForm_EditValueChanged;
//            object params_default = node["params_default"];
//            if (!Convert.IsDBNull(params_default)) {
//                Parser.LoadReportParamsFromXml(params_default as XElement, form);
//            }
//            return form;
//        }
//        #region Loger
//        private void Log(Tuple<DateTime, string> data)
//        {
//            InvokeIfNeed(() =>
//            {
//                var row = _logTable.AsEnumerable().FirstOrDefault();
//                string report_name = (row != null && row["name"] != DBNull.Value) ? (string)row["name"] : null;
//                string report_title = (row != null && row["title"] != DBNull.Value) ? (string)row["title"] : null;

//                AppendAnyText(data.Item2, data.Item1, report_name: report_name, report_title: report_title);
//            });
//        }

//        private void AppendAnyText(string text, DateTime? time = null, bool success = true, string report_name = null, string report_title = null)
//        {
//            time = time ?? DateTime.Now;

//            var row = _logTable.NewRow();
//            row["status"] = (success) ? 0 : 1;
//            row["time"] = time;
//            row["text"] = text;
//            row["name"] = report_name;
//            row["title"] = report_title;
//            _logTable.Rows.InsertAt(row, 0);

//            if ((bool)GetRibbonSource<ucTestReports>().ceAutoScrollLog.EditValue) viewLog.TopRowIndex = 0;

//            ToLogFile(text, time, success, report_title);
//        }
//        #endregion

//        private void InvokeIfNeed(MethodInvoker action)
//        {
//            if (IsDisposed) return;

//            if (InvokeRequired) Invoke(action);
//            else action();
//        }

//        private void ucTestReports_Load(object sender, EventArgs e)
//        {
//            Initialize();
//        }
//        private void TreeControl_GetNodeDisplayValue(object sender, DevExpress.XtraTreeList.GetNodeDisplayValueEventArgs e)
//        {
//            if (e.Node is TreeListAutoFilterNode) {
//                return;
//            }
//            if (e.Column.Name == "colReport" && XmlReports.IsDeveloperMode()) {
//                e.Value = e.Value + " (" + e.Node["name"] + ")";
//            } else if (e.Column.Name.StartsWith("colResult")) {
//                if (e.Value.Equals(Cmn.INT32_ZERO)) {
//                    e.Value = DBNull.Value;
//                }
//            }
//        }
//        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().StartTesting();
//        }
//        private void TreeControl_AfterFocusNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
//        {
//            CurrentNodeChanged();
//        }
//        private void CurrentNodeChanged()
//        {
//            //if (_inProcess) return;
//            TreeListNode node = ucReports.TreeControl.FocusedNode; 
//            Cursor.Current = Cursors.WaitCursor;
//            try {
//                if (node == null || node is TreeListAutoFilterNode || node.Field<string>("item_type") == "folder") {
//                    ucReports.ShowParams(null);
//                    ShowFullLog();
//                } else {
//                    var form = node["params"] as UIFormC;
//                    if (form == null) {
//                        form = GetFormByNode(node);
//                    }
//                    ucReports.ShowParams(form);
//                    ShowReportInLog(node.Field<string>("name"), node.Field<string>("title"));
//                }
//            } finally {
//                Cursor.Current = Cursors.Default;
//            }
//            UpdateUI();
//        }
//        private void viewLog_ShowFilterPopupListBox(object sender, FilterPopupListBoxEventArgs e)
//        {
//            if (e.Column.FieldName == "status") {
//                e.ComboBox.DrawItem += ComboBox_DrawItem;
//            }
//        }
//        private void ComboBox_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
//        {
//            var item = e.Item as FilterItem;
//            if (item == null || item.Value is FilterItem) {
//                return;
//            }
//            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
//                e.Appearance.BackColor = Cmn.GetHighlightColor();
//            }
//            e.Appearance.FillRectangle(e.Cache, e.Bounds);
//            foreach (ImageComboBoxItem comboItem in riiStatus.Items) {
//                if (comboItem.Value.Equals(item.Value)) {
//                    e.Graphics.DrawImage(ic2.Images[comboItem.ImageIndex], e.Bounds.Location);
//                    break;
//                }
//            }
//            e.Handled = true;
//        }
//        private void btnStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetRibbonSource<ucTestReports>().btnStop.Enabled = false;
//            GetCurrentControl<ucTestReports>()._token.Cancel();
//        }
//        private void btnClearSession_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().ClearSession();
//        }
//        private static Color GetColorByTestResult(TestResult tr)
//        {
//            switch (tr) {
//                case TestResult.Success:
//                    return Color.FromArgb(150, 225, 150);
//                case TestResult.CompareFault:
//                    return Color.FromArgb(255, 200, 100);
//                case TestResult.ExecuteError:
//                    return Color.FromArgb(225, 150, 150);
//                default:
//                    return Color.LightGray;
//            }
//        }
//        private enum TestResult
//        {
//            None = 0,
//            Success = 1,
//            CompareFault = 2,
//            ExecuteError = 3
//        }
//        private void TreeControl_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
//        {
//            if (e.Node is TreeListAutoFilterNode) {
//                return;
//            }
//            if (e.Column.Name == "colResultNew") {
//                TestResult result = (TestResult)e.Node.Field<int>("result_new");
//                if (result == TestResult.None) {
//                    return;
//                }
//                e.Appearance.BackColor = GetColorByTestResult(result);
//            } else if (e.Column.Name == "colResultOld") {
//                TestResult result = (TestResult)e.Node.Field<int>("result_old");
//                if (result == TestResult.None) {
//                    return;
//                }
//                e.Appearance.BackColor = GetColorByTestResult(result);
//            } else if (e.Column.Name == "colReport") {
//                if (e.Node == _currentNode) {
//                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
//                }
//            }
//        }
//        private void btnCheckAllReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().ucReports.TreeControl.CheckAll();
//        }
//        private void btnUncheckAllReports_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().ucReports.TreeControl.UncheckAll();
//        }
//        private void btnOpenTestFolder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().OpenTestFolder();
//        }
//        private void rbeCompareFolderPath_ButtonPressed(object sender, ButtonPressedEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().SelectCompareFolder();
//        }
//        #region IWCFServer
//        void IWCFServer.SendMessage(string text)
//        {
//            _lastClientMessage = text;
//        }
//        XElement IWCFServer.GetReportParams()
//        {
//            var xroot = new XElement(EName.root);
//            Parser.SaveReportParamsToXml(xroot, _currentNode["params"] as UIFormC);
//            return xroot;
//        }
//        string IWCFServer.GetReportName()
//        {
//            return _currentNode.Field<string>("name");
//        }
//        string IWCFServer.GetWorkFolder()
//        {
//            return _lastTestFolder;
//        }
//        void IWCFServer.SetClientState(ClientState state)
//        {
//            _clientState = state;
//        }
//        void IWCFServer.SetClientData(VDataSet data)
//        {
//            //
//        }
//        void IWCFServer.SetReportTime(string time)
//        {
//            InvokeIfNeed(() => _currentNode["time_old"] = time);
//        }
//        ServerCommand IWCFServer.GetCommand()
//        {
//            var cmd = _clientCommand;
//            _clientCommand = ServerCommand.Wait;
//            return cmd;
//        }
//        #endregion
//        private void UIFormC_SpecialTypeChanged(UIFormC sender, string special_type, object data)
//        {
//            TreeListNode node = FindNode("params", sender);

//            switch (special_type)
//            {
//                case "colsets":
//                    var vreport = GetReportByNode(node);

//                    var visible_colset_names = (IEnumerable<string>)data;
//                    XmlReports.SetColsetsVisible(vreport.Scheme, GetVDataSetByNode(node).SchemePreset, visible_colset_names);
//                    break;
//            }
//        }
//        private void UIForm_EditValueChanged(object sender, EventArgs e)
//        {
//            TreeListNode node = FindNode("params", sender);
//            node["params_changed"] = Cmn.BOOLEAN_TRUE;
//        }
//        private XElement UIFormC_NeedReportScheme(UIFormC sender)
//        {
//            TreeListNode node = FindNode("params", sender);
//            return GetVDataSetByNode(node).Scheme;
//        }
//        private VReport UIFormC_NeedReport(UIFormC sender)
//        {
//            TreeListNode node = FindNode("params", sender);

//            var vreport = GetReportByNode(node);
//            return vreport;
//        }
//        private void btnOpenCompareFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().OpenCompareFile();
//        }

//        private void ToLogFile(string text, DateTime? time = null, bool success = true, string report_title = null)
//        {
//            if (_logFileStream == null) return;

//            string error_mark = "[ошибка]";

//            time = time ?? DateTime.Now;

//            _logFileStream.WriteLine("{0} {1}{2}{3}",
//                time.Value.ToString("HH:mm:ss"),
//                (success) ? "" : error_mark,
//                (report_title != null) ? "[" + report_title + "]" : "",
//                text);
//        }

//        private void btnSaveScript_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            string file_name = ChooseSaveFilePath(db.Connection.GetAlias().ToLower());
//            if (file_name == null) return;

//            GetCurrentControl<ucTestReports>().SaveScript(file_name);
//        }
//        private void btnLoadScript_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            string file_name = ChooseLoadFilePath();
//            if (file_name == null) return;
//            //_lastTestFileName = Path.GetFileNameWithoutExtension(file_name);
//            var xml = XElement.Load(file_name);
//            if (xml == null || xml.Element(EName.reports) == null) {
//                ShowMessage.ShowError("Не удалось загрузить настройки из выбранного файла");
//                return;
//            }
//            WaitUIHelper.LastUsedUIHelper.Show("Загрузка настроек тестирования...", WaitUIMode.WaitPanel);
//            //Wait.Show("Загрузка настроек тестирования...");
//            GetCurrentControl<ucTestReports>().LoadScript(xml);
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            //Wait.Hide();
//        }
//        private void SaveScript(string file_name)
//        {
//            XElement xroot = new XElement(EName.root);
//            XElement xconnection = new XElement("connection", HelperCrypt.Encrypt(DataHelper.GetConnectionString(db.Connection)));
//            xroot.Add(xconnection);
//            XElement xsettings = new XElement("settings");
//            xroot.Add(xsettings);
//            //int compare = ((bool)ribSrcControl<ucTestReports>().ceCompareEnable.EditValue) ? 1 : 0;
//            //xsettings.Add(new XAttribute("compare", compare));
//            XElement xreports = new XElement(EName.reports);
//            xroot.Add(xreports);
//            var nodes = Cmn.GetAllTreeNodes(ucReports.TreeControl).Where(n => n.Checked && n.Field<string>("item_type") != "folder");
//            foreach (TreeListNode node in nodes) {
//                XElement xreport = new XElement(EName.report);
//                xreport.Add(new XAttribute(AName.name, node.Field<string>("name")));
//                xreport.Add(new XAttribute(AName.title, node.Field<string>("title")));
//                xreports.Add(xreport);
//                object pars = node["params"];
//                if ((!Convert.IsDBNull(pars)) && node.Field<bool>("params_changed")) {
//                    var xtemp = new XElement("temp");
//                    Parser.SaveReportParamsToXml(xtemp, pars as UIFormC);
//                    xreport.Add(xtemp.Element(EName.@params));
//                }
//            }
//            xroot.Save(file_name);
//        }
//        internal void LoadScript(XElement xml)
//        {
//            ClearSession();
//            //var xsettings = xml.Element("settings");
//            //bool compare = (xsettings.Attribute("compare").Value == "1");
//            //ribSrcControl<ucTestReports>().ceCompareEnable.EditValue = compare;
//            ucReports.TreeControl.UncheckAll();
//            ucReports.TreeControl.BeginUpdate();
//            foreach (XElement xreport in xml.Element(EName.reports).Elements(EName.report)) {
//                TreeListNode node = ucReports.TreeControl.FindNodeByKeyID(xreport.Attribute(AName.name).Value);
//                if (node != null) {
//                    ucReports.TreeControl.SetNodeCheckState(node, CheckState.Checked, true);
//                    var xparams = xreport.Element(EName.@params);
//                    if (xparams != null) {
//                        node["params_default"] = xreport;
//                        //Parser.LoadReportParamsFromXml(xreport, GetFormByNode(node));
//                    }
//                }
//            }
//            ucReports.TreeControl.EndUpdate();
//        }
//        private string ChooseSaveFilePath(string default_name)
//        {
//            using (var dlg = new SaveFileDialog()) {
//                dlg.InitialDirectory = SettingsHelper.SaveLoadTestSettingsFolder;
//                dlg.Filter = "(*.xml)|*.xml|(*.*)|*.*";
//                dlg.DefaultExt = "xml";
//                dlg.FileName = default_name;

//                if (dlg.ShowDialog() != DialogResult.OK) return null;

//                SettingsHelper.SaveLoadTestSettingsFolder = Path.GetDirectoryName(dlg.FileName);

//                return dlg.FileName;
//            }
//        }

//        private string ChooseLoadFilePath()
//        {
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = SettingsHelper.SaveLoadTestSettingsFolder;
//                dlg.Filter = "(*.xml)|*.xml|(*.*)|*.*";
//                dlg.DefaultExt = "xml";

//                if (dlg.ShowDialog() != DialogResult.OK) return null;

//                SettingsHelper.SaveLoadTestSettingsFolder = Path.GetDirectoryName(dlg.FileName);

//                return dlg.FileName;
//            }
//        }
//        private void rceCompareEnable_EditValueChanging(object sender, ChangingEventArgs e)
//        {
//            if (!Cmn.IsAdministrator() && (bool)e.NewValue) {
//                e.Cancel = true;
//                ShowMessage.ShowExclamation("Для сравнения с другой версией запустите приложение с правами администратора");
//            }
//        }
//        private TreeListNode FindNode(string field_name, object value)
//        {
//            return (ucReports.TreeControl.FocusedNode[field_name].Equals(value))
//                                 ? ucReports.TreeControl.FocusedNode
//                                 : Cmn.GetAllTreeNodes(ucReports.TreeControl).FirstOrDefault(n => n[field_name] == value);
//        }
//        private void TreeControl_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
//        {
//            e.CanCheck = !_inProcess;
//        }
//        private void ShowReportInLog(string report_name, string report_title)
//        {
//            gcLog.Text = string.Format("Лог ({0})", report_title);
//            viewLog.ActiveFilterString = string.Format("[NAME] = \'{0}\'", report_name);
//            _isFullLog = false;
//        }
//        private void ShowFullLog()
//        {
//            if (_isFullLog) return;
//            gcLog.Text = "Лог";
//            viewLog.ActiveFilterString = "";
//            _isFullLog = true;
//        }
//        private void CheckReportsXlsx()
//        {
//            foreach (TreeListNode node in Cmn.GetAllTreeNodes(ucReports.TreeControl)) {
//                if (node.Field<string>("item_type") != "folder") {
//                    var vreport = GetReportByNode(node);
//                    if (vreport.Descendants(EName.template).Attributes("print-xlsx").Any(a => a.Value == "1")) {
//                        node.Checked = true;
//                    }
//                }
//            }
//        }
//        private void CheckVisibleReports()
//        {
//            foreach (TreeListNode node in Cmn.GetAllTreeNodes(ucReports.TreeControl)) {
//                if (node.Field<string>("item_type") != "folder" && node.Field<bool>("visible")) {
//                    node.Checked = true;
//                }
//            }
//        }
//        private void btnShowFullLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucTestReports>();
//            ctrl.ShowFullLog();
//            ctrl.UpdateUI();
//        }
//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (_clientProcess != null) {
//                _clientProcess.Kill();
//                _clientProcess = null;
//            }
//            if (_logFileStream != null) {
//                _logFileStream.Close();
//                _logFileStream = null;
//            }
//            if (disposing && (components != null)) {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//        private void btnScriptingTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            new frmScriptingTest().Show();
//        }
//        private void btnStartComparing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().StartComparing();
//        }
//        private void btnCheckReportsXlsx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().CheckReportsXlsx();
//        }
//        private void btnCheckVisibleReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucTestReports>().CheckVisibleReports();
//        }
//        private void ceParamsVisible_EditValueChanged(object sender, EventArgs e)
//        {
//            var val = (bool)((BarEditItemLink)((BarEditItem)sender).Links[0]).EditValue;
//            SettingsHelper.ShowTestParamsPanel = val;
//            GetCurrentControl<ucTestReports>().ucReports.ShowParamsForm = val;
//        }
//    }
//}