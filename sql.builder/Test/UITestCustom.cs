//using System;
////using System.Windows.Forms;
//using infoenergo.app.common;
//using sql.builder.UI;

//namespace sql.builder.Test
//{
//    internal class UITestCustom: ICustom
//    {
//        SearchDogCombo _control;

//        public UITestCustom()
//        {
//            _control = new SearchDogCombo();
//            _control.Connection = db.Connection;
//            _control.Found += Control_OnFound;
//        }

//        private void Control_OnFound(object sender, DogovorFoundEventArgs args)
//        {
//            if (ValueChanged != null)
//            {
//                ValueChanged(this, EventArgs.Empty);
//            }
//        }

//        //public Control GetControl()
//        //{
//        //    return _control;
//        //}
//        public int GetControlHeight()
//        {
//            return 0;
//        }
//        public object GetDataValue()
//        {
//            return _control.Value;
//        }
//        public void SetDataValue(object value)
//        {
//            _control.Value = value;
//        }
//        public bool IsDataEmpty()
//        {
//            return (_control.Value == null);
//        }
//        public void ClearDataValue()
//        {
//            _control.Value = null;
//        }

//        public event EventHandler ValueChanged;
//    }
//}