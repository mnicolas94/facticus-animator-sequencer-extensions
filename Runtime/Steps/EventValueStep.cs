using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnimatorSequencerExtensions.Steps
{
    [Serializable]
    public class EventValueStep : AnimationStepBase
    {
        public override string DisplayName => "Event Animated Value";

        [SerializeField] private float _duration;
        [SerializeField] private bool _invert;
        [SerializeField] private CustomEase ease = CustomEase.InOutCirc;
        [SerializeField] private UnityEvent<float> _valueEvent;
        // todo add from -> to
        
        private float Timer { get; set; }

        private float Getter()
        {
            return Timer;
        }

        private void Setter(float value)
        {
            Timer = value;
            _valueEvent.Invoke(value);
        }

        public override void AddTweenToSequence(Sequence animationSequence)
        {
            animationSequence.AppendInterval(Delay);
            
            Timer = _invert ? 1 : 0;
            var endValue = _invert ? 0 : 1;
            var sequence = DOTween.To(Getter, Setter, endValue, _duration);
            
            sequence.SetEase(ease);
            if (FlowType == FlowType.Join)
                animationSequence.Join(sequence);
            else
                animationSequence.Append(sequence);
        }

        public override void ResetToInitialState()
        {
            Setter(_invert ? 1 : 0);
        }
    }
}