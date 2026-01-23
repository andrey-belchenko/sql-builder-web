//using System;
////using System.Windows.Forms;
//using infoenergo.app.common.Search.asuse.Controls;
//using infoenergo.search;
//
//namespace sql.builder.UI.Common.Other.Custom
//{
//    public class UITepSchemaSearch : ICustom, ICustomDisplacement
//    {
//
//        SearchTepSchemaCombo _control;
//
//        public UITepSchemaSearch()
//        {
//            _control = new SearchTepSchemaCombo();
//            _control.Margin = new Padding(0);
//            _control.Padding = new Padding(0);
//            _control.Connection = db.Connection;
//            _control.EntityFound += Control_OnFound;
//            _control.InplaceResultMode = true;
//            _control.ShowClearButton = false;
//            _control.UseContext = false;
//
//            foreach (var ctrl in Cmn.GetChildControlsOfType<Control>(_control))
//            {
//               
//                ctrl.Top = 0;
//                ctrl.Left = 0;
//            }
//        }
//
//        private void Control_OnFound(object sender, EntityFoundEventArgs args)
//        {
//            if (ValueChanged != null)
//            {
//                ValueChanged(this, EventArgs.Empty);
//            }
//        }
//
//        public Control GetControl()
//        {
//            return _control;
//        }
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
//
//        public event EventHandler ValueChanged;
//
//        public int GetWidthDisplacement()
//        {
//            return 3;
//        }
//    }
//}
