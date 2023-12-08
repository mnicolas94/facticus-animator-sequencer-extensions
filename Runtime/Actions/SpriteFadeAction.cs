using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class SpriteFadeAction : DOTweenActionBase
    {
        public override Type TargetComponentType => typeof(SpriteRenderer);

        public override string DisplayName => "Fade sprite renderer";

        [SerializeField] private float _alpha;

        private SpriteRenderer _target;
        private float _previousValue;
        
        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            if (_target == null)
            {
                if (!target.TryGetComponent(out _target))
                {
                    return null;
                }
            }

            _previousValue = _target.color.a;
            var tween = DOTween.To(
                () => _target.color.a,
                value =>
                    {
                        var color = _target.color;
                        color.a = value;
                        _target.color = color;
                    },
                _alpha,
                duration);
            return tween;
        }

        public override void ResetToInitialState()
        {
            if (_target != null)
            {
                var color = _target.color;
                color.a = _previousValue;
                _target.color = color;
            }
        }
    }
}