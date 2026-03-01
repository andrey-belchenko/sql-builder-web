using System.Linq;
namespace sql.builder.UI
{
    public class VLayoutControlContainerInfo : VLayoutItemInfo
    {
        public VLayoutControlContainerInfo(VLayoutGroupInfo parentGroup, object control)
        {

            parent = parentGroup;
            containedControl = control;
            if (parent != null)
            {
                parent.AddChild(this);
            }
            if (control != null)
            {
                if (!parentGroup.GetLayoutController().containedControls.ContainsKey(control.GetHashCode()))
                {
                    parentGroup.GetLayoutController().containedControls.Add(control.GetHashCode(), this);
                }
            }
        }





        public VLayout.Dock ControlDock = VLayout.Dock.Fill;

        // емцов поменял на public, не понял как до него добраться
        public VLayoutLabelInfo label = null;

        public override decimal GetWidthFixedAddition()
        {
            var wf = GetWidthFixed();
            if (wf != -1)
            {
                return 0;
            }
            if (label == null)
            {
                var lbl = GetParentGroup().Nodes.FirstOrDefault(n => n is VLayoutLabelInfo) as VLayoutLabelInfo;
                if (lbl != null)
                {
                    return lbl.GetWidthAndMarginForClient(0, false);
                }

            }
            return 0;
        }
        public override string GetText()
        {
            if (label != null)
            {
                return label.GetText();
            }
            return null;
        }

        public override void SetText(string text)
        {
            if (label == null)
            {
                label = new VLayoutLabelInfo();
                GetParent().AddBefore(label, this);

                label.Item = this;


            }

            label.SetText(text);
        }

        public override void SetHint(string value)
        {
            //if (label != null)
            //{
            //	label.SetHint(value);
            //}
            //GetTypedControl().SetHint(value);
        }

        private object containedControl;

        public object GetContainedControl()
        {
            return containedControl;
        }
        private void SendVisiblityToSource(bool value)
        {
            var contCtrl = GetContainedControl();
            //if (contCtrl is IucTableViewerContainer)
            //{
            //    (contCtrl as IucTableViewerContainer).SetVisibleInLayout(value);
            //}



        }
        public override void AfterHide()
        {
            SendVisiblityToSource(false);
        }
        public override void Show()
        {
            //var ctrl = (GetControl() as IVLayoutControlContainer);
            //if (label != null)
            //{

            //    //ctrl.SetLabel(label.GetTypedControl());
            //    label.ShowLabel();
            //}
            //ctrl.Show(this);
            //SendVisiblityToSource(true);

        }
        public override int GetControlHeight()
        {
            return 0;
            //return GetTypedControl().GetHeight(this);
        }

        public override int GetMarginLeft()
        {
            if (GetParentGroup().NoSpaces)
            {
                return 0;
            }
            return 1;

        }
        public override int GetMarginRight()
        {
            if (GetParentGroup().NoSpaces)
            {
                return 0;
            }

            return 1;
        }
        public override int GetMarginTop()
        {
            if (GetParentGroup().NoSpaces)
            {
                return 0;
            }
            return 1;
        }
        public override int GetMarginBottom()
        {
            if (GetParentGroup().NoSpaces)
            {
                return 0;
            }
            return 1;// д быть 1 1 но так выглядит ровнее
        }
    }


}
