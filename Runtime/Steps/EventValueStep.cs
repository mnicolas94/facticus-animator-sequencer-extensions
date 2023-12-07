using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using SerializableCallback;
using UnityEngine;
using UnityEngine.Events;

namespace AnimatorSequencerExtensions.Steps
{
    [Serializable]
    public class EventValueStep : AnimationStepBase
    {
        public override string DisplayName => "Event Animated Value";

        [SerializeField] private SerializableValueCallback<float> _from = new SerializableValueCallback<float>
        {
            Value = 0
        };
        [SerializeField] private SerializableValueCallback<float> _to = new SerializableValueCallback<float>
        {
            Value = 1
        };
        [SerializeField] private float _duration;
        [SerializeField] private bool _invert;
        [SerializeField] private CustomEase ease = CustomEase.InOutCirc;
        [SerializeField] private UnityEvent<float> _valueEvent;


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
            
            Timer = _invert ? _to.Value : _from.Value;
            var endValue = _invert ? _from.Value : _to.Value;
            var sequence = DOTween.To(Getter, Setter, endValue, _duration);
            
            sequence.SetEase(ease);
            if (FlowType == FlowType.Join)
                animationSequence.Join(sequence);
            else
                animationSequence.Append(sequence);
        }

        public override void ResetToInitialState()
        {
            Setter(_invert ? _to.Value : _from.Value);
        }
    }
}