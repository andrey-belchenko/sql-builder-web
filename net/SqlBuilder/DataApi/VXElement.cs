using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public class VXElement : XElement
    {
        private object tag;
        public VXElement(XName name)
            : base(name)
        {
        }
        public VXElement(XElement other)
            : base(other)
        {
        }
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }
        public string GetAttrValue(string name)
        {
            return this.AttrOrEmpty(name);
        }
        //public VEnvironment GetEnvironment()
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