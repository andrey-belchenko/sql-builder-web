namespace sql.builder.DataApi
{
    public sealed class VLabel : VSXElement
    {
        public VLabel()
            : base(EName.label)
        {
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
    }
}