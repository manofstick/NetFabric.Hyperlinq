﻿using System.Collections.Generic;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    static class StringExtensions
    {
        public static string ToCommaSeparated(this string[] strings)
            => string.Join(",", strings);

        public static string ToCommaSeparated(this IEnumerable<string> strings)
            => string.Join(",", strings);

        public static string ToCommaSeparated(this IReadOnlyList<string> strings)
            => strings.Count switch
            {
                0 => string.Empty,
                1 => strings[0],
                _ => PerformToCommaSeparated(strings),
            };

        static string PerformToCommaSeparated(IReadOnlyList<string> strings)
        {
            var result = new StringBuilder();
            _ = result.Append(strings[0]);
            for (var index = 1; index < strings.Count; index++)
            {
                _ = result.Append(',').Append(strings[index]);
            }
            return result.ToString();
        }
    }
}
