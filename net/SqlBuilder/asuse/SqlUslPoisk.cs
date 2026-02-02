using Devart.Data.Oracle;
using sql.builder.DataApi;

namespace sql.builder.asuse
{
    public static class SqlUslPoisk
    {
        public static void FillDogovorDataByt(string kod_dog_column, string query_name, OracleConnection con)
        {
            var sql = "delete from tmp_objfilter where kod_refobject = 2";
            XmlReports.executeNonQuery(sql, con);

            sql = string.Format(
                 "insert into tmp_objfilter (kod_refobject, objid, x_kodp, nump, name, kodd) " + 
                 "select 2, d.kod_dog, p.kodp, p.nump, p.name, p.kod_d " + 
                 "from {2} t " +
                 "inner join kr_dogovor d on d.kod_dog = t.{1} " +
                 "inner join kr_payer p on p.kodp = d.kodp " +
                 "where skod = '{0}' ", query_name, kod_dog_column, TextConst.DBObjects.TempTable);
            XmlReports.executeNonQuery(sql, con);
        }

        public static void FillAbonentDataByt(string kodp_column, string query_name, OracleConnection con)
        {
            var sql = "delete from tmp_objfilter where kod_refobject = 1";
            XmlReports.executeNonQuery(sql, con);

            sql = string.Format(
                 "insert into tmp_objfilter (kod_refobject, objid) " +
                 "select 1, t.{1} " +
                 "from {2} t " +
                 "where skod = '{0}' ", query_name, kodp_column, TextConst.DBObjects.TempTable);
            XmlReports.executeNonQuery(sql, con);
        }

        public static void FillPointDataByt(string kod_dog_column, string query_name, OracleConnection con)
        {
            var sql = "delete from tmp_objfilter where kod_refobject = 4";
            XmlReports.executeNonQuery(sql, con);

            sql = string.Format(
                 "insert into tmp_objfilter (kod_refobject, objid, x_kod_dog, x_kod_numobj)  " +
                 "select 4, po.kod_point, no.kod_dog, no.kod_numobj " +
                 "from {2} t " +
                 "inner join kr_numobj no on no.kod_dog = t.{1} " +
                 "inner join hv_point po on po.kod_numobj = no.kod_obj " +
                 "where skod = '{0}'", query_name, kod_dog_column, TextConst.DBObjects.TempTable);
            XmlReports.executeNonQuery(sql, con);
        }
    }
}
