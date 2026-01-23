//using System;
////using System.Windows.Forms;
////using DevExpress.Utils.Taskbar;
//using infoenergo;
//using infoenergo.ui.win.Forms;
//using sql.builder.Test;
//using sql.builder.XmlHelpers;

//namespace sql.builder.WinForms
//{
//    public partial class frmBaseSqlBuilder : FormBase
//    {
//        internal WaitUIHelper WaitUI { get; private set; }
//        public bool Loaded { get; private set; }

//        private bool _withWaitUi;
//        public bool WithWaitUI
//        {
//            get { return _withWaitUi; }
//            set
//            {
//                if (value && !_withWaitUi)
//                {
//                    WaitUI = new WaitUIHelper(this);
//                }
//                else if(!value && _withWaitUi)
//                {
//                    WaitUI.Dispose();
//                    WaitUI = null;
//                }

//                _withWaitUi = value;
//            }
//        }

//        private TaskbarAssistant _taskBarAssistent;

//        internal TaskbarAssistant TaskBarAssistent
//        {
//            get
//            {
//                // ленивая загрузка, т.к. в XP лезет exception
//                if (_taskBarAssistent == null)
//                {
//                    _taskBarAssistent = new TaskbarAssistant() { ParentControl = this };
//                }

//                return _taskBarAssistent;
//            }
//        }

//        public frmBaseSqlBuilder(bool withWaitUI = true)
//        {
//            InitializeComponent();
//            DoubleBuffered = true;
//            WithWaitUI = withWaitUI;

//            // из infoenergo.exe запускается во вкладке
//            if (GlobalValues.MainForm != null)
//            {
//                this.MdiParent = (Form)GlobalValues.MainForm;
//            }
//        }

//        protected override void OnFormClosing(FormClosingEventArgs e)
//        {
//            base.OnFormClosing(e);
//            if (WithWaitUI) WaitUI.Dispose();
//            // Чтобы контролы UIBase не блокировали закрытие формы при инвалидном значении
//            //e.Cancel = false;
//        }

//        private void frmBaseSqlBuilder_Load(object sender, EventArgs e)
//        {
//            Loaded = true;
//        }

//        //protected override CreateParams CreateParams
//        //{
//        //    get
//        //    {
//        //        var handleParam = base.CreateParams;
//        //        handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
//        //        return handleParam;
//        //    }
//        //}
//    }
//}
