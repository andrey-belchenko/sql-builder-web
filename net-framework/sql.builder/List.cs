using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices; // MethodImplAttribute

namespace sql.builder
{
    internal static class List
    {
        internal static bool IsNullOrEmpty<T>(IList<T> list)
        {
            return list == null || list.Count == 0;
        }
        #if !NETFX_40
        // Этот аттрибут появился только в .Net Framework 4.5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        internal static bool Any(this System.Collections.IList list)
        {
            return list.Count != 0;
        }
        internal static bool Any<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = 0; index < list.Count; index++) {
                if (predicate(list[index])) {
                    return true;
                }
            }
            return false;
        }
        internal static T FirstOrDefault<T>(this IList<T> list)
            where T : class
        {
            if (list.Count == 0) {
                return null;
            } else {
                return list[0];
            }
        }
        internal static T FirstOrDefault<T>(this IList<T> list, Func<T, bool> predicate)
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
        internal static T LastOrDefault<T>(this IList<T> list)
            where T : class
        {
            int count = list.Count;
            if (count == 0) {
                return null;
            } else {
                return list[count - 1];
            }
        }
        internal static T LastOrDefault<T>(this IList<T> list, Func<T, bool> predicate)
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
        internal static T First<T>(this IList<T> list)
        {
            if (list.Count == 0) {
                throw new InvalidOperationException();
            } else {
                return list[0];
            }
        }
        internal static T First<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = 0; index < list.Count; index++) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            throw new InvalidOperationException();
        }
        internal static T Last<T>(this IList<T> list)
        {
            int count = list.Count;
            if (count == 0) {
                throw new InvalidOperationException();
            } else {
                return list[count - 1];
            }
        }
        // Нет использования
        /*internal static T Last<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int index = list.Count - 1; index >= 0; index--) {
                T value = list[index];
                if (predicate(value)) {
                    return value;
                }
            }
            throw new InvalidOperationException();
        }*/
        internal static TResult[] SelectAsArray<TSource, TResult>(this IList<TSource> list, Func<TSource, TResult> selector)
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