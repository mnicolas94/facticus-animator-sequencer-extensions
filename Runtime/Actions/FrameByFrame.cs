using System;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class FrameByFrame : DOTweenActionBase
    {
        public override Type TargetComponentType => typeof(SpriteRenderer);

        public override string DisplayName => "Frame-by-frame animation";

        [SerializeField] private List<Sprite> _sprites;

        private SpriteRenderer _target;
        private Sprite _previousValue;
        
        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            if (_target == null)
            {
                if (!target.TryGetComponent(out _target))
                {
                    return null;
                }
            }

            _previousValue = _target.sprite;
            var tween = DOTween.To(
                () => 0f,
                value =>
                    {
                        int index = (int)(_sprites.Count * value);
                        index = Mathf.Clamp(index, 0, _sprites.Count - 1);
                        var sprite = _sprites[index];
                        _target.sprite = sprite;
                    },
                1f,
                duration);
            return tween;
        }

        public override void ResetToInitialState()
        {
            if (_target != null)
            {
                _target.sprite = _previousValue;
            }
        }
    }
}