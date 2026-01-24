namespace sql.builder.MP
{
    public class MPNumberType : MPType
    {
        public int? DecimalPlacesCount { get; private set; }

        public MPNumberType(int? decimalPlacesCount)
        {
            DecimalPlacesCount = decimalPlacesCount;

            Name = (decimalPlacesCount.HasValue) 
                ? string.Format("number({0})", decimalPlacesCount.Value)
                : "number";
        }
    }
}