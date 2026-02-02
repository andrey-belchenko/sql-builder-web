using System;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор часто используемых предикатов для XAttribute
    /// </summary>
    public static class APredicate
    {
        #region Условные предикаты (типа Func&lt;XAttribute, bool&gt;)
        public static bool IsBehaviorColumns(XAttribute a)
        {
            Contract.Assert(a != null);
            XName name = a.Name;
            //return TextConst.ANameArray.BehaviorColumns.Contains(a.Name.LocalName);
            return (name == AName.@default) || (name == AName.editable) || (name == AName.valid) || (name == AName.textsource) || (name == AName.mandatory) || (name == AName.visible) || (name == AName.new_val) || (name == AName.font_color);
        }
        public static bool IsCallAttributes(XAttribute a)
        {
            Contract.Assert(a != null);
            XName name = a.Name;
            //return Compiler.CallAttributes.Contains(a.Name.LocalName);
            return (name == AName.function) || (name == AName.type) || (name == AName.title) ||
                   (name == AName.class_title) || (name == AName.@as) || (name == AName.joinexp) ||
                   (name == AName.mp) || (name == AName.agg) || (name == AName.format) ||
                   (name == AName.nvl) || (name == AName.nullif) || (name == AName.key) ||
                   (name == AName.quickview) || (name == AName.qv_split) ||
                   (name == AName.c_master) || (name == AName.c_master_key) || (name == AName.colset) ||
                   (name == AName.color) || (name == AName.halign) || (name == AName.merge_key);
                 
        }
        public static bool IsAdditionalAttribute(XAttribute a)
        {
            Contract.Assert(a != null);
            //return Compiler.AdditionalAttributes.Contains(a.Name.LocalName);
            XName name = a.Name;
            return (name == AName.colset) || (name == AName.parname) || (name == AName.color) || (name == AName.font_color) || (name == AName.client_calc) || (name == AName.excel_calc) || (name == AName.halign) || (name == AName.merge_key) || (name == AName.is_fact_use);
        }
        public static bool IsCustomLayoutOptions(XAttribute a)
        {
            Contract.Assert(a != null);
            //return TextConst.ANameArray.CustomLayoutOptions.Contains(a.Name.LocalName);
            XName name = a.Name;
            return (name == AName.size) || (name == AName.min_size) || (name == AName.max_size) || (name == AName.position);
        }
        public static bool IsLayoutOptions(XAttribute a)
        {
            Contract.Assert(a != null);
            //TextConst.ANameArray.AllLayoutOptions.Contains(a.Name.LocalName)
            XName name = a.Name;
            return (name == AName.size) || (name == AName.min_size) || (name == AName.max_size) || (name == AName.position) ||
                   (name == AName.text_location) || (name == AName.text_visible) || (name == AName.is_layout_block) ||
                   (name == AName.width_perc) || (name == AName.width_fixed) || (name == AName.fixed_side) || (name == AName.fill_height);
        }
        public static bool IsColumnRecoveredAttribute(XAttribute a)
        {
            Contract.Assert(a != null);
            //TextConst.ANameArray.ColumnRecoveredAttributes.Contains(a.Name.LocalName)
            XName name = a.Name;
            return (name == AName.title) || (name == AName.agg) || (name == AName.class_title);
        }
        // string[] columnAttributesNamesCanDub = new string[] { "type", "agg", "format", TextConst.AName.CMaster, TextConst.AName.CMasterKey, "title", "class-title", "dimname", "pivot", TextConst.AName.IsFactUse };//убрал mp его нельзя повторять
        public static bool IsColumnAttributeCanDub(XAttribute a)
        {
            Contract.Assert(a != null);
            XName name = a.Name;
            return name == AName.type || name == AName.agg || name == AName.format ||
                   name == AName.c_master || name == AName.c_master_key || name == AName.title || name == AName.class_title || name == AName.dimname || name == AName.pivot || name == AName.is_fact_use;
        }
        // string[] sectRoleAttrsNames = new string[] { "cumulate", "sections" };
        public static bool IsSectRoleAttribute(XAttribute a)
        {
            Contract.Assert(a != null);
            XName name = a.Name;
            return name == AName.cumulate || name == AName.sections;
        }
        #endregion
        #region Предикаты (типа Func&lt;XAttribute, string&gt;)
        /// <summary>
        /// Возвращает значение аттрибута
        /// </summary>
        /// <param name="a">аттрибут</param>
        /// <returns>Значение аттрибута <paramref name="a"/></returns>
        /// <example>el.Descendants(EName.Column).Attributes(AName.Name).Select(APredicate.AttributeValue)</example>
        public static string AttributeValue(XAttribute a)
        {
            Contract.Assert(a != null);
            return a.Value;
        }
        #endregion
    }
}
