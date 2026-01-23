//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using sql.builder;
//using sql.builder;
//using System.Data;
//using Devart.Data.Oracle;
//namespace sql.builder.Test
//{
//    class OtherTests
//    {
//        public static object[] TestInsertPeniDogplat(decimal kod_sdp, decimal kod, decimal kodp)
//        {
//            if (!XmlReports.IsDeveloperMode())
//            {
//                System.Windows.Forms.MessageBox.Show("Функция не реализована");
//                return new object[] { };
//            }
//            var s = "insert into ur_dogplat (kod_sdp,kod,kod_sf,vid_real,summa,dat_form,prizn_konv) values ({0},{1},{2},{3},{4},{5},{6}) returning kod_dogplat into :p_kod_dogplat";

//            var s1 = string.Format(s
//                , kod_sdp
//                , kod
//                , "2225968"
//                , "7"
//                , "1111.111"
//                , "trunc(sysdate)" // поставить дату счета
//                , "1"
//                );
//            var cmd = new OracleCommand(s1, db.Connection);
//            cmd.Parameters.Add(new OracleParameter("p_kod_dogplat",OracleDbType.Number));
//            cmd.Parameters[0].Direction=ParameterDirection.Output;
//            cmd.ExecuteNonQuery();
//            var res = new object[] { cmd.Parameters[0].Value };
//            return res;
//        }
//    }
//}
