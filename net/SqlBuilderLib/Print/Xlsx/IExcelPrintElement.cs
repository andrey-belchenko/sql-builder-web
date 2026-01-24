using System;
using System.Collections.Generic;
using System.Data;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// Печатаемый элемент (строка или группа строк) шаблона Excel в формате xlsx
    /// </summary>
    /// <seealso cref="sql.builder.Print.XML.IExcelPrintElement"/>
    internal interface IExcelPrintElement
    {
        /// <summary>
        /// Группа строк
        /// </summary>
        IExcelPrintGroup Parent { get; }
        void Print(WorksheetPrint pi, DataSet data, bool use_data_reader, DataRow row);
        void DeleteNode(WorksheetPrint pi);
    }
    /// <summary>
    /// Группа строк шаблона Excel в формате xlsx
    /// </summary>
    /// <seealso cref="sql.builder.Print.XML.IExcelPrintGroup"/>
    internal interface IExcelPrintGroup : IExcelPrintElement
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
