namespace sql.builder.DataApi
{
    public sealed class VFormat : VSXElement
    {
        public VFormat()
            : base(EName.format)
        {
        }
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region Format
        public override bool P_Format_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.P_Name) + " " + this.P_Format;
            return s;
        }
        #endregion
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
    }
}