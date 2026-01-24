using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VDLink : VELink
    {
        internal VDLink()
            : base(EName.dlink)
        {
        }
        //#region Prime
        //public override bool P_Prime_Exists()
        //{
        //    var qry = Query();
        //    if (qry != null)
        //    {
        //        return qry.IsQube();
        //    }
        //    return false;
        //}
        //#endregion
    }
}