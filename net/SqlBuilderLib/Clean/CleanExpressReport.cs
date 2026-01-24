using System;
using System.Xml;
using ParamField = sql.builder.Controls.FormFields.ParamField;
using sql.builder.WinForms;

namespace sql.builder
{
    /// <summary>
    /// Методы работы с экспресс-формой отчета
    /// </summary>
    internal class CleanExpressReport : IDisposable
    {
        private CleanFrmExpressReport _frm;

        /// <summary>
        /// Возникает перед открытием отчета и передает путь к готовому файлу
        /// </summary>
        public event EventHandler<CleanExpressReportEventArgs> ReportOpening;

        
        #region OnEndedReport
        /// <summary>
        /// Triggers the EndedReport event.
        /// </summary>
        protected virtual void OnReportOpening(CleanExpressReportEventArgs e)
        {
            if (ReportOpening != null)
                // емцов - вернул this в sender
                ReportOpening(this, e);
        }
        #endregion

        /// <summary>
        /// Инициализация формы
        /// </summary>
        public CleanExpressReport()
        {
            _frm = new CleanFrmExpressReport();   
        }

       

        bool _init = false;
        /// <summary>
        /// Инициализирует отчет для выгрузки через экспресс-форму
        /// </summary>
        /// <param name="report_name">Имя отчета</param>
        /// <param name="form_title">Заголовок формы</param>
        public void Initialize(string report_name, string form_title = null)
        {
            if (_init) return;
            _frm.Initialize(report_name);
            _frm.ReportOpening += frm_ReportOpening;
            _init = true;
        }


        /// <summary>
        /// Задает и возвращает необходимость открытия распечатанный отчет
        /// </summary>
        public bool OpenDocumentAfterPrint
        {
            get { return _frm.OpenDocumentAfterPrint; }
            set { _frm.OpenDocumentAfterPrint = value; }
        }



      
        public string ExecuteReport(string templateName = null)
        {
            var templateInfo = _frm.GetTemplateInfo(templateName);
            return _frm.ExecuteReport(templateInfo);
        }

        void frm_ReportOpening(object sender, CleanExpressReportEventArgs e)
        {
            //говорим что все готово
            //var ae = new ExpressReportEventArgs();
            //ae.Path = e.Path;
            OnReportOpening(e);
        }



        /// <summary>
        /// Возвращает экземпляр поля для манипуляций с ним извне
        /// </summary>
        /// <param name="name">Имя параметра из xml-описания формы</param>
        /// <returns></returns>
        public ParamField GetParamField(string name)
        {
            return _frm.GetUIForm().GetParamField(name);
        }

        /// <summary>
        /// Освобождает ресурсы, использованные экспресс-формой
        /// </summary>
        public void Dispose()
        {

        }
    }
}