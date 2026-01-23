//using System;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.XtraEditors;
////using DevExpress.XtraSpreadsheet.Internal;
//using Microsoft.WindowsAPICodePack.Dialogs;
//using sql.builder.Controls;
//using sql.builder.Controls.Containers;
//using sql.builder.Controls.Testing;
//using sql.builder.DataApi;
//using sql.builder.XmlHelpers;

//namespace sql.builder.WinForms
//{
//    internal partial class frmWCFCompare : XtraForm, IWCFServer
//    {
//        DataTable _dtLog;

//        Process _clientProcess;
//        Task _workTask;

//        ucMainReports _control;

//        internal VDataSet dsServer;
//        internal VDataSet dsClient;

//        ServerCommand _clientCommand;

//        internal frmWCFCompare(ucMainReports control)
//        {
//            InitializeComponent();
//            _control = control;

//            _dtLog = new DataTable();
//            _dtLog.Columns.AddRange(new []
//            {
//                new DataColumn("time", typeof (DateTime)),
//                new DataColumn("text", typeof (string))
//            });

//            grLog.DataSource = _dtLog;
//        }

//        private void frmWCFCompare_Load(object sender, EventArgs e)
//        {
//            beClientPath.Text = SettingsHelper.CompareFolder;
//        }

//        public void AddToLog(string text)
//        {
//            _dtLog.Rows.Add(DateTime.Now, text);

//            viewLog.RefreshData();
//            viewLog.TopRowIndex = 0;

//            Application.DoEvents();
//        }

//        public void SendMessage(string text)
//        {
//            AddToLog(text);
//        }

//        public XElement GetReportParams()
//        {
//            var xroot = new XElement("root");
//            Parser.SaveReportParamsToXml(xroot, _control.CurrentGC.ParamFormC);

//            return xroot;
//        }

//        public string GetWorkFolder()
//        {
//            return null;
//        }

//        void IWCFServer.SetClientState(ClientState state)
//        {
//            if(state == ClientState.ExecutingReport)
//            {
//                Activate();

//                if (_control.CurrentGC.Mode == ucReportContainer.GridMode.NoGrid)
//                {
//                    _control.CurrentGC.ChangeGridMode(ucReportContainer.GridMode.ReportGrid);
//                }

//                _control.ExecuteReport();

//                dsServer = _control.CurrentGC.Grid.DataSource;

//                AddToLog("Отчёт на сервере успешно сформирован");
//            }
//        }

//        void IWCFServer.SetClientData(VDataSet data)
//        {
//            dsClient = data;
//        }

//        void IWCFServer.SetReportTime(string time)
//        {
//            //
//        }

//        ServerCommand IWCFServer.GetCommand()
//        {
//            var cmd = _clientCommand;
//            _clientCommand = ServerCommand.Wait;
//            return cmd;
//        }

//        public string GetReportName()
//        {
//            return _control.CurrentGC.Grid.ReportName;
//        }

//        private void Start()
//        {
//            if (_control.CurrentGC.Grid.IsCompareMode || beClientPath.EditValue == null) return;

//            AddToLog("Запуск сервера...");
//            WCFHelper.StartServer(this);
//            AddToLog("Сервер запущен");

//            AddToLog("Запуск клиента и ожидание ответа...");

//            _clientProcess = new Process();
//            _clientProcess.StartInfo.FileName = Path.Combine(SettingsHelper.CompareFolder, "sql.builder.exe");
//            _clientProcess.StartInfo.Arguments = string.Format("-client -sn=\"{0}\"", WCFHelper.ServiceName);
//            _clientProcess.Start();

//            _workTask = Task.Factory.StartNew(WCFServerProcess);

//            AddToLog("Начало формирования отчёта на сервере...");
//        }

//        private void Cancel()
//        {
//            _clientCommand = ServerCommand.CloseApplication;
//            //TopMost = false;

//            if (_clientProcess != null)
//            {
//                _clientProcess.Kill();
//                _clientProcess = null;
//            }

//            // чтобы процесс на сервере
//            if (dsClient == null)
//            {
//                dsClient = new VDataSet();
//            }

//            if (_workTask != null)
//            {
//                _workTask = null;
//            }

//            WCFHelper.ShutdownServer();

//            //_dtLog.Clear();
//        }

//        private void btnStart_Click(object sender, EventArgs e)
//        {
//            btnStart.Enabled = false;
//            Start();
//            btnCancel.Enabled = true;
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            btnCancel.Enabled = false;
//            Cancel();
//            btnStart.Enabled = true;
//        }

//        private void WCFServerProcess()
//        {
//            while (dsClient == null || dsServer == null)
//            {
//                //Thread.Sleep(1000);
//            }

//            if (dsClient.Tables.Count == 0) return;

//            InvokeIt(() => AddToLog("Данные с клиента получены. Начало сравнения..."));

//            _control.InvokeIfNeed(() =>
//            {
//                _control.CurrentGC.Grid.SetComparedMode(Cmn.ToVDataSet(dsClient));

//                InvokeIt(() =>
//                {
//                    AddToLog("Сравнение произведено");
//                    btnCancel.PerformClick();
//                    Activate();
//                });
//            });

//            _clientCommand = ServerCommand.CloseApplication;
//        }

//        internal void InvokeIt(MethodInvoker action)
//        {
//            if (this.IsDisposed) return;

//            if (this.InvokeRequired) this.Invoke(action);
//            else action();
//        }

//        private void beClientPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
//        {
//            //using (var dlg = new FolderBrowserDialog())
//            //{
//            //    dlg.SelectedPath = beClientPath.Text;
//            //    if (dlg.ShowDialog() == DialogResult.OK)
//            //    {
//            //        SettingsHelper.CompareFolder = dlg.SelectedPath;

//            //        string filepath = dlg.SelectedPath;
//            //        beClientPath.EditValue = filepath;
//            //    }
//            //}

//            // говорят на WIN XP не работает, но у нас же продвинутые заказчики
//            using (var dlg = new CommonOpenFileDialog())
//            {
//                dlg.IsFolderPicker = true;
//                dlg.InitialDirectory = beClientPath.Text;
//                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
//                {
//                    string filepath = dlg.FileName;
//                    SettingsHelper.CompareFolder = filepath;
//                    beClientPath.EditValue = filepath;
//                }
//            }
//        }
//    }
//}
