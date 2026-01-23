using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    [ConfigurationCollection(typeof(PathElement), AddItemName = "path")]
    internal class TakeServerPathsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)(element)).Name;
        }
        public PathElement this[int idx]
        {
            get { return (PathElement)BaseGet(idx); }
        }
    }
}