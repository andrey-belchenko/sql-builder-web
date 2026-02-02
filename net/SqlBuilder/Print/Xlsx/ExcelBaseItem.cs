using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelBaseItem
    {
        public XElement Xml { get; protected set; }
        public string ID { get; protected set; }
        internal ExcelPrintEnv Env { get; private set; }

        public ExcelBaseItem(XElement xitem, ExcelPrintEnv env)
        {
            Xml = xitem;
            Env = env;
        }
    }
}