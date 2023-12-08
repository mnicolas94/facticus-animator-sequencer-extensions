using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Steps
{
    [Serializable]
    public class RectTransformInterpolationStep : AnimationStepBase
    {
        public override string DisplayName => "Tween Target RectTransform Properties";
        
        [SerializeField] private RectTransform _targetRectTransform;
        
        [SerializeField] private float _duration = 1;
        [SerializeField] private CustomEase _ease = CustomEase.InOutCirc;
        [SerializeField] private DOTweenActionBase.AnimationDirection _direction;
        [SerializeField] private bool _isRelative;
        
        [Space]
        
        [SerializeField] private bool _useLocal;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _eulerAngles;
        [SerializeField] private Vector3 _scale = Vector3.one;
        [SerializeField] private Vector2 _anchorMin =  new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _anchorMax =  new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _anchoredPosition;
        [SerializeField] private Vector2 _sizeDelta;
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        
        [SerializeField] private bool _tweenPosition = true;
        [SerializeField] private bool _tweenEulerAngles = true;
        [SerializeField] private bool _tweenScale = true;
        [SerializeField] private bool _tweenAnchorMin = true;
        [SerializeField] private bool _tweenAnchorMax = true;
        [SerializeField] private bool _tweenAnchoredPosition = true;
        [SerializeField] private bool _tweenSizeDelta = true;
        [SerializeField] private bool _tweenPivot = true;

        private Vector3 _originalPosition;
        private Vector3 _originalEulerAngles;
        private Vector3 _originalScale;
        private Vector2 _originalAnchorMin;
        private Vector2 _originalAnchorMax;
        private Vector2 _originalAnchoredPosition;
        private Vector2 _originalSizeDelta;
        private Vector2 _originalPivot;

        private Tweener GetPositionTween => _useLocal
            ? DOTween.To(() => _targetRectTransform.localPosition, value => _targetRectTransform.localPosition = value, _position, _duration)
            : DOTween.To(() => _targetRectTransform.position, value => _targetRectTransform.position = value, _position, _duration);
        private Tweener GetEulerAnglesTween => _useLocal
            ? DOTween.To(() => _targetRectTransform.localEulerAngles, value => _targetRectTransform.localEulerAngles = value, _eulerAngles, _duration)
            : DOTween.To(() => _targetRectTransform.eulerAngles, value => _targetRectTransform.eulerAngles = value, _eulerAngles, _duration);
        private Tweener GetScaleTween => DOTween.To(() => _targetRectTransform.localScale, value => _targetRectTransform.localScale = value, _scale, _duration);
        private Tweener GetAnchorMinTween => DOTween.To(() => _targetRectTransform.anchorMin, value => _targetRectTransform.anchorMin = value, _anchorMin, _duration);
        private Tweener GetAnchorMaxTween => DOTween.To(() => _targetRectTransform.anchorMax, value => _targetRectTransform.anchorMax = value, _anchorMax, _duration);
        private Tweener GetAnchoredPositionTween => DOTween.To(() => _targetRectTransform.anchoredPosition, value => _targetRectTransform.anchoredPosition = value, _anchoredPosition, _duration);
        private Tweener GetSizeDeltaTween => DOTween.To(() => _targetRectTransform.sizeDelta, value => _targetRectTransform.sizeDelta = value, _sizeDelta, _duration);
        private Tweener GetPivotTween => DOTween.To(() => _targetRectTransform.pivot, value => _targetRectTransform.pivot = value, _pivot, _duration);
        
        public override void AddTweenToSequence(Sequence animationSequence)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(Delay);
            
            // store original values
            _originalPosition = _useLocal ? _targetRectTransform.localPosition : _targetRectTransform.position;
            _originalEulerAngles = _useLocal ? _targetRectTransform.localEulerAngles : _targetRectTransform.eulerAngles;
            _originalScale = _targetRectTransform.localScale;
            _originalAnchorMin = _targetRectTransform.anchorMin;
            _originalAnchorMax = _targetRectTransform.anchorMax;
            _originalAnchoredPosition = _targetRectTransform.anchoredPosition;
            _originalSizeDelta = _targetRectTransform.sizeDelta;
            _originalPivot = _targetRectTransform.pivot;
            
            // setup tweens
            if (_tweenPosition) SetupTween(GetPositionTween, sequence);
            if (_tweenEulerAngles) SetupTween(GetEulerAnglesTween, sequence);
            if (_tweenScale) SetupTween(GetScaleTween, sequence);
            if (_tweenAnchorMin) SetupTween(GetAnchorMinTween, sequence);
            if (_tweenAnchorMax) SetupTween(GetAnchorMaxTween, sequence);
            if (_tweenAnchoredPosition) SetupTween(GetAnchoredPositionTween, sequence);
            if (_tweenSizeDelta) SetupTween(GetSizeDeltaTween, sequence);
            if (_tweenPivot) SetupTween(GetPivotTween, sequence);
            
            if (FlowType == FlowType.Join)
                animationSequence.Join(sequence);
            else
                animationSequence.Append(sequence);
        }

        private void SetupTween(Tweener tween, Sequence sequence)
        {
            if (_direction == DOTweenActionBase.AnimationDirection.From)
            {
                tween.From(_isRelative);
            }

            tween.SetRelative(_isRelative);
            tween.SetEase(_ease);
            sequence.Join(tween);
        }

        public override void ResetToInitialState()
        {
            if (_useLocal)
            {
                _targetRectTransform.localPosition = _originalPosition;
                _targetRectTransform.localEulerAngles = _originalEulerAngles;
            }
            else
            {
                _targetRectTransform.position = _originalPosition;
                _targetRectTransform.eulerAngles = _originalEulerAngles;
            }
            _targetRectTransform.localScale = _originalScale;
            
            _targetRectTransform.anchorMin = _originalAnchorMin;
            _targetRectTransform.anchorMax = _originalAnchorMax;
            _targetRectTransform.anchoredPosition = _originalAnchoredPosition;
            _targetRectTransform.sizeDelta = _originalSizeDelta;
            _targetRectTransform.pivot = _originalPivot;
        }
        
        public override string GetDisplayNameForEditor(int index)
        {
            string display = "NULL";
            if (_targetRectTransform != null)
                display = _targetRectTransform.name;
            
            return $"{index}. Tween {display}(RectTransform) Properties";
        }

        public void MirrorCurrentValues()
        {
            if (_targetRectTransform == null)
            {
                Debug.LogWarning("Target Rect Transform is null");
                return;
            }
            
            if (_useLocal)
            {
                if (_tweenPosition) _position = _targetRectTransform.localPosition;
                if (_tweenEulerAngles) _eulerAngles = _targetRectTransform.localEulerAngles;
            }
            else
            {
                if (_tweenPosition) _position = _targetRectTransform.position;
                if (_tweenEulerAngles) _eulerAngles = _targetRectTransform.eulerAngles;
            }
            if (_tweenScale) _scale = _targetRectTransform.localScale;
            if (_tweenAnchorMin) _anchorMin = _targetRectTransform.anchorMin;
            if (_tweenAnchorMax) _anchorMax = _targetRectTransform.anchorMax;
            if (_tweenAnchoredPosition) _anchoredPosition = _targetRectTransform.anchoredPosition;
            if (_tweenSizeDelta) _sizeDelta = _targetRectTransform.sizeDelta;
            if (_tweenPivot) _pivot = _targetRectTransform.pivot;
        }
        
        public void SetCurrentValues()
        {
            if (_targetRectTransform == null)
            {
                Debug.LogWarning("Target Rect Transform is null");
                return;
            }
            
            if (_useLocal)
            {
                if (_tweenPosition) _targetRectTransform.localPosition = _position;
                if (_tweenEulerAngles) _targetRectTransform.localEulerAngles = _eulerAngles;
            }
            else
            {
                if (_tweenPosition) _targetRectTransform.position = _position;
                if (_tweenEulerAngles) _targetRectTransform.eulerAngles = _eulerAngles;
            }
            if (_tweenScale) _targetRectTransform.localScale = _scale;
            if (_tweenAnchorMin) _targetRectTransform.anchorMin = _anchorMin;
            if (_tweenAnchorMax) _targetRectTransform.anchorMax = _anchorMax;
            if (_tweenAnchoredPosition) _targetRectTransform.anchoredPosition = _anchoredPosition;
            if (_tweenSizeDelta) _targetRectTransform.sizeDelta = _sizeDelta;
            if (_tweenPivot) _targetRectTransform.pivot = _pivot;
        }
    }
}