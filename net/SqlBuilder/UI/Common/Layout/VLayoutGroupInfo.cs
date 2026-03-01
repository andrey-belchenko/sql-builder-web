using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows.Forms;

namespace sql.builder.UI
{
    public class VLayoutGroupInfo : VLayoutContainerInfo
    {
        public VLayoutGroupInfo(VLayout controller, VLayoutContainerInfo parentGroup)
            : base(controller, parentGroup)
        {

        }


        public override bool IsSelfVisible()
        {
            if (!visible) return false;
            if (Nodes.Any())
            {
                return GetFirstVisible() != null;
            }
            return true;
        }
        public override void Show()
        {
        }
        public void Hide()
        {
            //foreach (var node in Nodes)
            //{
            //    if (node.IsSelfVisible())
            //    {

            //        node.oldVisible = false;
            //        GetTypedControl().HideNode(node);
            //        node.AfterHide();
            //    }
            //}
        }

        //public IVLayoutGroup GetTypedControl()
        //{
        //    return GetControl() as IVLayoutGroup;
        //}
        private decimal minWidth = -1;
        public override decimal GetWidthMin()
        {

            if (minWidth == -1)
            {
                calculateMinWidth();
            }
            return minWidth;

        }
        private void calculateMinWidth()
        {
            minWidth = 0;
            foreach (var node in GetVisibleNodes())
            {

                var val = node.GetWidthMin();
                val += node.GetMarginLeft();
                val += node.GetMarginRight();
                val += GetPaddingRight();
                val += GetPaddingLeft();
                if (val > minWidth)
                {
                    minWidth = val;
                }
            }
        }
        private int maxLabelTextWidth;
        public void ResetChildValuesInfo()
        {
            //foreach (var node in Nodes)
            //{
            //    node.left = 0;
            //    node.width = 0;
            //    node.top = 0;
            //    node.left = 0;
            //}
            maxLabelTextWidth = -1;
            minWidth = -1;
        }
        public int GetMaxLabelTextWidth()
        {
            var prt = ParentOrSelfLayoutBlock();
            if (prt.maxLabelTextWidth == -1)
            {
                prt.calculateMaxLabelTextWidth();
            }
            return prt.maxLabelTextWidth;
        }




        private string text;

        public override void SetText(string value)
        {
            text = value;
            // GetTypedControl().SetText(GetText());
        }
        public override string GetText()
        {
            return text;
        }

        public List<VLayoutGroupInfo> AllChildsWithNoBorderAndSelf()
        {
            var list1 = GetVisibleNodes().Where(n => n is VLayoutGroupInfo).Select(g => (g as VLayoutGroupInfo)).Where(g => !(g.IsLayoutBlock)).ToList();
            var list = new List<VLayoutGroupInfo>();
            foreach (VLayoutGroupInfo gr1 in list1.ToList())
            {
                list.AddRange(gr1.AllChildsWithNoBorderAndSelf());
            }
            list.Add(this);
            return list;
        }


        public List<VLayoutGroupInfo> AllChildVisibleGroups()
        {
            var list1 = GetVisibleNodes().Where(n => n is VLayoutGroupInfo).Select(g => (g as VLayoutGroupInfo)).ToList();
            var list = new List<VLayoutGroupInfo>();
            foreach (VLayoutGroupInfo gr1 in list1.ToList())
            {
                list.AddRange(gr1.AllChildVisibleGroups());
            }
            list.Add(this);
            return list;
        }

        public List<VLayoutControlContainerInfo> AllChildControlContainers()
        {
            var list1 = AllChildVisibleGroups().SelectMany(e => GetVisibleNodes()).Where(n => n is VLayoutControlContainerInfo).Select(g => (g as VLayoutControlContainerInfo)).ToList();


            return list1;
        }

        public VLayoutGroupInfo ParentOrSelfLayoutBlock()
        {
            var gr = this;
            while (gr.GetParentGroup() != null && !(gr.IsLayoutBlock))
            {

                gr = gr.GetParentGroup();
            }
            if (gr.IsLayoutBlock)
            {
            }
            return gr;
        }

