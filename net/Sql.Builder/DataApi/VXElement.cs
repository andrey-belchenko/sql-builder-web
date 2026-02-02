using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal class VXElement: XElement
    {
        private object tag;
        internal VXElement(XName name)
            : base(name)
        {
        }
        internal VXElement(XElement other)
            : base(other)
        {
        }
        internal object Tag {
            get {
                return this.tag;
            }
            set {
                this.tag = value;
            }
        }
        internal string GetAttrValue(string name)
        {
            return this.AttrOrEmpty(name);
        }
        //internal VEnvironment GetEnvironment()
        //{
            //if (this.environment == null)
            //{
            //    this.environment = SearchEnvironment(this);
            //}
            //return this.environment;
        //    return XmlReports.Environment;
        //}
        //public VEnvironment environment;
    }
}