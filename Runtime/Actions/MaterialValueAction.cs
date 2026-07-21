using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using SerializableCallback;
using UnityEngine;

namespace AnimatorSequencerExtensions.Actions
{
    [Serializable]
    public class MaterialValueAction : DOTweenActionBase
    {
        public override string DisplayName => "Set Material variable value";

        public override Type TargetComponentType => typeof(Renderer);

        enum MaterialType { Shared, Instance}
        [SerializeField] private MaterialType _materialType = MaterialType.Shared;
        [SerializeField] private SerializableValueCallback<float> _value;
        [SerializeField] private string _variableName;
        
        private float? _previousState;
        private Material _previousTarget;

        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            var sr = target.GetComponent<Renderer>();
#if UNITY_EDITOR
            var material = sr.sharedMaterial;
#else
            var material = _materialType == MaterialType.Shared ? sr.sharedMaterial : sr.material;
#endif
            
            _previousTarget = material;
            _previousState = material.GetFloat(_variableName);

            var tweener = DOTween.To(
                () => material.GetFloat(_variableName),
                value => material.SetFloat(_variableName, value),
                _value.Value,
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