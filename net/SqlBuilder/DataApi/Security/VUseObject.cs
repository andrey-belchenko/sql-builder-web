using System.Collections.Generic;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;use-object object="asuse_ur_pir" editable="" /&gt;
    /// </summary>
    public sealed class VUseObject : VSXElement
    {
        public VUseObject()
            : base(EName.use_object)
        {
        }
        private VSXElement UsedObject()
        {
            return XmlReports.Environment.GetSecurityObject(this.P_CalledObject);
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            return this.UsedObject().AsList();
        }
        #region CalledObject
        public void P_CalledObject_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
        }
        public void P_CalledObject_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VSXElement> list = XmlReports.Environment.GetSecurityObjects();
            HashSet<string> names = new HashSet<string>();
            for (int index = 0; index < list.Count; index++)
            {
                VQueryCall el = list[index] as VQueryCall;
                if (el != null)
                {
                    string id = el.P_SecurityId;
                    if (!names.Contains(id))
                    {
                        table.AddRow(id);
                        names.Add(id);
                    }
                }
            }
        }
        public override bool P_CalledObject_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.P_CalledObject);
            if (this.P_WriteAccess == TextConst.AVBool.True)
            {
                s += " write";
            }
            VSXElement uo = this.UsedObject();
            if (uo != null)
            {
                s += " " + Italic(uo.P_Title);
            }
            return s;
        }
        #endregion
        #region WriteAccess
        public override string P_WriteAccess
        {
            get
            {
                return this.AttrOrEmpty(AName_.editable);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.editable, value);
            }
        }
        public override bool P_WriteAccess_Exists()
        {
            return true;
        }
        #endregion
    }
}