        private void calculateMaxLabelTextWidth()
        {

            maxLabelTextWidth = 0;
            foreach (VLayoutLabelInfo label in AllChildsWithNoBorderAndSelf().SelectMany(gr => gr.GetVisibleNodes()).Where(n => n is VLayoutLabelInfo))
            {

                var val = label.GetTextWidth();
                if (val > maxLabelTextWidth)
                {
                    maxLabelTextWidth = val;
                }
            }

        }

        public bool HasBorder = true;
        public bool IsLayoutBlock = false;
        public void SetBorderVisibility(bool value)
        {
            if (!(GetParent() is VLayoutTabsInfo))
            {
                HasBorder = value;
            }
        }

        public void SetIsLayoutBlock(bool value)
        {
            if (!(GetParent() is VLayoutTabsInfo))
            {
                IsLayoutBlock = value;
            }
        }

        public bool Uncollapsible = false;
        public void SetUncollapsible(bool value)
        {
            Uncollapsible = value;
        }
        private bool expanded = true;
        private bool oldExpanded = true;
        public void SetExpanded(bool value)
        {
            expanded = value;

            if (!expanded)
            {
                foreach (var node in AllChildControlContainers())
                {
                    node.AfterHide();
                }
            }
            //  layoutController.Show();//это тест нужно по другому
        }
        private bool isBold = false;
        private bool oldIsBold = false;
        public void SetBold(bool value)
        {
            isBold = value;
            if (isBold != oldIsBold)
            {
                if (IsVisible())
                {
                    oldIsBold = isBold;
                    //GetTypedControl().SetBold(isBold);
                }
            }
        }
        public void ExpandChanged()
        {
            //oldExpanded = (GetControl() as IVLayoutGroup).Expanded();
            //if (oldExpanded != expanded)
            //{
            //    SetExpanded(oldExpanded);
            //    GetLayoutController().RefreshLayout();

            //}
        }

        public void SizeChanged()
        {
            //   layoutController.Show();// это тест нужно по другому
            if (GetParent() == null)
            {
                GetLayoutController().hasChanges = true;
                GetLayoutController().RefreshLayoutIfNeed();
                //GetLayoutController().RefreshLayoutWithDelay();
            }
        }




        //public override IVControlsFactory GetControlsFactory()
        //{
        //    return layoutController.GetControlsFactory();
        //}
        public override int GetHeight()
        {
            return 0;
        }

        private bool isHeightOverflow = false;

        public override int GetControlBorderHeight()
        {
            return 0;
            //return GetTypedControl().BorderHeight();
            //if (this.HasBorder)
            //{
            //    return GetTypedControl().BorderHeight();
            //}
            //else
            //{
            //    return 0;
            //}
        }

        public override int GetControlBorderWidth()
        {
            return 0;
            //if (this.HasBorder)
            //{
            //    return GetControlsFactory().GroupBorderWidth();
            //}
            //else
            //{
            //    return 0;
            //}
        }
        public override int GetClientWidth()
        {
            return 0;
        }


        public override int GetClientHeight()
        {
            return 0;
        }

        public override int GetWidth()
        {
            return 0;
            //if (GetParent() != null)
            //{
            //    return base.GetWidth();
            //}
            //else
            //{
            //   return (GetControl() as IVLayoutGroup).ClientWidth(this);
            //}
        }


        public VLayoutNodeInfo GetFiller()
        {

            return GetVisibleNodes().Where(n => n.IsFiller).FirstOrDefault();
        }

        public VLayoutContainerInfo FindResizeble()
        {
            var gr = (VLayoutContainerInfo)this;
            while (!gr.IsFiller)
            {
                gr = gr.GetParent();


            }
            return gr;
        }



        public override void ResetAndCalculateChilds()
        {
            isHeightOverflow = false;
            CalculateChilds();
        }

