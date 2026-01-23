using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    [ConfigurationCollection(typeof(ProjectElement), AddItemName = "project")]
    internal class ProjectsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProjectElement)(element)).Name;
        }
        public ProjectElement this[int idx]
        {
            get { return (ProjectElement)BaseGet(idx); }
        }
    }
}