//using System.Data;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.DashboardCommon;
//using sql.builder.TFS;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls.Grids.ReportViewModes.Dashboard
//{
//    internal partial class ucDashboardBase : ucBase
//    {
//        protected DevExpress.DashboardCommon.Dashboard Dashboard { get; set; }

//        internal string ReportName { get; set; }
//        internal string FilePath { get; private set; }
//        internal bool Loaded { get; private set; }

//        public ucDashboardBase()
//        {
//            InitializeComponent();
//            Dashboard.DataLoading += Dashboard_DataLoading;
//            Dashboard.ConfigureDataConnection += Dashboard_ConfigureDataConnection;
//            if (XmlReports.IsDeveloperMode())
//            {
//                FilePath = Path.Combine(XmlReports.GetCurrentContentFolder(), "dashboards.xml");
//            }
//            else
//            {
//                FilePath = Path.Combine(XmlReports.GetCurrentContentFolder(), "dashboards.xml");
//            }
//        }

//        void Dashboard_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
//        {
            
//        }

//        void Dashboard_DataLoading(object sender, DashboardDataLoadingEventArgs e)
//        {
        
//        }

//        internal void SetData(DataTable dt)
//        {
//            DashboardObjectDataSource dds = null;
//            if (Dashboard.DataSources.Any())
//            {
//                dds = Dashboard.DataSources[0] as DashboardObjectDataSource;
//            }
//            else
//            {
//                dds = new DashboardObjectDataSource("По умолчанию");
//                Dashboard.DataSources.Add(dds);
//            }
        
//            dds.BeginUpdate();
//            dds.DataSource = dt;
//            dds.EndUpdate();
//        }
//        internal virtual void ReloadData()
//        {
            
//        }

//        internal void LoadDashboard()
//        {
//            var xdashboards = XDocument.Load(FilePath);
//            var xreport = xdashboards.Root.Elements("report").FirstOrDefault(el => el.Attribute("name").Value == ReportName);
//            if (xreport == null) return;

//            using (var ms = new MemoryStream())
//            {
//                xreport.Elements().First().Save(ms);
//                ms.Seek(0L, SeekOrigin.Begin);

//                DashboardObjectDataSource dds = null;
//                if (Dashboard.DataSources.Any())
//                {
//                    dds = Dashboard.DataSources[0] as DashboardObjectDataSource;
//                }
//                //else
//                //{
//                //    dds = new DashboardObjectDataSource("По умолчанию");
//                //    Dashboard.DataSources.Add(dds);
//                //}

//                Dashboard.LoadFromXml(ms);
//                if(dds != null) Dashboard.DataSources[0].Data = dds.DataSource;


//                //Dashboard.DataSources.Clear();
//                //Dashboard.DataSources.Add(dds);
//            }

//            Loaded = true;
//        }
//        internal void SaveDashboard()
//        {
//            TFSHelper.CheckOutFile(FilePath);

//            var xdashboards = XDocument.Load(FilePath);
//            var xreport = xdashboards.Root.Elements("report").FirstOrDefault(el => el.Attribute("name").Value == ReportName);
//            var xreport_new = new XElement("report", new XAttribute("name", ReportName));
//            using (var ms = new MemoryStream())
//            {
//                Dashboard.SaveToXml(ms);
//                ms.Seek(0L, SeekOrigin.Begin);
//                var xdata = XElement.Load(ms);
//                xreport_new.Add(xdata);
//            }

//            if (xreport == null)
//            {
//                xdashboards.Root.Add(xreport_new);
//            }
//            else
//            {
//                xreport.ReplaceWith(xreport_new);
//            }

//            xdashboards.Save(FilePath);
//        }
//    }
//}
