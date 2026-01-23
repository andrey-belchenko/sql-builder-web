namespace sql.builder.MP
{
    public class MPStringType : MPType
    {


        public int MaxLength { get; private set; }

        public MPStringType(int maxLength)
        {
            MaxLength = maxLength;

            Name = string.Format("string({0})", maxLength);
        }
    }
}