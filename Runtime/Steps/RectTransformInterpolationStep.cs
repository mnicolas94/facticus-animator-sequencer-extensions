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
        
        [SerializeField] private float _duration = 1;
        
        [SerializeField, ContextMenuItem("Mirror", nameof(MirrorCurrentValues))]
        private RectTransform _targetRectTransform;
        
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
            Sequence behaviourSequence = DOTween.Sequence();
            behaviourSequence.SetDelay(Delay);

            Tweener positionTween = null;
            Tweener eulerAnglesTween = null;
            if (_useLocal)
            {
                _originalPosition = _targetRectTransform.localPosition;
                _originalEulerAngles = _targetRectTransform.localEulerAngles;
                
                positionTween = DOTween.To(() => _targetRectTransform.localPosition, value => _targetRectTransform.localPosition = value, _position, _duration);
                eulerAnglesTween = DOTween.To(() => _targetRectTransform.localEulerAngles, value => _targetRectTransform.localEulerAngles = value, _eulerAngles, _duration);
            }
            else
            {
                _originalPosition = _targetRectTransform.position;
                _originalEulerAngles = _targetRectTransform.eulerAngles;
                
                positionTween = DOTween.To(() => _targetRectTransform.position, value => _targetRectTransform.position = value, _position, _duration);
                eulerAnglesTween = DOTween.To(() => _targetRectTransform.eulerAngles, value => _targetRectTransform.eulerAngles = value, _eulerAngles, _duration);
            }

            _originalScale = _targetRectTransform.localScale;
            _originalAnchorMin = _targetRectTransform.anchorMin;
            _originalAnchorMax = _targetRectTransform.anchorMax;
            _originalAnchoredPosition = _targetRectTransform.anchoredPosition;
            _originalSizeDelta = _targetRectTransform.sizeDelta;
            _originalPivot = _targetRectTransform.pivot;
            
            var scaleTween = DOTween.To(() => _targetRectTransform.localScale, value => _targetRectTransform.localScale = value, _scale, _duration);
            var anchorMinTween = DOTween.To(() => _targetRectTransform.anchorMin, value => _targetRectTransform.anchorMin = value, _anchorMin, _duration);
            var anchorMaxTween = DOTween.To(() => _targetRectTransform.anchorMax, value => _targetRectTransform.anchorMax = value, _anchorMax, _duration);
            var anchoredPositionTween = DOTween.To(() => _targetRectTransform.anchoredPosition, value => _targetRectTransform.anchoredPosition = value, _anchoredPosition, _duration);
            var sizeDeltaTween = DOTween.To(() => _targetRectTransform.sizeDelta, value => _targetRectTransform.sizeDelta = value, _sizeDelta, _duration);
            var pivotTween = DOTween.To(() => _targetRectTransform.pivot, value => _targetRectTransform.pivot = value, _pivot, _duration);
            
            behaviourSequence.Join(positionTween);
            behaviourSequence.Join(eulerAnglesTween);
            behaviourSequence.Join(scaleTween);
            behaviourSequence.Join(anchorMinTween);
            behaviourSequence.Join(anchorMaxTween);
            behaviourSequence.Join(anchoredPositionTween);
            behaviourSequence.Join(sizeDeltaTween);
            behaviourSequence.Join(pivotTween);
            
            if (FlowType == FlowType.Join)
                animationSequence.Join(behaviourSequence);
            else
                animationSequence.Append(behaviourSequence);
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