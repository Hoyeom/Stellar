using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Stellar.Runtime.Helper
{
    public static class StringHelper
    {
        public static string ToColorString([NotNull] string text, Color color)
        {
            int estimatedLength = text.Length + 16 + 9;
            StringBuilder sb = new StringBuilder(estimatedLength);
    
            sb.Append("<color=#");
            sb.Append(ColorUtility.ToHtmlStringRGBA(color));
            sb.Append('>');
            sb.Append(text);
            sb.Append("</color>");
    
            return sb.ToString();
        }

    }
}