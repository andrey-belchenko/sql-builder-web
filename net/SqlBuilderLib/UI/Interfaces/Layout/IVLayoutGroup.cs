using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVLayoutGroup : IVLayoutNode
    {
        VLayoutGroupInfo GetInfo(); // нужно избавиться от этого метода
        void ShowNode(VLayoutNodeInfo node);
        void HideNode(VLayoutNodeInfo node);
        void SetNodeTop(VLayoutNodeInfo node);
        void SetNodeHeight(VLayoutNodeInfo node);
        void SetNodeWidth(VLayoutNodeInfo node);
        void SetNodeLeft(VLayoutNodeInfo node);
        void SetExpanded(bool value);
        void SetScrollHeight(int value);
        void Init(VLayoutGroupInfo group);
        void SetText(string text);
        void SetBold(bool value);
        void SetVerticalScrollBarVisibility(bool value);
        int ClientWidth(VLayoutGroupInfo group);
        int ClientHeight(VLayoutGroupInfo group);
        void BeginLayoutChange();
        void EndLayoutChange();
        bool Expanded();
        int BorderHeight();
        int CollapsedHeight();
        IVBar GetBar();
        //{
        //    return 21;
        //}
    }
}
