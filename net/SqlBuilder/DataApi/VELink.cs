using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public class VELink : VQueryCall
    {
        protected VELink(XName name)
            : base(name)
        {
        }
        public VELink()
            : base(EName.elink)
        {
        }
        public override List<VSXElement> GetUsedElements()
        {
            VRelation rel = this.GetRelation();
            if (rel == null) {
                return new List<VSXElement>(0);
            } else {
                return new List<VSXElement>(1) { rel };
            }
        }
        //public override string P_Title
        //{
        //    get
        //    {
        //        return XTitle;
        //    }
        //}
        public override string XTitle {
            get {
                XAttribute attr = this.Attribute(AName_.title);
                if (attr != null) {
                    return attr.Value;
                } else {
                    VRelation rel = this.GetRelation();
                    if (rel != null) {
                        string s = rel.P_DXTitle;
                        if (!string.IsNullOrEmpty(s)) {
                            return s;
                        }
                    }
                    if (this.GetMainParent() != null) {
                        return this.GetMainParent().P_Title;
                    } else {
                        return null;
                    }
                }
            }
        }
        public override VQuery Query()
        {
            VQueryCall parent;
            if (this.GetParent() is VLinks) {
                VQuery qry = this.RootQuery() as VQuery;
                if (!qry.IsInherit() && qry.IsExtension()) {
                    parent = qry.MainSource();
                } else {
                    parent = qry;
                }
            } else {
                parent = (VQueryCall)this.GetParent();
            }
            VQuery q = (VQuery)parent.Query();
            if (q == null) {
                return null;
            }
            VQuery query = (VQuery)q.GetMainE();
            var rel = query.EntityType.ChildLinks().FirstOrDefault(l => l.P_DXName == this.SName());
            if (rel == null) {
                return null;
            }
            return rel.ChildQuery();
        }
        public override VRelation GetRelation()
        {
            VSXElement par = this.GetParent();
            VQueryCall parent;
            if (par is VLinks) {
                var qry = this.RootQuery() as VQuery;
                parent = qry.MainSource();
            } else {
                parent = par as VQueryCall;
            }
            VQuery query = parent.Query();
            if (query == null) {
                return null;
            }           
            var relation = query.EntityType.ChildLinks().FirstOrDefault(l => l.P_DXName == SName());
            return relation;
        }
        #region CalledQuery
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var ids = new List<string>();
           // foreach (VRelation rel in this.LinkParent().Query().EntityType.ChildLinks())
                foreach (VRelation rel in (this.LinkParentQuery().GetMainE() as VQuery).EntityType.ChildLinks()) {
                var s = rel.P_DXName;
                while (ids.Contains(s)) {
                    s += "[dub]";
                }
                table.Rows.Add(s, s, rel.P_DXTitle);
                ids.Add(s);
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeTypeInfo() + " " + this.GetNodeOtherInfo();
        }
        #endregion
    }
}