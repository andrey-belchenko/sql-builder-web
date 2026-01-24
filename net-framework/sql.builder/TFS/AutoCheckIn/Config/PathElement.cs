using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    internal class PathElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }
    }
}