using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VFunction : VSXElement
    {
        public VFunction()
            : base(EName.function)
        {
        }
        public string MakeText()
        {
            XElement el = new XElement(this);
            IList<XElement> list = new List<XElement>(el.Descendants(EName.val));
            int index;
            for (index = 0; index < list.Count; index++) {
                XElement val = list[index];
                val.ReplaceWith(new XText("[par" + (index + 1).ToString() + "]"));
            }
            // Убираем переводы строки
            StringBuilder sb = new StringBuilder(el.Value);
            sb.Replace("\r\n", " ");
            // sb.Replace("\r", "");
            // sb.Replace("\n", "");
            index = sb.Length - 1;
            while (index >= 0) {
                char ch = sb[index];
                if (ch == '\r' || ch == '\n') {
                    sb.Remove(index, 1);
                }
                index--;
            }
            return sb.ToString();
        }
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeValue(AName_.name, value);
            }
        }
        public override bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        public override void P_DataType_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVTypeArray.Real);
            table.AddRow(TextConst.AVDataType.Bool, TextConst.AVDataType.Bool);
            table.AddRow(TextConst.AVDataType.Variant, TextConst.AVDataType.Variant);
        }
        public override string P_DataType_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Text
        public override string P_Text {
            get {
                if (this.IsEmpty) {
                    return string.Empty;
                }
                string text = this.ToString(SaveOptions.DisableFormatting);
                int pos_1 = text.IndexOf('>') + 1;
                int pos_2 = text.LastIndexOf('<');
                return text.Substring(pos_1, pos_2 - pos_1);
            }
            /*set {
                XElement el = XElement.Parse("<function>" + value + "</function>");
                this.RemoveNodes();
                this.Add(el.Nodes());
            }*/
        }
        public bool P_Text_Editable()
        {
            return false;
        }
        public string P_Text_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        public override bool P_Text_Exists()
        {
            return true;
        }
        #endregion
    }
}