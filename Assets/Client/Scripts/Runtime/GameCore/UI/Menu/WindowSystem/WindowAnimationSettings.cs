using DG.Tweening;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "WindowAnimationSettings", menuName = "Game/UI/Window Animation Settings")]
    public class WindowAnimationSettings : ScriptableObject
    {
        [System.Serializable]
        public class AnimationPreset
        {
            public WindowAnimationType type;
            public float duration = 0.3f;
            public float scaleMultiplier = 1.1f;
            public Ease openEase = Ease.OutBack;
            public Ease closeEase = Ease.InBack;
        }

        [SerializeField] private AnimationPreset[] _presets;
        
        public AnimationPreset GetPreset(WindowAnimationType type)
        {
            foreach (var preset in _presets)
            {
                if (preset.type == type)
                    return preset;
            }
            
            Debug.LogWarning($"No preset found for animation type {type}, using default values");
            return new AnimationPreset { type = type };
        }
    }
} 