        public bool IsExpanded()
        {
            if (GetParent() is VLayoutTabsInfo)
            {
                return (GetParent() as VLayoutTabsInfo).IsTabSelected(this);
            }
            else
            {
                return expanded;
            }
        }


        public void CalculateChilds()
        {
            if (!Nodes.Any()) return;

            ResetChildValuesInfo();
            var cursor = GetFirstVisible();
            if (cursor == null) return;
            int top1 = 0;
            int grHeight = GetPaddingTop();
            var maxHeightMarginBottom = 0;
            while (cursor != null)
            {

                decimal filled = 0;
                var list1 = new List<VLayoutNodeInfo>();
                var overload = false;

                while (!overload && cursor != null)
                {

                    filled += cursor.GetWidthPercent();
                    if (filled > 100)
                    {
                        overload = true;


                    }
                    else
                    {
                        list1.Add(cursor);
                    }
                    cursor = cursor.GetNextVisible();
                }
                if (list1.Count == 0)
                {
                    list1.Add(cursor);
                }
                decimal fixedWidth = 0;
                cursor = list1[list1.Count - 1];
                while (cursor.NoBreakNext())
                {
                    if (list1.Count == 1)
                    {
                        break;
                    }
                    list1.Remove(cursor);
                    cursor = cursor.GetPreviousVisible();
                }

                foreach (var cursor1 in list1)
                {
                    fixedWidth += cursor1.GetWidthFixedOrZero() + cursor1.GetWidthFixedAddition();
                }


                overload = false;
                filled = 0;
                var list2 = new List<VLayoutNodeInfo>();
                var list3 = new List<VLayoutNodeInfo>();
                var clientWidth = this.GetClientWidth();
                var freeWidth = clientWidth - fixedWidth;

                foreach (var cursor1 in list1)
                {
                    if (!overload)
                    {
                        filled += cursor1.GetWidthAndMarginForClient(freeWidth, list1.Count == 1) + cursor1.GetWidthFixedAddition();
                        if (filled > clientWidth)
                        {
                            overload = true;
                            if (cursor.GetText() == "Телефон")
                            {
                            }
                        }
                        else
                        {
                            list2.Add(cursor1);
                        }


                    }
                    if (overload)
                    {
                        list3.Add(cursor1);
                    }
                }
                if (list2.Count == 0)
                {
                    list2.Add(list1.First());
                }
                cursor = list2[list2.Count - 1];
                while (cursor.NoBreakNext())
                {
                    if (list2.Count == 1)
                    {
                        break;
                    }
                    list2.Remove(cursor);
                    list3.Insert(0, cursor);
                    cursor = cursor.GetPreviousVisible();
                }


                decimal additionalPercent = 0;
                decimal additionalWidth = 0;
                foreach (var cursor3 in list3)
                {
                    additionalPercent += cursor3.GetWidthPercent();
                    additionalWidth += cursor3.GetWidthFixedOrZero() + cursor3.GetWidthFixedAddition();
                }
                var fullFreeWidth = (decimal)freeWidth + (decimal)additionalWidth;

                var list4 = list2.Where(e => e.GetWidthPercent() > 0).ToList();

                // манипуляции (не очень удачные) для оптимизации интекфейса при нехватке места
                if (list4.Count == 1)
                {
                    additionalPercent = 100 - list4[0].GetWidthPercent();// 100 % если одно поле на строке
                }
                decimal rowPercent = 0;
                foreach (var node in list4)
                {
                    rowPercent += node.GetWidthPercent();
                }

                additionalPercent += 100m - (rowPercent + additionalPercent);

                if (additionalPercent < 100 && additionalPercent > 0) // увеличивается % оставшихся полей пропорционально % убранных полей
                {
                    var percentMultiplicer = (decimal)additionalPercent / (100m - additionalPercent) + 1m;
                    // var additionalWidthPr = VLayoutNodeInfo.GetProportion(fullFreeWidth, additionalPercent);
                    fullFreeWidth = (decimal)fullFreeWidth * percentMultiplicer;
                }


                var left1 = 0;
                var maxHeightWithMargin = 0;
                var maxHeightMarginTop = 0;

                maxHeightMarginBottom = 0;
                var padding = GetPaddingLeft();
                bool first = true;


                int i = 0;
                int cnt = list2.Count;
                foreach (var cursor2 in list2)
                {
                    i++;


                    cursor2.isFirstInRow = null;
                    if (first)
                    {
                        if (cursor2.GetParent().isFirstInRow != null)
                        {
                            if ((cursor2.GetParent() as VLayoutGroupInfo).HasBorder ||
                                cursor2.GetParent().isFirstInRow != false) // запутался  костыль
                            {
                                //if (cursor2 is VLayoutLabelInfo)
                                //{
                                //   var t= (cursor2 as VLayoutLabelInfo).GetText();
                                //    if (t == "Штраф")
                                //    {

                                //    }
                                //}
                                cursor2.isFirstInRow = true;


                            }
                            else
                            {
                                cursor2.isFirstInRow = false;
                            }
                        }
                        else
                        {
                            cursor2.isFirstInRow = true;
                        }
                    }
                    else
                    {
                        cursor2.isFirstInRow = false;
                    }


                    first = false;
                    int width =
                        Convert.ToInt32(
                        Math.Round(
                        cursor2.GetWidthForClient(fullFreeWidth, list2.Count == 1) + cursor2.GetWidthFixedAddition()
                        , 0)
                          );
                    cursor2.width = width;

                    int dleft = padding + cursor2.GetMarginLeft();
                    left1 += dleft;

                    cursor2.left = left1;

                    cursor2.top = grHeight + cursor2.GetMarginTop();
                    left1 += cursor2.width + cursor2.GetMarginRight();


                    if (i == cnt)
                    {
                        var appliedWidth = left1 - GetPaddingRight();
                        if (cursor2.GetWidthFixed() == -1)
                        {
                            var rem = (clientWidth - appliedWidth);
                            if (rem != 0)
                            {
                                cursor2.width += rem;
                            }
                        }
                    }

                    var grp = cursor2 as VLayoutGroupInfo;




                    if (grp != null)
                    {
                        if (grp.IsExpanded())
                        {

                            if (!grp.IsFiller)
                            {
                                grp.ResetAndCalculateChilds();
                            }
                            else
                            {
                                cursor2.height = 0;
                            }
                        }
                    }
                    else
                    {
                        if (cursor2.IsFiller)
                        {
                            cursor2.height = 0;
                        }
                        else
                        {
                            if ((cursor2 is VLayoutItemInfo))
                            {
                                cursor2.height = (cursor2 as VLayoutItemInfo).GetControlHeight();
                            }
                        }
                    }


                    var fullHeight = cursor2.GetHeight() + cursor2.GetMarginTop() + cursor2.GetMarginBottom();
                    if (maxHeightWithMargin < fullHeight)
                    {
                        maxHeightWithMargin = fullHeight;
                        maxHeightMarginTop = cursor2.GetMarginTop();
                        maxHeightMarginBottom = cursor2.GetMarginBottom();
                    }
                    padding = 0;

                }


                foreach (var cursor2 in list2)
                {
                    if (cursor2 is VLayoutLabelInfo)
                    {
                        cursor2.top -= cursor2.GetMarginTop();
                        cursor2.top += maxHeightMarginTop;
                        var addHeight = (maxHeightWithMargin - maxHeightMarginTop - maxHeightMarginBottom - cursor2.GetHeight()) / 2;
                        cursor2.top += addHeight;
                    }

                }
                //if (GetParent() == null)
                //{
                grHeight += maxHeightWithMargin;
                // }
                cursor = list2[list2.Count - 1].GetNextVisible();
            }
            //if (GetParent() == null)
            //{
            grHeight += GetPaddingBottom();
            //}
            if (!this.IsFiller)
            {
                this.height = grHeight + GetControlBorderHeight();
            }
            else
            {
                var filler = GetFiller();
                var freeHeight = GetClientHeight() - grHeight;
                if (filler != null)
                {
                    var freeHeight1 = freeHeight + GetPaddingTop() + GetPaddingBottom();// запутался где нужно отнимать padding а где нет, так вроде ок
                    filler.height = freeHeight1;
                    grHeight += freeHeight1;
                    var node = filler.GetNextVisible();

                    while (node != null)
                    {
                        node.top += freeHeight1;
                        node = node.GetNextVisible();
                    }

                    var cntr = filler as VLayoutContainerInfo;
                    if (cntr != null)
                    {
                        cntr.ResetAndCalculateChilds();
                    }

                }

                if (!isHeightOverflow)
                {
                    isHeightOverflow = freeHeight < 0;
                    if (isHeightOverflow)
                    {

                        CalculateChilds();
                        //GetTypedControl().SetVerticalScrollBarVisibility(true);

                    }
                    else
                    {

                        //GetTypedControl().SetVerticalScrollBarVisibility(false);
                    }
                }




            }
            //if (GetParent() == null)
            //{
            //GetTypedControl().SetScrollHeight(grHeight);
            //}
        }



