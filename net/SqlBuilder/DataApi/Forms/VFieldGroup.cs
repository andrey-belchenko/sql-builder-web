using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public class VFieldGroup : VOutputElement, IVParent
    {
        protected VFieldGroup(XName name)
            : base(name)
        {
        }
        public VFieldGroup()
            : base(EName.fieldgroup)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Column, TextConst.EName.Fact, TextConst.EName.Call, TextConst.EName.Grid,
                                                TextConst.EName.Field, TextConst.EName.UseField, TextConst.EName.FieldGroup, TextConst.EName.TabContainer,
                                                TextConst.EName.SplitContainer, TextConst.EName.UseForm, TextConst.EName.Splitter, TextConst.EName.Label, TextConst.EName.Menu,
                                                TextConst.EName.ScrollArea, TextConst.EName.UICommand, TextConst.EName.Toolbar, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return Italic(this.P_SelfTitle);
        }
        #endregion
        //#region LayoutMode
        //public override bool P_LayoutMode_Exists()
        //{
        //    return true;
        //}
        //public override bool P_LayoutMode_Editable()
        //{
        //    return true;
        //}
        //#endregion
        #region Expanded
        public override bool P_Expanded_Exists()
        {
            return true;
        }
        #endregion
        #region Uncollapsible
        public override bool P_Uncollapsible_Exists()
        {
            return true;
        }
        #endregion
        #region IsForm
        public override bool P_IsForm_Exists()
        {
            return true;
        }
        #endregion
        #region FormSize
        public override bool P_FormSize_Exists()
        {
            return (this.P_IsForm == TextConst.AVBool.True);
        }
        #endregion
        #region NoBorder
        public override bool P_NoBorder_Exists()
        {
            return true;
        }
        #endregion
        #region Position
        public override bool P_Position_Exists()
        {
            return true;
        }
        #endregion
        #region Alias
        public override bool P_Alias_Exists()
        {
            return true;
        }
        #endregion
        #region Editable
        public override bool P_Editable_Exists()
        {
            return true;
        }
        #endregion
        #region ShowToolBar
        public override bool P_ShowToolBar_Exists()
        {
            return true;
        }
        #endregion
        #region WidthPerc
        public override bool P_WidthPerc_Exists()
        {
            VSXElement parent = this.GetParent();
            return parent is VSplitContainer || parent is VFieldGroup;
        }
        public override bool P_WidthPerc_Editable()
        {
            return true;
        }
        #endregion
    }
}