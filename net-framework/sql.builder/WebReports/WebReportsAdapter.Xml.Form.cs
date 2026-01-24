


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

        private static string GetEditorControlType(Editor editor)
        {
            if (editor is DateEditor)
            {
                return TextConst.AVControlType.Date;
            }
            else if (editor is SelectEditor)
            {
                if ((editor as SelectEditor).SingleSelection)
                {
                    return TextConst.AVControlType.Combo;
                }
                return TextConst.AVControlType.List;
            }
            throw new NotImplementedException(editor.GetType().Name + " is not supported");
        }

        private static string GetEditorDataType(Editor editor)
        {
            if (editor is DateEditor)
            {
                return TextConst.AVDataType.Date;
            }
            else if (editor is SelectEditor)
            {
                return TextConst.AVDataType.Number;
            }
            throw new NotImplementedException(editor.GetType().Name + " is not supported");
        }

        public static XElement GetFormXml(string reportName)
        {
            throw new NotImplementedException();
            //var reportId = ExtractIdFromTag(reportName);
            //WebReportsCache.Remove(reportId);
            //var form = WebReportsClient.GetForm(reportId);
            //return new XElement(TextConst.EName.Form,
            //      new XAttribute(TextConst.AName.Name, reportName),
            //      new XElement(TextConst.EName.Content,

            //            form.Fields.Select(field =>
            //            {
            //                var xfield = new XElement(TextConst.EName.Field,
            //                        new XAttribute(TextConst.AName.ControlType, GetEditorControlType(field.Editor)),
            //                        new XAttribute(TextConst.AName.DataType, GetEditorDataType(field.Editor)),
            //                        new XAttribute(TextConst.AName.Name, field.Name),
            //                        new XAttribute(TextConst.AName.Title, field.Label)
            //                );

            //                if (field.HasRequiredOption)
            //                {
            //                    // Пока при наличии правила required, статически устанавливается признак Mandatory
            //                    // TODO: При необходимости сделать динамическую обработку с вызовом метода 
            //                    xfield.Add(new XAttribute(TextConst.AName.Mandatory, "1"));
            //                }

            //                if (field.Editor is SelectEditor)
            //                {
            //                    //xfield.Add(new XAttribute(TextConst.AName.RowsLimit, "100"));
            //                    xfield.Add(
            //                       new XElement(TextConst.EName.ListQuery,
            //                           new XElement(TextConst.EName.Query,
            //                               new XAttribute(TextConst.AName.Name, CreateEditorMethodTag(reportId, field.Id, field.Editor.Id, "listItems"))
            //                           )
            //                       )
            //                   );
            //                }

            //                if (field.HasDefaultValue)
            //                {
            //                    xfield.Add(
            //                        new XElement(TextConst.EName.DefaultQuery,
            //                            new XElement(TextConst.EName.Query,
            //                                new XAttribute(TextConst.AName.Name, CreateFieldMethodTag(reportId, field.Id, "defaultValue"))
            //                            )
            //                        )
            //                    );
            //                }
            //                return xfield;
            //            }
            //        )
            //    )
            //);

        }

    }
}
