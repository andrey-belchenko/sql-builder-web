using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using infoenergo.core.Data;
using infoenergo.sys;

namespace sql.builder.Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void formParametersControl1_Load(object sender, EventArgs e)
        {
            formParametersControl1.QueryName = "poisk_ul";   
            formParametersControl1.Initialize();       
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = formParametersControl1.GetSqlData();

            var data = DataHelper.SqlGetTable(sql, Global.Connection);
            gridControl1.DataSource = data;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            formParametersControl1.SelectResultColumns(new []{"kod_dog"});
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            formParametersControl1.SelectResultColumns(new[] {"kodp" });
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            formParametersControl1.SelectResultColumns(new[] { "kod_numobj" });
        }
    }
}
