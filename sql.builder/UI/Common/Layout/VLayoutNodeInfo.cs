using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public class VLayoutNodeInfo
    {
       
        public int id = -1;
        public object GetId()
        {
            return id;
        }
       
        public virtual void InitControl()
        {

        }
        public object GetParentId()
        {
            if (GetParent() != null)
            {
                if (GetParent().GetParent() != null)
                {
                    return GetParent().GetId();
                }
            }
            return null;
        }
        public LinkedListNode<VLayoutNodeInfo> linkedListNode;


        private SortedList<string, object> props = new SortedList<string, object>();

        public void SetProperty(string name, object value)
        {
            props[name] = value;
        }
        public object GetProperty(string name)
        {
            if (props.ContainsKey(name))
            {
               return  props[name];
            }
            return null;
        }
        protected VLayoutContainerInfo parent;
        public VLayoutContainerInfo GetParent()
        {
            return parent;
        }
        public VLayoutGroupInfo GetParentGroup()
        {
            return parent as VLayoutGroupInfo;
        }
        public VLayoutSplitContainerInfo GetParentSplitContainer()
        {
            return parent as VLayoutSplitContainerInfo;
        }
        public VLayoutGroupInfo GetGroup()
        {
            if (this is VLayoutGroupInfo)
            {
                return this as VLayoutGroupInfo;
            }
            return GetParentGroup();
        }


        public virtual string GetText()
        {
            return null;
        }
        public virtual void SetText(string value)
        {
            
        }
		public virtual void SetHint(string value)
		{

		}

        public List<VLayoutGroupInfo> GetAllParentGroups()
        {
            var gr = GetParentGroup();
            var list = new List<VLayoutGroupInfo>();
            while (gr != null)
            {
                list.Add(gr);
                gr = gr.GetParentGroup();
            }
            return list;
        }
        public void SetParent(VLayoutContainerInfo value)
        {
             parent=value;
        }
        public int width = 0;
        public bool? isFirstInRow=null;
        public int left = 0;
        public int height = 0;
        public int top = 0;

       
        public int oldWidth = 0;
        public int oldLeft = 0;
        public int oldHeight = 0;
        public int oldTop = 0;

        //public int Height;
        //public int Width;
        //public int Left;
        //public int Top;
        public bool visible = true;
        public bool oldVisible = false;
        public bool visibleUndefined = false;
        public void SetVisibility(bool value)
        {
            if (value != visible)
            {
                visible = value;

                GetGroup().GetLayoutController().hasChanges = true;
            }

            //if (this is VLayoutControlContainerInfo)
            //{
            //    var cci = (this as VLayoutControlContainerInfo);

            //    if (cci.GetText() == "Тип абонента")
            //    {

            //    }
            //}
        }
        public virtual  bool IsSelfVisible()
        {
            return visible;
        }

        
        public virtual bool IsVisible()
        {
            var node = this;
            while (true)
            {
                if (node.CanBeVisible() && node.IsSelfVisible())
                {
                    node = node.GetParent();
                    if (node == null)
                    {
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
        }

        public bool isNewLine = false;
        protected IVLayoutNode control;


        public bool CanBeVisible()
        {
            if (GetParent() == null)
            {
                return true;
            }
            if (!GetParent().CanBeVisible())
            {
                return false;
            }
            else
            {
                if (GetParent() is VLayoutGroupInfo)// криво- переписать
                {
                    return (GetParent() as VLayoutGroupInfo).IsExpanded();
                }
                else
                {
                    return (this as VLayoutGroupInfo).IsExpanded();//вкладка
                }
            }
        }

        public void ResetControl()
        {
            control = null;
        }

        public VLayoutNodeInfo GetPrevious()
        {
            if (linkedListNode.Previous != null)
            {
                return linkedListNode.Previous.Value;
            }
            return null;
        }
        public VLayoutNodeInfo GetNext()
        {
            if (linkedListNode.Next != null)
            {
                return linkedListNode.Next.Value;
            }
            return null;
        }

        public VLayoutNodeInfo GetNextVisible()
        {
            var node = GetNext();
            while (node != null && !node.IsSelfVisible())
            {
                node = node.GetNext();
            }
            return node;
        }

        

        public VLayoutNodeInfo GetPreviousVisible()
        {
            var node = GetPrevious();
            while (node != null && !node.IsSelfVisible())
            {
                node = node.GetPrevious();
            }
            return node;
        }

        //public VLayoutNodeInfo GetNextGetNoFiller()
        //{
        //    var next = GetNext();

        //    while (next != null && next.IsFiller)
        //    {
        //        next.GetNext();
        //    }

        //    return next;
        //}

       

        public bool IsFiller = false;

        //public virtual List<VLayoutNodeInfo> GetSelfAndChilds()
        //{
        //    var list = new List<VLayoutNodeInfo>();
        //    list.Add(this);
        //    return list;
        //}

        //public virtual IVLayoutNode GetControl()
        //{
        //    return null;
        //}
        public virtual void AfterHide()
        {
            
        }
        public virtual void Show()
        {
           
        }

        
        //public virtual IVControlsFactory GetControlsFactory()
        //{
        //    return null;
        //}


        //public virtual int GetWidthNoExtra()
        //{
        //    return GetProportion(GetParent().GetClientWidth(), GetWidthPercent());
        //}

        
        //public virtual int GetWidthExtraObtained()
        //{
        //    var val = 0;
            
        //    val +=GetLeftExtraObtained();
        //    val-=GetWidthExtraGived();
        //    return val;
        //}

        //public virtual int GetLeftExtraObtained()
        //{
        //    var val = 0;
        //    if (GetPrevious() != null)
        //    {
        //        val += GetPrevious().GetWidthExtraGived();
        //    }
            
        //    return val;
        //}
        //// разница между шириной по проценту и максимальной шириной - то что нужно отдать другому элементу, пока отдается только с лева на право
        //public virtual int GetWidthExtraGived()
        //{
        //    var wne = GetWidthNoExtra();
        //    var wm = GetWidthFixed();
        //    if (wm == -1)
        //    {
        //        return 0;
        //    }

        //    if (wne <= wm)
        //    {
        //        return 0;
        //    }
        //    return wne - wm;
        //}

        //public virtual int GetWidth()
        //{
        //    var val = GetWidthFixed();
        //    if (val == -1)
        //    {
        //        val=GetProportion(GetRowFreeWidth() , GetWidthPercent());// GetWidthExtraObtained();
        //    }
        //    return val;
        //}
        public virtual int GetMarginLeft()
        {
            return 0;
        }
        public virtual int GetMarginRight()
        {
            return 0;
        }

        public virtual int GetMarginTop()
        {
            return 0;
        }
        public virtual int GetMarginBottom()
        {
            return 0;
        }
        
        
        public virtual decimal GetWidthAndMarginForClient(decimal clientFreeWidth, bool isSingleInRow)
        {
            decimal val = GetWidthFixed();
            if (val == -1)
            {
                val = GetProportion(clientFreeWidth, GetWidthPercent());// GetWidthExtraObtained();
                val-= (GetMarginLeft()+GetMarginRight());
                if (!isSingleInRow)
                {
                    val = GetWidthMinOrVal(val);
                }
              
            }
            val += (GetMarginLeft() + GetMarginRight());
            return val;
        }


        public virtual decimal GetWidthForClient(decimal clientFreeWidth, bool isSingleInRow)
        {
            var val = GetWidthFixed();
            if (val == -1)
            {
                val = GetProportion(clientFreeWidth, GetWidthPercent());// GetWidthExtraObtained();
                val -= (GetMarginLeft() + GetMarginRight());
                if (!isSingleInRow)
                {
                    val = GetWidthMinOrVal(val);
                }
            }
            return val;
        }


        

        public decimal sizePercent = 100;
        public decimal sizeTemp = 100;
        public decimal sizeFixed = -1;
        public void SetWidthPercent(int value)
        {
            sizePercent = value;
        }
        public void SetWidthFixed(int value)
        {
            sizeFixed = value;
        }

        public virtual int GetWidthPercentWithParents()
        {
            var percent = 100m;
            var node=this;
            while (node != null)
            {
                var perc = (decimal)node.GetWidthPercent();
                percent = percent * perc / 100m;
                node = node.GetParent();
            }

            return Convert.ToInt32( Math.Round(percent, 0));

        }
        public virtual decimal GetWidthPercent()
        {
            var fix = GetWidthFixed();
            decimal val = 0;
            if (fix == -1)
            {
                val = sizePercent;
                
            }
            else
            {
                val = 0;
            }
            return val;
        }


        private decimal widthMin = 0;

        public void SetWidthMin(int value)
        {
            widthMin = value;
        }

        public virtual decimal GetWidthMin()
        {
            if (GetWidthFixed() != -1)
            {
                return GetWidthFixed();
            }
            if (this is VLayoutControlContainerInfo)
            {
                return widthMin;
            }
            else
            {
                return 0;
            }
           
        }

        public virtual decimal GetWidthMinOrVal(decimal value)
        {
            if (value > GetWidthMin())
            {
                return value;
            }
            else
            {
                return GetWidthMin();
            }
            
           
        }


        public virtual decimal GetWidthFixedAddition() // для того чтобы наличие  кнопок не увеличивало размер соседних полей (кнопке добавляется размер ярлыка)
        {
            return 0;
        }
        
        public virtual decimal GetWidthFixed()
        {
            return sizeFixed;
        }

        //public virtual bool IsBreak()
        //{
        //    return false;
        //}


        public virtual bool NoBreakNext()
        {
            return false;
        }


        public static decimal GetProportion(decimal value , decimal percent)
        {
            var prop = ((decimal)value * ((decimal)percent / 100));
           return prop;
        }
        public static decimal GetPercent(int value, int fullValue)
        {
            var val = ((decimal)value / (decimal)fullValue) * 100;
            return val;
        }

        public virtual int GetWidth()
        {
            return width;
        }
        public virtual int GetHeight()
        {
            return height;
        }
        public virtual int GetLeft()
        {
            return left;
        }
        public virtual int GetTop()
        {
            return top;
          
        }

        public virtual decimal GetWidthFixedOrZero()
        {
            var val = GetWidthFixed();
            if (val == -1)
            {
                val = 0;
            }
            else
            {
                val += GetMarginRight() + GetMarginLeft();
            }
            return val;
        }

        //public virtual int GetWidthFixedOrZero()
        //{
        //    var val = GetWidthFixed();
        //    if (val == -1)
        //    {
        //        val = 0;
        //    }
        //    return val;
        //}
   
        //public virtual int GetRowFixedWidthLeftPre()
        //{
        //    var val = 0;
        //    if (!IsNewLinePre())
        //    {
        //        var node = this.GetPrevious();
        //        var br = false;
        //        while (!br && node != null)
        //        {
        //            val += node.GetWidthFixedOrZero();
        //            br = node.IsNewLine();
        //            node = node.GetPrevious();
        //        }
        //    }
            
        //    return val;
        //}
        //public virtual int GetRowFixedWidthLeft()
        //{
        //    var val = 0;
        //    var node = this;
        //    var br = false;
        //    while (!br && node != null)
        //    {
        //        val += node.GetWidthFixedOrZero();
        //        br = node.IsNewLine();
        //        node = node.GetPrevious();
        //    }

        //    return val;
        //}
        //public virtual int GetRowFixedWidthRight()
        //{
        //    var val = 0;
        //    var node = this;
           

        //    while (node != null && !node.IsNewLine())
        //    {
        //        val += node.GetWidthFixedOrZero();
        //        node = node.GetNext();
        //    }
        //    return val;
        //}
        //public virtual int GetRowFixedWidth()
        //{

        //    var val = GetRowFixedWidthLeft()+GetRowFixedWidthRight();
        //    return val;
        //}

        //public int GetRowFreeWidth()
        //{
        //    return GetParent().GetClientWidth() -GetRowFixedWidth();
        //}
        //public int GetRowFreeWidthLeft()
        //{
        //    return GetParent().GetClientWidth() - GetRowFixedWidthLeftPre();
        //}

       
        //public virtual bool IsNewLine()
        //{
        //    if (IsNewLinePre())
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (GetWidthMin() > GetRowFreeWidthLeft())
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
        //public virtual bool IsNewLinePre()
        //{
        //    if (GetPrevious() == null)
        //    {
        //        return true;
        //    }
        //    else if (IsBreak())
        //    {
        //        return true;
        //    }

        //    else
        //    {
        //        var prevEnd = GetPrevious().GetLeftPercent() + GetPrevious().GetWidthPercent();
        //        var thisEnd = prevEnd + GetWidthPercent();
        //        if (thisEnd > 100)
        //        {
        //            return true;
        //        }
        //        else
        //        {
                    
        //            return false;
        //        }
        //    }
        //}

        //public virtual int GetLeftPercent()
        //{
        //    if (GetPrevious() == null)
        //    {
        //        return 0;
        //    }
        //    else if (IsNewLine())
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        var prevEnd = GetPrevious().GetLeftPercent() + GetPrevious().GetWidthPercent();
        //        var thisEnd = prevEnd + GetWidthPercent();
        //        if (thisEnd > 100)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return prevEnd;
        //        }
        //    }
        //}

    }
    
}
