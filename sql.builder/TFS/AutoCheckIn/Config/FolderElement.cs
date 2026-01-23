using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    internal class FolderElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("gated-checkin")]
        public bool GatedCheckIn
        {
            get { return (bool)base["gated-checkin"]; }
            set { base["gated-checkin"] = value; }
        }

        [ConfigurationProperty("build-name", DefaultValue = null)]
        public string BuildName
        {
            get { return (string)base["build-name"]; }
            set { base["build-name"] = value; }
        }

        [ConfigurationProperty("forbiddenProjects")]
        public ProjectsCollection ForbiddenProjects
        {
            get { return ((ProjectsCollection)(base["forbiddenProjects"])); }
        }
    }
}