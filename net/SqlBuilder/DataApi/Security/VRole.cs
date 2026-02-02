using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;

namespace sql.builder.DataApi
{
    internal sealed class VRole : VSXElement, IVParent
    {
        internal VRole()
            : base(EName.role)
        {
        }
        private static string[] child_nodes = { TextConst.EName.UseObject, TextConst.EName.UseRole };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        private List<string> SelfPermissions(bool write)
        {
            IList<VSXElement> list = this.GetElementsP(EName.use_object);
            List<string> permissions = new List<string>(list.Count);
            for (int index = 0; index < list.Count; index++) {
                VUseObject uo = (VUseObject)list[index];
                if (!write || uo.P_WriteAccess == TextConst.AVBool.True) {
                    permissions.Add(uo.P_CalledObject);
                }
            }
            return permissions;
        }
        private HashSet<string> AllPermissions(bool write)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), write.ToString())) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), write.ToString()) as HashSet<string>);
            }
            List<string> list = this.SelfPermissions(write);
            IList<VSXElement> roles = this.GetElementsP(EName.use_role);
            for (int index = 0; index < roles.Count; index++) {
                VRole role = ((VUseRole)roles[index]).UsedRole();
                list.AddRange(role.AllPermissions(write));
            }
            var hs = new HashSet<string>(list.Distinct().ToList());
            AddCashValue(hs, MethodBase.GetCurrentMethod().ToString(), write.ToString());
            return hs;
        }
        internal bool HasReadPermission(string securityId)
        {
            return this.AllPermissions(false).Contains(securityId);
        }
        internal bool HasWritePermission(string securityId)
        {
            return this.AllPermissions(true).Contains(securityId);
        }
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.P_Name) + " " + Italic(this.P_SelfTitle);
            return s;
        }
        #endregion
    }
}