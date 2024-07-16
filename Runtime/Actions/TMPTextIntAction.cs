using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using SerializableCallback;
using TMPro;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public abstract class TMPTextIntActionBase<T> : DOTweenActionBase
    {
        public override Type TargetComponentType => typeof(TMP_Text);
        public override string DisplayName => $"TMP Text {typeof(T).Name}";

        [SerializeField] protected T _initialValue;
        [SerializeField] protected SerializableValueCallback<T> _initialValueGetter;
        [SerializeField] protected SerializableValueCallback<T> _targetValueGetter;
        [SerializeField] protected string _format;

        protected TMP_Text _tmpTextComponent;
        protected string _previousText;
        protected TMP_Text _previousTarget;

        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            if (_tmpTextComponent == null)
            {
                _tmpTextComponent = target.GetComponent<TMP_Text>();
                if (_tmpTextComponent == null)
                {
                    Debug.LogError($"{target} does not have {TargetComponentType} component");
                    return null;
                }
            }

            _previousText = _tmpTextComponent.text;
            _previousTarget = _tmpTextComponent;
            var tween = GetTween(duration);
            return tween;
        }

        protected virtual string GetText(T value)
        {
            if (!string.IsNullOrEmpty(_format))
            {
                return string.Format(_format, value);
            }

            return $"{value}";
        }
        
        protected abstract Tweener GetTween(float duration);

        public override void ResetToInitialState()
        {
            if (_previousTarget == null)
                return;
            
            if (_previousText == null)
                return;

            _previousTarget.text = _previousText;
        }
    }
    
    [Serializable]
    public class TMPTextIntAction : TMPTextIntActionBase<int>
    {
        protected override Tweener GetTween(float duration)
        {
            var tween = DOTween.To(
                () => _initialValue,
                val => _tmpTextComponent.text = GetText(val),
                _targetValueGetter.Value,
                duration);
            return tween;
        }
    }
    
    [Serializable]
    public class TMPTextFloatAction : TMPTextIntActionBase<float>
    {
        protected override Tweener GetTween(float duration)
        {
            var tween = DOTween.To(
                () => _initialValue,
                val => _tmpTextComponent.text = GetText(val),
                _targetValueGetter.Value,
                duration);
            return tween;
        }
    }
    
    [Serializable]
    public class TMPTextStringAction : TMPTextIntActionBase<string>
    {
        protected override Tweener GetTween(float duration)
        {
            var tween = DOTween.To(
                () => _initialValue,
                val => _tmpTextComponent.text = GetText(val),
                _targetValueGetter.Value,
                duration);
            return tween;
        }
    }
}