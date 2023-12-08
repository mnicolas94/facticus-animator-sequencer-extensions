using System;
using System.Linq;
using AnimatorSequencerExtensions.Steps;
using BrunoMikoski.AnimationSequencer;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerExtensions.Editor
{
    [CustomPropertyDrawer(typeof(RectTransformInterpolationStep))]
    public class RectTransformInterpolationStepDrawer : AnimationStepBasePropertyDrawer
    {
        private readonly (string, string)[] _customDrawers =
        {
            ("_tweenPosition", "_position"),
            ("_tweenEulerAngles", "_eulerAngles"),
            ("_tweenScale", "_scale"),
            ("_tweenAnchorMin", "_anchorMin"),
            ("_tweenAnchorMax", "_anchorMax"),
            ("_tweenAnchoredPosition", "_anchoredPosition"),
            ("_tweenSizeDelta", "_sizeDelta"),
            ("_tweenPivot", "_pivot"),
        };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var customProperties = _customDrawers.SelectMany(tuple =>
            {
                return new []{tuple.Item1, tuple.Item2};
            }).ToArray();
            
            DrawBaseGUI(position, property, label, customProperties);

            float originHeight = position.y;
            if (property.isExpanded)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.indentLevel++;
                position = EditorGUI.IndentedRect(position);
                EditorGUI.indentLevel--;

                // draw RectTransform properties
                position.y += base.GetPropertyHeight(property, label);
                foreach (var (shouldDrawPropertyName, propertyName) in _customDrawers)
                {
                    var shouldDrawProperty = property.FindPropertyRelative(shouldDrawPropertyName);
                    var childProperty = property.FindPropertyRelative(propertyName);
                    
                    // construct rects
                    var shouldRect = new Rect(position)
                    {
                        width = EditorGUIUtility.singleLineHeight  // a toggle is a square, so height is fine here
                    };
                    var childRect = new Rect(position)
                    {
                        xMin = shouldRect.xMax
                    };
                    
                    EditorGUI.PropertyField(shouldRect, shouldDrawProperty, GUIContent.none);
                    var shouldDraw = shouldDrawProperty.boolValue;
                    var guiEnabled = GUI.enabled;
                    GUI.enabled = shouldDraw;
                    EditorGUI.PropertyField(childRect, childProperty);
                    GUI.enabled = guiEnabled;
                    
                    position.y += shouldRect.height + EditorGUIUtility.standardVerticalSpacing;
                }
                
                // buttons
                var mirrorRect = new Rect(position);
                mirrorRect.xMin = position.xMax - 100;
                var mirrorGuiContent = new GUIContent("Mirror Values", "Set current values from target RectTransform");
                if (GUI.Button(mirrorRect, mirrorGuiContent))
                {
                    var target = (RectTransformInterpolationStep) property.managedReferenceValue;
                    target.MirrorCurrentValues();
                }
                
                var setValuesRect = new Rect(position);
                setValuesRect.xMin = position.xMax - 200;
                setValuesRect.xMax = mirrorRect.xMin;
                var setValuesGuiContent = new GUIContent("Set Values", "Set current values to target RectTransform");
                if (GUI.Button(setValuesRect, setValuesGuiContent))
                {
                    var target = (RectTransformInterpolationStep) property.managedReferenceValue;
                    target.SetCurrentValues();
                }
                position.y += setValuesRect.height;
                
                if (EditorGUI.EndChangeCheck())
                    property.serializedObject.ApplyModifiedProperties();
            }
            
            property.SetPropertyDrawerHeight(position.y - originHeight + EditorGUIUtility.singleLineHeight);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propertyHeight = base.GetPropertyHeight(property, label);
            return propertyHeight;
        }
    }
}