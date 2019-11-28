using System;
using System.Collections.Generic;

namespace DrawingPlayground.Extensions {

    internal static class IEnumerableExtensions {

        public static string JoinBy<T>(this IEnumerable<T> collection, string s) => string.Join(s, collection);

        public static string Concat<T>(this IEnumerable<T> collection) => string.Concat(collection);

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var t in collection) {
                action(t);
            }
        }

    }

}
