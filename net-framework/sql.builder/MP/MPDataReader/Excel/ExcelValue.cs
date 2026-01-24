namespace sql.builder.MP
{
    public class ExcelValue
    {
        public string Format { get; private set; }
        public object Value { get; private set; }

        public ExcelValue(string format, object value)
        {
            Format = format;
            Value = value;
        }
    }
}