using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.LookAndFeel;
//using DevExpress.Skins;
//using DevExpress.UserSkins;
//using DevExpress.XtraEditors;
using infoenergo.core;
using infoenergo.core.Data;
using infoenergo.sys;
//using infoenergo.ui.win;
//using sql.builder.Controls.Testing;
//using sql.builder.Properties;
using sql.builder.DataApi;
using sql.builder.UI.CommandItems;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
//using infoenergo.framework.Extensions.Oracle;
using System.Collections.Generic;
using sql.builder.Clean;

// Basic usage


namespace sql.builder
{
    public static class Program 
    {

        public static void Main(string[] args)
        {
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            //var conStr = infoenergo.framework.Global2.BuildConnectionString("asuse", "kl0pik", "realkazn");
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            var pars = new Dictionary<string, object>();


            pars.Add("p_date_s", new DateTime(2020, 1, 8));
            pars.Add("p_date_po", new DateTime(2025, 1, 8));
            pars.Add("p_kodp", new List<int> { 1172, 1210, 1211, 1212, 1214, 1215
                //, 1216, 1217, 1218, 1219
            });

            var path =  CleanSqlBuilder.ExecReportGetPath("asuse2.65211", pars, "65211.xlsx");
            Process.Start(path);
            Console.WriteLine("done");

        }
//        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //  ВНИМАНИЕ РАЗРАБОТЧИКУ!
//        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //public const string CURRENT_SCHEMA = "";      // глобальная схема для ALTER SESSION SET CURRENT_SCHEMA = ASUSE
//        //public static int KEEP_ALIVE_INTERVAL = 0;    // интервал опроса БД в минутах, для поддержки сессии в живом состоянии. Если 0, то не поддерживаем.
//        private static ConnectionSource connection_source;
//        [STAThread]
//        public static void Main1(string[] args)
//        {
//            // ============================================
//            // подключение скинов
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            BonusSkins.Register();
//            //DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.infoenergoskins).Assembly); // register skin infoenergo
//            SkinManager.EnableFormSkins();
//            SkinManager.EnableMdiFormSkins();
//            UserLookAndFeel.Default.SetSkinStyle(SettingsHelper.DevExpressSkinName);
//            // infoenergo.sys.Global.SetDefaultSkin(); // скин по умолчанию.
//            //или так: 
//            //  DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2013"); // скин по умолчанию.
//            // ============================================
//            // пытаемся отловить все пропущенные эксепшны здесь
//            Application.ThreadException += ApplicationThreadException;
//            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
//            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
//            //Test3.TestMergeExcel();
//            //return;
//            //TfsAutoCheckIn();
//            //Test3.NameIsAdAll();
//            //return;
//            // без этого кода при изменении версии exe настройки пользователей слетают
//            try {
//                if (Settings.Default.UpdateSettings) {
//                    Settings.Default.Upgrade();
//                    Settings.Default.UpdateSettings = false;
//                    Settings.Default.Save();
//                }
//            } catch { }
//            if (args.Length == 1 && args[0] == "-rs") {
//                var timer = new Stopwatch();
//                timer.Start();
//                RecompileScheme();
//                timer.Stop();
//                XtraMessageBox.Show(timer.Elapsed.ToString("g"), "Затрачено времени");
//                return;
//            }
//            if (args.Length > 0 && args[0] == "-server") {
//                string file = args[1].Split('=')[1].Trim('"');
//                bool compare = (args[2].Split('=')[1] == "1");
//                AsWCFServer(file, compare);
//                return;
//            }
//            if (args.Length > 0 && args[0] == "-client") {
//                string service_name = args[1].Split('=')[1].Trim('"');
//                AsWCFClient(service_name);
//                return;
//            }
//            string express_report_name = "";
//			string loginFormDatabase;
//			string loginFormUser;
//			bool loginFormArgsFound = TryGetLoginFormArgs(Environment.GetCommandLineArgs(), out loginFormDatabase, out loginFormUser);
//            if (!String.IsNullOrEmpty(HelperCentura.GetConnectionString()) && HelperCentura.GetConnectionString() != "user id=;password=;data source=;pooling=false;") {
//                connection_source = ConnectionSource.Centura;
//            } else if (loginFormArgsFound) {
//				connection_source = ConnectionSource.LoginForm;
//			} else if (args.Length == 4) {
//                connection_source = ConnectionSource.CmdArgs;
//            } else {
//                connection_source = ConnectionSource.LoginForm;
//            }
//            switch (connection_source) {
//                // Из Центуры
//                case ConnectionSource.Centura:
//                    Global.Connection = new OracleConnection(HelperCentura.GetConnectionString());
//                    Global.Connection.Open(useGlobalSettings: true);
//                    db.Connection = Global.Connection;
//                    infoenergo.framework.DataHelper.setNlsDateFormat(Global.Connection);
//                    var pars = HelperCentura.GetParameters();
//                    var depPar = pars.FirstOrDefault(p => p.Name == "dep");
//                    if (depPar != null && !string.IsNullOrEmpty(depPar.Value)) {
//                        DataTable depNameTbl = db.ExecuteDataTable("select name from kr_org where kodp=" + depPar.Value);
//                        if (depNameTbl.Rows.Count != 0) {
//                           XmlReports.DepTitle = depNameTbl.Rows[0][0].ToString();
//                        }
//                    }
//                    XmlReports.SetInputParameters(pars);
//                    // Достаем имя отчёта, если оно есть в параметрах
//                    XElement param = XmlReports.InputParams.Elements(EName.param).SearchByAttribute(AName.name, "repname");
//                    if (param != null) {
//                        express_report_name = param.Element(EName.@const).Value;
//                    }
//                    break;
//                // Через вызов exe с параметрами
//                case ConnectionSource.CmdArgs:
//                    //Global.Connection = new OracleConnection(String.Format("data source={0};user id={1};password={2};", args[0], args[1], HelperCrypt.Decrypt(args[2])));
                    
//                    //Global.Connection = new OracleConnection(String.Format("data source={0};user id={1};password={2};", args[0], args[1], HelperCrypt.Decrypt(args[2])));
//                    //Global.Connection.Open(useGlobalSettings: true);
//                    Global.Connect(args[1], HelperCrypt.Decrypt(args[2]), args[0]);
//                    db.Connection = Global.Connection;
//                    infoenergo.framework.DataHelper.setNlsDateFormat(Global.Connection);
//                    // если есть префикс folder - это имя папки
//                    string name = args[3];
//                    if (name.StartsWith("folder:")) {
//                        XmlReports.SetInputParameter("folder", name.Substring("folder:".Length));
//                    } else if (name.StartsWith("report:")) {
//                        SqlBuilder.LoadProjectForReport(name.Substring("report:".Length));
//                    } else {
//                        express_report_name = name;
//                    }
//                    break;
//                // Через вызов напрямую или командную строку с параметрами логина
//				case ConnectionSource.LoginForm:
//                    if (!LoginForm(loginFormDatabase, loginFormUser)) {
//                        Application.Exit(); //этот метод не обязан завершать процесс, и в случае отвала сервера БД здесь приложение не только не терминируется, но и виснет
//                        Environment.Exit(0); //Быстрое и безболезненное самоубийство с кодом возврата 0 
//                    }
//                    break;
//            }
//            // устанавливаем текущую схему в сессии
//            /*try {
//               // Global.SetCurrentEsysSchema();
//            } catch (Exception ex) {
//                ExceptionHandler.LogExceptionToDatabase(ex, "Не удалось установить текущую схему в сессии oracle", null, ExceptionHandler.ApplicationId);
//            }*/
//            if (db.Connection.DataSource.ToUpper() != "ENERGO3" && db.Connection.DataSource.ToUpper() != "KIDO") {
//                Security.LoadPermissions(db.Connection.UserId, db.Connection);
//            }
//            //express_report_name = "kurort1";
//            //express_report_name = "28007-ba";
//            //   express_report_name = "25499";
//            //express_report_name = "26630";
//            //express_report_name = "43107";
//            //express_report_name = "is_rep_rosseti";
//            //XmlReports.SetInputParameter("folder","ur_journals,ur_reports");
//            // Если exe вызван с параметром имя отчета - запускаем expressform
//            //SqlBuilder.PreviewReport("35210-new", false);
//            if (express_report_name != "") {
//				if (connection_source != ConnectionSource.LoginForm) {
//					var report = new sql.builder.ExpressReport();
//					report.Initialize(express_report_name);
//                    foreach (XElement param in XmlReports.InputParams.Elements(EName.param)) {
//						var pf = report.GetParamField(param.Attribute(AName.name).Value);
//						if (pf != null) {
//							decimal decimalParam;
//                            Type type = pf.GetValueType();
//                            if (type == typeof(string)) {
//								pf.SetValue(param.Element(EName.@const).Value);
//                            } else if (type == typeof(decimal) && decimal.TryParse(param.Element(EName.@const).Value, out decimalParam)) {
//								pf.SetValue(decimalParam);
//							}
//						}
//					}
//					report.ShowDialog(express_report_name);
//				} else { //old-style
//					var frm = new frmExpressReport(true);
//					if (!frm.Initialize(express_report_name)) {
//						if (!LoginForm() || !frm.Initialize(express_report_name)) {
//							Application.Exit();
//							Environment.Exit(0);
//						}
//					}
//					Application.Run(frm);
//				}
//            } else {
//                //XmlReports.GenerateNavigators();
//                //Test3.aaaa1();
//                //return;
//                //var frm = new Test4();
//                //frm.QueryName = "asuse2.41125";
//                var frm = new frmMain();
//                frm.Initialize();
//                //var frm = new Form1();
//                Application.Run(frm);
//            }
//            // действия, которые надо выполнить на завершении приложения
//            finalizeApplication();
//        }
//        /// <summary>
//        /// Сюда вписывать все, что нужно закрыть при закрытии/падении приложения
//        /// </summary> 
//        private static void finalizeApplication()
//        {
//            try {
//                if (keepAlive != null)
//                    keepAlive.KeepAlive(false); // выключать поддержку соединения необязательно, но вай нот?
//            } catch { } // специально душим все исключения, чтобы приложение молча завершилось
//        }
//        /*private static void TestExe()
//        {
//            var parameters = String.Format("{0} {1} {2} {3}",
//                Global.Connection.DataSource,
//                Global.Connection.UserId,
//                HelperCrypt.Encrypt(Global.Connection.Password),
//                "folder:ur_reports");

//            var path = Path.Combine(Environment.CurrentDirectory, @"sql.builder.exe");
//            var process = Process.Start(path, parameters);
//            process.WaitForExit();
//        }*/
//        /*private static void TfsAutoCheckIn()
//        {
//            var con = new OracleConnection("data source=asuse;user id=asuse;password=kl0pik;");
//            con.Open(useGlobalSettings: true);

//            Global.Connection = con;
//            db.Connection = con;

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
//        }*/
//        private static void AsWCFClient(string service_name)
//        {
//            WCFHelper.StartClient(service_name);
//            try {
//                Global.Connection = new OracleConnection(HelperCrypt.Decrypt(WCFHelper.ServerData.GetEncryptedConnectionString()));
//                Global.Connection.Open(useGlobalSettings: true);
//                infoenergo.framework.DataHelper.setNlsDateFormat(Global.Connection);
//                if (db.Connection.GetAlias().ToUpper() != "ENERGO3") Security.LoadPermissions(db.Connection.UserId, db.Connection);
//                WCFHelper.ServerData.SendMessage("Клиент подключен к базе");
//                var frm = new frmMain();
//                frm.Icon = Properties.Resources.infoenergo_inverted;
//                SqlBuilder.ShowPopupWaitForms = false;
//                frm.Initialize();
//                Application.Run(frm);
//            } catch (Exception ex) {
//                WCFHelper.ServerData.SendMessage(ex.Message + "\r\n" + ex.StackTrace);
//                WCFHelper.ServerData.SetClientState(ClientState.Fault);
//                Application.Exit();
//            }
//        }
//        private static void AsWCFServer(string file, bool compare)
//        {

//            var xml = XElement.Load(file);
//            if (xml == null || xml.Element("reports") == null)
//            {
//                ShowMessage.ShowError("Не удалось загрузить настройки из выбранного файла");
//                return;
//            }

//            Global.Connection = new OracleConnection(HelperCrypt.Decrypt(xml.Element("connection").Value));
//            Global.Connection.Open(useGlobalSettings: true);
//            infoenergo.framework.DataHelper.setNlsDateFormat(Global.Connection);
//            if (db.Connection.GetAlias().ToUpper() != "ENERGO3") Security.LoadPermissions(db.Connection.UserId, db.Connection);

//            //var frm = new frmMain
//            //{
//            //    Icon = Properties.Resources.infoenergo_inverted
//            //};
            
//            //frm.Initialize();

//            string file_name = Path.GetFileNameWithoutExtension(file);
//            var frm = new XtraForm()
//            {
//                Icon = Properties.Resources.infoenergo_inverted,
//                Text = file_name,
//                WindowState = FormWindowState.Maximized
//            };

//            var tc = new ucTestReports()
//            {
//                Dock = DockStyle.Fill
//            };
//            frm.Controls.Add(tc);

//            SqlBuilder.ShowPopupWaitForms = false;

//            XmlReports.TestFile = xml;
//            XmlReports.TestName = file_name;
//            XmlReports.TestCompare = compare;

//            frm.Load += (sender, args) => tc.Start();
//            Application.Run(frm);
//        }

//        /*private static void TestQuery()
//        {
//            var con = new OracleConnection("data source=asuse;user id=asuse;password=kl0pik;");
//            //var con = new OracleConnection("data source=alpha;user id=plan;password=qwaser;");
//            con.Open(useGlobalSettings: true);

//            Global.Connection = con;
//            db.Connection = con;
//            using (var frm = new TestQuery())
//            {
//                //XmlReports.InputParams = XElement.Parse("<params><param name=\"dep\"><const>1172</const></param><param name=\"tep_el\"><const>1</const></param></params>");
//                XmlReports.schemeName = "asuse2";
//                XmlReports.customerId = "1";

//                frm.ShowDialog();
//            }
//        }*/
//        private static void RecompileScheme()
//        {
//            //var con = new OracleConnection("data source=asuse;user id=asuse;password=kl0pik;");
//            var con = new OracleConnection(Global.BuildConnectionString("asuse", "kl0pik", "asuse"));
//            //var con = new OracleConnection("data source=alpha;user id=plan;password=qwaser;");
//            con.Open(useGlobalSettings: true); 
//            Global.Connection = con;
//            db.Connection = con;
//            infoenergo.framework.DataHelper.setNlsDateFormat(con);
//            //Wait.Show("Перекомпиляция схемы");
//            WaitUIHelper.LastUsedUIHelper.Show("Перекомпиляция схемы", WaitUIMode.WaitPanel);
//            XmlReports.Init(true);
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            //Wait.Hide();
//        }
//		private static bool LoginForm(string loginFormDatabase = null, string loginFormUser = null)
//		{
//			BaseLogin baseLogin;
//            if (!string.IsNullOrEmpty(loginFormUser) && !string.IsNullOrEmpty(loginFormDatabase)) {
//                baseLogin = new BaseLogin(loginFormDatabase, loginFormUser);
//            } else if (!string.IsNullOrEmpty(loginFormDatabase)) {
//                baseLogin = new BaseLogin(loginFormDatabase);
//            } else {
//                baseLogin = new BaseLogin();
//            }
//            if (baseLogin.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(baseLogin.ConnectionUser)
//                                                          && !String.IsNullOrEmpty(baseLogin.ConnectionPassword)
//                                                          && !String.IsNullOrEmpty(baseLogin.ConnectionDataSource)) {
//                db.Connection = Global.Connection;
//                return true;
//            } else {
//                return false;
//            }
//		}
//		private static bool TryGetLoginFormArgs(string[] args, out string loginFormDatabase, out string loginFormUser)
//		{
//			bool argsfound = false;
//			loginFormDatabase = null;
//			loginFormUser = null;

//			//     /database=<database>
//			if (!String.IsNullOrEmpty(findArg(args, "/database")))
//			{
//				loginFormDatabase = findArg(args, "/database");
//				argsfound = true;
//			}
//			//     /user=<username>
//			if (!String.IsNullOrEmpty(findArg(args, "/user")))
//			{
//				loginFormUser = findArg(args, "/user");
//			}

//			return argsfound;
//		}
//		// возвращает значения аргумента
//		// предполагает, что значения лежат в виде строк: argname=argvalue
//		private static string findArg(string[] args, string argName, bool allowRegEx = true)
//		{
//			string result = null;
//			if (args.Length > 0)
//			{
//				for (int i = 0; i < args.Length; i++)
//				{
//					if (argName.Trim().ToLower().Equals(args[i].Trim().ToLower()))
//					{
//						result = args[i];
//						break;
//					}
//					if (allowRegEx)
//					{
//						// аргументы типа argname=value
//						string pattern = @"(?<=" + argName + @"=)\S+";
//						System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(args[i], pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//						if (m.Success)
//						{
//							result = m.Value;
//							break;
//						}
//					}
//				}
//			}
//			return result;
//		}
//        #region Глобальный отлов исключений
//        /// <summary>
//        /// глобальный отлов исключений-1
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
//        {
//            var ex = (Exception)e.ExceptionObject;
//            ProcessUnhandledException(ex);
//        }

//        /// <summary>
//        /// Ловим ошибки UI (глобальный отлов исключений-2)
//        /// </summary>
//        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
//        {
//            ProcessUnhandledException(e.Exception);
//        }

//        public static void ProcessUnhandledException(Exception ex)
//        {
//            DialogResult result = DialogResult.No;
//            try
//            {
//                UIFormsPool.Clear();

//                // 14.04.17 - чтобы убить Wait формы
//                foreach (var c in Application.OpenForms.Cast<Form>()
//                    .SelectMany(f => Cmn.GetChildControlsOfType<ucBase>(f))
//                    .ToArray()) c.OnBaseEvent(ucBase.ucBaseEventType.Exception);

//                ExceptionHandler.HandleException(ex, "Непредвиденная ошибка интерфейса приложения. Свяжитесь с разработчиками.");

//                // 19.02.15 Емцов - при отладке не предлагаем завершить приложение
//#if DEBUG
//                result = DialogResult.Yes;
//#else
//                result = XtraMessageBox.Show("Произошла непредвиденная ошибка интерфейса приложения. Продолжить?", @"Ошибка приложения", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
//#endif
//            }
//            catch
//            {
//                try
//                {
//                    XtraMessageBox.Show(@"Фатальная ошибка. Выходим из приложения!", @"Фатальная ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);
//                } 
//                finally
//                {
//                    finalizeApplication();
//                    Environment.Exit(0);
//                }
//            }

//            // Exits the program when the user clicks Abort.
//            if (result == DialogResult.No)
//            {
//                finalizeApplication();
//                Environment.Exit(0);
//            }
//        }
//        #endregion

//        #region privates
//        private static ConnectionKeepAlive keepAlive = null;
//        #endregion

//        enum ConnectionSource
//        {
//            Centura,
//            CmdArgs,
//            LoginForm
//        }
    }
}
