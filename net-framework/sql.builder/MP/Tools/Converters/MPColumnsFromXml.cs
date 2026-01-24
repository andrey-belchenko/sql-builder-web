using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.MP.Tools
{
    public static class MPColumns
    {
        public static MPColumn[] FromXml(XElement xquery)
        {
            var list = new List<MPColumn>();
            foreach (var xcolumn in xquery.Element("select").Elements("column"))
            {
                MPType type = null;
                if(xcolumn.Attribute("type").Value == "string")
                {
                    type = new MPStringType(int.Parse(xcolumn.Attribute("data-size").Value));
                }
                else if (xcolumn.Attribute("type").Value == "number")
                {
                    if (xcolumn.Attribute("data-size") == null)
                    {
                        type = new MPNumberType(null);   
                    }
                    else
                    {
                        type = new MPNumberType(int.Parse(xcolumn.Attribute("data-size").Value));   
                    }
                }
                else if (xcolumn.Attribute("type").Value == "date")
                {
                    type = new MPDateType();
                }

                bool isKey = (xcolumn.Attribute("key") != null && xcolumn.Attribute("key").Value == "1");
                bool isMain = (xcolumn.Attribute("main") != null && xcolumn.Attribute("main").Value == "1");
                var col = new MPColumn(xcolumn.Attribute("column").Value, type, isKey, isMain);
                list.Add(col);
            }

            return list.ToArray();
        }



        //public static MPColumn[] FromDB(string tableName)
        //{
        //    var xquery= XmlHelpers.XmlSchemeBuilder.XmlTableStructure(tableName);

        //    return FromXml(xquery);


        //}
    }
}