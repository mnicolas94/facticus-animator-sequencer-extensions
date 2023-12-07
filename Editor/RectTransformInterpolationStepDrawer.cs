using AnimatorSequencerExtensions.Steps;
using BrunoMikoski.AnimationSequencer;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerExtensions.Editor
{
    [CustomPropertyDrawer(typeof(RectTransformInterpolationStep))]
    public class RectTransformInterpolationStepDrawer : AnimationStepBasePropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            var buttonRect = new Rect(position);
            buttonRect.xMin = position.xMax - 100;
            if (GUI.Button(buttonRect, "Mirror target"))
            {
                var target = (RectTransformInterpolationStep) property.managedReferenceValue;
                target.MirrorCurrentValues();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propertyHeight = base.GetPropertyHeight(property, label);

            Debug.Log(propertyHeight);
            
            return propertyHeight + EditorStyles.miniButton.fixedHeight;
        }
    }
}