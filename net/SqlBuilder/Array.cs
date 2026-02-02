using System;
using System.Runtime.CompilerServices; // MethodImplAttribute
using System.Collections.Generic;

namespace sql.builder
{
    public static class Array
    {
        public static bool IsNullOrEmpty<T>(T[] arr)
        {
            return arr == null || arr.Length == 0;
        }
        public static bool Contains(this string[] arr, string str)
        {
            return System.Array.IndexOf<string>(arr, str) >= 0;
        }
        /// <summary>
        /// Возвращает пустой массив
        /// </summary>
        public static T[] Empty<T>() // убрать при переходе на .Net 4.6, там такой метод есть в классе System.Array
        {
            return EmptyArray<T>.Value;
        }
        private static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
        public static void Fill<T>(T[] array, T value)
        {
            for (int index = 0; index < array.Length; index++) {
                array[index] = value;
            }
        }
        // Нет использования
        /*public static bool Any<T>(this T[] array)
        {
            return array.Length > 0;
        }*/
        #if !NETFX_40
        // Этот аттрибут появился только в .Net Framework 4.5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Any<T>(this T[] array, Predicate<T> predicate)
        {
            return System.Array.Exists<T>(array, predicate);
        }
        // Нет использования
        /*public static T FirstOrDefault<T>(this T[] array)
        {
            if (array.Length == 0) {
                return default(T);
            } else {
                return array[0];
            }
        }*/
        // Нет использования
        /*public static T FirstOrDefault<T>(this T[] array, Predicate<T> predicate)
        {
            return System.Array.Find<T>(array, predicate);
        }*/
        // Нет использования
        /*public static T LastOrDefault<T>(this T[] array)
        {
            int count = array.Length;
            if (count == 0) {
                return default(T);
            } else {
                return array[count - 1];
            }
        }*/
        // Нет использования
        /*public static T LastOrDefault<T>(this T[] array, Predicate<T> predicate)
        {
            return System.Array.FindLast<T>(array, predicate);
        }*/
        public static TResult[] Select<TSource, TResult>(this TSource[] array, Func<TSource, TResult> selector)
        {
            int count = array.Length;
            if (count == 0) {
                return Array.Empty<TResult>();
            } else {
                TResult[] result = new TResult[count];
                for (int index = 0; index < count; index++) {
                    result[index] = selector(array[index]);
                }
                return result;
            }
        }
    }
}