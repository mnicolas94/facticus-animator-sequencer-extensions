using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class RectTransformPivotAction : DOTweenActionBase
    {
        public override Type TargetComponentType => typeof(RectTransform);
        
        public override string DisplayName => "RectTransform Pivot";
                                   
        [SerializeField] private Vector2 _pivot;
        
        public Vector2 Pivot
        {
            get => _pivot;
            set => _pivot = value;
        }
                                   
        [SerializeField]
        private AxisConstraint axisConstraint;
        public AxisConstraint AxisConstraint
        {
            get => axisConstraint;
            set => axisConstraint = value;
        }
                                   
        private RectTransform _previousTarget;
        private Vector2 _previousPivot;
                                   
        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            _previousTarget = target.transform as RectTransform;
            _previousPivot = _previousTarget.pivot;
            var tween = _previousTarget.DOPivot(_pivot, duration);
            tween.SetOptions(axisConstraint);
                                   
            return tween;
        }
                                   
        public override void ResetToInitialState()
        {
            if (_previousTarget == null)
                return;
                                   
            _previousTarget.pivot = _previousPivot;
        }
    }
}