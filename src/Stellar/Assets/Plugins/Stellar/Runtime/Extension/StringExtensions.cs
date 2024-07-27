using JetBrains.Annotations;
using Stellar.Runtime.Helper;
using UnityEngine;

namespace Stellar.Runtime.Extension
{
    public static class StringExtensions
    {
        public static string ToColorString([NotNull] this string text, Color color)
        {
            return StringHelper.ToColorString(text, color);
        }
    }

    public static class VectorExtensions
    {
        
    }
}