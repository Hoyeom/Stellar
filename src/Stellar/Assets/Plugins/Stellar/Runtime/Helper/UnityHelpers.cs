using UnityEngine;

namespace Stellar.Runtime.Helper
{
    public static class UnityHelpers
    {
        public static TComponent GetOrAddComponent<TComponent>(in GameObject gameObject) where TComponent : Component
        {
            if (gameObject.TryGetComponent(out TComponent component))
            {
                return component;
            }

            return gameObject.AddComponent<TComponent>();
        }
    }
}