using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    public partial class VSXElement : VXElement
    {


        public virtual string PartId()
        {
            return P_PartId;
        }

        public XElement AsXElementApplyingParts()
        {
            var xelement = new XElement(this);
            var vsxelement = VSXElement.Get(xelement);
            //vsxelement.environment = GetEnvironment();
            vsxelement.VirtualParent = this.GetParent();

            var elements = new List<VSXElement>();
            elements.Add(vsxelement);

            while (elements.Count != 0)
            {
                foreach (var element in elements)
                {
                    var elements1 = VSXElement.GetElementsP(element);
                    element.Elements().Remove();
                    element.Add(elements1);
                }
                elements = elements.SelectMany(VSXElement.GetElementsP).ToList();
            }


            return vsxelement;
        }
        #region GetElementsP()
        /*public List<VSXElement> GetElementsP()
        {
            var list = new List<VSXElement>();
            var list1 = this.Elements().Where(EPredicate.IsNotExcuded).ToList().Select(VSXElement.Get).ToList();
            foreach (VSXElement el in list1) {
                var els = VUsePart.PartContentOrSelf(el);
                foreach (VSXElement el1 in els) {
                    list.Add(el1);
                }
            }
            return list;
        }*/
        public static IList<VSXElement> GetElementsP(VSXElement parent)
        {
            var list = new List<VSXElement>();
            IList<XElement> elements = parent.Elements().Where(EPredicate.IsNotExcuded).ToList();
            for (int index_1 = 0; index_1 < elements.Count; index_1++)
            {
                VSXElement el = VSXElement.Get(elements[index_1]);
                VUsePart use_part = el as VUsePart;
                if (use_part != null)
                {
                    list.AddRange(use_part.Content());
                }
                else
                {
                    list.Add(el);
                }
            }
            return list;
        }
#if !FRAMEWORK_40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IList<VSXElement> GetElementsP()
        {
            return VSXElement.GetElementsP(this);
        }
        public IList<VSXElement> GetElementsP(XName name)
        {
            Contract.Assert(name != null);
            var list = new List<VSXElement>();
            IList<XElement> elements = this.Elements().Where(EPredicate.IsNotExcuded).ToList();
            for (int index_1 = 0; index_1 < elements.Count; index_1++)
            {
                XElement el = elements[index_1];
                if (el.Name == EName.usepart)
                {
                    VUsePart use_part = VSXElement.Get<VUsePart>(el);
                    IList<VSXElement> els = use_part.Content();
                    for (int index_2 = 0; index_2 < els.Count; index_2++)
                    {
                        VSXElement el1 = els[index_2];
                        if (el1.Name == name)
                        {
                            list.Add(el1);
                        }
                    }
                }
                else if (el.Name == name)
                {
                    list.Add(VSXElement.Get(el));
                }
            }
            return list;
        }
        #endregion
        #region GetDescedantsAndSelfP()
        public static IList<VSXElement> GetDescedantsAndSelfP(VSXElement parent)
        {
            Contract.Assert(parent != null);
            List<VSXElement> list = VSXElement.GetDescedantsP(parent);
            list.Add(parent);
            return list;
        }
        public IList<VSXElement> GetDescedantsAndSelfP(XName name)
        {
            Contract.Assert(name != null);
            List<VSXElement> list = this.GetDescedantsP(name);
            if (this.Name == name)
            {
                list.Add(this);
            }
            return list;
        }
        #endregion
        #region GetDescedantsP()
        public static List<VSXElement> GetDescedantsP(VSXElement parent)
        {
            Contract.Assert(parent != null);
            IList<VSXElement> childs = VSXElement.GetElementsP(parent);
            var list = new List<VSXElement>(childs.Count);
            for (int index = 0; index < childs.Count; index++)
            {
                VSXElement child = childs[index];
                list.Add(child);
                list.AddRange(VSXElement.GetDescedantsP(child));
            }
            return list;
        }
        public List<VSXElement> GetDescedantsP(XName name)
        {
            Contract.Assert(name != null);
            IList<VSXElement> childs = VSXElement.GetElementsP(this);
            var list = new List<VSXElement>();
            for (int index = 0; index < childs.Count; index++)
            {
                VSXElement child = childs[index];
                if (child.Name == name)
                {
                    list.Add(child);
                }
                list.AddRange(child.GetDescedantsP(name));
            }
            return list;
        }
        public List<VSXElement> GetDescedantsP(Func<VSXElement, bool> predicate)
        {
            Contract.Assert(predicate != null);
            IList<VSXElement> childs = VSXElement.GetElementsP(this);
            var list = new List<VSXElement>();
            for (int index = 0; index < childs.Count; index++)
            {
                VSXElement child = childs[index];
                if (predicate(child))
                {
                    list.Add(child);
                }
                list.AddRange(child.GetDescedantsP(predicate));
            }
            return list;
        }
        /*public List<VSXElement> GetDescedantsP(string name)
        {
            var list = new List<VSXElement>();
            foreach (VSXElement el in GetElementsP())
            {
                list.Add(el);
                foreach (VSXElement el1 in el.GetDescedantsP())
                {
                    list.Add(el1);
                }
            }
            if (name != null)
            {
                list = list.Where(e => e.Name.LocalName == name).ToList();
            }
            return list;
        }*/
        /*public List<VSXElement> GetDescedantsP(string[] names)
        {
            var list = new List<VSXElement>();
            foreach (VSXElement el in GetElementsP())
            {
                list.Add(el);
                foreach (VSXElement el1 in VSXElement.GetDescedantsP(el))
                {
                    list.Add(el1);
                }
            }
           
           list = list.Where(e => names.Contains( e.Name.LocalName)).ToList();
            
            return list;
        }*/
        #endregion
        public VSXElement BaseElement;
        public VSXElement UsePartElement;
        public VSXElement BaseElementOrSelf()
        {
            if (this.BaseElement != null)
            {
                return BaseElement;
            }
            else
            {
                return this;
            }
        }
    }
}