using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Steps
{
    [Serializable]
    public class PlayAudio : AnimationStepBase
    {
        public override string DisplayName => "Play audio";
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private bool _timeScaleIndependent;

        public override void AddTweenToSequence(Sequence animationSequence)
        {
            animationSequence.AppendInterval(Delay);

            var sequence = DOTween.Sequence();
            
            sequence.AppendCallback(() =>
            {
                _audioSource.Stop();
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            });
            var tween = DOTween.To(
                () => 0f,
                _ =>
                {
                    if (_timeScaleIndependent) _audioSource.pitch = Time.timeScale * animationSequence.timeScale;
                },
                1f,
                _audioClip.length
            );
            sequence.Append(tween);
            sequence.AppendCallback(() =>
            {
                _audioSource.Stop();
            });
            
            if (FlowType == FlowType.Join)
                animationSequence.Join(sequence);
            else
                animationSequence.Append(sequence);
        }

        public override void ResetToInitialState()
        {
            
        }
    }
}