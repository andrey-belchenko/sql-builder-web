using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;dimension name="" time-type="" timeline="" /&gt;
    /// &lt;dimension name="" class-type="" /&gt;
    /// </summary>
    internal sealed partial class VDimension : VSXElement
    {
        internal VDimension()
            : base(EName.dimension)
        {
        }
        public override string XName {
            get {
                XAttribute attr = this.Attribute(AName_.name);
                return attr.Value;
            }
        }
        /*internal List<string> GetTimelineDimNames()
        {
            return XmlReports.Environment.GetDimensionsByTimeline(P_Timeline).Select(e => e.XName).ToList();
        }*/
        internal VQuery Query()
        {
            string query_name = this.P_CalledQuery;
            if (string.IsNullOrEmpty(query_name)) {
                return null;
            } else {
                return XmlReports.Environment.GetQuery(query_name);
            }
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            VQuery qry = this.Query();
            if (qry != null) {
                return new List<VSXElement>(1) { qry };
            } else {
                return new List<VSXElement>(0);
            }
        }
        internal XElement GetTimeAttrExpression(string tableAlias, string columnName, string attrName)
        {
            XElement col = Factory.NewColumn(tableAlias, columnName);
            XElement el = null;
            switch (this.P_TimeType) {
                case TextConst.AVTimeType.Month:
                    el = GetMonthAttrExpression(col, attrName);
                    break;
                case TextConst.AVTimeType.Month2:
                    el = GetMonth2AttrExpression(col, attrName);
                    break;
                case TextConst.AVTimeType.Day:
                    el = GetDayAttrExpression(col, attrName);
                    break;
                case TextConst.AVTimeType.Year:
                    el = GetYearAttrExpression(col, attrName);
                    break;
                case TextConst.AVTimeType.Str:
                    el = GetStrAttrExpression(col, attrName);
                    break;
                case TextConst.AVTimeType.Num:
                    el = GetNumAttrExpression(col, attrName);
                    break;
            }
            return el;
        }
        internal string GetTimeAttrType(string attrName)
        {
            if (string.IsNullOrEmpty(attrName)) {
                return string.Empty;
            }
            switch (this.P_TimeType) {
                case TextConst.AVTimeType.Day:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.Date;
                    }
                    break;
                case TextConst.AVTimeType.Month:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.Number;
                        case TextConst.AVTimeAttr.Name:
                            return TextConst.AVDataType.String;
                    }
                    break;
                case TextConst.AVTimeType.Month2:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.Number;
                    }
                    break;
                case TextConst.AVTimeType.Str:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.String;
                    }
                    break;
                case TextConst.AVTimeType.Num:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.Number;
                    }
                    break;
                case TextConst.AVTimeType.Year:
                    switch (attrName) {
                        case TextConst.AVTimeAttr.Val:
                            return TextConst.AVDataType.Number;
                    }
                    break;
             }
            return string.Empty;
         }
        private static XElement GetMonthAttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Name:
                    el = Factory.NewCall(TextConst.AVFunction.YmToChar, column);
                    break;
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        private static XElement GetMonth2AttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Name:
                    el = Factory.NewCall(TextConst.AVFunction.Ym2ToChar, column);
                    break;
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        private static XElement GetDayAttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        private static XElement GetYearAttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        private static XElement GetStrAttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        private static XElement GetNumAttrExpression(XElement column, string attrName)
        {
            XElement el = null;
            switch (attrName) {
                case TextConst.AVTimeAttr.Val:
                    el = column;
                    break;
            }
            return el;
        }
        #region TimeType
        public override string P_TimeType {
            get {
                return this.AttrOrEmpty(AName_.time_type);
            }
            set {
                this.SetAttributeNotEmpty(AName_.time_type, value);
                if (!string.IsNullOrEmpty(value)) {
                    this.RemoveAttribute(AName_.class_type);
                }
            }
        }
        public override bool P_TimeType_Exists()
        {
            return true;
        }
        public void P_TimeType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_TimeType_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromStringArray(table, TextConst.AVTimeTypeArray.All);
            table.AddRow(string.Empty, string.Empty);
        }
        #endregion
        #region CalledQuery
        public override string P_CalledQuery {
            get {
                return this.AttrOrEmpty(AName_.class_type);
            }
            set {
                this.SetAttributeNotEmpty(AName_.class_type, value);
                if (!string.IsNullOrEmpty(value)) {
                    this.RemoveAttribute(AName_.timeline);
                    this.RemoveAttribute(AName_.time_type);
                }
            }
        }
        public override string P_CalledQuery_Title()
        {
            return "Тип объекта";
        }
        public override void P_CalledQuery_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromRealQueries(table);
        }
        public override bool P_CalledQuery_Exists()
        {
            return true;
        }
        #endregion
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.P_Name) + " " + this.P_CalledQuery;
            string timeline = this.P_Timeline;
            if (!string.IsNullOrEmpty(timeline)) {
                s += timeline + ".";
            }
            s += this.P_TimeType;
            return s;
        }
        #endregion
        #region Timeline
        public override string P_Timeline {
            get {
                return this.AttrOrEmpty(AName_.timeline);
            }
            set {
                this.SetAttributeNotEmpty(AName_.timeline, value);
            }
        }
        public override bool P_Timeline_Exists()
        {
            return true;
        }
        #endregion
        #region ParentFieldName
        public override void P_ParentFieldName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VQuery query = this.Query();
            if (query != null) {
                IList<VSXElement> cols = query.Columns();
                for (int index = 0; index < cols.Count; index++) {
                    string name = cols[index].XName;
                    table.AddRow(name, name);
                }
            }
        }
        public override bool P_ParentFieldName_Exists()
        {
            return true;
        }
        #endregion
    }
}