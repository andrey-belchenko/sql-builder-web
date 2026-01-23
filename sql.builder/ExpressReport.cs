//using System;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml;
//using ParamField = sql.builder.Controls.FormFields.ParamField;
//using sql.builder.WinForms;

//namespace sql.builder
//{
//    /// <summary>
//    /// ������ ������ � ��������-������ ������
//    /// </summary>
//    public class ExpressReport : IDisposable
//    {
//        private frmExpressReport _frm;

//        /// <summary>
//        /// ��������� ����� ��������� ������ � �������� ���� � �������� �����
//        /// </summary>
//        public event EventHandler<ExpressReportEventArgs> ReportOpening;

//        /// <summary>
//        /// ������� ��������� ������������ �������� �����
//        /// </summary>
//        public event EventHandler<ExpressReportEventArgs> CustomPrint;


//        #region OnEndedReport
//        /// <summary>
//        /// Triggers the EndedReport event.
//        /// </summary>
//        protected virtual void OnReportOpening(ExpressReportEventArgs e)
//        {
//            if (ReportOpening != null)
//                // ����� - ������ this � sender
//                ReportOpening(this, e);
//        }
//        #endregion

//        /// <summary>
//        /// ������������� �����
//        /// </summary>
//        public ExpressReport()
//        {
//            _frm = new frmExpressReport(false, false);   
//        }

       

//        bool _init = false;
//        /// <summary>
//        /// �������������� ����� ��� �������� ����� ��������-�����
//        /// </summary>
//        /// <param name="report_name">��� ������</param>
//        /// <param name="form_title">��������� �����</param>
//        public void Initialize(string report_name, string form_title = null)
//        {
//            if (_init) return;

//            _frm.Initialize(report_name);
//            if (form_title != null)
//            {
//                _frm.Text = form_title;
//            }
//            _frm.ReportOpening += frm_ReportOpening;
//            _frm.CustomPrint += frm_CustomPrint;

//            _init = true;
//        }

//        /// <summary>
//        /// �������������� �����, ���������� ��������-�����
//        /// </summary>
//        /// <param name="report_name">��� ������</param>
//        /// <param name="form_title">��������� �����</param>
//        public void Show(string report_name, string form_title = null)
//        {
//            // �������� ������� �������� ����� ����������� ������ �� ��������� �����
//            Initialize(report_name, form_title);

//            _frm.WithWaitUI = true;
//            _frm.WaitUI.CanEmbed = true;
//            _frm.Show();         
//        }



//        /// <summary>
//        /// �������������� �����, ���������� ��������-�����, ��������� �����
//        /// </summary>
//        /// <param name="report_name">��� ������</param>
//        /// <param name="form_title">��������� �����</param>
//        /// <param name="dialog">True - �������� ����� � ��������� ����; false - �������� ����� � ����������� ����</param>
//        public void ShowAndExecute(string report_name, string form_title = null, bool dialog = false)
//        {
//            // �������� ������� �������� ����� ����������� ������ �� ��������� �����
//            Initialize(report_name, form_title);

//            _frm.WithWaitUI = true;
//            _frm.WaitUI.CanEmbed = true;
//            _frm.ShowAndExecute(dialog);
//        }

//        /// <summary>
//        /// �������������� �����, ���������� ��������-����� � ��������� ����
//        /// </summary>
//        /// <param name="report_name">��� ������</param>
//        /// <param name="form_title">��������� ������</param>
//        /// <returns>��������� ������� � ��������� ����� �����</returns>
//        public DialogResult ShowDialog(string report_name, string form_title = null)
//        {
//            // �������� ������� �������� ����� ����������� ������ �� ��������� �����
//            Initialize(report_name, form_title);

//            _frm.WithWaitUI = true;
//            _frm.WaitUI.CanEmbed = true;
//            return _frm.ShowDialog();
//        }

//        /// <summary>
//        /// ���������� �������������� ������������������
//        /// </summary>
//        public void  ShowPreview()
//        {
//             _frm.Show();
//        }

//        /// <summary>
//        /// ��������� �����
//        /// </summary>
//        /// <param name="templateInfo">���������� � �������</param>
//        /// <returns>������ ��� ����� � �������</returns>
//        public string ExecuteReport(XmlNode templateInfo = null)
//        {
//            return _frm.ExecuteReport(templateInfo);
//        }

//		/// <summary>
//		/// ��������� ������������ ������
//		/// </summary>
//		/// <returns>���������� �������������� DataSet � ������� ������</returns>
//		public DataSet GetExecuteReportResult()
//		{
//			return _frm.GetExecuteReportResult();
//		}

//        void frm_CustomPrint(object sender, ExpressReportEventArgs e)
//        {
//            if (CustomPrint != null)
//            {
//                // ����� - expressreport �������� ��� sender ������ ����� �����
//                CustomPrint(this, e);
//            }
//        }

//        void frm_ReportOpening(object sender, ExpressReportEventArgs e)
//        {
//            //������� ��� ��� ������
//            //var ae = new ExpressReportEventArgs();
//            //ae.Path = e.Path;
//            OnReportOpening(e);
//        }

//        /// <summary>
//        /// ���������� ������, �� ������� ����� ������
//        /// </summary>
//        /// <returns></returns>
//        public TableLayoutPanel GetButtonsPanel()
//        {
//            return _frm.GetButtonsPanel();
//        }

//        /// <summary>
//        /// ���������� �������� ����������, ��������� �� �����
//        /// </summary>
//        /// <returns></returns>
//        public DataSet GetParamsData()
//        {
//            return _frm.GetParamsData();
//        }

//        /// <summary>
//        /// ������ � ���������� ��������� ������� ������� ����� � �� ��������
//        /// </summary>
//        public bool WorkFolderVisible
//        {
//            get { return _frm.WorkFolderVisible; } 
//            set { _frm.WorkFolderVisible = value; }
//        }

//        /// <summary>
//        /// ������ � ���������� ������������� �������� ������������� �����
//        /// </summary>
//        public bool OpenDocumentAfterPrint
//        {
//            get { return _frm.OpenDocumentAfterPrint; }
//            set { _frm.OpenDocumentAfterPrint = value; }
//        }

//        /// <summary>
//        /// ������������� ��������� ResultData � ���������� CustomPrint
//        /// </summary>
//        /// <param name="param"></param>
//        public void DoCustomPrint(string param)
//        {
//            _frm.DoCustomPrint(param);
//        }

//        /// <summary>
//        /// ���������� ������ ������ ���� ��� ��������� ������� ���������, � ��������� ������ ���������� ��������� � �����������
//        /// </summary>
//        /// <returns></returns>
//        public string ValidateParams()
//        {
//            return _frm.ValidateParams();
//        }

//        /// <summary>
//        /// ���������� ��������� ���� ��� ����������� � ��� �����
//        /// </summary>
//        /// <param name="name">��� ��������� �� xml-�������� �����</param>
//        /// <returns></returns>
//        public ParamField GetParamField(string name)
//        {
//            return _frm.GetUIForm().GetParamField(name);
//        }

//        /// <summary>
//        /// ����������� �������, �������������� ��������-������
//        /// </summary>
//        public void Dispose()
//        {
//            if (_frm != null) _frm.Dispose();
//        }
//    }
//}