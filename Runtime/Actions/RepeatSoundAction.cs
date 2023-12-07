using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;
using SerializableCallback;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class RepeatSoundAction : DOTweenActionBase
    {
        [SerializeField] private SerializableValueCallback<int> _timesToPlayCallback;
        [SerializeField] private float _repeatPeriod;
        
        public override Type TargetComponentType => typeof(AudioSource);
        public override string DisplayName => "Repeat sound";

        private AudioSource _audioSource;

        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            if (_audioSource == null)
            {
                _audioSource = target.GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    Debug.LogError($"{target} does not have {TargetComponentType} component");
                    return null;
                }
            }

            var timesToPlay = _timesToPlayCallback.Value + 1;  // at least 1, avoid 0, play early
            var periodByTimesToPlay = duration / timesToPlay;
            var period = Mathf.Max(periodByTimesToPlay, _repeatPeriod);
            var lastTimePlayed = 0f;
            var tween = DOTween.To(
                () => lastTimePlayed,
                val =>
                {
                    var elapsed = val - lastTimePlayed;
                    if (elapsed >= period)
                    {
                        _audioSource.Play();
                        lastTimePlayed = val;
                    }
                },
                duration,
                duration);
            return tween;
        }

        public override void ResetToInitialState()
        {
        }
    }
}