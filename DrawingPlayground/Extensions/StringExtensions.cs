using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DrawingPlayground.Extensions {

    internal static class StringExtensions {

        public static Match Match(this string s, Regex regex) => regex.Match(s);

        public static bool IsMatch(this string s, Regex regex) => regex.IsMatch(s);

        public static IEnumerable<Match> Matches(this string s, Regex regex) => regex.Matches(s).OfType<Match>();

        public static string Replace(this string s, Regex regex, string replacement) => regex.Replace(s, replacement);

        public static string Join<T>(this string s, IEnumerable<T> collection) => string.Join(s, collection);

    }

}
