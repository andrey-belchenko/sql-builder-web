using System.Collections.Generic;

namespace sql.builder.WebReports.Client
{
    internal class ParamsForm : ConfigItem
    {
        public List<Field> Fields { get; set; }


        public ParamsForm()
        {
            Fields = new List<Field>();
        }
    }
}
