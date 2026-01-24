using System;

namespace sql.builder.MP
{
    public class MPColumn
    {
        private string name;
        private MPType type;
        private bool is_key, is_main;
        public string Name { get { return this.name; } }
        public MPType Type { get { return this.type; } }
        public bool IsKey { get { return this.is_key; } }
        public bool IsMain { get { return this.is_main; } }
        public MPColumn(string name, MPType type, bool isKey = false, bool isMain = false)
        {
            this.name = name;
            this.type = type;
            this.is_key = isKey;
            this.is_main = isMain;
        }
        internal static string GetName(MPColumn col)
        {
            return col.name;
        }
    }
}
