
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static string ProcessField(VForm form, VField field)
        {

            var fieldName = field.P_Field;
            var fieldFileName = $"field_{ClearName(fieldName)}";
            ProcessQuery(field.ListQuery());
            ProcessQuery(field.DefaultQuery());
            return null;

        }
    }
}
