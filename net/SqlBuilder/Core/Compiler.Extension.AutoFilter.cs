using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder
{
    public partial class Compiler
    {
        private static void AddQueryAutoFilterParsAndConds(XElement xquery) 
            // предположительно ресурсоемкая операция, может замедлить precompile
        {
            if (xquery.AttrOrDefault(AName.auto_filter, string.Empty) != TextConst.AVBool.True) {
                return;
            }
            if (!xquery.Elements(EName.select).Elements().Any(e => e.AttrOrDefault(AName.auto_filter, false))) {
                if (!xquery.Elements(EName.from).Descendants().Any(e => e.AttrOrDefault(AName.auto_filter, false))) {
                    return;
                }
            }
            // если условия только на линках , проскаивает мимо, но если убрать то ошибка.
            string name = xquery.Attribute(AName.name).Value;
            // Емцов - вызов Environment запускал бесконечный цикл PreCompile
            // теперь без кэширования - понаблюдать за производительностью
            VQuery query = XmlReports.Environment.GetQuery(name);
            //XElement q = XmlReports.Environment.Manager.Elements("queries").Elements("query").FirstOrDefault(e => e.Attribute("name").Value == name);
            //var query = (VQuery)VSXElement.Get(q);
            var xpars = xquery.Element(EName.@params);
            if (xpars == null) {
                xpars = new XElement(EName.@params);
                xquery.AddFirst(xpars);
            }
            XElement xqubeWhere = null;
            var xqube = xquery.Elements(EName.from).Elements(TextConst.EName.Qube).FirstOrDefault();
            if (xqube != null) {
                xqubeWhere = xqube.Element(EName.where);
                if (xqubeWhere == null) {
                    xqubeWhere = new XElement(EName.where);
                    xqube.Add(xqubeWhere);
                }
            }
            var xwhere = xquery.Element(EName.where);
            XElement[] wExprs = null;
            if (xwhere == null) {
                xwhere = new XElement(EName.where);
                xquery.Add(xwhere);
            } else {
                wExprs = xwhere.Elements().ToArray();
                wExprs.Remove();
            }
            XElement xand = Factory.NewCall(TextConst.AVFunction.And, Factory.NewCall(TextConst.AVFunction.True));
            if (wExprs != null) {
                xand.Add(wExprs);
            }
            xwhere.Add(xand);
            if (xqubeWhere == null) {
                xqubeWhere = xand;
            }
            var elsForFilter = query.ElementsForAutoFilter();
            foreach (var el in elsForFilter) {
                IList<XElement> xpar = null;
                IList<XElement> xcond = null;
                bool isfact = false;
                if (el is VQueryCall) {
                    xpar = createListParams(el);
                    xcond = createListCond(el);
                } else {
                    switch (el.XDataType()) {
                        case TextConst.AVDataType.Date:
                            xpar = createRangeParams(el);
                            xcond = createRangeCond(el);
                            break;
                        case TextConst.AVDataType.Number:
                            xpar = createRangeParams(el);
                            xcond = createRangeCond(el);
                            break;
                        case TextConst.AVDataType.String:
                            xpar = createStringParams(el);
                            xcond = createStringCond(el);
                            break;
                        case TextConst.AVDataType.Clob:
                            xpar = createStringParams(el);
                            xcond = createStringCond(el);
                            break;
                    }
                    if (el.GetDescedantsAndSelfP(EName.fact).Count != 0) {
                        isfact = true;
                    }
                }
                xpars.Add(xpar);
                if (isfact) {
                    xand.Add(xcond);
                } else {
                    xqubeWhere.Add(xcond);
                }
            }
            xquery.Descendants().Attributes(AName.auto_filter).Remove();
        }
        private static IList<XElement> createStringParams(VSXElement el)
        {
            string dataType = el.XDataType();
            string parName = TextConst.Pfx.ParamVar + el.XName;
            XElement xpar = Factory.NewParam(parName, dataType);
            return new XElement[1] { xpar };
        }
        private static IList<XElement> createStringCond(VSXElement el)
        {
            string parName = TextConst.Pfx.ParamVar + el.XName;
            XElement xcall = Factory.NewCall(TextConst.AVFunction.LikeSNull);
            xcall.Add(new XAttribute(AName.optional, TextConst.AVBool.True));
            xcall.Add(new XElement(el));
            xcall.Add(Factory.NewUseParam(parName));
            return new XElement[1] { xcall };
        }
        private static IList<XElement> createListParams(VSXElement el)
        {
            string parName = TextConst.Pfx.ParamVar + el.XName;
            XElement xpar = Factory.NewParam(parName, TextConst.AVDataType.Array);
            return new XElement[1] { xpar };
        }
        private static IList<XElement> createListCond(VSXElement el)
        {
            VLink link = (VLink)el;
            VQuery qry = link.Query();
            VSXElement keyCol = qry.KeyColumn();
            string parName = TextConst.Pfx.ParamVar + el.XName;
            string funcName;
            if (keyCol.XDataType() == TextConst.AVDataType.String) {
                funcName = TextConst.AVFunction.InSNull;
            } else {
                funcName = TextConst.AVFunction.InNNull;
            }
            XElement xcall = Factory.NewCall(funcName);
            xcall.Add(new XAttribute(AName.optional, TextConst.AVBool.True));
            xcall.Add(Factory.NewColumn(el.XName, keyCol.XName));
            xcall.Add(Factory.NewUseParam(parName));
            return new XElement[1] { xcall };
        }
        private static IList<XElement> createRangeParams(VSXElement el)
        {
            string dataType = el.XDataType();
            string parName = TextConst.Pfx.ParamVar + el.XName;
            XElement[] pars = new XElement[2];
            pars[0] = Factory.NewParam(parName + "1", dataType);
            pars[1] = Factory.NewParam(parName + "2", dataType);
            return pars;
        }
        private static IList<XElement> createRangeCond(VSXElement el)
        {
            string parName = TextConst.Pfx.ParamVar + el.XName;
            XElement xcall_1 = Factory.NewCall(TextConst.AVFunction.GreaterOrEqual);
            xcall_1.Add(new XAttribute(AName.optional, TextConst.AVBool.True));
            xcall_1.Add(new XElement(el));
            xcall_1.Add(Factory.NewUseParam(parName + "1"));
            //
            XElement xcall_2 = Factory.NewCall(TextConst.AVFunction.LessOrEqual);
            xcall_2.Add(new XAttribute(AName.optional, TextConst.AVBool.True));
            xcall_2.Add(new XElement(el));
            xcall_2.Add(Factory.NewUseParam(parName + "2"));
            //
            return new XElement[2] { xcall_1, xcall_2 };
        }
    }
}
