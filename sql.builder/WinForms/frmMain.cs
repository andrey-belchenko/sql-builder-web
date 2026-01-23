//using System;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.XtraBars;
//using infoenergo.app.common;
//using infoenergo.Context;
//using infoenergo.sys;
//using MenuItem = infoenergo.app.common.MainRibbon.MenuItem;

//namespace sql.builder.WinForms
//{
//    public partial class frmMain : frmBaseSqlBuilder, IContextForm
//    {
//        public XElement TestSettings;
//        public static void ShowForm(Form mdiParent = null)
//        {
//            var frm = new frmMain() { MdiParent = mdiParent };
//            frm.Initialize();


//            frm.Show();

//        }

//        public frmMain()
//        {
//            InitializeComponent();
//        }




//        public void Initialize()
//        {
//            if (db.Connection == null) db.Connection = Global.Connection;
//            if (XmlReports.InputParams == null) XmlReports.InputParams = Cmn.GetFakeGlobalParams();



//            ucMainSqlBuilder1.Initialize();
//            Text = XmlReports.GetMainTitle();
//        }

//        private void frmMain_Load(object sender, EventArgs e)
//        {
//            //
//        }

//        public static frmMain Create(string folderName=null)
//        {
//            var frm = new frmMain();
            
//            if (folderName != null)
//            {
//                XmlReports.SetInputParameter("folder", folderName);
//            }
//            frm.Initialize();
            

//            return frm;
//        }

//        #region Реализация IContextForm
//        public string UserText { get; set; }
//        public class MenuButtonAllReports : MenuItem
//        {
//            public bool BeginGroup()
//            {
//                return false;
//            }

//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmMain), false) as frmMain;
//                if (!frm.Loaded)
//                {
//                    //frm.ContextGroup = new SqlReportsContextGroup();
//                    //XmlReports.SetInputParameter("folder", "");
//                    frm.Initialize();
//                }

//                frm.Show();
//                frm.Activate();



//                base.OnClick(link);
//            }
//        }


//        public class MenuButtonPrisReports : MenuItem
//        {
//            public bool BeginGroup()
//            {
//                return false;
//            }

//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmMain), false) as frmMain;
//                if (!frm.Loaded)
//                {
                   
//                    XmlReports.SetInputParameter("folder", "pris");
//                    frm.Initialize();
//                }



//                frm.Show();
//                frm.Activate();

//                base.OnClick(link);
//            }
//        }


//        public class MenuButtonReportsProj45567 : MenuItem
//        {
//            public bool BeginGroup()
//            {
//                return false;
//            }

//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmMain), false) as frmMain;
//                if (!frm.Loaded)
//                {
//                    //XmlReports.UseInfoenergoRibbon = false;
//                    //frm.ContextGroup = new SqlReportsContextGroup();

//                    XmlReports.SetInputParameter("folder", "remont");
//                    frm.Initialize();
//                }



//                frm.Show();
//                frm.Activate();

//                base.OnClick(link);
//            }
//        }

//		public class MenuButtonReport55973 : MenuItem
//		{
//			public bool BeginGroup()
//			{
//				return false;
//			}

//			protected override void OnClick(BarItemLink link)
//			{
//				var report = new ExpressReport();
//				report.Show("asuse2.55973");
//				base.OnClick(link);
//			}
//		}

             
//        public class MenuButtonUrJournals : MenuItem
//        {
//            // чтобы открывалось не более одного раза
//            public class frmMainUrJournals : frmMain
//            {
//            }

//            public bool BeginGroup()
//            {
//                return false;
//            }

//            protected override void OnClick(BarItemLink link)
//            {
//                var frm = Run(typeof(frmMainUrJournals), false) as frmMainUrJournals;
//                if (!frm.Loaded)
//                {
//                    XmlReports.SetInputParameter("folder", "ur_journals,ur_reports");
//                    XmlReports.SetInputParameter("title", "Арбитражные отчёты и журналы");
//                    frm.Initialize();
//                }

//                frm.Show();
//                frm.Activate();

//                base.OnClick(link);
//            }
//        }

//        public ContextGroupBase ContextGroup { get; set; }
//        public GlobalContextGroupBase GlobalContextGroup
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }


//        #endregion
//    }
//}
