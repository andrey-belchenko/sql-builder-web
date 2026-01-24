using System.Configuration;

namespace sql.builder.TFS.AutoCheckIn.Config
{
    [ConfigurationCollection(typeof(FolderElement), AddItemName = "folder")]
    internal class FoldersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)(element)).Name;
        }
        public FolderElement this[int idx]
        {
            get { return (FolderElement)BaseGet(idx); }
        }
    }
}