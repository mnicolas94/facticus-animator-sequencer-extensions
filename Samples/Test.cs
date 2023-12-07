using System;
using SerializableCallback;
using UnityEngine;

namespace AnimatorSequencerExtensions.Samples
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private SerializableValueCallback<float> _from;
        [SerializeField] private SerializableValueCallback<float> _to;
        [SerializeField] private Nested _nested;
    }

    [Serializable]
    public class Nested
    {
        [SerializeField] private SerializableValueCallback<float> _nestedValue;
    }
}