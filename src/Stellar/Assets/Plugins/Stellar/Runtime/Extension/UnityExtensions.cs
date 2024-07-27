using Stellar.Runtime.Helper;
using UnityEngine;

namespace Stellar.Runtime.Extension
{
    public static class UnityExtensions
    {
        public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            return UnityHelpers.GetOrAddComponent<TComponent>(gameObject);
        }
    }
}
