using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

namespace Lunity
{
    public class WorldSpaceInspector : MonoBehaviour
    {

        public GameObject TargetObject;

        public Component TargetComponent;

        public int TargetComponentIndex;

        public GameObject TemplateText;

        private FieldInfo[] _fields;
        private PropertyInfo[] _properties;

        private List<Text> _fieldTexts;
        private List<Text> _propertyTexts;
        private List<Text> _extraTexts;

        private bool _initialized;
        
        private const BindingFlags OnlyLookup = BindingFlags.Public | 
                                                BindingFlags.NonPublic | 
                                                BindingFlags.Instance | 
                                                BindingFlags.Static | 
                                                BindingFlags.DeclaredOnly;
        
        void Update()
        {
            if (TargetObject == null)
                return;
            
            if (!_initialized) {
                Initialize();
            }

            for (var i = 0; i < _fields.Length; i++) {
                _fieldTexts[i].text = _fieldTexts[i].name + ": " + _fields[i].GetValue(TargetComponent);
            }

            for (var i = 0; i < _properties.Length; i++) {
                _propertyTexts[i].text = _propertyTexts[i].name + ": " + _properties[i].GetValue(TargetComponent);
            }
        }

        public void Initialize()
        {
            TemplateText.SetActive(true);
            if (TemplateText == null) {
                TemplateText = transform.Find("Group/TemplateText").gameObject;
            }

            try {
                TargetComponent = TargetObject.GetComponents<Component>()[TargetComponentIndex];
            } catch (System.Exception) {
                return;
            }

            _fields = TargetComponent.GetType().GetFields(OnlyLookup);
            _properties = TargetComponent.GetType().GetProperties(OnlyLookup);

            if (_fieldTexts == null) _fieldTexts = new List<Text>();
            foreach (var text in _fieldTexts) Destroy(text.gameObject);
            _fieldTexts.Clear();

            if (_propertyTexts == null) _propertyTexts = new List<Text>();
            foreach (var text in _propertyTexts) Destroy(text.gameObject);
            _propertyTexts.Clear();
            
            if (_extraTexts == null) _extraTexts = new List<Text>();
            foreach (var text in _extraTexts) Destroy(text.gameObject);
            _extraTexts.Clear();

            if (_fields.Length > 0) {
                _extraTexts.Add(SpawnText("FieldTitle", "--------Fields--------", true));

                foreach (var field in _fields) {
                    _fieldTexts.Add(SpawnText(field.Name));
                }
                
                _extraTexts.Add(SpawnText("Space"));
            }

            if (_properties.Length > 0) {
                _extraTexts.Add(SpawnText("PropertyTitle", "------Properties------", true));

                foreach (var property in _properties) {
                    _propertyTexts.Add(SpawnText(property.Name));
                }
            }

            TemplateText.SetActive(false);

            _initialized = true;
        }

        private Text SpawnText(string name = "Text", string value = "", bool bold = false)
        {
            var newObj = Instantiate(TemplateText, TemplateText.transform.parent, true) as GameObject;
            newObj.transform.localPosition = Vector3.zero;
            newObj.name = name;
            var text = newObj.GetComponent<Text>();
            text.text = value;
            if (bold) text.fontStyle = FontStyle.Bold;
            return newObj.GetComponent<Text>();
        }

        public bool GetComponentEnum(out GUIContent[] names, out int[] indices)
        {
            try {
                var componentList = TargetObject.GetComponents<Component>();
                names = new GUIContent[componentList.Length];
                indices = new int[componentList.Length];
                for (var i = 0; i < componentList.Length; i++) {
                    names[i] = new GUIContent(componentList[i].GetType().Name);
                    indices[i] = i;
                }
            } catch (System.Exception e) {
                Debug.LogError("Failed for some reason: " + e);
                names = new [] {new GUIContent("Error")};
                indices = new [] {-1};
                return false;
            }

            return true;
        }
    }
}