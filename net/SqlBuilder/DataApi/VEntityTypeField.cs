using System;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /*internal sealed class VEntityTypeField : VSXElement
    {
        internal VEntityTypeField(XName name)
            : base(name)
        {
        }
        internal static VEntityTypeField GetOrCreate(XElement element)
        {
            //VEntityTypeField item = new VEntityTypeField("item");
            //if (element.GetType() != item.GetType()) {
            if (element is VEntityTypeField) {
                return (VEntityTypeField)element;
            } else {
                VEntityTypeField item = new VEntityTypeField(element.Name);
                element.ReplaceWith(item);
                //item.environment = SearchEnvironment(item);
                return item;
            }
        }
        public new string Name {
             get {
                 return this.AttrOrDefault(AName_.As, string.Empty);
             }
        }
        internal string Title {
             get {
                 return this.AttrOrDefault(AName_.title, string.Empty);
             }
         }
    }*/
}