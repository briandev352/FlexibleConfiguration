// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration.Internal
{
    public class ConfigurationKeyComparer : IComparer<string>
    {
        private static readonly string[] _keyDelimiterArray = new[] { ConfigurationPath.KeyDelimiter };

        public static ConfigurationKeyComparer Instance { get; } = new ConfigurationKeyComparer();

        public int Compare(string x, string y)
        {
            var xParts = x?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var yParts = y?.Split(_keyDelimiterArray, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            // Compare each part until we get two parts that are not equal
            for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                x = xParts[i];
                y = yParts[i];

                var value1 = 0;
                var value2 = 0;

                var xIsInt = x != null && int.TryParse(x, out value1);
                var yIsInt = y != null && int.TryParse(y, out value2);

                int result = 0;

                if (!xIsInt && !yIsInt)
                {
                    // Both are strings
                    result = string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
                }
                else if (xIsInt && yIsInt)
                {
                    // Both are int 
                    result = value1 - value2;
                }
                else
                {
                    // Only one of them is int
                    result = xIsInt ? -1 : 1;
                }

                if (result != 0)
                {
                    // One of them is different
                    return result;
                }
            }

            // If we get here, the common parts are equal.
            // If they are of the same length, then they are totally identical
            return xParts.Length - yParts.Length;
        }
    }
}
