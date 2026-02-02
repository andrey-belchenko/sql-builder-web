using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VParam : VQueryCall, IVParent
    {
        public VParam()
            : base(EName.param)
        {
        }
        public override VQuery Query()
        {
            return XmlReports.Environment.GetQuery(this.P_CalledQuery);
        }
        public bool IsObject()
        {
            return this.P_CalledQuery != string.Empty;
        }
        public VSXElement GetFactParam(List<VSXElement> factParams, int index)
        {
            if (factParams.Any(VSXElement.HasParameterName)) {
                string name = this.AName();
                for (int i = 0; i < factParams.Count; i++) {
                    VSXElement param = factParams[i];
                    if (param.P_ParName == name) {
                        return param;
                    }
                }
                return null;
            } else if (factParams.Count > index) {
                return factParams[index];
            } else {
                return null;
            }
        }
        public object GetRuntimeValue(List<VSXElement> factParams, VDataSet dataSet, DataRow row, VDataColumn col, int index)
        {
            VSXElement factParam = this.GetFactParam(factParams, index);
            if (factParam == null) {
                VSXElement defVal = this.GetElementsP().FirstOrDefault();
                if (defVal != null) {
                    return defVal.GetRuntimeValue(dataSet, row, col);
                } else {
                    return Cmn.undefinedString;
                }
            } else {
                object val;
                //if ((factParam is VUseParam) && paramsDataSet != null)
                //{
                //    val = factParam.GetRuntimeValue(paramsDataSet, null, null);
                //}
                //else
                //{
                     val = factParam.GetRuntimeValue(dataSet, row,col);
                //}
                return val;
            }
        }
        public override VField Field()
        {
            return XmlReports.Environment.GetField(this.P_Field);
        }
        public override List<VSXElement> GetUsedElements()
        {
            var list = new List<VSXElement>(2);
            list.Add(this.Query());
            list.Add(this.Field());
            return list;
        }
        private static string[] child_nodes = { TextConst.EName.Const, TextConst.EName.Link, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            string s;
            if (!string.IsNullOrEmpty(this.P_IsRet)) {
                s = "out ";
            } else {
                s = string.Empty;
            }
            s += Bold(this.P_FormalParName) + " ";
            string query = this.P_CalledQuery;
            if (string.IsNullOrEmpty(query)) {
                s += this.P_DataType;
            } else {
                s += query;
            }
            s += " " + Italic(this.P_SelfTitle);
            return s;
        }
        #endregion
        #region SelfDataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        public override string P_DataType_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        public override void P_DataType_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromStringArray(table, TextConst.AVTypeArray.Real);
            table.AddRow(TextConst.AVDataType.Array, TextConst.AVDataType.Array);
        }
        #endregion
        #region CalledQuery
        public override string P_CalledQuery {
            get {
                return this.AttrOrEmpty(AName_.class_type);
            }
            set {
                this.SetAttributeNotEmpty(AName_.class_type, value);
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
        #region FormalParName
        public override bool P_FormalParName_Exists()
        {
            return true;
        }
        public override string P_FormalParName_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        //#region FormalParNameS
        //public override bool P_FormalParNameS_Exists()
        //{
        //    return true;
        //}
        //#endregion
        public override bool P_Field_Exists()
        {
            return true;
        }
        #region RowsLimit
        public override bool P_RowsLimit_Exists()
        {
            return true;
        }
        #endregion
        #region ParentFieldName
        public override bool P_ParentFieldName_Exists()
        {
            return true;
        }
        #endregion
        #region ParamType
        public override string P_ParamType {
            get {
                return this.AttrOrEmpty(AName_.param_type);
            }
            set {
                this.SetAttributeNotEmpty(AName_.param_type, value);
            }
        }
        public override bool P_ParamType_Exists()
        {
            return true;
        }
        public void P_ParamType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_ParamType_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromStringArray(table, TextConst.AVParamTypesArray.All);
        }
        #endregion
        #region IsRet
        public override bool P_IsRet_Exists()
        {
            return true;
        }
        #endregion
    }
}
