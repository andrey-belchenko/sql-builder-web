////using System.Windows.Forms;
//using System.Xml.Linq;
//using infoenergo.ui.win.Forms;
//using sql.builder.Controls;
//using sql.builder.DataApi;

//namespace sql.builder.WinForms
//{
//    internal partial class frmDataError : FormBase
//    {
//        public static void Show(ErrorType error_type, object data, string classTitle)
//        {
//            using (var frm = new frmDataError(error_type, data, classTitle))
//            {
//                frm.ShowDialog();
//            }
//        }

//        public frmDataError()
//        {
//            InitializeComponent();
//        }
//        ucGridBase grid = null;
//        frmDataError(ErrorType error_type, object data, string classTitle)
//            : this()
//        {
//            if (error_type == ErrorType.ChildRecordsExist)
//            {
//                Text = "Невозможно удалить строку";

//                var vds = data as VDataSet;
//                var vdt = vds.Tables[0] as VDataTable;

//                var xscheme = new XElement(TextConst.EName.Scheme, vdt.Scheme);
//                xscheme.Elements().Elements(TextConst.EName.Childs).Remove();
//                var xroot = new XElement(TextConst.EName.Root, xscheme);

//                // инициализация ui грида
//                grid = new ucReferenceGrid() {/*Dock = DockStyle.Fill*/};
//                grid.Margin = new Padding(0);
//                grid.BeginUpdate();
//                grid.Name = vdt.TableName;
//                grid.LoadSchemeSettingsFromXml(xroot);
//                grid.DataSource = vds;
//                grid.EndUpdate();

//                grid.SetEditable(false);
//                grid.SetToolbarVisible(false);
//                grid.SetSummaryVisible(false);
//                grid.SetMultiselect(false);

//                grid.SetTitle("Существуют следующие дочерние записи: " + classTitle);

//                Controls.Add(grid);
//                //Controls.SetChildIndex(pFooter, 0);
//            }
//            else if (error_type == ErrorType.NoWriteAccess)
//            {
//                Text = "Отсутствуют права на запись";

//                var security_id = data as string;
//                var ad = new ucAccessDenied(security_id, true) { Dock = DockStyle.Fill };
//                Controls.Add(ad);
//            }
//        }

//        private void btnOk_Click(object sender, System.EventArgs e)
//        {
//            Close();
//        }

//        internal enum ErrorType
//        {
//            ChildRecordsExist,
//            NoWriteAccess
//        }

//        private void frmDataError_Shown(object sender, System.EventArgs e)
//        {
//            grid.Dock = DockStyle.Fill;// чтобы сработал resize
//        }
//    }
//}
