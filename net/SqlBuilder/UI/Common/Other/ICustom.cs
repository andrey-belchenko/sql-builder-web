using System;
//using System.Windows.Forms;

namespace sql.builder.UI
{
    /// <summary>
    /// Интерфейс для добавления собственных контролов на форму поиска FormParametersControl и их взаимодействия с ней.
    /// Пример реализации с выбором одного значения sql.builder.Test.UITestCustom.
    /// Пример реализации с выбором нескольких значений sql.builder.Test.UITestCustomArray. 
    /// </summary>
    public interface ICustom
    {
        int GetControlHeight();

        /// <summary>
        /// Возвращает значение (или коллекцию значений), выбранные/введенные в контроле.
        /// Если значений выбрано несколько, тип должен реализовывать IEnumerable&lt;object>.
        /// </summary>
        /// <returns></returns>
        object GetDataValue();

        /// <summary>
        /// Задаёт значение (или коллекцию значений), выбранные/введенные в контроле.
        /// Если значений выбрано несколько, тип будет реализовывать IEnumerable&lt;object>.
        /// </summary>
        /// <returns></returns>
        void SetDataValue(object value);

        /// <summary>
        /// Возвращает истину, если в контроле не введено/не выбрано значение.
        /// </summary>
        /// <returns></returns>
        bool IsDataEmpty();

        /// <summary>
        /// Сбрасывает все введённые/выбранные значения в контроле.
        /// </summary>
        /// <returns></returns>
        void ClearDataValue();

        /// <summary>
        /// Возникает, когда в контроле изменяется введённое/выбранное значение (либо значения).
        /// </summary>
        event EventHandler ValueChanged;
    }

    public interface ICustomDisplacement
    {


        /// <summary>
        /// Насколько нужно увеличить ширину по сравнению со штатными контролами
        /// </summary>
        /// <returns></returns>
        int GetWidthDisplacement();

    }
}