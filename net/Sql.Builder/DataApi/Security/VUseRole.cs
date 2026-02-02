using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;use-role object="" /&gt;
    /// </summary>
    internal sealed class VUseRole : VSXElement
    {
        internal VUseRole()
            : base(EName.use_role)
        {
        }
        internal VRole UsedRole()
        {
            return XmlReports.Environment.GetRole(this.P_CalledObject);
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            return this.UsedRole().AsList();
        }
        #region CalledObject
        public void P_CalledObject_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public void P_CalledObject_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VRole> roles = XmlReports.Environment.GetRoles();
            for (int index = 0; index < roles.Count; index++) {
                VRole role = roles[index];
                table.AddRow(role.P_Name, role.P_Title);
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
            VRole role = this.UsedRole();
            if (role != null) {
                s += " " + Italic(role.P_SelfTitle);
            }
            return s;
        }
        #endregion
    }
}