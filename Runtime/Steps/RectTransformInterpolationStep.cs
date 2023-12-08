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

        private Vector3 _originalPosition;
        private Vector3 _originalEulerAngles;
        private Vector3 _originalScale;
        private Vector2 _originalAnchorMin;
        private Vector2 _originalAnchorMax;
        private Vector2 _originalAnchoredPosition;
        private Vector2 _originalSizeDelta;
        private Vector2 _originalPivot;
        
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
            
            // create tweens
            Tweener positionTween = null;
            Tweener eulerAnglesTween = null;
            if (_useLocal)
            {
                positionTween = DOTween.To(() => _targetRectTransform.localPosition, value => _targetRectTransform.localPosition = value, _position, _duration);
                eulerAnglesTween = DOTween.To(() => _targetRectTransform.localEulerAngles, value => _targetRectTransform.localEulerAngles = value, _eulerAngles, _duration);
            }
            else
            {
                positionTween = DOTween.To(() => _targetRectTransform.position, value => _targetRectTransform.position = value, _position, _duration);
                eulerAnglesTween = DOTween.To(() => _targetRectTransform.eulerAngles, value => _targetRectTransform.eulerAngles = value, _eulerAngles, _duration);
            }
            
            var scaleTween = DOTween.To(() => _targetRectTransform.localScale, value => _targetRectTransform.localScale = value, _scale, _duration);
            var anchorMinTween = DOTween.To(() => _targetRectTransform.anchorMin, value => _targetRectTransform.anchorMin = value, _anchorMin, _duration);
            var anchorMaxTween = DOTween.To(() => _targetRectTransform.anchorMax, value => _targetRectTransform.anchorMax = value, _anchorMax, _duration);
            var anchoredPositionTween = DOTween.To(() => _targetRectTransform.anchoredPosition, value => _targetRectTransform.anchoredPosition = value, _anchoredPosition, _duration);
            var sizeDeltaTween = DOTween.To(() => _targetRectTransform.sizeDelta, value => _targetRectTransform.sizeDelta = value, _sizeDelta, _duration);
            var pivotTween = DOTween.To(() => _targetRectTransform.pivot, value => _targetRectTransform.pivot = value, _pivot, _duration);

            // set tweens' direction
            if (_direction == DOTweenActionBase.AnimationDirection.From)
            {
                positionTween.From(_isRelative);
                eulerAnglesTween.From(_isRelative);
                scaleTween.From(_isRelative);
                anchorMinTween.From(_isRelative);
                anchorMaxTween.From(_isRelative);
                anchoredPositionTween.From(_isRelative);
                sizeDeltaTween.From(_isRelative);
                pivotTween.From(_isRelative);
            }
            
            // set whether is relative or not
            positionTween.SetRelative(_isRelative);
            eulerAnglesTween.SetRelative(_isRelative);
            scaleTween.SetRelative(_isRelative);
            anchorMinTween.SetRelative(_isRelative);
            anchorMaxTween.SetRelative(_isRelative);
            anchoredPositionTween.SetRelative(_isRelative);
            sizeDeltaTween.SetRelative(_isRelative);
            pivotTween.SetRelative(_isRelative);
            
            // join tweens to sequencce
            sequence.Join(positionTween);
            sequence.Join(eulerAnglesTween);
            sequence.Join(scaleTween);
            sequence.Join(anchorMinTween);
            sequence.Join(anchorMaxTween);
            sequence.Join(anchoredPositionTween);
            sequence.Join(sizeDeltaTween);
            sequence.Join(pivotTween);

            sequence.SetEase(_ease);
            if (FlowType == FlowType.Join)
                animationSequence.Join(sequence);
            else
                animationSequence.Append(sequence);
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
                _position = _targetRectTransform.localPosition;
                _eulerAngles = _targetRectTransform.localEulerAngles;
            }
            else
            {
                _position = _targetRectTransform.position;
                _eulerAngles = _targetRectTransform.eulerAngles;
            }
            _scale = _targetRectTransform.localScale;
            _anchorMin = _targetRectTransform.anchorMin;
            _anchorMax = _targetRectTransform.anchorMax;
            _anchoredPosition = _targetRectTransform.anchoredPosition;
            _sizeDelta = _targetRectTransform.sizeDelta;
            _pivot = _targetRectTransform.pivot;
        }
    }
}