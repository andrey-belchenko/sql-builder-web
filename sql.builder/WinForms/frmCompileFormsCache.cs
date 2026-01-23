//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using System.Threading;
////using DevExpress.Export.Xl;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.ui.win;
//using infoenergo.ui.win.Forms;
//using sql.builder.Controls;
//using sql.builder.DataApi;

//namespace sql.builder.WinForms
//{
//    /// <summary>
//    /// Форма перекомпиляции форм
//    /// </summary>
//    internal partial class frmCompileFormsCache : FormBase
//    {
//        #region Закрытые переменные
//        private DateTime _begin_time;
//        private VSourcedElement[] _vforms;
//        private BackgroundWorker bw;
//        private DataTable _dt_log;
//        #endregion
//        #region Открытые методы
//        internal frmCompileFormsCache(bool start_immediately)
//        {
//            this.InitializeComponent();
//            IList<VSXElement> forms = ucDataEditorMain.GetFormsList();
//            int count = forms.Count;
//            this._vforms = new VSourcedElement[count];
//            for (int index = 0; index < count; index++) {
//                this._vforms[index] = (VSourcedElement)forms[index];  // здесь либо VForm, либо VQuery
//            }
//            this.pbProgress.Properties.Maximum = count;
//            // obsolete
//            this._dt_log = new DataTable();
//            this._dt_log.Columns.Add("status", typeof(int));
//            this._dt_log.Columns.Add("time", typeof(DateTime));
//            this._dt_log.Columns.Add("report", typeof(string));
//            this._dt_log.Columns.Add("text", typeof(string));
//            this.grLog.DataSource = this._dt_log;
//            //
//            this.bw = new BackgroundWorker();
//            this.bw.WorkerSupportsCancellation = true;
//            this.bw.DoWork += this.DoWork;
//            if (start_immediately) {
//                this.Start();
//            }
//        }
//        private void Start()
//        {
//            if (!this.bw.IsBusy) {
//                this._dt_log.Rows.Clear();
//                this.pbProgress.Position = 0;
//                this.bw.RunWorkerAsync();
//            }
//        }
//        #endregion
//        #region Закрытые методы
//        private void RefreshLog()
//        {
//            this.viewLog.RefreshData();
//            this.viewLog.TopRowIndex = 0;
//            TimeSpan time_span = (DateTime.Now - this._begin_time);
//            this.lTime.Text = "Затраченное время " + time_span.Hours.ToString("D2") + ":" + time_span.Minutes.ToString("D2") + ":" + time_span.Seconds.ToString("D2");
//        }
//        private void BeginForm(string form_name)
//        {
//            this.AppendSuccess(form_name, "Начало компиляции формы...");
//            this.RefreshLog();
//            this.lDebug.Text = form_name;
//        }
//        private void EndFormSuccess(string form_name)
//        {
//            this.AppendSuccess(form_name, "Форма успешно скомпилирована");
//            this.RefreshLog();
//            this.pbProgress.Position++;
//        }
//        private void EndFormError(string form_name, string error_message)
//        {
//            this.AppendError(form_name, "Форма не скомпилирована." + Environment.NewLine + error_message);
//            this.RefreshLog();
//            this.pbProgress.Position++;
//        }
//        private void Finish(string message)
//        {
//            this.AppendSuccess("Компиляция завершена", message);
//            this.RefreshLog();
//        }
//        private void DoWork(object sender, DoWorkEventArgs e)
//        {
//            int errors_count = 0;
//            this._begin_time = DateTime.Now;
//            for (int index = 0; index < this._vforms.Length; index++) {
//                if (this.bw.CancellationPending) {
//                    break;
//                }
//                VSourcedElement vform = this._vforms[index];
//                this.InvokeIt(this.BeginForm, vform.P_Name);
//                //XElement xform = null;
//                try {
//                    vform.SetTimeStamp();
//                    //xform = vform.CreateDataSetAndFormInfo(false);
//                    VForm.GetFormXelementAndDataSet(vform.XName);
//                    this.InvokeIt(this.EndFormSuccess, vform.P_Name);
//                } catch (Exception ex) {
//                    this.InvokeIt(this.EndFormError, vform.P_Name, ex.Message);
//                    errors_count++;
//                }
//            }
//            this.InvokeIt(this.Finish, "Ошибок: " + errors_count.ToString());
//        }
//        private void InvokeIt(Action action)
//        {
//            if (!this.IsDisposed) {
//                if (this.InvokeRequired) {
//                    this.Invoke(action);
//                } else {
//                    action.Invoke();
//                }
//            }
//        }
//        private void InvokeIt(Action<string> action, string param)
//        {
//            if (!this.IsDisposed) {
//                if (this.InvokeRequired) {
//                    this.Invoke(action, param);
//                } else {
//                    action.Invoke(param);
//                }
//            }
//        }
//        private void InvokeIt(Action<string, string> action, string param_1, string param_2)
//        {
//            if (!this.IsDisposed) {
//                if (this.InvokeRequired) {
//                    this.Invoke(action, param_1, param_2);
//                } else {
//                    action.Invoke(param_1, param_2);
//                }
//            }
//        }
//        private void AppendSuccess(string form_name, string text)
//        {
//            this._dt_log.AddRow(Cmn.INT32_ZERO, DateTime.Now, form_name, text);
//            this._dt_log.AcceptChanges();
//        }
//        private void AppendError(string form_name, string text)
//        {
//            this._dt_log.AddRow(Cmn.INT32_ONE, DateTime.Now, form_name, text);
//            this._dt_log.AcceptChanges();
//        }
//        #endregion
//        #region Обработчики событий
//        private void frmCompileFormsCache_FormClosed(object sender, FormClosedEventArgs e)
//        {
//            this.bw.CancelAsync();
//        }
//        private void btnBeginTest_Click(object sender, EventArgs e)
//        {
//            this.Start();
//        }
//        private void viewLog_ShowFilterPopupListBox(object sender, FilterPopupListBoxEventArgs e)
//        {
//            if (e.Column.FieldName == "status") {
//                e.ComboBox.DrawItem += this.ComboBox_DrawItem;
//            }
//        }
//        private void ComboBox_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
//        {
//            var item = e.Item as FilterItem;
//            if (item == null || item.Value is FilterItem) {
//                return;
//            }
//            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
//                e.Appearance.BackColor = DevExpress.Skins.CommonSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default).Colors[DevExpress.Skins.CommonColors.Highlight];
//            }
//            e.Appearance.FillRectangle(e.Cache, e.Bounds);
//            foreach (ImageComboBoxItem comboItem in this.riiStatus.Items) {
//                if (comboItem.Value.Equals(item.Value)) {
//                    e.Graphics.DrawImage(imageCollection.Images[comboItem.ImageIndex], e.Bounds.Location);
//                    break;
//                }
//            }
//            e.Handled = true;
//        }
//        #endregion
//    }
//}