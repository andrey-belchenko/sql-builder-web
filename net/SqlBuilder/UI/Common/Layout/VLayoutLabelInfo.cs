namespace sql.builder.UI
{
    public class VLayoutLabelInfo : VLayoutItemInfo
    {






        private string text;
        private string hint;

        public override void SetText(string value)
        {
            text = value;

            //if (text == "Штраф")
            //{

            //}
            if (control != null)
            {

                //GetTypedControl().SetText(GetText());
            }
            (GetParent() as VLayoutGroupInfo).ResetChildValuesInfo();

        }

        public override void SetHint(string value)
        {
            hint = value;
            if (control != null)
            {

                //GetTypedControl().SetHint(value);
            }

        }

        public VLayoutControlContainerInfo Item;

        public override bool IsSelfVisible()
        {
            if (Item == null)
            {
                return base.IsSelfVisible();
            }
            else
            {
                return Item.IsSelfVisible();
            }
        }
        public override string GetText()
        {
            return text;
        }
        public string GetHint()
        {
            return hint;
        }
        public int GetTextWidth()
        {
            return 0;
            //return GetTypedControl().GetTextWidth();
        }
        public override decimal GetWidthFixed()
        {
            // return GetTextWidth();
            return (GetParent() as VLayoutGroupInfo).GetMaxLabelTextWidth();
        }

        public override void Show()
        {
            if (Item == null) // иначе показывается вместе с контролом
            {
                ShowLabel();
            }
        }


        public void ShowLabel()
        {

            if (isBold != oldIsBold)
            {
                oldIsBold = isBold;
                //GetTypedControl().SetBold(isBold);
            }

            if (IsTextAlignmentLeft() != oldIsTALeft)
            {

                oldIsTALeft = IsTextAlignmentLeft();
                //GetTypedControl().SetTextAlignment(oldIsTALeft);
            }

        }

        public override int GetMarginLeft()
        {
            return 3;
        }
        public override int GetMarginRight()
        {
            return 2;
        }

        public override int GetMarginTop()
        {
            return 1;
        }
        public override int GetMarginBottom()
        {
            return 1;
        }
        private bool isBold = false;
        private bool oldIsBold = false;
        public void SetBold(bool value)
        {
            isBold = value;
        }

        //private bool isTALeft = false;
        private bool oldIsTALeft = true;
        //public void SetTextAlignment(bool value)
        //{
        //    isTALeft = value;
        //}
        public bool IsTextAlignmentLeft()
        {
            if (isFirstInRow != true)
            {
                return false;
            }
            return true;
        }


        public override bool NoBreakNext()
        {
            return true;
        }

        public override int GetControlHeight()
        {
            return 0;
            //return GetControlsFactory().LabelHeight();
        }

    }


}
