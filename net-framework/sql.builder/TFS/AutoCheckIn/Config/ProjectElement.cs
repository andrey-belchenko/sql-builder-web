using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    internal class ProjectElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("selected"), DefaultSettingValue("false")]
        public bool Selected
        {
            get { return ((bool)(base["selected"])); }
            set { base["selected"] = value; }
        }

        [ConfigurationProperty("takeServerPaths")]
        public TakeServerPathsCollection TakeServerPaths
        {
            get { return ((TakeServerPathsCollection)(base["takeServerPaths"])); }
        }

        [ConfigurationProperty("takeLocalPaths")]
        public TakeLocalPathsCollection TakeLocalPaths
        {
            get { return ((TakeLocalPathsCollection)(base["takeLocalPaths"])); }
        }
    }
}