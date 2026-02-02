using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
	public sealed partial class VPart : VSXElement
    {
        public VPart()
            : base(EName.part)
        {
            this.KeyField = AName_.id;
        }
        public override string PartId()
        {
            return this.AttrOrDefault(AName_.id, string.Empty);
        }
        public List<VSXElement> Content()
        {
            List<VSXElement> content = Elements().Where(e => e.Name != EName.@params).ToList().SelectAsArray(VSXElement.Get).ToList();
            return content;
        }
        public static void ApplyParams(XElement element, XElement factParams, XElement formalParams)
        {
            int i = 0;
            if (formalParams == null) return;
            foreach (XElement formalParam in formalParams.Elements()) {
                //  !!! Лишние повторения для каждого элемента. Пределать?
                string param_name = formalParam.AttrOrDefault(AName_.name, string.Empty);
                XElement factParam = factParams.Elements().SearchByAttribute(AName_.parname, param_name);
                if (factParam == null) {
                    factParam = factParams.Elements().ElementAt(i);
                }
                foreach (XElement el in element.Descendants(EName.useparam).ToList()) {
                    if (el.Attribute(AName_.name).Value == param_name) {
                        el.ReplaceWith(new XElement(factParam));
                    }
                }
                string text = "";
                if (factParam != null) {
                    text = factParam.Value.Replace("'", "");
                }
                string svar = "[:" + param_name + "]";
                foreach (XAttribute attr in element.DescendantsAndSelf().Attributes()) {
                    string value = attr.Value;
                    if (value.Contains(svar)) {
                        attr.Value = value.Replace(svar, text);
                    }
                }
                i++;
            }
        }
        public List<VSXElement> Content(VUsePart usePart)
        {
            List<VSXElement> content = this.Content();
            //List<VSXElement> content = Elements().Where(e => e.Name.LocalName != "params").ToList()
            //    .Select(e1=> VSXElement.Get( new XElement(e1))).ToList();
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement element in content) {
                XElement newElement = VSXElement.GetP(new XElement(element), this);
                ApplyParams(newElement, usePart, this.Element(EName.@params));
                Cmn.CopyAttribute(usePart, newElement, TextConst.AName.As);
                Cmn.CopyAttribute(usePart, newElement, TextConst.AName.Description);
                Cmn.CopyAttribute(usePart, newElement, TextConst.AName.Title);
                Cmn.CopyAttribute(usePart, newElement, TextConst.AName.Group);
                VSXElement newVElement = VSXElement.GetP(newElement, this);
                list.Add(newVElement);
                newVElement.BaseElement = element;
                newVElement.UsePartElement = usePart;
            }
            List<VSXElement> list1 = new List<VSXElement>();
            //Кусок ниже не проверен
            foreach (VSXElement el in list) {
                //if (el is VUsePart)
                //{
                //    var el1 = el;
                //}
                foreach (VSXElement el1 in VUsePart.PartContentOrSelf(el)) {
                    list1.Add(el1);
                    el1.VirtualParent = usePart.GetParent();
                }
            }
            return list1;
        }
        public VUsePart FirstUse()
        {
            //XElement el=   GetEnvironment().SchemeNative.Descendants("usepart").Where(e =>e.Attribute("part")!=null && e.Attribute("part").Value == P_IdName).FirstOrDefault();
            //if (el != null)
            //{
            //    return (VUsePart)Get(el);
            //}
            return FirstUse(XmlReports.Environment, this.P_IdName);
        }
        public static VUsePart FirstUse(VEnvironment env, string name)
        {
            XElement el = env.Manager.GetNativeScheme().Descendants(EName.usepart).SearchByAttribute(AName_.part, name);
            if (el != null) {
                return VSXElement.Get<VUsePart>(el);
            } else {
                return null;
            }
        }
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.id);
            }
            set {
                this.SetIdName(AName_.id, value);
            }
        }
        public override bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
    }
}