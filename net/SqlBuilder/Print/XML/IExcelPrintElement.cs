using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;

namespace sql.builder.Print.XML
{
    /// <summary>
    /// Печатаемый элемент (строка или группа строк) шаблона Excel в формате xml
    /// </summary>
    /// <seealso cref="sql.builder.Print.Xlsx.IExcelPrintElement"/>
    public interface IExcelPrintElement
    {
        /// <summary>
        /// Группа строк
        /// </summary>
        IExcelPrintGroup Parent { get; }
        void Print(XmlWriter writer, DataSet dataSet, DataRow row, bool print_big_data);
        void ClearData();
    }
    /// <summary>
    /// Группа строк шаблона Excel в формате xml
    /// </summary>
    /// <seealso cref="sql.builder.Print.Xlsx.IExcelPrintGroup"/>
    public interface IExcelPrintGroup : IExcelPrintElement
    {
        /// <summary>
        /// Печатаемые элементы шаблона (строки и группы строк)
        /// </summary>
        IList<IExcelPrintElement> Childs { get; }
        /// <summary>
        /// Возвращает TableReference для таблицы DataSet'а с именем <paramref name="table_name"/>
        /// </summary>
        /// <param name="table_name">наименование таблицы</param>
        /// <returns>Экземпляр TableReference</returns>
        TableReference GetTableReference(string table_name);
    }
}