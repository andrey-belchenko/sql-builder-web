using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public abstract class VOutputElement: VSXElement
    {
        protected VOutputElement(XName name)
            : base(name)
        {
        }
    }
}