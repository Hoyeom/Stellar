using System;
using UnityEngine;

namespace Stellar.Runtime
{
    [CreateAssetMenu(menuName = "Stellar/SpriteAnimatorController", fileName = "SpriteAnimatorController", order = 0)]
    public class SpriteAnimatorController : ScriptableObject
    {
        public State[] States;
        
        [Serializable]
        public struct State
        {
            public string Key;
            public SpriteAnimationClip Clip;
        }
    }
}