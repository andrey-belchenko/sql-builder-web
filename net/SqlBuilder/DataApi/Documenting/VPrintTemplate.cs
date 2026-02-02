using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi.Documenting
{
    /*public class VPrintTemplate:VSourcedElement
    {
        public VPrintTemplate(XElement element)
            : base(element)
        {

    
        }

        public string XReport()
        {
            return GetAttrValue(TextConst.AName.Report);
        }
        public VReport Report()
        {
            return GetEnvironment().GetReport(XReport());
        }

        public override List<VQueryCall> AllSources()
        {
           return Report().Queries();
        }

        public static XElement ProcessTemplateInfo(XElement templateInfo)
        {
            templateInfo = new XElement(templateInfo);
            foreach (XElement templateSheet in templateInfo.Elements(TextConst.EName.ExcelSheet).ToList())
            {
                foreach (XElement colinfo in templateSheet.Elements(TextConst.EName.ExcelColumn).ToList())
                {
                    List<string> varNames = new List<string>();
                    foreach (XElement xvar in colinfo.Elements(TextConst.EName.Column).ToList())
                    {
                        string[] vv = xvar.Value.Split('.');
                        string qname = vv[0];
                        string cname = vv[vv.Length - 1];
                        string vname = qname + "." + cname;

                        if (!varNames.Contains(vname))
                        {
                            varNames.Add(vname);
                            xvar.SetAttributeValue(TextConst.AName.Table, qname);
                            xvar.SetAttributeValue(TextConst.AName.Column, cname);
                            xvar.Value = "";
                        }
                        else
                        {
                            xvar.Remove();
                        }
                    }

                }

            }
                        return templateInfo;
        }


        public override string XName
        {
            get
            {
                return GetAttrValue("name");
            }
        }

        
        #region NodeText



        public override string GetNodeOtherInfo()
        {
            
            return XName;
        }

        #endregion
    }*/
}