        public override int GetMarginLeft()
        {
            if (!HasBorder)
            {
                return 0;
            }
            if (GetParentSplitContainer() != null && GetParentSplitContainer().IsVertical && GetPrevious() != null)
            {
                return 0;
            }
            return 3;
        }
        public override int GetMarginRight()
        {
            if (!HasBorder)
            {
                return 0;
            }
            if (GetParentSplitContainer() != null && GetParentSplitContainer().IsVertical && GetNext() != null)
            {
                return 0;
            }
            return 3;
        }

        public override int GetMarginTop()
        {
            if (!HasBorder)
            {
                return 0;
            }
            if (GetParentSplitContainer() != null && !GetParentSplitContainer().IsVertical && GetPrevious() != null)
            {
                return 0;
            }

            return 3;
        }

        public override int GetMarginBottom()
        {
            if (!HasBorder)
            {
                return 0;
            }
            if (GetParentSplitContainer() != null && !GetParentSplitContainer().IsVertical && GetNext() != null)
            {
                return 0;
            }
            return 3;
        }
        public bool NoSpaces = false;
        public int GetPaddingLeft()
        {
            if (NoSpaces)
            {
                return 0;
            }
            if (IsParentLikeGroup() && !HasBorder)
            {
                return 0;
            }
            return 3;
        }
        public int GetPaddingRight()
        {
            if (NoSpaces)
            {
                return 0;
            }
            if (IsParentLikeGroup() && !HasBorder)
            {
                return 0;
            }
            return 3;
        }
        private bool IsParentLikeGroup()
        {
            return ((GetParent() is VLayoutGroupInfo) || (GetParent() is VLayoutSplitContainerInfo));
        }
        public int GetPaddingBottom()
        {
            if (NoSpaces)
            {
                return 0;
            }
            if (GetParent() == null)
            {
                return 3;
            }
            if (IsParentLikeGroup() && !HasBorder)
            {
                return 0;
            }
            return 3;
        }
        // 2 4 выглядит более одинаково чем 3 3
        public int GetPaddingTop()
        {
            if (NoSpaces)
            {
                return 0;
            }
            if (GetParent() == null)
            {
                return 3;
            }
            if (IsParentLikeGroup() && !HasBorder)
            {
                return 0;
            }
            return 3;
        }

        public EventHandler TabSelected = null;

        public void RaiseTabSelected()
        {
            if (TabSelected != null)
            {
                TabSelected(this, null);
            }
        }


        public EventHandler Showed = null;

        private void RaiseShowed()
        {
            if (Showed != null)
            {
                Showed(this, null);
            }
        }

    }

}
