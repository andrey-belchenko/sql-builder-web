//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
////using DevExpress.Utils.Drawing.Helpers;
////using DevExpress.XtraEditors;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.WinForms;

//namespace sql.builder.Special.Kido
//{
//    static class Lkk
//    {
//        public static void AcceptZayavSelection(DataSet ds)
//        {
//            var vds = (ds as VDataSet);
//            var form = vds.Form;
//            var tbl = vds.GetTable("np_z_vo");
//            var sb1 = new StringBuilder();
//            var sb2 = new StringBuilder();
//            var q1 = "";
//            var q2 = "";
//            bool alert1 = false;
//            bool alert2 = false;
//            foreach (var r in tbl.SelectedRows)
//            {
//                if (r["user_id"] != DBNull.Value)
//                {
//                    sb1.Append(q1 + r["num_z"].ToString());
//                    q1 = ", ";
//                    alert1 = true;
//                }
//                if (r["bad_kont"] != DBNull.Value)
//                {
//                    sb2.Append(q2 + r["num_z"].ToString());
//                    q2 = ", ";
//                    alert2 = true;
//                }
//            }
//            var msg = new StringBuilder();
//            if (alert1)
//            {
                
//                msg.AppendLine("Следующие заявки будут отвязаны от текущих ЛКК:" + sb1.ToString() + ".");
//            }

//            if (alert2)
//            {
               
//                msg.AppendLine("В следующих заявках будет установлено новое основное контактное лицо:" + sb2.ToString() + ".");
//            }
//            var res = DialogResult.OK;
//            if (msg.Length > 0)
//            {
               
//                res = XtraMessageBox.Show(msg.ToString(), "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                
//            }
//            if (res == DialogResult.OK)
//            {
//                form.SaveDataAndClose();
//            }
           
//        }
//    }
//}
