#if ENABLED_SPLINES

using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class SplineAnimateAction : DOTweenActionBase
    {
        [SerializeField] private SplineAnimate _splineAnimate;
        [SerializeField] private bool _reversed;

        public override string DisplayName => "SplineAnimateAction";
        
        public override Type TargetComponentType => typeof(SplineAnimate);
        
        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            var startValue = _reversed ? 1 : 0;
            var endValue = _reversed ? 0 : 1;

            _splineAnimate.NormalizedTime = startValue;
            
            var sequence = DOTween.To(
                () => _splineAnimate.NormalizedTime,
                val =>
                {
                    _splineAnimate.NormalizedTime = val;
                },
                endValue,
                duration);

            return sequence;
        }

        public override void ResetToInitialState()
        {
            _splineAnimate.NormalizedTime = _reversed ? 1 : 0;
        }
    }
}

#endif