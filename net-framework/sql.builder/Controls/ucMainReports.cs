using System;
using System.Collections.Generic;
using System.Diagnostics;
using Contract = System.Diagnostics.Contracts.Contract;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
////using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.LookAndFeel;
//using DevExpress.Skins;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraBars.Docking2010.Views.Tabbed;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using GridView = DevExpress.XtraGrid.Views.Grid.GridView;
//using GridColumn = DevExpress.XtraGrid.Columns.GridColumn;
//using TreeListNode = DevExpress.XtraTreeList.Nodes.TreeListNode;
//using DevExpress.XtraWaitForm;
using infoenergo.sys;
using sql.builder.Controls.Containers;
//using sql.builder.Controls.Grids;
using sql.builder.DataApi;
using sql.builder.ExcelApi;
using sql.builder.Print.Xlsx;
//using sql.builder.Test;
//using sql.builder.TFS;
//using sql.builder.TFS.AutoCheckIn;
//using sql.builder.TFS.AutoCheckIn.Commands;
using sql.builder.UI;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
using DataTable = System.Data.DataTable;
//using sql.builder.WebReports;

namespace sql.builder.Controls
{
    internal partial class ucMainReports //: ucBase
    {
//        #region Переменные
//        //private const string _layout_version = "1.0";
//        private readonly Dictionary<string, ucReportContainer> _grid_containers;
//        private readonly Dictionary<string, string> _open_titles;
//        private readonly Dictionary<string, ucFormContainer> _form_containers;

//        private ucBaseContainer _current_container;
//        internal ucReportContainer CurrentGC { get { return _current_container as ucReportContainer; } }
//        internal ucFormContainer CurrentFC { get { return _current_container as ucFormContainer; } }

//        private bool _report_changing;
//        private bool _show_invisible_reports;

//        ucReportsTree cReportsTree;

//        //private string _default_layout_name = "Пользовательский";
//        //private string _current_layout_name;

//        BarButtonItem btnInvestProHelp;
//        bool events_attached;
//        #endregion
//        #region События
//        //
//        #endregion
//        #region Открытые методы
//        public static string MainPanelName = "Работа с отчётами";
//        public ucMainReports()
//        {
//            InitializeComponent();

//            this.rpReports.Text = MainPanelName;
//            if (sql.builder.UI.UIStatic.IsMpep)
//            {
//                rpgDataEditor.Visible = true;
//            }
//            Multiple = true;
//            //dockManager.LayoutVersion = _layout_version;
//            _grid_containers = new Dictionary<string, ucReportContainer>();
//            _open_titles = new Dictionary<string, string>();
//            _form_containers = new Dictionary<string, ucFormContainer>();
//        }

//        public override void Initialize()
//        {
//            cbDevexpressSkins.EditValue = SettingsHelper.DevExpressSkinName;

//            string[] folders = XmlReports.GetInputFolderNames();
//            string report = XmlReports.GetInputReportName();
//            _show_invisible_reports = SettingsHelper.ShowInvisibleReports;

//            // db.CurrentConnection = connection.CurrentConnection;

//            //dmReports.ForceInitialize();
//            dockManager.ForceInitialize();
//            dockManager.BeginUpdate();
//            //ribbon.ForceInitialize();
//            var rib = GetRibbonSource<ucMainReports>();

//            if (report != "")
//            {
//                ShowContainer(report);
//                //(CurrentGC.Parent as DockPanel).Options.ShowCloseButton = false;
//            }
//            else
//            {
//                cReportsTree = new ucReportsTree();
//                cReportsTree.Dock = DockStyle.Fill;
//                cReportsTree.CurrentNodeChanged += ucReportTree_OnCurrentNodeChanged;
//                cReportsTree.LoadGridSettings += ucReportTree_OnLoadGridSettings;
//                cReportsTree.NeedSettingData += ucReportTree_OnNeedSettingData;
//                cReportsTree.ReportNodeDeleted += ucReportTree_OnReportNodeDeleted;
//                cReportsTree.ReportNeedUpdate += ucReportTree_OnReportNeedUpdate;
//                cReportsTree.Initialize(folders);
//                cReportsTree.ReloadAllWithoutData(true);
//#if DEBUG
//                if (XmlReports.IsDeveloperMode())
//                {
//                    cReportsTree.SetInvisibleReportsVisible(this._show_invisible_reports);
//                }
//#endif
//                DockPanel panel = dockManager.AddPanel(DockingStyle.Left);
//                //panel.BackColor = Color.Transparent;
//                panel.Width = 400;
//                panel.Text = "Навигатор";
//                panel.Controls.Add(cReportsTree);
//            }

//            //if (XmlReports.customerId == "17") // уберу, вроде не востребовано, не совместимо с другими проектами ЛЭ
//            //{
//            //    btnInvestProHelp = rib.InvestProInstructionsButton(folders.FirstOrDefault());
//            //}

//            dockManager.EndUpdate();

//            if (WCFHelper.IsClient)
//            {
//                Task.Factory.StartNew(WCFClientProcess);
//            }
//        }

//        private void WCFClientProcess()
//        {
//            WCFHelper.ServerData.SendMessage("Клиент запущен");
//            string folder = WCFHelper.ServerData.GetWorkFolder();

//            string repname = null;

//            while (true)
//            {
//                try
//                {
//                    WCFHelper.ServerData.SetClientState(ClientState.Ready);

//                    var cmd = WCFHelper.ServerData.GetCommand();
//                    if (cmd == ServerCommand.Wait)
//                    {
//                        Thread.Sleep(300);
//                    }
//                    else if (cmd == ServerCommand.ExecuteReport)
//                    {
//                        InvokeIfNeed(() =>
//                        {
//                            if (repname != null)
//                            {
//                                CloseReport(repname);
//                            }
//                            repname = WCFHelper.ServerData.GetReportName();
//                            XElement xusereport = XmlReports.GetUseReport(repname);
//                            string project;
//                            if (xusereport != null)
//                            {
//                                project = xusereport.Attribute(AName.project).Value;
//                            }
//                            else
//                            {
//                                project = "asuse2";
//                            }
//                            XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//                            OpenReport(repname, null, false);
//                            Parser.LoadReportParamsFromXml(WCFHelper.ServerData.GetReportParams(), CurrentGC.ParamFormC);
//                            WCFHelper.ServerData.SendMessage("Начало формирования отчёта на клиенте...");
//                            WCFHelper.ServerData.SetClientState(ClientState.ExecutingReport);
//                            if (CurrentGC.Mode == ucReportContainer.GridMode.NoGrid)
//                            {
//                                CurrentGC.ChangeGridMode(ucReportContainer.GridMode.ReportGrid);
//                            }
//                            ExecuteReport();
//                            WCFHelper.ServerData.SendMessage("Отчёт на клиенте успешно сформирован");
//                            WCFHelper.ServerData.SetReportTime(CurrentGC.Grid.GetFormingTime());
//                            WCFHelper.ServerData.SetData(CurrentGC.Grid.DataSource);
//                        });
//                    }
//                    else if (cmd == ServerCommand.PrintExcel)
//                    {
//                        Printing.outputFolder = Path.Combine(folder, repname);

//                        InvokeIfNeed(() =>
//                        {
//                            try
//                            {
//                                WCFHelper.ServerData.SendMessage("Начало экспорта в Excel на клиенте...");
//                                WCFHelper.ServerData.SetClientState(ClientState.PrintingExcel);

//                                if (CurrentGC.GetReport(null).PrintTemplates().Any())
//                                {
//                                    PrintForms(null, false, print_first: false);
//                                }
//                                else
//                                {
//                                    ExportToXls(false);
//                                }
//                            }
//                            catch (Exception e)
//                            {
//                                File.WriteAllText(@"D:\XML2017\log.txt", e.ToString());
//                            }

//                        });
//                    }
//                    else if (cmd == ServerCommand.CloseApplication)
//                    {
//                        InvokeIfNeed(Application.Exit);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    WCFHelper.ServerData.SendMessage(ex.Message + "\r\n" + ex.StackTrace);
//                    WCFHelper.ServerData.SetClientState(ClientState.Fault);
//                    Thread.Sleep(500);
//                    //InvokeIfNeed(Application.Exit);
//                }
//            }
//        }
//        protected override void AfterChangeRibbon()
//        {
//            ucMainReports rib = this.GetRibbonSource<ucMainReports>();
//            rib.rcbDevexpressSkins.Items.Clear();
//            SkinContainerCollection skins = SkinManager.Default.Skins;
//            for (int index = 0; index < skins.Count; index++)
//            {
//                rib.rcbDevexpressSkins.Items.Add(skins[index].SkinName);
//            }
//            rib.cbDevexpressSkins.EditValue = SettingsHelper.DevExpressSkinName;
//            // this.FillWorkFolder();
//            string path = SqlBuilder.GetWorkFolderPath();
//            rib.btnWorkFolderPath.EditValue = path;
//            Printing.outputFolder = path;
//            //            
//#if DEBUG
//            if (XmlReports.IsDeveloperMode())
//            {
//                rib.rpgDebug.Visible = true;
//                rib.ceShowInvisibleReports.EditValue = this._show_invisible_reports;
//            }
//#endif
//        }
//        public override void UpdateRibbon()
//        {
//            //FillLayouts();
//            //SetLastLayout();





//            RefreshButtonsStates();

//            // добавляем кнопку для вызова инструкций ИнвестПро
//            var rib = GetRibbonSource<ucMainReports>();
//            if (btnInvestProHelp != null)
//            {
//                rib.rpgHelp.Visible = true;
//                rib.rpgHelp.ItemLinks.Add(btnInvestProHelp, true);
//                btnInvestProHelp = null;
//            }

//            if (!events_attached)
//            {
//                rib.cbDevexpressSkins.EditValueChanged += cbDevexpressSkins_EditValueChanged;
//                events_attached = true;
//            }

//            var current_gc = _current_container as ucReportContainer;

//            if (current_gc == null)
//            {
//                return;
//            }
//            if (_current_container != null)
//            {
//                var xreport = XmlReports.GetReport(current_gc.Grid.OriginalName);
//                var xmlreport = new XmlDocument();
//                xmlreport.LoadXml(xreport.ToString());

//                GeneratePrintTemplates(xmlreport.FirstChild.SelectNodes("print-templates//template"));
//            }

//        }
//        protected override string GetDefaultPageText()
//        {
//            return rpReports.Text;
//        }
//        protected override IEnumerable<string> GetIgnorablePageTexts()
//        {
//            if (XmlReports.IsInfoenergo) return new[] { rpSettings.Text };

//            return base.GetIgnorablePageTexts();
//        }
        internal static void CopyParsToResult(UIFormC form, VDataSet result) //20171201 Новое , возможны ошибки
        {
            if (form == null)
            {
                return;
            }
            VDataSet ds = form.DataSource;
            if (ds == null)
            {
                return;
            }
            VDataTable params_table = ds.ParamsTable;
            if (params_table == null)
            {
                return;
            }
            if (result.Tables[TextConst.AVTable.Pars] == null)
            {
                VDataTable pars_table = new VDataTable();
                //Cmn.CopyVTable(params_table, pars_table);
                pars_table.TableName = TextConst.AVTable.Pars;
                DataColumnCollection cols = params_table.Columns;
                int index;
                for (index = 0; index < cols.Count; index++)
                {
                    DataColumn col = cols[index];
                    pars_table.AddColumn(col.ColumnName.ToUpper(), col.DataType, col.Caption);
                }
                DataRow row;
                if (params_table.Rows.Count == 0)
                {
                    row = null;
                }
                else
                {
                    DataRow src_row = params_table.Rows[0];
                    row = pars_table.NewRow();
                    for (index = 0; index < cols.Count; index++)
                    {
                        row[index] = src_row[index];
                    }
                    pars_table.Rows.Add(row);
                }
                foreach (UIBase f in form.controls.Values)
                {
                    string field_name = f.FieldName.ToUpper();
                    IRange range = f as IRange;
                    if (range != null)
                    {
                        VDataColumn col_1 = pars_table.AddColumn(field_name + "1_TEXT", typeof(string));
                        VDataColumn col_2 = pars_table.AddColumn(field_name + "2_TEXT", typeof(string));
                        if (row != null)
                        {
                            string value_1, value_2;
                            range.GetText(out value_1, out value_2);
                            row[col_1] = value_1;
                            row[col_2] = value_2;
                        }
                    }
                    else
                    {
                        VDataColumn col = pars_table.AddColumn(field_name + "_TEXT", typeof(string));
                        if (row != null)
                        {
                            if (f is UIList)
                            {
                                row[col] = (f as UIList).FullText;
                            }
                            else
                            {
                                row[col] = f.GetText();
                            }
                        }
                    }
                }
                result.Tables.Add(pars_table);
            }
        }
//        private VReport GetReport()
//        {
//            ucTableViewerContainer grid = CurrentGC.Grid;
//            if (grid.DataSource != null)
//            {
//                return grid.DataSource.Report;
//            }
//            else
//            {
//                return XmlReports.Environment.GetPrecompiledReport(grid.ReportName);
//            }
//        }
//        private string getReportSql()
//        {
//            string sql = string.Empty;
//            VReport rep = GetReport();
//            XElement rep_params = CurrentGC.ParamFormC.GetValue();
//            XElement rep_params2 = CurrentGC.SaveToXml(SettingsType.Params);
//            VDataSet reportResult = rep.Result(rep_params, CurrentGC.UseRepository, null, true, CurrentGC.Grid.DataSource == null ? null : CurrentGC.Grid.DataSource.schemePreset);
//            reportResult.Refresh(rep_params, ref sql, CurrentGC.UseRepository, true, true);
//            OracleCommand cmd1 = (reportResult.Tables[0] as VDataTable).DataAdapter.SelectCommand;
//            OracleCommand cmd = VDBSelectCommand.CopyCommand(cmd1);
//            cmd.ParameterCheck = true; // чтобы коллекция Parameters заполнилась при установке CommandText
//            cmd.CommandText = sql;
//            string[] ParamNames = Cmn.GetParameterNames(cmd.Parameters);
//            cmd.ParameterCheck = false;
//            cmd.Parameters.Clear();
//            VDataTable.SetCommandParams(reportResult, (reportResult.Tables[0] as VDataTable), cmd, ParamNames);
//            sql = VDBSelectCommand.GetCmdParametrizedText(cmd);
//            return sql;
//        }
//        internal void ExecuteReport(XElement reportScheme = null)
//        {
//            if (CurrentGC == null || CurrentGC.Grid.FromFile || CurrentGC.Grid.IsCompareMode)
//            {
//                return;
//            }
//            OnBaseEvent(ucBaseEventType.ExecuteStart, "Идёт формирование отчёта");
//            //Чтобы прочитались параметры
//            CurrentGC.Grid.GetControl().SetFocus();
//            // Отчёт
//            VReport rep;
//            if (reportScheme == null)
//            {
//                rep = GetReport();
//            }
//            else
//            {
//                rep = XmlReports.Environment.GetPrecompiledReport(reportScheme);// Для детализации отчетов с grsets
//            }
//            //Заплатка для подмены отчета 32274
//            if (CurrentGC.ChangeReportName != null)
//            {
//                var xmlreport = new XmlDocument();
//                xmlreport.LoadXml(rep.ToString());
//                CurrentGC.ChangeTemplateInfo = xmlreport.FirstChild.SelectSingleNode("print-templates//template");
//            }
//            // Параметры  
//            XElement rep_params = CurrentGC.CustomParams
//                          ?? (CurrentGC.ParamForm != null ? CurrentGC.ParamForm.getValue() : null)
//                          ?? (CurrentGC.ParamFormC != null ? CurrentGC.ParamFormC.GetValue() : null);
//            XElement rep_params2 = this.CurrentGC.SaveToXml(SettingsType.Params);
//            ucMainReports ctrl = this.GetRibbonSource<ucMainReports>();
//            if (Cmn.BOOLEAN_TRUE.Equals(ctrl.ceStoreDefaultParams.EditValue))
//            {
//                this.SaveDefaultParams();
//            }
//            VDataSet reportResult;
//            decimal kod_log = Logger.ReportStart(CurrentGC.Grid.ReportName, rep_params2 ?? rep_params);
//            Compiler.isProcessingPivots = false;
//            /* try { */
//            CurrentGC.BeginForming();
//            // Данные
//            reportResult = rep.Result(rep_params, CurrentGC.UseRepository, null, true, CurrentGC.Grid.DataSource == null ? null : CurrentGC.Grid.DataSource.schemePreset);
//            if (CurrentGC.Grid.DataSource != null) reportResult.IsVertica = CurrentGC.Grid.DataSource.IsVertica;
//            //reportResult.Refresh(true);
//            if (Logger.IsAcive)
//            {
//                Logger.Log("Начало выполнения запроса...");
//            }
//            if (rep.IsSimpleParams)
//            {
//                reportResult.Refresh(rep_params, CurrentGC.UseRepository);
//            }
//            else
//            {
//                reportResult.Refresh(CurrentGC.UseRepository);
//                //reportResult.Refresh(false, rep_params, CurrentGC.UseRepository);
//            }
//            UIFormC frm = CurrentGC.GetParentParamsForm();
//            if (frm == null)
//            {
//                frm = CurrentGC.ParamFormC;
//            }
//            CopyParsToResult(frm, reportResult); //20171201 Новое , возможны ошибки
//            CurrentGC.EndForming();
//            //Cmn.CopyTable(
//            Logger.ReportFinish(kod_log);
//            /*}
//            catch (Exception ex)
//            {
//                Logger.ReportError(kod_log, ex.Message, ex.StackTrace);
//                OnBaseEvent(ucBaseEventType.ExecuteComplete);

//                throw;
//            }*/
//            CurrentGC.Grid.BeginUpdate();
//            CurrentGC.Grid.DataSource = reportResult;
//            CurrentGC.UpdateGridSettings(fixed_scheme: false, layout_changed: false);
//            CurrentGC.Grid.EndUpdate();
//            // Сохраняем текущее состояние
//            if (!CurrentGC.Grid.IsTemplate && !_report_changing)
//            {
//                _report_changing = true;
//                if (cReportsTree != null)
//                {
//                    XElement xdoc = new XElement(EName.root, new XElement(reportResult.Scheme));
//                    cReportsTree.SetData(CurrentGC.Grid.ReportName, xdoc);
//                }
//                _report_changing = false;
//            }
//            OnBaseEvent(ucBaseEventType.ExecuteComplete);
//        }
//        private void ExportToXls(bool show_messages = true)
//        {
//            Contract.Assert(this.CurrentGC != null);
//            if (!this.CheckWorkFolder())
//            {
//                return;
//            }
//            if (this.CurrentGC.Grid.DataSource == null || !this.CurrentGC.Grid.DataSource.IsRefreshed || !this.CurrentGC.Actual)
//            {
//                this.ExecuteReport();
//            }
//            string full_path = Printing.GetFreeName(Printing.outputFolder, this.CurrentGC.ContainerTitle, "xlsx");
//            //_current_gc.Grid.exp
//            // ExportType.WYSIWYG
//            bool dx_export = (this.CurrentGC.Grid.DataSource.Report.P_DxExport == "1");
//            if (!dx_export)
//            {
//                var rg = CurrentGC.Grid;// as IReportGrid;
//                dx_export = (rg == null || rg.ViewMode == TableViewMode.Pivot || rg.ViewMode == TableViewMode.Tree);
//            }
//            if (dx_export)
//            {
//                this.CurrentGC.Grid.ExportToXlsx(full_path, this.CurrentGC.ContainerTitle);
//            }
//            //// делаем перенос слов во всех ячейках листа
//            //// что бы небыло диалога "Сохранить изменения ...?"
//            //var exApp = new Application() { DisplayAlerts = false };
//            //var wb = exApp.Workbooks.Open(full_path);
//            //foreach (Worksheet sh in wb.Worksheets)
//            //{
//            //    sh.Rows.WrapText = true;
//            //    sh.Rows.AutoFit();
//            //}
//            //wb.SaveAs(full_path, XlFileFormat.xlWorkbookDefault);
//            //wb.Close();
//            //exApp.Quit();
//            else
//            {
//                this.OnBaseEvent(ucBaseEventType.ExecuteStart, "Формирование файла");
//                GridView view = null;
//                if (this.CurrentGC.Grid.GetGridControl() != null)
//                {
//                    view = (this.CurrentGC.Grid.GetGridControl() as sql.builder.Controls.Grids.ReportViewModes.ucGridWF).GetMainView();
//                }
//                // отключаем колонки, по которым нет данных для печати
//                DataTable dt = this.CurrentGC.Grid.GetDataTable();
//                List<GridColumn> columns = new List<GridColumn>();
//                int index;
//                for (index = 0; index < view.Columns.Count; index++)
//                {
//                    GridColumn col = view.Columns[index];
//                    if (col.Visible && dt.Columns[col.FieldName] == null)
//                    {
//                        columns.Add(col);
//                        col.Visible = false;
//                    }
//                }
//                Printing.Print(view, this.CurrentGC.ContainerTitle, full_path, prepare: false);
//                for (index = 0; index < columns.Count; index++)
//                {
//                    columns[index].Visible = true;
//                }
//                this.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//            }
//            if (show_messages)
//            {
//                Cmn.OpenPrintedFile(full_path);
//            }
//        }
//        private IList<string> PrintForms(XElement reportScheme, bool show_messages = true, bool load_devexpress = false, bool print_first = true, bool force_refresh = false)
//        {
//            List<XmlNode> list = new List<XmlNode>(1);
//            BarLinkContainerItem dbtn = GetRibbonSource<ucMainReports>().dbtnPrintForm;
//            XmlNode template = dbtn.Tag as XmlNode;
//            if (template == null)
//            {
//                foreach (BarItemLink item in dbtn.ItemLinks)
//                {
//                    template = item.Item.Tag as XmlNode;
//                    if (template != null)
//                    {
//                        list.Add(template);
//                        if (print_first)
//                        {
//                            break;
//                        }
//                    }
//                }
//            }
//            else
//            {
//                list.Add(template);
//            }
//            int count = list.Count;
//            string[] output_path = new string[count];
//            for (int index = 0; index < count; index++)
//            {
//                template = list[index];
//                output_path[index] = this.PrintForm(template, reportScheme, null, show_messages, load_devexpress, force_refresh);
//            }
//            return output_path;
//        }
//        private string PrintForm(XmlNode template, XElement reportScheme, XDocument xTemplate, bool show_messages, bool load_devexpress, bool force_refresh)
//        {
//            Contract.Assert(template != null);
//            Contract.Assert(template.Name == TextConst.EName.Template);
//            if (this.CurrentGC == null)
//            {
//                return null;
//            }
//            if (template.AttrOrDefault(TextConst.AName.PrintXlsx, false))
//            {
//                return this.PrintFormNew(template, reportScheme, show_messages, false, load_devexpress, force_refresh);
//            }
//            if (!this.CheckWorkFolder())
//            {
//                return null;
//            }
//            if (force_refresh || CurrentGC.Grid.DataSource == null || !CurrentGC.Grid.DataSource.IsRefreshed || !CurrentGC.Actual)
//            {
//                this.ExecuteReport(reportScheme);
//            }
//            this.OnBaseEvent(ucBase.ucBaseEventType.ExecuteStart, "Формирование файла");
//            if (this.CurrentGC.ChangeTemplateInfo != null)
//            {
//                template = (XmlNode)this.CurrentGC.ChangeTemplateInfo;
//            }
//            bool clientView = template.AttrOrDefault(TextConst.AName.ClientView, false);
//            if (clientView)
//            {
//                this.CurrentGC.Grid.AllowOpenExcel();
//            }
//            XElement data = new XElement(EName.root, new XElement(CurrentGC.Grid.DataSource.Scheme));
//            if (template.Attributes["print-proc"] == null)
//            { // Преобразование dataSet в xml не требуется для нового варианта формирования excel
//                Parser.SaveReportDataToXml(data, CurrentGC.Grid.DataSource);
//            }
//            if (Logger.IsAcive)
//            {
//                Logger.Log(string.Format("Начало формирования файла \"{0}\"...", template.Attributes["title"].Value));
//            }
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.LoadXml(data.ToString());
//#if DEBUG
//            if (XmlReports.IsDeveloperMode())
//            {
//                this.CurrentGC.BeginPrinting();
//            }
//#endif
//            string full_path = Printing.Print(xmlDoc, this.CurrentGC.Grid.DataSource, template, xTemplate, show_messages);
//#if DEBUG
//            if (XmlReports.IsDeveloperMode())
//            {
//                this.CurrentGC.EndPrinting();
//            }
//#endif
//            this.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//            if (load_devexpress)
//            {
//                this.CurrentGC.Grid.ShowExcel(full_path);
//                File.Delete(full_path);
//            }
//            if (show_messages)
//            {
//                Cmn.OpenPrintedFile(full_path);
//            }
//            return full_path;
//        }
//        private string PrintFormNew(XmlNode template, XElement reportScheme, bool show_messages, bool onlyColumns, bool load_devexpress, bool force_refresh)
//        {
//            Contract.Assert(template != null);
//            Contract.Assert(template.Name == TextConst.EName.Template);
//            Contract.Assert(this.CurrentGC != null);
//            if (!this.CheckWorkFolder())
//            {
//                return null;
//            }
//            if (force_refresh || this.CurrentGC.Grid.DataSource == null || !this.CurrentGC.Grid.DataSource.IsRefreshed || !this.CurrentGC.Actual)
//            {
//                this.ExecuteReport(reportScheme);
//            }
//            if (this.CurrentGC.ChangeTemplateInfo != null)
//            {
//                template = (XmlNode)this.CurrentGC.ChangeTemplateInfo;
//            }
//            bool clientView = template.AttrOrDefault(TextConst.AName.ClientView, false);
//            if (clientView)
//            {
//                this.CurrentGC.Grid.AllowOpenExcel();
//            }
//            this.OnBaseEvent(ucBaseEventType.ExecuteStart, "Формирование файла");
//            if (Logger.IsAcive)
//            {
//                Logger.Log("Начало формирования файла...");
//            }
//            string template_path = Path.Combine(Printing.templatesFolder, "excel", template.Attributes[TextConst.AName.Name].Value);
//            string output_path = Printing.GetFreeName(Printing.outputFolder, template.Attributes[TextConst.AName.Title].Value, "xlsx");
//            ExcelPrintOptions options = new ExcelPrintOptions(XElement.Parse(template.OuterXml));
//            options.OnlyColumns = onlyColumns;
//            options.UseDataReader = (CurrentGC.Grid.DataSource.Report.P_UseDataReader == TextConst.AVBool.True);
//            if (template.Attributes[TextConst.AName.FormatSource] != null)
//            {
//                options.FormatSource = Path.Combine(Printing.templatesFolder, "excel", template.Attributes[TextConst.AName.FormatSource].Value);
//            }
//            //if (options.DeleteUnusedColumns) {
//            options.UsedVariables = this.CurrentGC.Grid.DataSource.Tables.Cast<DataTable>().SelectMany(ExcelUtils.GetVariablesNames).ToArray();
//            //}
//            options.NeedConvert = (Path.GetExtension(template_path) != ".xlsx");
//            options.NeedPostProcess = template.AttrOrDefault(TextConst.AName.PostProcess, true);
//            XmlAttribute output_format_attr = template.Attributes["output-format"];
//            ExcelPrintOptions.FileFormat file_format;
//            if (output_format_attr != null && Enum.TryParse<ExcelPrintOptions.FileFormat>(output_format_attr.Value, out file_format))
//            {
//                //options.OutputFormat = (ExcelPrintOptions.FileFormat)Enum.Parse(typeof(ExcelPrintOptions.FileFormat), template.Attributes["output-format"].Value, true);
//                options.OutputFormat = file_format;
//            }
//#if DEBUG
//            options.CopyTemplate = XmlReports.IsDeveloperMode();
//            if (XmlReports.IsDeveloperMode())
//            {
//                this.CurrentGC.BeginPrinting();
//            }
//#endif
//            ExcelPrintDocument.ExcelPrintErrors err = ExcelPrintDocument.PrintNew(template_path, output_path, CurrentGC.Grid.DataSource, options);
//#if DEBUG
//            if (XmlReports.IsDeveloperMode())
//            {
//                this.CurrentGC.EndPrinting();
//            }
//#endif
//            if (err == ExcelPrintDocument.ExcelPrintErrors.NoData)
//            {
//                if (show_messages)
//                {
//                    XtraMessageBox.Show("По заданным условиям нет данных для печати");
//                }
//                output_path = string.Empty;
//            }
//            this.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//            if (output_format_attr != null)
//            {
//                if (ExcelPrintDocument.LastPrintedFilePath != null)
//                {
//                    output_path = ExcelPrintDocument.LastPrintedFilePath;
//                    ExcelPrintDocument.LastPrintedFilePath = null;
//                }
//                else
//                {
//                    output_path = Path.ChangeExtension(output_path, output_format_attr.Value);
//                }
//            }
//            if (load_devexpress)
//            {
//                this.CurrentGC.Grid.ShowExcel(output_path);
//                File.Delete(output_path);
//            }
//            if (show_messages)
//            {
//                Cmn.OpenPrintedFile(output_path);
//            }
//            return output_path;
//        }
//        private void ChooseReportParams()
//        {
//            if (this.CurrentGC == null)
//            {
//                return;
//            }
//            UIFormC form = this.CurrentGC.ParamFormC;
//            if (form == null)
//            {
//                return;
//            }
//            if (form.ChooseReportParams())
//            {
//                this.ucVGridBase_LayoutChanged();
//            }
//        }
//        private void ShowGroupsEditor()
//        {
//            if (this.CurrentGC == null || this.CurrentGC.Mode != ucReportContainer.GridMode.ReportGrid)
//            {
//                return;
//            }
//            this.CurrentGC.ShowGroupsEditor();
//        }
//        private void ShowColumnsEditor()
//        {
//            if (CurrentGC == null || CurrentGC.Mode != ucReportContainer.GridMode.ReportGrid) return;

//            var current_table_name = CurrentGC.Grid.GetTopTableName();

//            var xViewColumns_preset = CurrentGC.Grid.DataSource.SchemePreset
//                    .Elements(EName.table)
//                    .First(tbl => tbl.Attribute(AName.@as).Value == current_table_name)
//                    .Element(EName.viewcolumns);
//            var xViewColumns_old = CurrentGC.Grid.DataSource.Report.Scheme
//                    .Elements(EName.table)
//                    .First(tbl => tbl.Attribute(AName.@as).Value == current_table_name)
//                    .Element(EName.viewcolumns);

//            var textDecode = ucTableViewerContainer.GetTitlesTextDecode(xViewColumns_old.Parent, CurrentGC.ParamFormC.DataSource);

//            using (var frm = new frmColumnsEditorNew())
//            {
//                frm.Initialize(xViewColumns_old, xViewColumns_preset, textDecode);
//                if (frm.ShowDialog() != DialogResult.OK) return;

//                var xViewColumns_new = frm.GetColumnsXml();

//                XmlReports.CorrectScheme(xViewColumns_new, xViewColumns_old);
//                xViewColumns_preset.ReplaceWith(xViewColumns_new);

//                CurrentGC.UpdateGridSettings(fixed_scheme: true);
//            }
//        }
//        private void ShowTestReports(bool start_immediately = false, bool only_opened = true)
//        {
//            new Thread(() =>
//            {
//                var frm = new frmTestReports(this, start_immediately, only_opened);
//                //if(errors != null && errors.Any()) frm.ShowErrors(errors);
//                frm.ShowDialog();
//            }).Start();
//        }
//        private bool SelectWorkFolder()
//        {
//            bool result;
//            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
//            {
//                dlg.Description = "Выберите папку, в которую будут сохраняться отчёты";
//                result = dlg.ShowDialog() == DialogResult.OK;
//                if (result)
//                {
//                    string path = dlg.SelectedPath;
//                    SettingsHelper.WorkFolder = path;
//                    this.GetRibbonSource<ucMainReports>().btnWorkFolderPath.EditValue = path;
//                    Printing.outputFolder = path;
//                }
//            }
//            return result;
//        }
//        //#region layout отключен
//        //private void FillLayouts()
//        //{
//        //    var ctrl = ribSrcControl<ucMainReports>();
//        //    ctrl.rcbLayouts.Items.Clear();
//        //    var layouts_path = Path.Combine(Environment.CurrentDirectory, "sql.builder", "layouts");
//        //    if (Directory.Exists(layouts_path))
//        //    {
//        //        ctrl.rcbLayouts.Items.AddRange(Directory.GetFiles(layouts_path, "*.layout.xml")
//        //                .Select(file => Path.GetFileName(file).Replace(".layout.xml", String.Empty)).ToArray());
//        //    }
//        //    ctrl.rcbLayouts.Items.Add(_default_layout_name);
//        //}
//        //public void SetLastLayout()
//        //{
//        //    var ctrl = ribSrcControl<ucMainReports>();
//        //    if (_current_layout_name == null)
//        //    {
//        //        _current_layout_name = db.SelectDefaultLayoutName();
//        //    }
//        //    ctrl.cbLayouts.EditValue = _current_layout_name ?? ctrl.rcbLayouts.Items[0];
//        //}
//        //public void SaveDefaultLayout()
//        //{
//        //    dmView.Controller.CloseAll();
//        //    byte[] xml;
//        //    using (var stream = new MemoryStream())
//        //    {
//        //        dockManager.SaveLayoutToStream(stream);
//        //        xml = stream.GetBuffer();
//        //    }
//        //    var project_dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
//        //    var layouts_path = Path.Combine(project_dir, "sql.builder", "layouts");
//        //    if (Directory.Exists(layouts_path))
//        //    {
//        //        var default_layout_path = Path.Combine(layouts_path, "Обычный.layout.xml");
//        //        XmlTFSHelper.CheckOutFile(default_layout_path);
//        //        using (var stream = new FileStream(default_layout_path, FileMode.Create))
//        //        {
//        //            stream.Write(xml, 0, xml.Length);
//        //        }
//        //    }
//        //}
//        //public void SaveUserLayout()
//        //{
//        //    if (_current_layout_name == null) return;
//        //    string data = null;
//        //    if (_current_layout_name == _default_layout_name)
//        //    {
//        //        using (var stream = new MemoryStream())
//        //        {
//        //            dockManager.SaveLayoutToStream(stream);
//        //            data = Encoding.UTF8.GetString(stream.GetBuffer());
//        //        }
//        //    }
//        //    db.MergeLayout(_current_layout_name, data);
//        //}
//        //public void LoadLayout(string layout_name)
//        //{
//        //    dockManager.BeginUpdate();
//        //    if (layout_name == _default_layout_name)
//        //    {
//        //        var data = db.SelectDefaultLayoutData();
//        //        if (String.IsNullOrEmpty(data)) return;
//        //        // проверяем версию
//        //        var xserializer = XDocument.Parse("<root>" + data.Trim('\0') + "</root>").Root.Element("XtraSerializer");
//        //        var version = xserializer.Elements("property").First(el => el.Attribute("name").Value == "#LayoutVersion").Value;
//        //        // грузим только если версия совпала
//        //        if (version == _layout_version)
//        //        {
//        //            using (var stream = new MemoryStream())
//        //            {
//        //                stream.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
//        //                stream.Seek(0, SeekOrigin.Begin);
//        //                _report_changing = true;
//        //                ucReportSettingsTree.SupressFocusChanging = true;
//        //                dockManager.RestoreLayoutFromStream(stream);
//        //                ucReportSettingsTree.SupressFocusChanging = false;
//        //                _report_changing = false;
//        //            }
//        //        }
//        //    }
//        //    else
//        //    {
//        //        var layouts_path = Path.Combine(Environment.CurrentDirectory, "sql.builder", "layouts");
//        //        if (Directory.Exists(layouts_path))
//        //        {
//        //            _report_changing = true;
//        //            ucReportSettingsTree.SupressFocusChanging = true;
//        //            dockManager.RestoreFromXml(Path.Combine(layouts_path, layout_name + ".layout.xml"));
//        //            ucReportSettingsTree.SupressFocusChanging = false;
//        //            _report_changing = false;
//        //        }
//        //    }
//        //    // контрол с деревом отчётов должен лежать на панельке
//        //    if (dockManager.Panels.Any())
//        //    {
//        //        var reports_panel = dockManager.Panels.FirstOrDefault(p => p.Name == "dpReports") ?? dockManager.Panels[0];
//        //        if (!reports_panel.ControlContainer.Controls.Contains(ucReportSettingsTree))
//        //        {
//        //            ucReportSettingsTree.SupressFocusChanging = true;
//        //            reports_panel.ControlContainer.Controls.Add(ucReportSettingsTree);
//        //            reports_panel.Name = "dpReports";
//        //            ucReportSettingsTree.SupressFocusChanging = false;
//        //        }
//        //    }
//        //    dockManager.EndUpdate();
//        //}
//        //public void BeforeLayoutChange(string old_layout_name)
//        //{
//        //    if (old_layout_name != _default_layout_name) return;
//        //    _report_changing = true;
//        //    dmView.Controller.CloseAll();
//        //    SaveUserLayout();
//        //    _report_changing = false;
//        //}
//        //public void LayoutChange(string new_layout_name)
//        //{
//        //    if (_report_changing) return;
//        //    _report_changing = true;
//        //    dmView.Controller.CloseAll();
//        //    // Загружаем пользовательский интерфейс по названию
//        //    LoadLayout(new_layout_name);
//        //    _current_layout_name = new_layout_name;
//        //    _report_changing = false;
//        //}
//        //#endregion
//        private void ReloadXml()
//        {
//            this.dmReports.View.Controller.CloseAll();
//            WaitUIHelper.LastUsedUIHelper.ForceHide();
//            //Wait.ForceHide();
//            this.OnBaseEvent(ucBase.ucBaseEventType.ExecuteStart, "Перезагрузка схемы");
//            XmlReports.Init(true);
//            if (this.cReportsTree != null)
//            {
//                this.cReportsTree.ReadExpandedState();
//                this.cReportsTree.ReloadAllWithoutData(true);
//            }
//            this.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//            //VCashUtils.ClearCash(); есть в XmlReports.Init()
//            WaitUIHelper.LastUsedUIHelper.ForceHide();
//            //Wait.ForceHide();
//        }
//        private void SetStoreDefaultParamsMode(bool store_params)
//        {
//            if (this.CurrentGC == null || this.CurrentGC.Grid.IsTemplate || this.CurrentGC.ParamFormC == null)
//            {
//                return;
//            }
//            if (store_params)
//            {
//                this.CurrentGC.ParamFormC.SetDefaultParams(this.CurrentGC.SaveToXml(SettingsType.Params).Element(EName.@params), true);
//            }
//            else
//            {
//                this.CurrentGC.ParamFormC.SetDefaultParams(null, true);
//                this.DeleteDefaultParams();
//            }
//        }
//        // Тестирование отчётов
//        internal Dictionary<string, string>[] GetReports(bool only_opened)
//        {
//            return (only_opened) ? _grid_containers.Keys.Where(key => !key.EndsWith("_loaded")).Select(this.cReportsTree.GetReport).ToArray()
//                                 : cReportsTree.GetAllVisibleReports();
//        }
//        internal Tuple<bool, string> ShowAndExecute(string report_name)
//        {
//            try
//            {
//                ShowContainer(report_name);
//                RefreshButtonsStates();
//            }
//            catch (Exception ex)
//            {
//                return new Tuple<bool, string>(false, "Ошибка при открытии отчёта:" + Environment.NewLine + ex.Message);
//            }
//            try
//            {
//                this.ExecuteReport();
//                // пытаемся распечатать
//                BarLinkContainerItem dbtn = GetRibbonSource<ucMainReports>().dbtnPrintForm;
//                XmlNode template = dbtn.Tag as XmlNode;
//                if (template != null)
//                {
//                    this.PrintForm(template, null, null, false, false, false);
//                }
//                else
//                {
//                    foreach (BarItemLink item in dbtn.ItemLinks)
//                    {
//                        template = item.Item.Tag as XmlNode;
//                        if (template != null)
//                        {
//                            this.PrintForm(template, null, null, false, false, false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return new Tuple<bool, string>(false, "Ошибка при формировании отчёта:" + Environment.NewLine + ex.Message);
//            }
//            return new Tuple<bool, string>(true, "Отчёт успешно сформирован");
//        }
//        internal void CloseReport(string report_name, bool close_tab = true)
//        {
//            ucReportContainer gc = null;
//            ucFormContainer fc = null;

//            if (_grid_containers.TryGetValue(report_name, out gc))
//            {
//                if (_current_container == gc) _current_container = null;
//                if (gc.Grid != null)
//                {
//                    if (gc.Grid.DataSource != null)
//                    {
//                        gc.Grid.DataSource.Clear();
//                        gc.Grid.DataSource = null;
//                    }

//                    if (gc.Grid.IsTemplate && cReportsTree != null)
//                    {
//                        cReportsTree.SetReportLayoutChanged(gc.ContainerName, false);
//                    }

//                    gc.Grid.OpenExcel -= OnOpenExcel;
//                }
//                if (gc.ParamFormC != null)
//                {
//                    gc.ParamFormC.AnyValueChanged -= UIForm_AnyValueChanged;
//                    // gc.ParamFormC.DataSource.Clear();// !!! Бельченко. Вызывает цепочку событий и ошибку. Убрал вроде это не обязательно.
//                    gc.ParamFormC.DataSource = null;
//                }
//                else if (gc.ParamForm != null)
//                {
//                    gc.ParamForm.AnyValueChanged -= UIForm_AnyValueChanged;
//                }

//                _grid_containers.Remove(gc.ContainerName);
//            }
//            else if (_form_containers.TryGetValue(report_name, out fc))
//            {
//                if (_current_container == fc) _current_container = null;
//                // чтобы не диспозились формы в кэше
//                fc.Controls.Clear();
//                _form_containers.Remove(fc.ContainerName);
//            }

//            if (close_tab)
//            {
//                var doc = (Document)dmReports.GetDocument(gc);
//                if (doc != null) dmReports.View.Controller.Close(doc);
//            }

//            // Вызывает задержки в 2 мин на сервере ТатТепла!
//            //GC.Collect();
//        }

//        // активный документ сбрасывается когда контрол становится невидимым
//        BaseDocument doc = null;
//        public override void SaveState()
//        {
//            doc = dmView.ActiveDocument;
//        }
//        public override void RestoreState()
//        {
//            if (doc != null) dmView.Controller.Activate(doc);
//        }

//        #endregion
//        #region Закрытые методы
//        private ucReportContainer GetGridContainer(Dictionary<string, string> report_info, bool from_file, XElement xreport, VReport report)
//        {
//            ucReportContainer container = null;
//            var repname = report_info["repname"] + (from_file ? "_loaded" : "");

//            if (!_grid_containers.TryGetValue(repname, out container))
//            {
//                if (_open_titles.ContainsKey(report_info["title"])) // чтобы нельзя было открыть 2 отчета с одинаковым именем, а то путаница при работе с детализацией
//                {
//                    CloseReport(_open_titles[report_info["title"]]);
//                    _open_titles.Remove(report_info["title"]);
//                }
//                container = new ucReportContainer();

//                container.Initialize(report_info, report, from_file, xreport);

//                container.Dock = DockStyle.Fill;

//                if (container.ParamForm != null)
//                {
//                    container.ParamForm.AnyValueChanged += UIForm_AnyValueChanged;
//                }
//                else if (container.ParamFormC != null)
//                {
//                    container.ParamFormC.AnyValueChanged += UIForm_AnyValueChanged;
//                }

//                container.Grid.OpenExcel += OnOpenExcel;

//                _grid_containers.Add(repname, container);
//                _open_titles.Add(container.ContainerTitle, repname);
//            }

//            return container;
//        }
//        private ucFormContainer GetFormContainer(Dictionary<string, string> report_info, bool from_file)
//        {
//            ucFormContainer container = null;
//            var repname = report_info["repname"] + (from_file ? "_loaded" : "");

//            // if (!_form_containers.TryGetValue(repname, out container))
//            if (!_form_containers.ContainsKey(repname))
//            {
//                container = new ucFormContainer();
//                _form_containers.Add(repname, container);

//                container.Initialize(report_info);

//                container.Dock = DockStyle.Fill;


//            }
//            else
//            {
//                container = _form_containers[repname];
//            }

//            return container;
//        }
//        internal void OpenReport(XElement report, object[] pars, bool execute = true, UIFormC parentParamsForm = null)
//        {
//            ShowContainer(report, pars, parentParamsForm);
//            if (execute)
//            {
//                if (report.AttrOrDefault(AName.nogrid, false))
//                {
//                    var ss = PrintForms(report, false, false, force_refresh: !XmlReports.IsDeveloperMode());
//                    CloseReport(report.AttrOrEmpty(AName.name));
//                    Process.Start(ss.First());
//                }
//                else
//                {
//                    ExecuteReport(report);
//                }
//            }
//        }
//        internal void OpenReport(string report_name, object[] pars, bool execute, string project = null)
//        {
//            ShowContainer(report_name, pars);
//            if (execute) ExecuteReport();
//        }

//        private void ShowOpenedContainer(string report_name)
//        {
//            var dict = new Dictionary<string, string>();
//            dict.Add("repname", report_name);
//            dict.Add("old", false.ToString());
//            if (_grid_containers.ContainsKey(report_name))
//            {
//                dict.Add("item_type", TextConst.EName.UseReport);
//            }
//            else if (_form_containers.ContainsKey(report_name))
//            {
//                dict.Add("item_type", TextConst.EName.UseForm);
//            }

//            ShowContainer(dict, false, null, null);
//        }


//        private void ShowContainer(XElement reportScheme, object[] pars = null, UIFormC parentParamsForm = null)
//        {
//            var dict = XmlReports.GetReportInfo(reportScheme);
//            var report = XmlReports.Environment.GetPrecompiledReport(reportScheme);

//            XElement xpars = null;
//            if (pars != null)
//            {
//                xpars = VDataSet.ParsObjectArrayToXelement(pars, report.Element(TextConst.EName.Params));
//            }

//            var xreport = new XElement(EName.root, report.GetSchemeWithColumnsPreset());
//            ShowContainer(dict, false, xreport, xpars, report, parentParamsForm);

//        }
//        private void ShowContainer(string report_name, object[] pars = null)
//        {
//            Dictionary<string, string> dict = XmlReports.GetReportInfo(report_name);
//            VReport report = XmlReports.Environment.GetPrecompiledReport(report_name);
//            XElement xpars = null;
//            if (pars != null)
//            {
//                xpars = VDataSet.ParsObjectArrayToXelement(pars, report.Element(TextConst.EName.Params));
//            }
//            XElement xreport = new XElement(EName.root, report.GetSchemeWithColumnsPreset());
//            ShowContainer(dict, false, xreport, xpars);
//        }
//        private void ShowContainer(Dictionary<string, string> report_info, bool from_file, XElement xdoc, XElement xpars = null, VReport vreport = null, UIFormC parentParamsForm = null)
//        {
//            if (report_info["item_type"] == TextConst.EName.UseForm)
//            {
//                _current_container = GetFormContainer(report_info, from_file);
//            }
//            else
//            {
//                if (report_info.ContainsKey("original_name"))
//                {
//                    if (vreport == null)
//                    {
//                        vreport = XmlReports.Environment.GetReport(report_info["original_name"]);
//                    }
//                    if (vreport == null)
//                    {
//                        vreport = VSXElement.Get<VReport>(VReport.getReportOrQuery(report_info["original_name"], XmlReports.Environment.Manager.GetScheme()));
//                    }
//                }

//                _current_container = GetGridContainer(report_info, from_file, xdoc, vreport);
//                _current_container.SetParentParamsForm(parentParamsForm);
//                if (xpars != null) CurrentGC.SetCustomParams(xpars);
//            }

//            dmReports.View.AddDocument(_current_container);
//            dmReports.View.ActivateDocument(_current_container);
//            _current_container.Focus();


//            foreach (var f in Cmn.GetChildControlsOfType<sql.builder.UI.WinForms.UIFormControl>(_current_container)) f.GetVForm().LayoutRefresh();
//            // Cmn.GetChildControlsOfType<UIFormC2Control>(_current_container).ForEach(f => f.Form.LayoutRefresh());
//        }

//        //private ucBaseContainer GetContainerByDocument(Document document)
//        //{
//        //    var control = document.Control.Controls[0];

//        //    while (!(control is ucBaseContainer)) control = control.Controls[0];
//        //    return (ucBaseContainer)control;
//        //    //if (document.Control.Text != String.Empty)
//        //    //{
//        //    //    return (ucReportGrid)document.Control.Controls[0];
//        //    //}
//        //    //else
//        //    //{
//        //    //    return (ucReportGrid)document.Form.Controls[0].Controls[0].Controls[0];
//        //    //}
//        //}
//        /// <summary>
//        /// Инициализирует кнопку "Печатная форма"
//        /// </summary>
//        /// <param name="templates">Список печатных форм текущего отчёта</param>
//        private void GeneratePrintTemplates(XmlNodeList templates)
//        {
//            ucMainReports ctrl = this.GetRibbonSource<ucMainReports>();
//            BarLinkContainerItem ribbon_item = ctrl.dbtnPrintForm;
//            ribbon_item.ClearLinks();
//            ribbon_item.Tag = null;
//            int count = templates.Count;
//            if (count == 1)
//            {
//                ribbon_item.Tag = templates[0];
//            }
//            else
//            {
//                for (int index = 0; index < count; index++)
//                {
//                    XmlNode template = templates[index];
//                    BarButtonItem button = new BarButtonItem();
//                    button.Tag = template;
//                    button.ItemClick += ctrl.btnPrintForm_OnClick;
//                    BarItemLink item = ribbon_item.AddItem(button);
//                    item.Caption = template.Attributes["title"].Value;
//                }
//            }
//        }
//        /*private void SetWorkFolder(string path)
//        {
//            SettingsHelper.WorkFolder = path;
//            this.GetRibbonSource<ucMainReports>().btnWorkFolderPath.EditValue = path;
//            Printing.outputFolder = path;
//        }*/
//        private string GetWorkFolder()
//        {
//            ucMainReports ctrl = this.GetRibbonSource<ucMainReports>();
//            return (ctrl.btnWorkFolderPath.EditValue != null) ? (string)ctrl.btnWorkFolderPath.EditValue : string.Empty;
//        }
//        private bool CheckWorkFolder()
//        {
//            ucMainReports ctrl = this.GetRibbonSource<ucMainReports>();
//            string work_folder = this.GetWorkFolder();
//            if (Directory.Exists(work_folder))
//            {
//                return true;
//            }
//            bool result = this.SelectWorkFolder();
//            if (!result)
//            {
//                XtraMessageBox.Show("Прежде чем сформировать отчёт, необходимо выбрать рабочую папку", "Рабочая папка не выбрана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//            return result;
//        }
//        private bool needMinimizeRibbon = false;
//        private void RefreshButtonsStates()
//        {

//            if (XmlReports.IsInfoenergo)
//            {
//                var ribon1 = GetCurrentControl<ucMainReports>().SearchTopRibbon();
//                if (ribon1 != null)
//                {
//                    if (CurrentGC != null)
//                    {
//                        if (ribon1.Minimized)
//                        {
//                            needMinimizeRibbon = true;
//                            ribon1.AllowMinimizeRibbon = false;
//                        }
//                    }
//                    else
//                    {
//                        if (needMinimizeRibbon)
//                        {
//                            ribon1.AllowMinimizeRibbon = true;
//                            ribon1.Minimized = true;
//                        }
//                    }
//                }
//            }
//            ucMainReports rib = this.GetRibbonSource<ucMainReports>();
//#if DEBUG
//            if (XmlReports.IsDeveloperMode())
//            {
//                rib.btnGroupEditor.Visibility = BarItemVisibility.Always;
//            }
//            else
//            {
//                rib.btnGroupEditor.Visibility = BarItemVisibility.Never;
//            }
//#endif
//            if (CurrentGC == null)
//            {
//                rib.btnCreateTemplate.Enabled = false;
//                rib.btnSaveTemplate.Enabled = false;
//                rib.btnDeleteTemplate.Enabled = false;
//                rib.btnExecuteReport.Enabled = false;
//                rib.btnExportToXls.Enabled = false;
//                rib.dbtnPrintForm.Enabled = false;
//                rib.btnColumnsEditor.Enabled = false;
//                rib.ceStoreDefaultParams.EditValue = null;
//                rib.ceStoreDefaultParams.Enabled = false;
//                rib.rpgRepository.Visible = false;
//                if (_current_container == null)
//                {
//                    rib.btnSaveAllData.Enabled = false;
//                    rib.btnRefreshData.Enabled = false;
//                }
//                else
//                {
//                    rib.btnSaveAllData.Enabled = true;
//                    rib.btnRefreshData.Enabled = true;
//                }
//                return;
//            }
//            else
//            {
//                rib.btnSaveAllData.Enabled = false;
//                rib.btnRefreshData.Enabled = false;
//            }
//            VReport report = null;
//            if (CurrentGC.Mode == ucReportContainer.GridMode.ReportGrid)
//            {
//                VDataSet ds = this.CurrentGC.Grid.DataSource;
//                if (ds == null)
//                {
//                    report = null;
//                }
//                else
//                {
//                    report = ds.Report;
//                }
//                if (report != null && report.P_AllowSave == TextConst.AVBool.True)
//                {
//                    if (this.cReportsTree == null)
//                    {
//                        rib.btnCreateTemplate.Enabled = false;
//                        rib.btnSaveTemplate.Enabled = false;
//                        rib.btnDeleteTemplate.Enabled = false;
//                    }
//                    else
//                    {
//                        bool is_template = this.CurrentGC.Grid.IsTemplate;
//                        rib.btnCreateTemplate.Enabled = !is_template;
//                        rib.btnSaveTemplate.Enabled = this.cReportsTree.GetReportLayoutChanged(this.CurrentGC.Grid.ReportName);
//                        rib.btnDeleteTemplate.Enabled = is_template;
//                    }
//                }
//                else
//                {
//                    rib.btnCreateTemplate.Enabled = false;
//                    rib.btnSaveTemplate.Enabled = false;
//                    rib.btnDeleteTemplate.Enabled = false;
//                }
//                rib.btnExecuteReport.Enabled = (!CurrentGC.Grid.FromFile && !CurrentGC.Grid.IsCompareMode);
//                rib.btnExportToXls.Enabled = true;
//                rib.dbtnPrintForm.Enabled = !CurrentGC.Grid.IsCompareMode && (rib.dbtnPrintForm.Tag != null || rib.dbtnPrintForm.ItemLinks.Count > 1) && (report.P_ViewMode != TextConst.AVViewModes.Excel);
//                rib.btnColumnsEditor.Enabled = (!CurrentGC.Grid.IsCompareMode && !CurrentGC.Grid.FromFile && report != null && report.P_EditColumns == TextConst.AVBool.True);
//                rib.btnReportParams.Enabled = (!CurrentGC.Grid.IsCompareMode && !CurrentGC.Grid.FromFile && report != null && report.P_ParamsCustomization == TextConst.AVBool.True);
//                rib.btnExpressReport.Enabled = (rib.dbtnPrintForm.Tag != null || rib.dbtnPrintForm.ItemLinks.Count > 1);
//            }
//            else if (CurrentGC.Mode == ucReportContainer.GridMode.ReferenceGrid)
//            {
//                rib.btnCreateTemplate.Enabled = false;
//                rib.btnSaveTemplate.Enabled = false;
//                rib.btnDeleteTemplate.Enabled = false;
//                rib.btnExecuteReport.Enabled = (CurrentGC.Grid.FromFile == false);
//                rib.btnExportToXls.Enabled = true;
//                rib.dbtnPrintForm.Enabled = false;
//                rib.btnColumnsEditor.Enabled = false;
//                rib.btnExpressReport.Enabled = false;
//            }
//            else if (CurrentGC.Mode == ucReportContainer.GridMode.NoGrid)
//            {
//                if (CurrentGC.IsHiddenExcel)
//                {
//                    rib.btnExecuteReport.Enabled = true;
//                    rib.btnExportToXls.Enabled = true;
//                    rib.dbtnPrintForm.Enabled = false;
//                    CurrentGC.Grid.SetText("Чтобы вывести отчёт, задайте параметры и нажмите кнопку \"Сформировать отчет\"");
//                }
//                else
//                {
//                    rib.btnExecuteReport.Enabled = false;
//                    rib.btnExportToXls.Enabled = false;
//                    rib.dbtnPrintForm.Enabled = (rib.dbtnPrintForm.Tag != null || rib.dbtnPrintForm.Links.Count > 0);
//                }
//                rib.btnCreateTemplate.Enabled = false;
//                rib.btnSaveTemplate.Enabled = false;
//                rib.btnDeleteTemplate.Enabled = false;
//                rib.btnColumnsEditor.Enabled = false;
//                rib.btnExpressReport.Enabled = (rib.dbtnPrintForm.Tag != null || rib.dbtnPrintForm.ItemLinks.Count > 0);
//            }
//            // Сохранять параметры
//            if (CurrentGC.Grid.IsTemplate || CurrentGC.ParamFormC == null)
//            {
//                rib.ceStoreDefaultParams.EditValue = null;
//                rib.ceStoreDefaultParams.Enabled = false;
//            }
//            else
//            {
//                rib.ceStoreDefaultParams.EditValue = (CurrentGC.ParamFormC.DefaultParams != null);
//                rib.ceStoreDefaultParams.Enabled = true;
//            }
//            if (report != null && report.P_UseRepository != "")
//            {
//                // rib.rpgRepository.Visible = true;
//                if (CurrentGC.UseRepository == 2)
//                {
//                    CurrentGC.UseRepository = 1;
//                }

//                rib.ceUseRepository.EditValue = CurrentGC.UseRepository;

//            }
//            else
//            {
//                rib.rpgRepository.Visible = false;
//            }

//            //rib.btnTest.Enabled = rib.dbtnPrintForm.Enabled;
//        }
//        private void SaveDefaultParams()
//        {
//            if (this.CurrentGC == null || this.CurrentGC.Grid.IsTemplate || this.CurrentGC.ParamFormC == null)
//            {
//                return;
//            }
//            XElement xreport = this.CurrentGC.SaveToXml(SettingsType.Params);
//            if (xreport == null)
//            {
//                return;
//            }
//            db.MergeDefaultReportSetting(this.CurrentGC.Grid.ReportName, xreport.ToString());
//        }
//        private void DeleteDefaultParams()
//        {
//            if (this.CurrentGC == null || this.CurrentGC.Grid.IsTemplate || this.CurrentGC.ParamFormC == null)
//            {
//                return;
//            }
//            db.DeleteDefaultReportSetting(this.CurrentGC.Grid.ReportName);
//        }
//        private void CompareReports()
//        {
//            if (CurrentGC == null || CurrentGC.Grid.DataSource == null) return;

//            if (!CurrentGC.Grid.IsCompareMode)
//            {
//                var xRoot = CurrentGC.SaveToXml(fixed_scheme: false);
//                Parser.SaveReportDataToXml(xRoot, CurrentGC.Grid.DataSource);
//            }

//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = GetWorkFolder();
//                dlg.Filter = "(*.rxml) Отчёты SqlBuilder|*.rxml|(*.xml)|*.xml|(*.*)|*.*";
//                if (dlg.ShowDialog() == DialogResult.OK)
//                {
//                    var ds1 = CurrentGC.Grid.DataSource;
//                    var ds2 = Parser.LoadReportDataFromXml(XElement.Load(dlg.FileName));

//                    var str1 = String.Join("", ds1.Tables.Cast<DataTable>().OrderBy(dt => dt.TableName).Select(dt => dt.TableName));
//                    var str2 = String.Join("", ds2.Tables.Cast<DataTable>().OrderBy(dt => dt.TableName).Select(dt => dt.TableName));
//                    if (str1 != str2)
//                    {
//                        XtraMessageBox.Show("Структура отчетов различается. Невозможно выполнить сравнение.", "Недопустимая операция",
//                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                        return;
//                    }

//                    OnBaseEvent(ucBase.ucBaseEventType.ExecuteStart, "Идет процесс сравнения отчётов");

//                    CurrentGC.Grid.SetComparedMode(ds2);

//                    OnBaseEvent(ucBaseEventType.ExecuteComplete);
//                    RefreshButtonsStates();
//                }
//            }
//        }

//        private void CompareReports2()
//        {
//            //new Thread(() =>
//            //{
//            //    var frm = new frmWCFCompare(this);
//            //    frm.ShowDialog();
//            //}).Start();

//            var frm = new frmWCFCompare(this);
//            frm.ShowDialog();

//        }

//        private void BeforeClose()
//        {
//            if (XmlReports.IsInfoenergo)
//            {
//                var ribon1 = GetCurrentControl<ucMainReports>().SearchTopRibbon();
//                if (ribon1 != null)
//                {

//                    if (needMinimizeRibbon)
//                    {
//                        ribon1.AllowMinimizeRibbon = true;
//                        ribon1.Minimized = true;
//                    }

//                }
//            }
//            //_report_changing = true;
//            //dmView.Controller.CloseAll();
//            //SaveUserLayout();
//            //_report_changing = false;
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (!IsRibbonOnly) BeforeClose();

//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//        //private static void CopyRibbonFromOtherForm(RibbonControl ribbon, Form form)
//        //{

//        //    var sourceRibbon = (RibbonControl)Cmn.GetProperty(form, "Ribbon");

//        //    foreach (var page in sourceRibbon.Pages.Cast<RibbonPage>().ToList())
//        //    {
//        //        ribbon.Pages.Add(page);
//        //    }

//        //}

//        /*private BarButtonItem InvestProInstructionsButton(string folder_name = null)
//        {
//            // в UpdateRibbon это делать плохая идея, тк форма показывается раньше времени
//            // поэтому перенес долгую часть в Initialize

//            var type = ReflectionHelper.GetLoadedType("ipsupport.Net.InstructionBarButtonClass");
//            if (type == null) return null;

//            string kod_name = string.IsNullOrEmpty(folder_name) ? "reportManagerBut" : folder_name;
//            var btn = Activator.CreateInstance(type, kod_name) as BarButtonItem;
//            btn.Caption = "Инструкции";

//            return btn;
//        }*/
//        #endregion
//        #region Обработчики событий
//        private void dmReports_DocumentActivate(object sender, DocumentEventArgs e)
//        {
//            if (_report_changing || e.Document == null) return;

//            _report_changing = true;

//            _current_container = (ucBaseContainer)e.Document.Control;//GetContainerByDocument((Document)e.Document);
//            //if (_current_container.ParamFormC != null)
//            //{
//            //    _current_container.ParamFormC.ApplyVisibitlity();
//            //}

//            if (CurrentGC != null)
//            {
//                var xreport = XmlReports.GetReport(CurrentGC.Grid.OriginalName);
//                var xmlreport = new XmlDocument();
//                xmlreport.LoadXml(xreport.ToString());

//                GeneratePrintTemplates(xmlreport.FirstChild.SelectNodes("print-templates//template"));
//            }

//            _report_changing = false;

//            RefreshButtonsStates();
//        }
//        private void dmView_DocumentAdded(object sender, DocumentEventArgs e)
//        {
//            //e.Document.Properties.AllowFloat = DefaultBoolean.False;

//            if (CurrentGC != null)
//            {
//                if (CurrentGC.Mode == ucReportContainer.GridMode.ReportGrid || CurrentGC.Mode == ucReportContainer.GridMode.NoGrid)
//                {
//                    if (CurrentGC.Grid.IsTemplate) e.Document.ImageIndex = 3;
//                    //else if (!CurrentGC.Grid.IsVisible) e.Document.ImageIndex = 4;
//                    else e.Document.ImageIndex = 1;
//                }
//                else if (CurrentGC.Mode == ucReportContainer.GridMode.ReferenceGrid)
//                {
//                    e.Document.ImageIndex = 2;
//                }
//            }
//            else if (CurrentFC != null)
//            {
//                e.Document.ImageIndex = 2;
//            }

//            if (_current_container.ParamFormC != null)
//            {
//                e.Document.Caption = _current_container.ContainerTitle;
//                if (_current_container.ParamFormC.FormUseType != UIFormC.UseType.DataEditor) // Бельченко 12.05.2017, вместе с этим для DataEditor RefreshData выполняется аж 3 раза
//                {
//                    _current_container.ParamFormC.RefreshData();
//                }
//                // перенес на событие Activate, тк в этот момент контрол еще не добавлен на форму
//                //_current_container.ParamFormC.ApplyVisibitlity();
//            }

//            // ParamFormC.RefreshData() генерит это событие. пришлось вынести
//            if (CurrentGC != null)
//            {
//                CurrentGC.Grid.AddLayoutChangedHandler(ucVGridBase_LayoutChanged, ucVGridBase_LayoutChanged2);
//            }
//        }
//        private void dmView_DocumentClosing(object sender, DocumentCancelEventArgs e)
//        {
//            var container = e.Document.Control as ucBaseContainer;

//            CloseReport(container.ContainerName, false);

//            // Активируем какой-нибудь из оставшихся
//            // сделать нормально
//            if (_grid_containers.Count > 0)
//            {
//                var gc_last = _grid_containers.Last();
//                ShowOpenedContainer(gc_last.Key);
//            }
//            else if (_form_containers.Count > 0)
//            {
//                var fc_last = _form_containers.Last();
//                ShowOpenedContainer(fc_last.Key);
//            }

//            RefreshButtonsStates();
//        }
//        /*
//        private void dockManager_EndDocking(object sender, EndDockingEventArgs e)
//        {
//            //var ctrl = ribSrcControl<ucMainReports>();

//            //_report_changing = true;
//            //ctrl.cbLayouts.EditValue = _default_layout_name;
//            //_current_layout_name = _default_layout_name;
//            //_report_changing = false;
//        }
//        private void dockManager_EndSizing(object sender, EndSizingEventArgs e)
//        {
//            //var ctrl = ribSrcControl<ucMainReports>();

//            //_report_changing = true;
//            //ctrl.cbLayouts.EditValue = _default_layout_name;
//            //_current_layout_name = _default_layout_name;
//            //_report_changing = false;
//        }*/
//        private void UIForm_AnyValueChanged(object sender, EventArgs args)
//        {
//            if (_report_changing || CurrentGC == null) return;

//            if (CurrentGC.Grid.IsTemplate && !CurrentGC.Grid.FromFile && cReportsTree != null)
//            {
//                cReportsTree.SetReportLayoutChanged(CurrentGC.Grid.ReportName, true);
//            }
//            RefreshButtonsStates();
//        }

//        #region Ribbon
//        // Формирование и экспорт
//        private bool ValidateParams()
//        {
//            var ctrl = GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC == null) return true;
//            var msg = ctrl.CurrentGC.GetParamsValidation();
//            if (string.IsNullOrEmpty(msg))
//            {
//                return true;
//            }
//            else
//            {
//                XtraMessageBox.Show(msg, "Не все параметры указаны корректно",
//                    MessageBoxButtons.OK,
//                    MessageBoxIcon.Warning);
//                return false;
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Сформировать отчёт"
//        /// </summary>
//        private void btnExecuteReport_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC == null || !this.ValidateParams())
//            {
//                return;
//            }
//            ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//            if (grid == null)
//            {
//                ctrl.ExecuteReport();
//            }
//            if (ctrl.CurrentGC.IsHiddenExcel)
//            {
//                ctrl.CurrentGC.IsHiddenExcel = false;
//                ctrl.CurrentGC.ChangeGridMode(ucReportContainer.GridMode.ReportGrid, TableViewMode.Excel);
//                ctrl.CurrentGC.UpdateGridEvents();
//                //ctrl.CurrentGC.ResetDataSource(ctrl.CurrentGC.Grid.DataSource.Report);
//            }
//            if (grid.ViewMode == TableViewMode.Excel)
//            {
//                bool tmp = Printing.CreateRefs;
//                Printing.CreateRefs = true;
//                ctrl.PrintForms(null, false, true, true, !XmlReports.IsDeveloperMode());
//                Printing.CreateRefs = tmp;
//            }
//            else
//            {
//                ctrl.ExecuteReport();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Экспорт в Excel"
//        /// </summary>
//        private void btnExportToXls_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC == null || !this.ValidateParams())
//            {
//                return;
//            }
//            ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//            if (ctrl.CurrentGC.IsHiddenExcel || (grid != null && grid.ViewMode == TableViewMode.Excel))
//            {
//                ctrl.PrintForms(null);
//            }
//            else
//            {
//                ctrl.ExportToXls();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Печатная форма"
//        /// </summary>
//        private void btnPrintForm_OnClick(object sender, ItemClickEventArgs e)
//        {

//            XmlNode template = (XmlNode)e.Item.Tag;
//            if (template != null && this.ValidateParams())
//            {
//                ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//                if (WebReportsAdapter.IsWebReport(ctrl.CurrentGC))
//                {
//                    WebReportsAdapter.ExecuteReport(ctrl.CurrentGC);
//                }
//                else
//                {
//                    ctrl.PrintForm(template, null, null, true, false, false);
//                }

//            }
//        }
//        #region Шаблоны отчёта
//        /// <summary>
//        /// Реакция на кнопку "Создать шаблон"
//        /// </summary>
//        private void btnCreateSetting_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//                if (grid.FromFile || grid.IsCompareMode)
//                {
//                    return;
//                }
//                ctrl.cReportsTree.CreateReportTemplate(grid.ReportName);
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Сохранить шаблон"
//        /// </summary>
//        private void btnSaveCurrentSetting_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//                if (grid.FromFile || grid.IsCompareMode || !grid.IsTemplate)
//                {
//                    return;
//                }
//                ctrl.cReportsTree.UpdateReportTemplate(grid.ReportName, ctrl.CurrentGC.SaveToXml(SettingsType.SchemeAndParams));
//                ctrl.RefreshButtonsStates();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Удалить шаблон"
//        /// </summary>
//        private void btnDeleteCurrentSetting_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//                if (grid.FromFile || grid.IsCompareMode || !grid.IsTemplate)
//                {
//                    return;
//                }
//                ctrl.cReportsTree.DeleteReportTemplate(grid.ReportName);
//            }
//        }
//        #endregion
//        // Параметры отчёта
//        private void btnColumnsEditor_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().ShowColumnsEditor();
//        }
//        private void btnReportParams_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().ChooseReportParams();
//        }
//        private void rceStoreDefaultParams_EditValueChanged(object sender, EventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().SetStoreDefaultParamsMode(((CheckEdit)sender).Checked);
//        }
//        // Рабочая папка
//        private bool _folder_button_pressed = false;
//        private void rbtnWorkFolderPath_Click(object sender, EventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            string work_folder = ctrl.GetWorkFolder();
//            if (this._folder_button_pressed || string.IsNullOrEmpty(work_folder))
//            {
//                ctrl.SelectWorkFolder();
//            }
//            else
//            {
//                Process.Start(work_folder);
//            }
//            this._folder_button_pressed = false;
//        }
//        private void rbtnWorkFolderPath_ButtonPressed(object sender, ButtonPressedEventArgs e)
//        {
//            this._folder_button_pressed = true;
//        }
//        private void rbtnWorkFolderPath_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
//        {
//            e.DisplayText = e.Value != null ? Cmn.CutString(e.Value.ToString(), 48) : String.Empty;
//        }

//        // Дополнительно
//        private void cbDevexpressSkins_EditValueChanged(object sender, EventArgs e)
//        {
//            var skin_name = (string)((BarEditItemLink)((BarEditItem)sender).Links[0]).EditValue;
//            UserLookAndFeel.Default.SetSkinStyle(skin_name);
//            // UserLookAndFeel.Default.ActiveStyle.
//            SettingsHelper.DevExpressSkinName = skin_name;
//        }
//        //private void rcbLayouts_EditValueChanging(object sender, ChangingEventArgs e)
//        //{
//        //var cur_val = ((ComboBoxEdit)sender).EditValue;
//        //if (cur_val == null) return;

//        //thisControl<ucMainReports>().BeforeLayoutChange((string)e.OldValue);
//        //}
//        //private void cbLayouts_EditValueChanged(object sender, EventArgs e)
//        //{
//        //var layout_name = (string)((BarEditItemLink)((BarEditItem)sender).Links[0]).EditValue;
//        //thisControl<ucMainReports>().LayoutChange(layout_name);
//        //}
//        /// <summary>
//        /// Реакция на кнопку "Сохранить отчёт в файл"
//        /// </summary>
//        private void btnSaveReportToFile_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                ucTableViewerContainer grid = ctrl.CurrentGC.Grid;
//                if (grid.DataSource != null)
//                {
//                    using (var dlg = new SaveFileDialog())
//                    {
//                        dlg.InitialDirectory = ctrl.GetWorkFolder();
//                        dlg.Filter = "Отчёты SqlBuilder (*.rxml)|*.rxml|XML (*.xml)|*.xml|Все файлы (*.*)|*.*";
//                        dlg.FileName = grid.ReportTitle;
//                        dlg.DefaultExt = "rxml";
//                        if (dlg.ShowDialog() == DialogResult.OK)
//                        {
//                            XElement xdoc = ctrl.CurrentGC.SaveToXml(SettingsType.All, false);
//                            xdoc.Save(dlg.FileName);
//                        }
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Загрузить отчёт из файла"
//        /// </summary>
//        private void btnLoadReportFromFile_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = ctrl.GetWorkFolder();
//                dlg.Filter = "Отчёты SqlBuilder (*.rxml)|*.rxml|XML (*.xml)|*.xml|Все файлы (*.*)|*.*";
//                dlg.DefaultExt = "rxml";
//                if (dlg.ShowDialog() == DialogResult.OK)
//                {
//                    XElement xdoc = XElement.Load(dlg.FileName);
//                    Dictionary<string, string> report_info = Parser.LoadReportInfoFromXml(xdoc);
//                    // пока так - достаем проект из навигатора
//                    XElement xusereport = XmlReports.GetUseReport(report_info["repname"]);
//                    if (xusereport != null)
//                    {
//                        string project = xusereport.Attribute(AName.project).Value;
//                        XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//                    }
//                    ctrl.ShowContainer(report_info, true, xdoc);
//                    //XmlParse.ClearData(xdoc.Root);
//                    ctrl.RefreshButtonsStates();
//                }
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Сравнить отчёты"
//        /// </summary>
//        private void btnCompareReports_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().CompareReports();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Сравнить отчёт с другой версией"
//        /// </summary>
//        private void btnCompareReports2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().CompareReports2();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Тестирование открытых отчётов"
//        /// </summary>
//        private void btnTestOpenedReports_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().ShowTestReports(false, true);
//        }
//        /// <summary>
//        /// Реакция на кнопку "Тестирование всех отчётов"
//        /// </summary>
//        private void btnTestAllReports_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucMainReports>();
//            if (ctrl.cReportsTree == null) return;
//            //var errors = ctrl.cReportsTree.ReloadAllReports();
//            ctrl.ShowTestReports(true, false);
//        }
//        // Отладка
//        /// <summary>
//        /// Реакция на кнопку "CCB SQL"
//        /// </summary>
//        private void btnCCBSQL_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //thisControl<ucMainReports>().Test1();
//            //thisControl<ucMainReports>().CcbGenSql();
//            CcbGenSqlUtils.Generate();
//        }
//        //void Test1()
//        //{
//        //    var con = infoenergo.sys.Global.Connection;
//        //    var cmd = new  Devart.Data.Oracle.OracleCommand("BEGIN HG_OTG_REPORT.report_r024(to_date('01.01.2018','DD.MM.YYYY'), to_date('08.10.2018','DD.MM.YYYY'),0); END;",con);
//        //    cmd.ExecuteNonQuery();
//        //    cmd.CommandText = "select * from hv_otg_rep_r024";
//        //    var da = new Devart.Data.Oracle.OracleDataAdapter(cmd);
//        //    var tbl = new System.Data.DataTable();
//        //    da.Fill(tbl);
//        //    //var form = this.ParentForm;
//        //    //if (form == null) return;
//        //    //form.SuspendLayout();
//        //    //foreach (Control control in form.Controls)
//        //    //{
//        //    //    control.Enabled = false;
//        //    //}
//        //    //var wait = new ProgressPanel() {BorderStyle = BorderStyles.Simple};
//        //    //wait.Location = Location = new Point((Width - wait.Width) / 2, (Height - wait.Height) / 2);
//        //    //form.Controls.Add(wait);
//        //    //form.Controls.SetChildIndex(wait, 0);
//        //    //form.ResumeLayout();
//        //}
//        private static void ShowForm<TForm>()
//            where TForm : Form, new()
//        {
//            TForm form = System.Windows.Forms.Application.OpenForms.OfType<TForm>().FirstOrDefault();
//            if (form != null)
//            {
//                form.WindowState = FormWindowState.Normal;
//                form.Activate();
//            }
//            else
//            {
//                form = new TForm();
//                form.Show();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Открыть TestQuery"
//        /// </summary>
//        private void btnOpenTestQuery_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ShowForm<TestQuery>();
//            //TestQuery form = System.Windows.Forms.Application.OpenForms.OfType<TestQuery>().FirstOrDefault();
//            //if (form != null) {
//            //    form.WindowState = FormWindowState.Normal;
//            //    form.Activate();
//            //} else {
//            //    form = new TestQuery();
//            //    form.Show();
//            //}
//        }
//        /// <summary>
//        /// Реакция на кнопку "Показать/спрятать грид"
//        /// </summary>
//        private void btnChangeGridMode_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                if (ctrl.CurrentGC.Mode == ucReportContainer.GridMode.NoGrid)
//                {
//                    if (ctrl.cReportsTree != null)
//                    {
//                        ctrl.cReportsTree.ReloadData(ctrl.CurrentGC.Grid.ReportName);
//                    }
//                    ctrl.CurrentGC.ChangeGridMode(ucReportContainer.GridMode.ReportGrid);
//                }
//                else if (this.CurrentGC.Mode == ucReportContainer.GridMode.ReportGrid)
//                {
//                    ctrl.CurrentGC.ChangeGridMode(ucReportContainer.GridMode.NoGrid);
//                }
//                ctrl.RefreshButtonsStates();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Открыть ExpressForm"
//        /// </summary>
//        private void btnExpressReport_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //this.GetCurrentControl<ucMainReports>().ShowExpressForm();
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            //decimal p_kod_lim_doc = 33;
//            //var pars = new Dictionary<string, object>();
//            //pars.Add("p_kod_lim_doc", p_kod_lim_doc);
//            //sql.builder.SqlBuilder.PreviewReport("62425.62425", pars);
//            //return;
//            //var frm = new frmExpressReport(false);
//            //frm.Initialize("62425.62425");
//            if (ctrl.CurrentGC != null)
//            {
//                frmExpressReport.ShowForm(ctrl.CurrentGC.Grid.OriginalName);
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Перезагрузить схему"
//        /// </summary>
//        private void btnReloadXml_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            ctrl.ReloadXml();
//            // костыль, чтобы в редакторе схемы тоже обновлялся список
//            ucQueriesEditor ctrl2 = Cmn.GetChildControlsOfType<ucQueriesEditor>(ctrl.ParentForm).FirstOrDefault();
//            if (ctrl2 != null)
//            {
//                XmlReports.Environment.Manager.GetController().LoadState();
//                ctrl2.RefreshQueryList();
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "МегаТест"
//        /// </summary>
//        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //FormParametersControl ctrl = new sql.builder.Controls.FormParametersControl();
//            //ctrl.QueryName = "39463-2";
//            //ctrl.Initialize();
//            //DataSet ds = ctrl.GetParamsDataSet();

//            //var frm = new Form();
//            //frm.Controls.Add(ctrl);
//            //frm.ShowDialog();

//            //return;

//            // Форма с параметрами 39463
//            var rep1 = new ExpressReport();

//            var btn = new Button();

//            // по клику формируем отчет по контр этапам
//            btn.Click += (sender1, args) =>
//            {

//                var rep2 = new ExpressReport();

//                rep2.OpenDocumentAfterPrint = false;
//                rep2.Initialize("30581");

//                // копирование параметров
//                string[] fields_copy = { "kod_doc", "kod_dirisp", "kod_doc_kontr", "date", "datef", "kod_titul_ip", };
//                string[] fields_clear = { "kod_direct", "pr_gorobl", "kod_klass_titul", "kod_ofz" };
//                foreach (var field in fields_copy)
//                {
//                    rep2.GetParamField(field).SetValue(rep1.GetParamField(field).GetValue());
//                }
//                foreach (var field in fields_clear)
//                {
//                    rep2.GetParamField(field).ClearValue();
//                }

//                rep2.GetParamField("kod_name_tmp").SetValue(Cmn.DECIMAL_TWO);
//                rep2.GetParamField("pr_svod").SetValue(Cmn.DECIMAL_ONE);
//                rep2.GetParamField("pr_detail").SetValue(Cmn.DECIMAL_ONE);

//                // подписываемся на событие когда отчёт по контр этапам сформирован
//                rep2.ReportOpening += (a, b) =>
//                {
//                    // путь к excel файлу
//                    var aaa = b.Path;
//                };

//                rep2.ExecuteReport();
//            };
//            rep1.GetButtonsPanel().Controls.Add(btn);
//            rep1.Show("39463-1");

//            //new frmScriptingTest().Show();
//            return;
//            //var xroot = new XElement("root");
//            //Parser.SaveReportParamsToXml(xroot, thisControl<ucMainReports>().CurrentGC.ParamFormC);
//            //Parser.LoadReportParamsFromXml(xroot, thisControl<ucMainReports>().CurrentGC.ParamFormC);
//            //return;
//            //WCFHelper.StartServer();

//            //Process.Start(@"C:\root\main\all\sql.builder\bin\x86\Debug - копия\sql.builder.exe", "-client");

//            //return;

//            //var x = XmlReports.Environment.Manager.GetNativeScheme().Descendants("query").First();
//            //Cache.SaveQueryInfoToCache(x, x.Attribute("name").Value);

//            //var y = Cache.GetQueryInfoFromCache(x.Attribute("name").Value, DateTime.MaxValue,false);

//            //return;

//            //VExcelWorkbook wb = new VExcelWorkbook(@"C:\tfs\all\sql.builder\sql.builder\printTemplate\excel\25499.xml");

//            //VExcelSheet sheet = wb.Sheet(0);
//            // VExcelRow r = sh.Row(2);
//            // VExcelCell cc = r.Cell(3);
//            // cc.Value = sh.Row(1).Cell(0);
//            //  int ii = cc.Index();

//            //foreach (VExcelCell cell1 in sheet.FindCells("cbegin"))
//            //{
//            //    string tableName = cell1.Value.ToString().Split(' ')[0].Split(':')[1];
//            //    VExcelCell cell2 = sheet.FindCell("cend:" + tableName);
//            //    VExcelRange range = new VExcelRange(cell1, cell2);
//            //    VExcelCell tagCell = cell1.Row.Cell(cell2.Index() + 1);
//            //    //  tagCell.Insert(range);
//            //    // tagCell.Insert(range);
//            //    // tagCell.Insert(range);
//            //    range.Remove();
//            //}


//            //  VExcelRange ra = new VExcelRange(sh.Row(2).Cell(1), sh.Row(3).Cell(2));

//            // cc.Insert(ra);


//            //wb.Save(@"C:\tfs\all\sql.builder\sql.builder\printTemplate\excel\25499.xml-test.xml");
//        }
//        /// <summary>
//        /// Реакция на кнопку "Колонки для excel"
//        /// </summary>
//        private void btnColsInfoForExcel_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucMainReports>().ColsInfoForExcel();
//        }
//        private void ColsInfoForExcel()
//        {
//            if (CurrentGC == null) return;
//            VDataSet ds = CurrentGC.Grid.DataSource;
//            string s1 = "";
//            foreach (DataTable tbl in ds.Tables)
//            {
//                s1 += tbl.TableName;
//                s1 += Environment.NewLine;

//                List<string> grSets = new List<string>();


//                if (tbl.Columns.Contains(TextConst.AVSpecColumnGrset.GrSetName))
//                {
//                    grSets = tbl.Rows.Cast<DataRow>().Select(r => r[TextConst.AVSpecColumnGrset.GrSetName].ToString()).Distinct().ToList();
//                }

//                if (grSets.Count == 0)
//                {
//                    grSets.Add("");
//                }

//                foreach (string gr in grSets)
//                {
//                    var rgvar = "";
//                    if (gr != "")
//                    {
//                        rgvar = "." + gr;
//                    }
//                    foreach (DataColumn col in tbl.Columns)
//                    {
//                        var colVar = "[:" + tbl.TableName + rgvar + "." + col.ColumnName + "]";
//                        s1 += colVar + "\t";
//                    }
//                    s1 += Environment.NewLine;
//                }


//            }
//            // string s1 = string.Join(((char)9).ToString(), ds.SchemeNative.Elements("table").First().Elements("columns").Elements().Select(e1 => "value:" + e1.Ancestors("table").First().Attribute("as").Value + "." + e1.Attribute("name").Value).ToArray());
//            Cmn.TxtOutput(s1, "columnsForTemplate");
//        }
//        private static void WriteAndShowXML(XElement data, string file_name)
//        {
//            XmlWriterSettings settings = new XmlWriterSettings();
//            settings.Indent = true;
//            using (XmlWriter writer = XmlWriter.Create(file_name, settings))
//            {
//                data.WriteTo(writer);
//                writer.Close();
//            }
//            Process.Start(file_name);
//        }
//        /// <summary>
//        /// Реакция на кнопку "Посмотреть XML схему"
//        /// </summary>
//        private void btnShowXMLSchema_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                VDataSet ds = ctrl.CurrentGC.Grid.DataSource;
//                if (ds != null)
//                {
//                    //XElement scheme = new XElement(ds.SchemePreset);
//                    string file_name = Printing.GetFreeName(Path.GetTempPath(), "scheme", "xml");
//                    WriteAndShowXML(ds.SchemePreset, file_name);
//                }
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Очистить реестр"
//        /// </summary>
//        private void btnClearRegistry_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(Environment.UserName + @"\sql.builder");
//        }
//        /// <summary>
//        /// Реакция на кнопку "Открыть стартовую директорию"
//        /// </summary>
//        private void btnOpenStartupPath_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            Process.Start(System.Windows.Forms.Application.StartupPath);
//        }
//        #endregion
//        #endregion
//        #region Обработчики событий внешних компонентов
//        // выбранный отчет изменился
//        private void ucReportTree_OnCurrentNodeChanged()
//        {
//            Dictionary<string, string> setting = this.cReportsTree.GetFocusedReport();
//            string report_name = setting["repname"];
//            if (UIStatic.IsMpep)
//            {
//                if (!this.cReportsTree.IsInFolder("ext_data") && !this.cReportsTree.IsInFolder("nastr")
//                    /*
//                    && !cReportsTree.IsInFolder("vbf_spr_direct_sap")
//                    && !cReportsTree.IsInFolder("po_algoritm")
//                    && !cReportsTree.IsInFolder("vb_spr_vid_dey")
//                    && !cReportsTree.IsInFolder("vb_spr_vid_energ")
//                    && !cReportsTree.IsInFolder("52072_data_check")
//                    && !cReportsTree.IsInFolder("vb_utv_periods")
//                    */ )
//                {
//                    string v = db.ExecuteObject("SELECT nvl(max(1),0) as val FROM V$VERSION where BANNER like '%11%'").ToString();
//                    if (v == "0")
//                    {
//                        ShowMessage.ShowError("Текущая версия oracle не поддерживается");
//                        return;
//                    }
//                }
//            }
//            string item_type = setting["item_type"];
//            if (item_type == TextConst.EName.UseForm)
//            {
//                //if (!_form_containers.ContainsKey(setting["repname"]))
//                //{
//                //    ucReportSettingsTree.ReloadData(setting["repname"]);
//                //}
//                //else
//                //{
//                //setting["form"] = report["form"];
//                ShowContainer(setting, false, null);
//                //}
//                //if (setting["item_type"] == TextConst.EName.UseForm)
//                //{
//                //    var uiform = ucDataEditorMain.CreateForm(setting["repname"], null, false);
//                //    var testFrm = new frmDynamicEditor();

//                //    testFrm.Text = uiform.Title;
//                //    testFrm.Name = uiform.FormName;
//                //    testFrm.Controls.Add(uiform);
//                //    uiform.ApplyVisibitlity();
//                //    testFrm.Show();
//                //    return;
//                //}
//            }
//            else
//            {
//                if (!this._grid_containers.ContainsKey(report_name) && (item_type != "nogrid"))
//                {
//                    this.cReportsTree.ReloadData(report_name);
//                }
//                else
//                {
//                    this.ShowContainer(setting, false, null);
//                }

//                XElement xreport = XmlReports.GetReport(setting["original_name"]);
//                var xmlreport = new XmlDocument();
//                xmlreport.LoadXml(xreport.ToString());
//                this.GeneratePrintTemplates(xmlreport.FirstChild.SelectNodes("print-templates//template"));
//            }
//            this.RefreshButtonsStates();
//        }
//        // поменялась настройка: надо загрузить новую
//        private void ucReportTree_OnLoadGridSettings(XElement xml, string report_name)
//        {
//            if (_report_changing) return;

//            _report_changing = true;

//            Dictionary<string, string> setting = cReportsTree.GetReport(report_name);
//            //var report = cReportsTree.GetMainReport(report_name);

//            XmlReports.Environment.Manager.LoadProjectIfNeed(setting["project"]);
//            ShowContainer(setting, false, xml);

//            //RefreshButtonsStates();

//            // для справочников обновляем данные сразу
//            if (CurrentGC != null && CurrentGC.Mode == ucReportContainer.GridMode.ReferenceGrid && CurrentGC.Grid.DataSource == null)
//            {
//                btnExecuteReport.PerformClick();
//            }

//            _report_changing = false;
//        }
//        // необходимо вернуть текущие настройки грида
//        private XElement ucReportTree_OnNeedSettingData(SettingsType type)
//        {
//            if (CurrentGC == null) return null;

//            return CurrentGC.SaveToXml(type);
//        }
//        //текущая настройка изменила состояние: измененная/неизмененная
//        //private void ucReportSettingsTree_OnReportLayoutChanged(bool changed)
//        //{
//        //    RefreshButtonsStates();
//        //}
//        //необходимо пометить текущую настройку как измененную
//        private void ucVGridBase_LayoutChanged2(object sender, EventArgs args)
//        {
//            ucVGridBase_LayoutChanged();
//        }
//        private void ucVGridBase_LayoutChanged()
//        {
//            if (CurrentGC == null) return;

//            if (CurrentGC.Grid.IsTemplate && !CurrentGC.Grid.FromFile && cReportsTree != null)
//            {
//                cReportsTree.SetReportLayoutChanged(CurrentGC.Grid.ReportName, true);
//                RefreshButtonsStates();
//            }
//        }
//        // если был удален шаблон - закрываем открытые отчеты
//        private void ucReportTree_OnReportNodeDeleted()
//        {
//            TreeListNode node = this.cReportsTree.GetFocusedNode();
//            Contract.Assume(node != null);
//            string report_name = node.Field<string>("name");
//            ucReportContainer gc;
//            if (this._grid_containers.TryGetValue(report_name, out gc))
//            {
//                // получаем документ, к которому привязана панель
//                var doc = (Document)dmReports.GetDocument(gc);
//                dmView.Controller.Close(doc);
//            }
//        }
//        private void ucReportTree_OnReportNeedUpdate()
//        {
//            TreeListNode node = this.cReportsTree.GetFocusedNode();
//            Contract.Assume(node != null);
//            string report_name = node.Field<string>("name");
//            ucReportContainer gc;
//            if (this._grid_containers.TryGetValue(report_name, out gc))
//            {
//                // получаем документ, к которому привязана панель
//                var doc = (Document)this.dmReports.GetDocument(gc);
//                doc.Caption = node.Field<string>("title");
//            }
//        }
//        #endregion
//        /// <summary>
//        /// Реакция на кнопку "XViewColumns"
//        /// </summary>
//        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports _this = this.GetCurrentControl<ucMainReports>();
//            if (_this.CurrentGC == null || _this.CurrentGC.Mode != ucReportContainer.GridMode.ReportGrid)
//            {
//                return;
//            }
//            string current_table_name = (_this.CurrentGC.Grid).GetTopTableName();
//            XElement xViewColumns_preset = _this.CurrentGC.Grid.DataSource.SchemePreset
//                    .Elements(EName.table)
//                    .First(tbl => tbl.Attribute(AName.@as).Value == current_table_name)
//                    .Element(EName.viewcolumns);
//            XElement xviewcolumns = new XElement(EName.columns);
//            xviewcolumns.Add(xViewColumns_preset.Elements());
//            xviewcolumns.Descendants().Attributes().Where(a => a.Name != AName.name && a.Name != AName.title).ToList().Remove();
//            Cmn.HtmlOutput(xviewcolumns.ToString(), "ViewColumns");
//        }
//        private void rceUseRepository_EditValueChanging(object sender, ChangingEventArgs e)
//        {
//            if (CurrentGC == null) return;
//            GetCurrentControl<ucMainReports>().CurrentGC.UseRepository = (int)e.NewValue;
//        }
//        /// <summary>
//        /// Реакция на кнопку "Наш экспорт в Excel"
//        /// </summary>
//        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().testPrintExcel();
//        }
//        private void testPrintExcel()
//        {
//            if (_current_container == null || CurrentGC == null) return;

//            if (!CheckWorkFolder())
//            {
//                return;
//            }

//            if (CurrentGC.Grid.DataSource == null || !CurrentGC.Grid.DataSource.IsRefreshed || !CurrentGC.Actual) ExecuteReport();

//            // создание шаблона и печать по отчету
//            XmlDocument doc = new XmlDocument();
//            doc.LoadXml(string.Format(@"<template name=""{0}.xml"" title=""{1}"" print-proc=""2"" />", CurrentGC.Grid.ReportName, CurrentGC.Grid.ReportTitle));

//            XDocument excelTemplateCreateExcelTemplate = ExcelTemplate.CreateExcelTemplate(CurrentGC.Grid);
//            this.PrintForm(doc.DocumentElement.CloneNode(false), null, excelTemplateCreateExcelTemplate, true, false, false);
//        }
//        //private void WordPrintTest()
//        //{
//        //if (_current_gc == null) return;

//        //var input_path1 = Path.Combine(XmlReports.GetVSProjectPath(), @"sql.builder\printTemplate\word", @"Иск1об_VL_RKC.dot");
//        //var input_path2 = Path.Combine(XmlReports.GetVSProjectPath(), @"sql.builder\printTemplate\word", @"isk.dot");
//        //var out_path = Path.Combine(Printing.outputFolder, "word_test.docx");

//        //var view = _current_gc.Grid.MainView;
//        //var selected_rows = view.GetSelectedRows().Select(i => view.GetDataRow(i));
//        //if (!selected_rows.Any()) return;

//        //var ds2 = new DataSet();
//        //ds2.Tables.Add(new DataTable());
//        //ds2.Tables[0].Columns.AddRange(new[]
//        //{
//        //    new DataColumn("abon_name"),
//        //    new DataColumn("num_reg"),
//        //    new DataColumn("sum"),
//        //    new DataColumn("dat_dog"),
//        //    new DataColumn("ym_s"),
//        //    new DataColumn("ym_po"),
//        //    new DataColumn("zadol")
//        //});
//        //ds2.Tables[0].Rows.Add("Тут ФИО", "номерр", "100400", "01.01.20001", "2001.20", "4999.54", "11$");

//        //var dogs = new Dictionary<string, string>();
//        //dogs.Add("@@номер_суд_участка", "sud_name");
//        //dogs.Add("@@ответчик", "abon_name");
//        //dogs.Add("@@Адрес_абонента_без_инд_СФ", "abon_adr_fiz");
//        //dogs.Add("@@Дата_док", "dat_doc");
//        //dogs.Add("@@Номер_документа", "num_doc");
//        //dogs.Add("@@период_с", "ym_s");
//        //dogs.Add("@@период_по", "ym_po");
//        //dogs.Add("@@сумма_задолженности_по_основной_реализации", "summa_osn");
//        //dogs.Add("@@сумма_задолженности_по_пеням", "summa_peny");
//        //dogs.Add("@@сумма_задолженности_общая", "summa_no_gp");
//        //dogs.Add("@@Сумма_ГП", "summa_gp");

//        //dogs.Add("@@Абонент", "abon_name");
//        //dogs.Add("@@Номер", "num_reg");
//        //dogs.Add("@@сумма_задолженности", "sum");
//        //dogs.Add("@@№дог_дата", "dat_dog");

//        //var options = new ConvertOptions() {DecodeVars = dogs};
//        //var template1 = TemplatesConverter.GetWordTemplate(input_path1, options);
//        //var input_stream2 = new FileStream(input_path2, FileMode.Open);
//        //var template2 = TemplatesConverter.GetWordTemplate(input_path2, options);

//        //var sw = new Stopwatch();
//        //sw.Start();

//        //using (var word = new WordReport())
//        //{
//        //    foreach (var row in selected_rows)
//        //    {
//        //        word.Print(template1, row);
//        //    }
//        //    word.Print(template2, ds2);

//        //    word.SaveToFile(out_path);
//        //}

//        //sw.Stop();

//        //XtraMessageBox.Show(string.Format("Затрачено {0}:{1}:{2}.{3}",
//        //    sw.Elapsed.Hours.ToString("D2"), sw.Elapsed.Minutes.ToString("D2"),
//        //    sw.Elapsed.Seconds.ToString("D2"), sw.Elapsed.Milliseconds.ToString("D3")), "");

//        //input_stream2.Close();
//        //Process.Start(out_path);
//        //}
//        /// <summary>
//        /// Реакция на кнопку "Печать Word"
//        /// </summary>
//        private void btnWordPrintTest_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //GetCurrentControl<ucMainReports>().WordPrintTest();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Сформировать Vertica"
//        /// </summary>
//        private void btnExecuteVertica_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC == null) return;

//            ctrl.CloseReport(ctrl.CurrentGC.Grid.ReportName);
//            //ctrl.CurrentGC.Grid.DataSource.IsVertica = true;
//            //ctrl.ExecuteReport();
//            //ctrl.CurrentGC.Grid.DataSource.IsVertica = false;
//        }
//        /// <summary>
//        /// Реакция на кнопку "Тест размазывания колонок"
//        /// </summary>
//        private void btnTest_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            XmlNode template = (XmlNode)dbtnPrintForm.Tag;
//            if (template != null && ctrl.CurrentGC != null)
//            {
//                ctrl.PrintFormNew(template, null, true, onlyColumns: true, load_devexpress: false, force_refresh: false);
//            }
//            //var report = new ExpressReport();
//            //report.Initialize("33386");
//            //report.GetParamField("dep").SetValue(1330M);
//            //report.GetParamField("kod_mat").SetValue(1877M);
//            //report.GetParamField("kod_folders").SetValue(21351M);
//            //report.GetParamField("urist").SetValue("Юрка");
//            //report.Show("33386");
//        }
//        /// <summary>
//        /// Реакция на кнопку "Загрузить параметры из лога"
//        /// </summary>
//        private void btnLoadLogParams_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //SqlBuilder.PreviewReport("41050");
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                using (var frm = new frmParamsHistory())
//                {
//                    frm.Initialize(ctrl.CurrentGC.Grid.ReportName);
//                    if (frm.ShowDialog() == DialogResult.Yes)
//                    {
//                        XElement xroot = frm.GetSelectedParams();
//                        if (xroot != null && xroot.Name == EName.root)
//                        {
//                            ctrl.CurrentGC.LoadFromXml(xroot, SettingsType.Params);
//                        }
//                    }
//                }
//            }
//        }
//        internal void InvokeIfNeed(MethodInvoker action)
//        {
//            if (this.IsDisposed) return;

//            if (this.InvokeRequired) this.Invoke(action);
//            else action();
//        }

//        private void rceShowInvisibleReports_CheckedChanged(object sender, EventArgs e)
//        {
//            bool visible = ((CheckEdit)sender).Checked;
//            GetCurrentControl<ucMainReports>().SetInvisibleReportsVisible(visible);
//        }

//        internal void SetInvisibleReportsVisible(bool visible)
//        {
//            _show_invisible_reports = visible;
//            SettingsHelper.ShowInvisibleReports = visible;
//            cReportsTree.SetInvisibleReportsVisible(visible);
//        }
//        /// <summary>
//        /// Реакция на кнопку "Перезагрузить список"
//        /// </summary>
//        private void btnReloadNavigator_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            reloadList();
//            //Wait.ForceHide();
//        }

//        private void reloadList()
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            ctrl.dmReports.View.Controller.CloseAll();
//            if (ctrl.cReportsTree != null)
//            {
//                ctrl.cReportsTree.ReadExpandedState();
//                ctrl.cReportsTree.ReloadAllWithoutData(true);
//            }
//            VCashUtils.ClearCash();
//            WaitUIHelper.LastUsedUIHelper.ForceHide();
//        }
//        private void OnOpenExcel(object sender, EventArgs args)
//        {
//            bool tmp = Printing.CreateRefs;
//            Printing.CreateRefs = true;
//            this.CurrentGC.UpdateGridEvents();
//            BarLinkContainerItem dbtn = this.GetRibbonSource<ucMainReports>().dbtnPrintForm;
//            XmlNode template = dbtn.Tag as XmlNode;
//            if (template != null)
//            {
//                string output_path = this.PrintForm(template, null, null, false, false, false);
//                this.CurrentGC.Grid.ShowExcel(output_path);
//            }
//            Printing.CreateRefs = tmp;
//        }
//        private void btnTFSAutoCheckIn_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            using (var server = new TFSServer(false))
//            {
//                var controller = new AutoCheckInController(server);
//                controller.AddCustomAlwaysLockalPaths(XmlReports.Environment.Manager.GetAllProjects().Select(p => p.FileNativePath));
//                controller.AddCustomAlwaysLockalPaths(XmlReports.Environment.Manager.GetAllProjects().Select(p => p.FileCompiledPath));
//                using (var frm = new frmAutoCheckIn(controller))
//                {
//                    frm.ShowDialog();
//                }
//            }
//        }
//        private void btnGroupEditor_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucMainReports>().ShowGroupsEditor();
//        }
//        /// <summary>
//        /// Реакция на кнопку "Данные для АСФО"
//        /// </summary>
//        private void barBtnShowMsbiMenu_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ShowForm<FormMSBI>();
//        }
//        /// <summary>
//        /// Реакция на кнопку "ParsXml"
//        /// </summary>
//        private void btnParametersXML_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                XElement parameters = new XElement(ctrl.GetReport().GetElementsP(EName.@params).First());
//                IList<XElement> xparvals = ctrl.CurrentGC.ParamFormC.GetValue().Elements().ToList();
//                foreach (XElement formal_param in parameters.Elements(EName.param))
//                {
//                    string name = formal_param.AttrOrDefault(AName.name, null);
//                    formal_param.RemoveNodes();
//                    for (int index = 0; index < xparvals.Count; index++)
//                    {
//                        XElement fact_param = xparvals[index];
//                        if (fact_param.AttrOrEmpty(AName.name) == name)
//                        {
//                            formal_param.Add(fact_param.Elements());
//                            break;
//                        }
//                    }
//                }
//                string file_name = Printing.GetFreeName(Path.GetTempPath(), "pars", "xml");
//                WriteAndShowXML(parameters, file_name);
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Получить SQL"
//        /// </summary>
//        private void btnGenerateSQL_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            string sql = ctrl.getReportSql();
//            Cmn.SqlOutput(sql);
//        }
//        //private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
//        //{
//        //	thisControl<ucMainReports>().registerDataExtract();
//        //}
//        /// <summary>
//        /// Реакция на кнопку "Зарегистрировать представление"
//        /// </summary>
//        private void btnRegExtact_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            if (ctrl.CurrentGC != null)
//            {
//                var frm = new sql.builder.VForms.VFrmRegExtract();
//                if (frm.Show() == DialogResult.OK)
//                {
//                    string extract_name = frm.newName;
//                    string project = frm.curProj;
//                    VDataSet ds = ctrl.CurrentGC.Grid.DataSource;
//                    XElement scheme = new XElement(ds.SchemePreset);
//                    string sql = ctrl.getReportSql();
//                    string fixedNamesSql = DashboardUtils.DataExtractHelper.MergeSqlScheme(sql, scheme);
//                    DashboardUtils.DataExtractHelper.RegisterDataExtract(fixedNamesSql, project, extract_name, "true");

//                }
//            }
//        }
//        /// <summary>
//        /// Реакция на кнопку "Генерация ЛКК"
//        /// </summary>
//        private void btnGenerateLKK_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            CodeGenerationUtilsLKK.Generate();
//        }
//        /// <summary>
//        /// Реакция на кнопку "АСУТП Пакеты"
//        /// </summary>
//        private void btnASUTPPackage_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            PackageGenerationASUTP.Generate(false);
//        }
//        /// <summary>
//        /// Реакция на кнопку "АСУТП описание"
//        /// </summary>
//        private void btnASUTPDoc_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            DokGenerationASUTP.GeterateDoc();
//        }
//        /// <summary>
//        /// Реакция на кнопку "АСУТП Пакеты (Т)"
//        /// </summary>
//        private void btnASUTPPackageT_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            PackageGenerationASUTP.Generate(true);
//        }

//        private void rceDevMode_EditValueChanged(object sender, EventArgs e)
//        {
//            var value = ((CheckEdit)sender).Checked;
//            WebReportsAdapter.Enabled = value;
//            reloadList();
//        }

//        private void btnDevPrepareData_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            WebReportsAdapter.DevPrepareData(ctrl.CurrentGC);
//        }

//        private void btnDevPrint_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ucMainReports ctrl = this.GetCurrentControl<ucMainReports>();
//            WebReportsAdapter.DevExecuteView(ctrl.CurrentGC);
//        }


    }
}