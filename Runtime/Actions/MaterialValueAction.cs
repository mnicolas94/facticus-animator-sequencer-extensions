using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class MaterialValueAction : DOTweenActionBase
    {
        public override string DisplayName => "Set Material variable value";

        public override Type TargetComponentType => typeof(Renderer);

        [SerializeField] private float _value;
        [SerializeField] private string _variableName;
        
        private float? _previousState;
        private Material _previousTarget;

        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            var sr = target.GetComponent<Renderer>();
            var material = sr.material;
            
            _previousTarget = material;
            _previousState = material.GetFloat(_variableName);

            var tweener = DOTween.To(
                () => material.GetFloat(_variableName),
                value => material.SetFloat(_variableName, value),
                _value,
                duration
            );

            return tweener;
        }

        public override void ResetToInitialState()
        {
            if (!_previousState.HasValue)
                return;

            _previousTarget.SetFloat(_variableName, _previousState.Value);
        }
    }
}