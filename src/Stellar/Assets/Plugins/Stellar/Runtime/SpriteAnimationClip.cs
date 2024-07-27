using System;
using UnityEngine;

namespace Stellar.Runtime
{
    
    [CreateAssetMenu(menuName = "Stellar/SpriteAnimationClip", fileName = "SpriteAnimationClip", order = 0)]
    public class SpriteAnimationClip : ScriptableObject
    {
        public bool Loop;
        public int SampleRate;
        public Frame[] Frames;

        [Serializable]
        public struct Frame
        {
            public Property[] Properties;
            
            [Serializable]
            public struct Property
            {
                public string Path;
                public Sprite Sprite;
            }
        }
    }
}