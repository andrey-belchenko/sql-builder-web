using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VSelect : VOutputElement, IVParent
    {
        public VSelect()
            : base(EName.select)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Column, TextConst.EName.Call, TextConst.EName.Const, TextConst.EName.Fact, TextConst.EName.Query, TextConst.EName.Band, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region CalledQuery
        public override string P_CalledQuery
        {
            get
            {
                return this.AttrOrEmpty(AName_.call);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.call, value);
            }
        }
        public override string P_CalledQuery_Title()
        {
            return "Запрос для получения данных при отложенной загрузке";
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
        #region NodeText
        //public override string GetNodeInfo()
        //{
        //    return GetNodeOtherInfo();
        //}
        public override string GetNodeOtherInfo()
        {
            string called_query = this.P_CalledQuery;
            string s;
            if (!string.IsNullOrEmpty(called_query))
            {
                s = " (fetch using " + Bold(called_query) + ")";
            }
            else
            {
                s = string.Empty;
            }
            return s;
        }
        #endregion
        #region Списки контекстного добавления элементов
        //public override void MakeChildContextLists(XElement xlists)
        //{
        //    MakeFunctionList(xlists, "any");
        //    MakeColumnsList(xlists);
        //}
        public override void CL_Call_Content(List<XElement> list)
        {
            MakeFunctionList(list, "any");
        }
        public override void CL_Column_Content(List<XElement> list)
        {
            MakeColumnsList(list, true);
        }
        public override List<VContextListsType> ContextListAllowedTypes()
        {
            return new List<VContextListsType>() {
                VContextListsType.Call,
                VContextListsType.Column
            };
        }
        #endregion
    }
}