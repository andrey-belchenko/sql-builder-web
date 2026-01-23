//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using DevExpress.XtraEditors;
//using sql.builder.DataApi;

//namespace sql.builder.Controls
//{
//    internal partial class ucAccessDenied : XtraUserControl
//    {
//        public ucAccessDenied(string security_id, bool write)
//        {
//            InitializeComponent();

//            StringBuilder text = (write) 
//                ? new StringBuilder("Отсутствуют права на запись. Обратитесь к администратору.") 
//                : new StringBuilder("Отсутствуют права на просмотр. Обратитесь к администратору.");

//            string[] roles = VSecurityUtils.GetNecessaryRoles(security_id, write);
//            if (roles.Length == 0) {
//                labelControl1.Text = text.ToString();
//                return;
//            }

//            // если имя роли - число, то считаем что это kod_menu из rk_menu
//            decimal kod_menu = -1M;
//            List<decimal> kod_menu_list = new List<decimal>();
//            foreach (var role in roles)
//            {
//                if(decimal.TryParse(role, out kod_menu))
//                {
//                    kod_menu_list.Add(kod_menu);
//                }
//            }

//            var dt = db.SelectMenuInfo(kod_menu_list.ToArray());
//            if (dt.Rows.Count == 0)
//            {
//                labelControl1.Text = text.ToString();
//                return;
//            }

//            text.AppendLine();
//            text.AppendLine();

//            if (dt.Rows.Count == 1)
//            {
//                if (write) text.AppendLine("Для возможности редактирования необходимы права на запись на пункт меню:");
//                else text.AppendLine("Для возможности просмотра необходимы права на чтение на пункт меню:");
//            }
//            else
//            {
//                if (write) text.AppendLine("Для возможности редактирования необходимы права на запись на любой из пунктов меню:");
//                else text.AppendLine("Для возможности просмотра необходимы права на чтение на любой из пунктов меню:");
//            }

//            foreach (var row in dt.AsEnumerable())
//            {
//                text.AppendLine(string.Format("{0} - " + "\"{1}\"", row["KOD_MENU"], row["MENU"]));
//            }

//            labelControl1.Text = text.ToString();
//        }
//    }
//}
