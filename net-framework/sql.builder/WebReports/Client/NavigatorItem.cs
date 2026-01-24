
using System.Collections.Generic;

namespace sql.builder.WebReports.Client
{
    internal class NavigatorItem: ConfigItem
    {
        public string Title { get; set; }
        public Folder Parent { get; set; }

        private string _path = null;

        public string Path()
        {
            if (_path == null)
            {
                if (Parent != null)
                {
                    _path =  this.Parent.Path() + "/" + this.Title;
                }
                else
                {
                    _path = this.Title;
                }
            }
           
            return _path;
        }

    }
}
