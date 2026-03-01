namespace sql.builder.Special.Report
{
    class HelperRep
    {
        public static string getStrFromObj(object[] arrObj)
        {
            if (arrObj == null || arrObj.Length == 0)
                return "";
            string strFromObj = arrObj[0].ToString();
            for (int i = 1; i < arrObj.Length; i++)
                strFromObj += ", " + arrObj[i];

            return strFromObj;
        }

    }
}
