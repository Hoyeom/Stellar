using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Stellar.Runtime.Helper
{
    public static class StringHelper
    {
        public static string ToColorString([NotNull] string text, Color color)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<color=");
            sb.Append('#');
            sb.Append(ColorUtility.ToHtmlStringRGBA(color));
            sb.Append('>');
            sb.Append(text);
            sb.Append("</color>");
            return sb.ToString();
        }
    }
}