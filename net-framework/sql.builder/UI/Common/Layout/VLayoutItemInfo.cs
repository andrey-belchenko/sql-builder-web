using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public class VLayoutItemInfo : VLayoutNodeInfo
    {
       
       
       
       
        
        
        //public override IVControlsFactory GetControlsFactory()
        //{
        //    return GetParent().GetControlsFactory();
           
        //}




        public void ChangeParent(VLayoutGroupInfo newParent)
        {
            var oldParent = parent;
            oldParent.RemoveChild(this);
            parent = newParent;
            parent.AddChild(this);
            visibleUndefined = true;
         
          //  newParent.GetTypedControl().ShowNode(this);
        }

        public virtual int GetControlHeight()
        {
            return 0;
        }


        
        
        //public override int GetHeight()
        //{
        //    //int height = GetControlsFactory().RowHeight();
        //    return height;
        //}

    }
    
    
}
