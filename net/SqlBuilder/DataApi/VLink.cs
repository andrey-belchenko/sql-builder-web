using System.Collections.Generic;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VLink : VQueryCall
    {
        public VLink()
            : base(EName.link)
        {
        }
        public override List<VSXElement> GetUsedElements()
        {
            if (!IsDimenson())
            {
                var rel = GetRelation();
                if (rel == null) return new List<VSXElement>();

                return rel.AsList();
            }
            else
            {
                var rel = LinkedDimension();
                if (rel == null) return new List<VSXElement>();

                return rel.AsList();
            }
        }

        public override VRelation GetRelation()
        {
            //var parent = (VQueryCall)this.LinkParent();



            //var query = parent.Query();
            var query = LinkParentQuery();
            if (query != null)
            {
                var relation = query.EntityType.ParentLinks().FirstOrDefault(l => l.PName() == this.SName());
                return relation;
            }
            else
            {
                return null;
            }


        }



        public override VQuery Query()
        {

            if (IsFactLink())
            {
                var fact = XmlReports.Environment.GetFactSource(P_CalledQuery);
                if (fact == null)
                {
                    return null;
                }
                var dim = XmlReports.Environment.GetDimension(fact.P_FactDimension);
                if (dim != null)
                {
                    return dim.Query();
                }
                else
                {
                    return null;
                }
            }

            else if (GetParent() is VQube || GetParent() is VDimSet)
            {
                var dim = XmlReports.Environment.GetDimension(P_CalledQuery);

                if (dim != null)
                {
                    return dim.Query();
                }
                else
                {
                    return null;
                }
            }



            else
            {
                var relation = GetRelation();
                if (relation != null)
                {
                    return relation.ParentQuery();
                }
                else
                {
                    return null;
                }
            }
        }


        public VDimension LinkedDimension()
        {

            return XmlReports.Environment.GetDimension(P_CalledQuery);

        }

        public bool IsDimenson()
        {
            return (GetParent() is VQube || GetParent() is VDimSet);
        }

        public bool IsFactLink()
        {
            return (GetParent() is VFactLinks);
        }
        #region CalledQuery
        public override string P_CalledQuery
        {
            get
            {
                return this.AttrOrEmpty(AName_.name);
            }
            set
            {
                this.SetAttributeValue(AName_.name, value);
            }
        }
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();

            if (IsDimenson())
            {


                foreach (VDimension dim in XmlReports.Environment.GetDimensions())
                {
                    table.Rows.Add(dim.XName, dim.XName, dim.P_Title, dim.P_CalledQuery);
                }
            }
            else if (IsFactLink())
            {




                if (RootQuery() != null)
                {
                    foreach (VExpression exp in RootQuery().Expressions())
                    {
                        if (exp.P_FactDimension != "")
                        {
                            table.Rows.Add(exp.XName, exp.XName, exp.P_Title, exp.P_FactDimension);
                        }
                    }
                }
                foreach (VExpression exp in XmlReports.Environment.GetExpressions())
                {
                    // list.Add(exp.XName, null);
                    if (exp.P_FactDimension != "")
                    {
                        table.Rows.Add(exp.XName, exp.XName, exp.P_Title, exp.P_FactDimension);
                    }

                }

                foreach (VSXElement exp in XmlReports.Environment.GetFactColumns())
                {
                    // list.Add(exp.P_Fact, null);
                    if (exp.P_FactDimension != "")
                    {
                        table.Rows.Add(exp.P_Fact, exp.P_Fact, exp.P_Title, exp.P_FactDimension);
                    }
                }


            }

            else
            {
                foreach (VRelation rel in LinkParentQuery().EntityType.ParentLinks())
                {
                    var s = rel.PName();
                    if (rel.ParentQuery() != null)
                    {
                        table.Rows.Add(rel.PName(), rel.PName(), rel.XTitle, rel.P_CalledQuery);
                    }

                }

            }





        }


        #endregion

        #region NodeText


        public override string GetNodeInfo()
        {
            return GetNodeTypeInfo() + " " + GetNodeOtherInfo();
        }



        #endregion


        #region AllRows



        public override bool P_AllRows_Exists()
        {

            return GetParent() is VQube || GetParent() is VDimSet;

        }

        #endregion


        #region OnlyForCond



        public override bool P_OnlyForCond_Exists()
        {

            return GetParent() is VQube || GetParent() is VDimSet;

        }

        #endregion


        #region IsTree


        public override bool P_IsTree_Exists()
        {

            return IsDimenson();

        }
        #endregion



        #region IsTreeSplitCols
        public override bool P_IsTreeSplitCols_Exists()
        {

            return IsDimenson();

        }
        #endregion


        #region AutoFilter
        public override bool P_AutoFilter_Exists()
        {

            return true;

        }
        #endregion

    }
}
