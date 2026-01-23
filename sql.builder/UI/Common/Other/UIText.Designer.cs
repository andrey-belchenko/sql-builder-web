//using System.Xml.Linq;
//using System.Linq;
//using sql.builder.DataApi;
//namespace sql.builder.UI
//{
//    internal partial class UIText
//    {
//        private IVTextEdit beControl;

//        protected new void BeginHandInitialize(XElement xfield)
//        {
//            BeginInitializeBase();
//            bool isNumber = false;
//            var format = Cmn.GetAttrValue(xfield, TextConst.AName.Format).ToLower();
//            if (format.Length > 0)
//            {
//                var f = format[0];
//                if ((new char[] { 'n', 'f', 'c' }).Contains(f))
//                {
//                    isNumber = true;
//                }
//            }
//            if (isNumber)
//            {
//                beControl = UIStatic.GetControlsfactory().CreateNumberEdit();
//            }
//            else
//            {
//                beControl = UIStatic.GetControlsfactory().CreateTextEdit();
//            }
//        }
//        protected override void initializeComponent(XElement xfield)
//        {
//            base.initializeComponent();
//            BeginHandInitialize(xfield);
//            SetEditor(this.beControl);
//            EndHandInitialize();
//        }
//        protected new void EndHandInitialize()
//        {
//            beControl.EndInit();
//            EndInitializeBase();
//        }
//    }
//}
