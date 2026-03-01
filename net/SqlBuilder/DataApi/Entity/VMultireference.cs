namespace sql.builder.DataApi
{
    /*public class VMultireference:VColumn
    {
        public VMultireference(XElement element)
            : base(element)
        {
          
        }

        public override  List<VQuery> SourceQuery()
        {


            var rel = GetRealation();
            var list = new List<VQuery>();
            if (rel != null)
            {

                list.Add(rel.ChildQuery());
            }
            return list;
        }
        public VRelation GetRealation()
        {
            var src = Source();
            VRelation rel = null;
            if (src != null)
            {
                rel = src.Query().EntityType.ChildLink(P_CalledQuery);
            }
            return rel;
        }

        #region CalledQuery
   

        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();

            var src = Source();

            if (src != null)
            {

  
                foreach (var r in src.Query().EntityType.ChildLinks())
                {
                    var s1 = "";
                    s1 += r.P_DXName;
                    var qry2 = r.ChildQuery();
                 
                    table.Rows.Add(s1, s1, qry2.Title());
                }
            }
         

        }

        
        #endregion



        #region Column
        public override void P_Column_ListRefresh(VDataTable table)
        {
            P_Column_List(table);


            List<string> names = new List<string>();
            foreach (VSXElement el in GetRealation().ChildQuery().Columns())
            {
                string name = el.XName;
                if (!names.Contains(name))
                {
                    AddColumnInfoToList(table, name, el);
                    names.Add(name);
                }

            }
        }
       
        #endregion



        #region NodeText

        public override string GetNodeInfo()
        {
            return GetNodeOtherInfo();
        }

        public override string GetNodeOtherInfo()
        {
            string s = "";
            
            s += P_Table + ".";

            s += P_CalledQuery + ".";

            string sAs = P_Alias;
            if (sAs == "")
            {
                s += ColorBrown(Bold(P_Column));
            }
            else
            {
                s += Bold(P_Column);
            }


            s += Bold("[]");
            if (sAs != "")
            {
                s += " as " + ColorBrown(Bold(sAs));
            }
            s += " " + Italic(P_Title);
            return s;
        }

        #endregion
  

       
    }*/
}
