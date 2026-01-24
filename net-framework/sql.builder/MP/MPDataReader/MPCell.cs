namespace sql.builder.MP
{
    public class MPCell
    {
        public MPColumn Column { get; private set; }
        public MPRow Row { get; private set; }

        // todo: как бы придумать чтобы избежать упаковки/распоковки значимых типов
        object _value;

        public MPCell(MPColumn column, MPRow row)
        {
            Column = column;
            Row = row;
        }

        public void SetValue(object value)
        {
            // todo: проверка соответствия типов
            _value = value;
        }

        public object GetValue()
        {
            return _value;
        }
    }
}
