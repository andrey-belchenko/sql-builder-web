using System;
using System.Xml;
using ParamField = sql.builder.Controls.FormFields.ParamField;
using sql.builder.WinForms;

namespace sql.builder
{
    /// <summary>
    ///    - 
    /// </summary>
    internal class CleanExpressReport : IDisposable
    {
        private CleanFrmExpressReport _frm;

        /// <summary>
        ///          
        /// </summary>
        public event EventHandler<CleanExpressReportEventArgs> ReportOpening;

        
        #region OnEndedReport
        /// <summary>
        /// Triggers the EndedReport event.
        /// </summary>
        protected virtual void OnReportOpening(CleanExpressReportEventArgs e)
        {
            if (ReportOpening != null)
                //  -  this  sender
                ReportOpening(this, e);
        }
        #endregion

        /// <summary>
        ///  
        /// </summary>
        public CleanExpressReport()
        {
            _frm = new CleanFrmExpressReport();   
        }

       

        bool _init = false;
        /// <summary>
        ///      -
        /// </summary>
        /// <param name="report_name"> </param>
        /// <param name="form_title"> </param>
        public void Initialize(string report_name, string form_title = null)
        {
            if (_init) return;
            _frm.Initialize(report_name);
            _frm.ReportOpening += frm_ReportOpening;
            _init = true;
        }


        /// <summary>
        ///       
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
            //   
            //var ae = new ExpressReportEventArgs();
            //ae.Path = e.Path;
            OnReportOpening(e);
        }



        /// <summary>
        ///        
        /// </summary>
        /// <param name="name">   xml- </param>
        /// <returns></returns>
        public ParamField GetParamField(string name)
        {
            return _frm.GetUIForm().GetParamField(name);
        }

        /// <summary>
        ///  ,  -
        /// </summary>
        public void Dispose()
        {

        }
    }
}