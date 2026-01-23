//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.ui.win;
//using infoenergo.ui.win.Forms;
//using sql.builder.Controls;
//using sql.builder.XmlHelpers;

//namespace sql.builder.WinForms
//{
//    internal partial class frmTestReports : FormBase
//    {
//        #region Закрытые переменные
//        private ucMainReports _form;

//        private DateTime _begin_time;
//        private Dictionary<string, string>[] _report_names;

//        private bool _working;
//        private bool _closed;

//        private BackgroundWorker bw;
//        private DataTable _dt_log;
//        #endregion
//        #region Открытые методы
//        public frmTestReports(ucMainReports form, bool start_immediately = false, bool only_opened = true)
//        {
//            InitializeComponent();
//            _form = form;

            
//            _report_names = _form.GetReports(only_opened);

//            // obsolete
//            //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

//            _dt_log = new DataTable();
//            _dt_log.Columns.AddRange(new[]
//            {
//                new DataColumn("status", typeof(int)),
//                new DataColumn("time", typeof(DateTime)),
//                new DataColumn("report", typeof(string)),
//                new DataColumn("text", typeof(string)),
//            });
//            grLog.DataSource = _dt_log;

//            bw = new BackgroundWorker();
//            bw.DoWork += DoWork;

//            if(start_immediately) Start();
//        }
//        //public void ShowErrors(Tuple<string, string>[] errors)
//        //{
//        //    InvokeIfNeed(() =>
//        //    {
//        //        foreach (var error in errors)
//        //        {
//        //            _dt_log.Rows.Add(1, DateTime.Now, error.Item1, string.Format("Отчёт загружен с ошибками:{1}{0}", error.Item2, Environment.NewLine));
//        //        }
//        //    });
//        //}
//        public void Start()
//        {
//            if (!_working)
//            {
//                _dt_log.Rows.Clear();

//                pbProgress.Properties.Maximum = _report_names.Length;
//                pbProgress.Position = 0;
//                bw.RunWorkerAsync();
//            }
//        }
//        #endregion
//        #region Закрытые методы
//        private void DoWork(object sender, DoWorkEventArgs e)
//        {
//            _working = true;

//            int errors_count = 0;
//            for (int i = 0; i < _report_names.Length; i++)
//            {
//                if (_closed) break;

//                _begin_time = DateTime.Now;

//                AppendBegining(_report_names[i]["repname"], _report_names[i]["title"]);

//                InvokeIt(() =>
//                {
//                    viewLog.RefreshData();
//                    viewLog.TopRowIndex = 0;
//                    lDebug.Text = _report_names[i]["title"];
//                    timer.Start();
//                });

//                var result = (Tuple<bool, string>) _form.Invoke((Func<object>)(() =>
//                {
//                    Logger.BeginSession();

//                    Logger.LogEvent += (data) =>
//                    {
//                        AppendAnyText(_report_names[i]["title"], data);
//                        InvokeIt(() =>
//                        {
//                            viewLog.RefreshData();
//                            viewLog.TopRowIndex = 0;
//                        });
//                    };

//                    var res = _form.ShowAndExecute(_report_names[i]["repname"]);
//                    _form.CloseReport(_report_names[i]["repname"]);

//                    Logger.EndSession();

//                    return res;
//                }));

//                AppendResult(_report_names[i]["title"], result);
//                if (!result.Item1) errors_count ++;

//                InvokeIt(() =>
//                {
//                    viewLog.RefreshData();
//                    viewLog.TopRowIndex = 0;
//                    pbProgress.Position++;
//                    timer.Stop();
//                });
//            }

//            AppendFinish(errors_count);
//            InvokeIt(() =>
//            {
//                viewLog.RefreshData();
//                viewLog.TopRowIndex = 0;
//            });

//            _working = false;
//        }
//        private void InvokeIt(MethodInvoker action)
//        {
//            if (this.IsDisposed) return;

//            if (this.InvokeRequired) this.Invoke(action);
//            else action();
//        }
//        private void AppendBegining(string report_name, string report_title)
//        {
//            var text = "Начало формирования отчёта...";
//            var text2 = Cmn.GetAvgReportFormingTime(report_name);
//            if (text2 != null) text = text + " (ожидаемое время ~" + text2 + ")";

//            _dt_log.Rows.Add(0, DateTime.Now, report_title, text);
//        }
//        private void AppendFinish(int errors_count)
//        {
//            _dt_log.Rows.Add(0, DateTime.Now, "Проверка завершена", "Ошибок: " + errors_count);
//        }
//        private void AppendResult(string report_title, Tuple<bool, string> result)
//        {
//            if (result.Item1)
//            {
//                _dt_log.Rows.Add(0, DateTime.Now, report_title, string.Format("Отчёт успешно сформирован"));
//            }
//            else
//            {
//                _dt_log.Rows.Add(1, DateTime.Now, report_title, string.Format("Отчёт не сформирован.{0}{1}", Environment.NewLine, result.Item2));
//            }
//        }

//        private void AppendAnyText(string report_title, Tuple<DateTime, string> data)
//        {
//            _dt_log.Rows.Add(0, data.Item1, report_title, data.Item2);
//        }

//        #endregion
//        #region Обработчики событий
//        private void frmTestReports_FormClosed(object sender, FormClosedEventArgs e)
//        {
//            _closed = true;
//        }
//        private void btnBeginTest_Click(object sender, EventArgs e)
//        {
//            Start();
//        }
//        private void timer_Tick(object sender, EventArgs e)
//        {
//            var time_span = (DateTime.Now - _begin_time);

//            lTime.Text = String.Format("Затраченное время {0}:{1:00}:{2:00}",
//                time_span.Hours, time_span.Minutes, time_span.Seconds);
//        }

//        private void viewLog_ShowFilterPopupListBox(object sender, FilterPopupListBoxEventArgs e)
//        {
//            if (e.Column.FieldName == "status")
//            {
//                e.ComboBox.DrawItem += ComboBox_DrawItem;
//            }
//        }

//        void ComboBox_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
//        {
//            var item = e.Item as FilterItem;
//            if (item == null) return;

//            if (item.Value is FilterItem) return;

//            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
//                e.Appearance.BackColor = Cmn.GetHighlightColor();

//            e.Appearance.FillRectangle(e.Cache, e.Bounds);

//            foreach (ImageComboBoxItem comboItem in riiStatus.Items)
//            {
//                if (comboItem.Value.Equals(item.Value))
//                {
//                    e.Graphics.DrawImage(imageCollection.Images[comboItem.ImageIndex], e.Bounds.Location);
//                    break;
//                }
//            }
//            e.Handled = true;
//        }
      
//        #endregion
//    }
//}
