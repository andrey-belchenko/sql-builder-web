using System.Collections.Generic;

namespace sql.builder.WebReports.Client
{
    internal class Field : ConfigItem
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public Editor Editor { get; set; }
        public bool HasDefaultValue { get; set; }
        public bool HasRequiredOption { get; set; }

        public List<string> DefaultValueDeps { get; set; }

    }
}
