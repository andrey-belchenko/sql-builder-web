using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VGridColumns : VOutputElement, IVParent
    {
        public VGridColumns()
            : base(EName.columns)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Column, TextConst.EName.Call, TextConst.EName.Fact, TextConst.EName.Band, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region Списки контекстного добавления элементов
        //public override void MakeChildContextLists(XElement xlists)
        //{
        //    MakeFunctionList(xlists, "any");
        //    MakeColumnsList(xlists);
        //}
        public override void CL_Column_Content(List<XElement> list)
        {
            MakeColumnsList(list, false);
        }
        public override List<VContextListsType> ContextListAllowedTypes()
        {
            return new List<VContextListsType>() { VContextListsType.Column };
        }
        #endregion
    }
}