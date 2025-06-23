using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var cleaned = new string(input
                .ToLower()
                .Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                .ToArray());

            return cleaned.SequenceEqual(cleaned.Reverse());
        }
    }
}