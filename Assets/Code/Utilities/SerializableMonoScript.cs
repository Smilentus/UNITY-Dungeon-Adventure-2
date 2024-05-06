using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dimasyechka.Code.Utilities
{
    [System.Serializable]
    public class SerializableMonoScript
    {
        System.Type m_Type = null;
        [SerializeField]
        string m_TypeName = null;
        public System.Type Type
        {
            get
            {
                if (m_Type == null)
                {
                    if (string.IsNullOrEmpty(m_TypeName))
                        return null;
                    m_Type = System.Type.GetType(m_TypeName);
                }
                return m_Type;
            }
            set
            {
                m_Type = value;
                if (m_Type == null)
                    m_TypeName = "";
                else
                    m_TypeName = m_Type.AssemblyQualifiedName;
            }
        }
    }
    [System.Serializable]
    public class SerializableMonoScript<T> : SerializableMonoScript where T : class
    {
        public T CreateSOInstance()
        {
            var type = Type;
            if (type != null)
                return (T)(object)ScriptableObject.CreateInstance(type);
            return default(T);
        }
        public T AddToGameObject(GameObject aGO)
        {
            var type = Type;
            if (type != null)
                return (T)(object)aGO.AddComponent(type);
            return default(T);
        }
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializableMonoScript), true)]
    public class SerializableTypePropertyDrawer : PropertyDrawer
    {
        private System.Type m_FilterType = null;
        private static Dictionary<System.Type, MonoScript> m_MonoScriptCache = new Dictionary<System.Type, MonoScript>();
        private static MonoScript GetMonoScript(System.Type aType)
        {
            if (aType == null)
                return null;
            if (m_MonoScriptCache.TryGetValue(aType, out MonoScript script) && script != null)
            {
                return script;
            }
            var scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            foreach (var s in scripts)
            {
                var type = s.GetClass();
                if (type != null)
                    m_MonoScriptCache[type] = s;
                if (type == aType)
                    script = s;
            }
            return script;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_FilterType == null)
            {
                if (fieldInfo.FieldType.IsGenericType)
                {
                    var types = fieldInfo.FieldType.GetGenericArguments();
                    if (types != null && types.Length == 1)
                        m_FilterType = types[0];
                }
                else
                    m_FilterType = typeof(UnityEngine.Object);
            }
            var typeName = property.FindPropertyRelative("m_TypeName");
            System.Type type = System.Type.GetType(typeName.stringValue);
            MonoScript monoScript = GetMonoScript(type);
            EditorGUI.BeginChangeCheck();
            monoScript = (MonoScript)EditorGUI.ObjectField(position, label, monoScript, typeof(MonoScript), true);
            if (EditorGUI.EndChangeCheck())
            {
                if (monoScript == null)
                    typeName.stringValue = "";
                else
                {
                    var newType = monoScript.GetClass();
                    if (newType != null && m_FilterType.IsAssignableFrom(newType))
                        typeName.stringValue = newType.AssemblyQualifiedName;
                    else
                        Debug.LogWarning("Dropped type does not derive or implement " + m_FilterType.Name);
                }
            }
        }
    }

#endif
}