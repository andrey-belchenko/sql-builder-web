using System.Collections.Generic;

namespace sql.builder.WebReports.Client
{
    internal class Folder :  NavigatorItem
    {
 
        public List<NavigatorItem> Items { get; set; }

        public Folder()
        {
            Items = new List<NavigatorItem>();
        }

        public List<NavigatorItem> AllItems()
        {

            var items = new List<NavigatorItem>();
            foreach (var item in Items)
            {
                items.Add(item);
                if (item is Folder)
                {
                    items.AddRange((item as Folder).AllItems());
                }
            }
            return items;
        }
    }
}
