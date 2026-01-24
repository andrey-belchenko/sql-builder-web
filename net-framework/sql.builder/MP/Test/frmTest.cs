using System;
using System.Diagnostics;
//using System.Windows.Forms;
using System.Xml.Linq;

using sql.builder.MP.Tools;

namespace sql.builder.MP.Test

{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run();
        }

        void Run()
        {
            //сломал
            //var watch = new Stopwatch();
            //watch.Start();

            //string excelPath = "D:\\test\\data_big.xlsx";
            //string tableName = "vem_is_ad_all";
            //var xquery = XElement.Load("Test\\vem_is_ad_all.xml");
            //var columns = MPColumns.FromXml(xquery);

            //using (IExcelEnvironment excelEnv = new ExcelMSEnvironment())
            //{
            //    IMPDataReader reader = new ExcelDataReader(excelEnv, columns, excelPath, 4); 
            //    IMPDataStore saver = new MPDataToFile();
            //    saver.SaveData(reader);

            //    watch.Stop();
            //    memoEdit4.Text += string.Format("{0:g} Чтение/конвертация данных \r\n", watch.Elapsed);
            //    watch.Start();
            //}

            //IMPDataLoader loader = new MPSqlLoader();
            //loader.FillTable(tableName, columns);
            //watch.Stop();
            //memoEdit4.Text += string.Format("{0:g} Загрузка в БД \r\n", watch.Elapsed);

            //memoEdit1.Text = ((MPSqlLoader)loader).GetLastLogText();
            //memoEdit2.Text = ((MPSqlLoader)loader).GetLastBadText();
            //memoEdit3.Text = ((MPSqlLoader)loader).GetLastDiscardText();
        }
    }
}
