


using sql.builder.DataApi;
using sql.builder.WebReports.Client;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.WebReports
{
    internal static partial class WebReportsAdapter
    {

        

        public static bool IsWebQuery(XElement value)
        {

            return false;

            //var xfrom = value.Element(TextConst.EName.From);

            //if (xfrom == null)
            //{
            //    return false;
            //}

            //var xtable = xfrom.Element(TextConst.EName.Table);

            //if (xtable == null)
            //{
            //    return false;
            //}

            //var name = xtable.Attribute(TextConst.AName.Name)!=null ? xtable.Attribute(TextConst.AName.Name).Value : null;
            //return IsWebItem(name);
        }

        public static XElement GetQueryXml(string queryName)
        {
            throw new NotImplementedException();
            //var jtag = DeserializeTag(queryName);
            //var methodName = jtag["methodName"]!=null ? jtag["methodName"].ToString() : null;
            ////var itemId = jtag["itemId"].ToString();
            //var fieldId = jtag["fieldId"]!=null ? jtag["fieldId"].ToString(): null;
            //var reportId = jtag["reportId"].ToString();
            //Field field = null;
            //if (fieldId != null)
            //{
            //    field = WebReportsClient.GetField(reportId,fieldId);
            //}
            //var xselect = new XElement(TextConst.EName.Select);
            //var deps = new System.Collections.Generic.List<string>();

            //if (methodName == "defaultValue")
            //{
            //    deps.AddRange(field.DefaultValueDeps);
            //}



            //if (field != null && field.Editor is SelectEditor && methodName == "listItems")
            //{
            //    var editor = field.Editor as SelectEditor;
            //    deps.AddRange(editor.ListItemsDeps);
            //    var i = 0;
            //    foreach (var colName in editor.AllColumns())
            //    {
            //        i++;
            //        var fieldName = colName;
            //        var isKey = editor.KeyField == colName ? "1" : "0";
            //        var isListCol = editor.Columns.Contains(colName) ? "1" : "0";
            //        var isName = editor.DisplayField == colName ? "1" : "0";
            //        if (isListCol == "1")
            //        {
            //            fieldName = "field"+i; // Если имя и заголовок у колонки одинаковые она не отображается
            //        }
            //        var xcolumn = new XElement(TextConst.EName.Column,
            //            new XAttribute(TextConst.AName.Table, "a"),
            //            new XAttribute(TextConst.AName.Column, fieldName),
            //            new XAttribute(TextConst.AName.Title, colName),
            //            new XAttribute(TextConst.AName.IsListColumn, isListCol), // Похоже, это ни на что не влияет
            //            new XAttribute(TextConst.AName.Key, isKey),
            //            new XAttribute(TextConst.AName.IsNameColumn, isName) // Похоже, это ни на что не влияет
            //        );
            //        if (isKey == "1")
            //        {
            //            // TODO: если понадобится предусмотреть другие типы, пока не понятно как
            //            xcolumn.Add(new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Number));
            //        }
            //        xselect.Add(xcolumn);
            //    }
            //}
            //else if (field != null && field.Editor is SelectEditor && methodName == "defaultValue")
            //{
            //    xselect.Add(
            //            new XElement(TextConst.EName.Column,
            //                new XAttribute(TextConst.AName.Table, "a"),
            //                new XAttribute(TextConst.AName.Column, "key"),
            //                 // TODO: если понадобится предусмотреть другие типы, пока не понятно как
            //                new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Number)
            //            ),
            //            new XElement(TextConst.EName.Column,
            //                new XAttribute(TextConst.AName.Table, "a"),
            //                new XAttribute(TextConst.AName.Column, "name")
            //            )
            //        );

            //}
            //else
            //{
            //    xselect.Add(
            //            new XElement(TextConst.EName.Column,
            //                new XAttribute(TextConst.AName.Table, "a"),
            //                new XAttribute(TextConst.AName.Column, "value")
            //            )
            //        );
            //}
            //var xquery = new XElement(TextConst.EName.Query,
            //            xselect,
            //            new XElement(TextConst.EName.From,
            //                new XElement(TextConst.EName.Table,
            //                    new XAttribute(TextConst.AName.Name, queryName),
            //                    new XAttribute(TextConst.AName.As, "a")
            //                )
            //            )
            //        );

            //if (deps.Any())
            //{
            //    var xparams = deps.Select(it => new XElement(TextConst.EName.Param, 
            //             new XAttribute(TextConst.AName.Name, it),
            //             // TODO: сделать подстановку типа по типу поля от которого зависим, когда понядобится 
            //             new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Array)

            //        )).ToArray();
            //    xquery.AddFirst(new XElement(TextConst.EName.Params, xparams));
            //}
            //return xquery;

        }

        public static XElement GetReportXml(string reportName)
        {
            return new XElement(TextConst.EName.Report,
                        new XAttribute(TextConst.AName.Name, reportName),
                        new XAttribute(TextConst.AName.Form, reportName),
                         new XAttribute(TextConst.AName.Title, ""),
                         new XElement(TextConst.EName.PrintTemplates,
                            new XElement(TextConst.EName.Excel,
                                new XElement(TextConst.EName.Template)
                            )
                         )
                   );

        }


    }
}
