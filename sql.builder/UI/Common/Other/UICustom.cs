//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;

//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.XmlHelpers;

//namespace sql.builder.UI
//{
//    internal partial class UICustom : UIBase, IVControl
//    {
//        private ICustom _control;
//        public UICustom()
//        {
//            //InitializeComponent();
//        }
//        public ICustom GetControlObject()
//        {
//            return this._control;
//        }
//        public override void Initialize(XElement xfield, UIFormC form)
//        {
//            string datatype = xfield.AttrOrDefault(AName.type, TextConst.AVDataType.String);
//            Type type;
//            ReturnType rtype;
//            bool allow_manual_used_set;
//            if (datatype == TextConst.AVDataType.Array) {
//                rtype = ReturnType.Array;
//                type = typeof(object);
//                allow_manual_used_set = false;
//            } else {
//                rtype = ReturnType.Simple;
//                allow_manual_used_set = true;
//                if (datatype == TextConst.AVDataType.Date) {
//                    type = typeof(DateTime);
//                } else if (datatype == TextConst.AVDataType.Number) {
//                    type = typeof(decimal);
//                } else {
//                    type = typeof(string);
//                }
//            }
//            this.BaseInitialize(xfield, form, rtype, type, allow_manual_used_set);
//            this.InitControl();
//        }
//        protected override void InitControl()
//        {
//            // чтобы устанавливался UsedParam на VDataTable
//            this.ShowCheck = true;
//            CreateCustomControl();
//        }
//        private void CreateCustomControl()
//        {
//            string type_name = this.xfield.AttrOrEmpty(AName.type_name);
//            if (string.IsNullOrEmpty(type_name)) {
//                throw new InvalidOperationException("Не указан атрибут type-name для поля:" + Environment.NewLine + xfield.ToString());
//            }
//            string assembly_name = this.xfield.AttrOrEmpty(AName.assembly);
//            Type control_type;
//            ReflectionHelper.ResolveAssemblyType(assembly_name, type_name, out control_type);
//            if (control_type == null) {
//                throw new ArgumentException("Не удалось найти указанный тип контрола для поля: " + Environment.NewLine + this.XField.ToString());
//            }
//            if (!control_type.GetInterfaces().Contains(typeof(ICustom))) {
//                throw new ArgumentException("Контрол " + type_name + " не реализует интерфейс sql.builder.UI.ICustom");
//            }
//            this._control = (ICustom)Activator.CreateInstance(control_type);
//            this._control.ValueChanged += _control_OnValueChanged;
//            Control ctrl = this._control.GetControl();
//            ctrl.Enter += Control_Enter;
//            ctrl.Dock = DockStyle.Fill;
//            //AddPart(ctrl, PartDest.Root);
//            this.AddPart(ctrl);
//        }
//        private void _control_OnValueChanged(object sender, EventArgs args)
//        {
//            if (_control.IsDataEmpty())
//            {
//                ClearSourceValues();
//                Used = false;
//            }
//            else if(SourceType == ReturnType.Simple)
//            {
//                SetSourceValue(_control.GetDataValue());
//                Used = true;
//            }
//            else if (SourceType == ReturnType.Array) {
//                IEnumerable<object> values = this._control.GetDataValue() as IEnumerable<object>;
//                if (values == null) {
//                    throw new ArgumentException("Контрол " + this._control.GetType().FullName + " c множественным выбором вернул значение, тип которого не реализует IEnumerable<object>");
//                }
//                object[] selected_new = values.ToArray();
//                object[] selected_old = new object[this.array_edit_value.Rows.Count];
//                for (int index = 0; index < this.array_edit_value.Rows.Count; index++) {
//                    selected_old[index] = this.array_edit_value.Rows[index][0];
//                }
//                foreach (object value in selected_old.Except(selected_new)) {
//                    this.SetSourceValue(new Tuple<object, string, bool>(value, string.Empty, false));
//                }
//                foreach (object value in selected_new.Except(selected_old)) {
//                    this.SetSourceValue(new Tuple<object, string, bool>(value, string.Empty, true));
//                }
//                this.Used = selected_new.Length != 0;
//            }
//        }

//        public override void SetControlValue(object value, int index = 1)
//        {
//            _control.SetDataValue(value);
//        }

//        private void Control_Enter(object sender, EventArgs e)
//        {
//            Form.LastActiveField = this;
//        }

//        public override int GetHeight()
//        {
//            int height = _control.GetControlHeight();
//            return (height > 0) ? height : base.GetHeight();
//        }
//    }
//}
