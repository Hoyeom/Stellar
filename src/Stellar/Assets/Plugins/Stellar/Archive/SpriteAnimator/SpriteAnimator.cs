using System;
using System.Collections.Generic;
using System.Linq;
using Stellar.Runtime;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteAnimatorController _spriteAnimatorController; // 애니메이션 컨트롤러
    private readonly Dictionary<string, Transform> _pathCache = new Dictionary<string, Transform>(); // 경로 캐시
    private string _currentState; // 현재 상태
    private float _timeSinceLastFrame; // 마지막 프레임 이후 경과 시간
    private int _currentFrameIndex; // 현재 프레임 인덱스

    // 게임 오브젝트가 활성화될 때 초기화
    private void Awake()
    {
        InitializeAnimationState();
    }

    public void Update()
    {
        UpdateAnimation();
    }

    // 애니메이션 상태 초기화
    private void InitializeAnimationState()
    {
        if (_spriteAnimatorController.States.Length == 0)
        {
            Debug.LogWarning("애니메이션 컨트롤러에 상태가 없습니다.");
            return;
        }

        var firstState = _spriteAnimatorController.States[0];
        _currentState = firstState.Key;
        _timeSinceLastFrame = 0f;
        _currentFrameIndex = 0;

        if (firstState.Clip != null)
        {
            ApplyFrameProperties(firstState.Clip.Frames[_currentFrameIndex]);
        }
    }

    // 애니메이션 업데이트
    private void UpdateAnimation()
    {
        var state = _spriteAnimatorController.States.FirstOrDefault(s => s.Key == _currentState);

        if (state.Clip != null && state.Clip.Frames.Length > 0)
        {
            _timeSinceLastFrame += Time.deltaTime;
            float frameDuration = 1f / state.Clip.SampleRate;
            
            if (_timeSinceLastFrame >= frameDuration)
            {
                _timeSinceLastFrame -= frameDuration;
                _currentFrameIndex = (_currentFrameIndex + 1) % state.Clip.Frames.Length;
                Debug.Log($"OnFrame {_currentFrameIndex}");
                ApplyFrameProperties(state.Clip.Frames[_currentFrameIndex]);
            }
        }
    }

    // 프레임 속성 적용
    private void ApplyFrameProperties(SpriteAnimationClip.Frame frame)
    {
        foreach (var property in frame.Properties)
        {
            var spriteRenderer = GetCachedSpriteRenderer(property.Path);
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = property.Sprite;
            }
            else
            {
                Debug.LogWarning($"경로에 SpriteRenderer를 찾을 수 없습니다: {property.Path}");
            }
        }
    }

    // 캐시된 SpriteRenderer를 가져오는 메서드
    private SpriteRenderer GetCachedSpriteRenderer(string path)
    {
        if (!_pathCache.TryGetValue(path, out Transform cachedTransform))
        {
            cachedTransform = FindChildByPath(transform, path);
            if (cachedTransform != null)
            {
                _pathCache[path] = cachedTransform;
            }
        }
        return cachedTransform?.GetComponent<SpriteRenderer>();
    }

    // 경로를 사용하여 자식 GameObject를 찾는 메서드
    private Transform FindChildByPath(Transform parent, string path)
    {
        string[] names = path.Split('/');
        Transform current = parent;

        foreach (string name in names)
        {
            current = current.Find(name);
            if (current == null)
            {
                Debug.LogWarning($"경로를 따라 GameObject를 찾을 수 없습니다: {path}");
                return null;
            }
        }
        return current;
    }
}
