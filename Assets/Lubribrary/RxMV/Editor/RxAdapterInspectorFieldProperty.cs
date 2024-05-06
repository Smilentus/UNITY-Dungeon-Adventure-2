using System.Linq;
using Dimasyechka.Lubribrary.RxMV.Utilities;
using UnityEditor;
using UnityEngine;

namespace Dimasyechka.Lubribrary.RxMP.Editor
{
    [CustomPropertyDrawer(typeof(UniRxReflectionField))]
    public class RxAdapterInspectorFieldProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var names = property.FindPropertyRelative("ReflectiveNames");
            string[] list = new string[names.arraySize + 1];
            list[0] = "None";
            for (int i = 0; i < names.arraySize; i++)
            {
                list[i + 1] = names.GetArrayElementAtIndex(i).stringValue;
            }

            var selectIndex = property.FindPropertyRelative("SelectedIndex");
            var selectName = property.FindPropertyRelative("SelectedName");

            int selectIndexValue = selectIndex.intValue;

            selectIndexValue += 1;

            if (list.Length > selectIndexValue && !string.IsNullOrEmpty(selectName.stringValue))
            {
                if (list[selectIndexValue] != selectName.stringValue)
                {
                    selectIndexValue = list.ToList().FindIndex(x => x == selectName.stringValue);
                }
            }

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            EditorGUI.BeginChangeCheck();
            selectIndexValue = EditorGUI.Popup(position, property.displayName, EditorGUI.showMixedValue ? -1 : selectIndexValue, list);
            if (EditorGUI.EndChangeCheck())
            {
                if (selectIndexValue < 0)
                    selectIndexValue = 0;

                if (selectIndexValue < list.Length)
                    selectName.stringValue = list[selectIndexValue];

                selectIndexValue -= 1;

                selectIndex.intValue = selectIndexValue;
                EditorGUI.showMixedValue = false;
            }
        }
    }
}
