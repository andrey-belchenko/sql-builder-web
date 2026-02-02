using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices; // MethodImplAttribute

namespace sql.builder
{
    public static class List
    {
        public static bool IsNullOrEmpty<T>(IList<T> list)
        {
            return list == null || list.Count == 0;
        }
        #if !NETFX_40
        // Этот аттрибут появился только в .Net Framework 4.5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Any(this System.Collections.IList list)
        {
            return list.Count != 0;
        }
        public static bool Any<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = 0; index < list.Count; index++) {
                if (predicate(list[index])) {
                    return true;
                }
            }
            return false;
        }
        public static T FirstOrDefault<T>(this IList<T> list)
            where T : class
        {
            if (list.Count == 0) {
                return null;
            } else {
                return list[0];
            }
        }
        public static T FirstOrDefault<T>(this IList<T> list, Func<T, bool> predicate)
            where T : class
        {
            for (int index = 0; index < list.Count; index++) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            return null;
        }
        public static T LastOrDefault<T>(this IList<T> list)
            where T : class
        {
            int count = list.Count;
            if (count == 0) {
                return null;
            } else {
                return list[count - 1];
            }
        }
        public static T LastOrDefault<T>(this IList<T> list, Func<T, bool> predicate)
            where T : class
        {
            for (int index = list.Count - 1; index >= 0; index--) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            return null;
        }
        public static T First<T>(this IList<T> list)
        {
            if (list.Count == 0) {
                throw new InvalidOperationException();
            } else {
                return list[0];
            }
        }
        public static T First<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = 0; index < list.Count; index++) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            throw new InvalidOperationException();
        }
        public static T Last<T>(this IList<T> list)
        {
            int count = list.Count;
            if (count == 0) {
                throw new InvalidOperationException();
            } else {
                return list[count - 1];
            }
        }
        // Нет использования
        /*public static T Last<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = list.Count - 1; index >= 0; index--) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            throw new InvalidOperationException();
        }*/
        public static TResult[] SelectAsArray<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector)
        {
            int count = list.Count;
            if (count == 0) {
                return Array.Empty<TResult>();
            } else {
                TResult[] result = new TResult[count];
                for (int index = 0; index < count; index++) {
                    result[index] = selector(list[index]);
                }
                return result;
            }
        }
   }
}