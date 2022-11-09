using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{

    [RequireComponent(typeof(LayoutGroup))]
    public class SelectableList : MonoBehaviour
    {
        public class SelectableItemData
        {
            public string Label;
            public object Data;

            public SelectableItemData(string label, object data)
            {
                Label = label;
                Data = data;
            }
        }

        private class SelectableItem
        {
            public SelectableItemData Data;
            public Button Button;

            public SelectableItem(SelectableItemData data, Button button)
            {
                Data = data;
                Button = button;
            }
        }


        public EventHandler<object> OnItemSelected;
        public SelectableItemData CurrentSelection;
        public int ItemCount => Items?.Count ?? 0;

        private RectTransform _itemTemplate;
        private List<SelectableItem> Items;
        private bool _initialized;

        public void Awake()
        {
            if (!_initialized) Initialize();
        }

        public void Initialize()
        {
            if (_initialized) return;

            Items = new List<SelectableItem>();

            _itemTemplate = transform.Find("Template").GetComponent<RectTransform>();
            _itemTemplate.gameObject.SetActive(false);

            _initialized = true;
        }

        public void AddItem(string label, object data)
        {
            if (!_initialized) Initialize();

            if (Items.Any((item => item.Data == data))) {
                Debug.LogError("Items list already contains " + data);
                return;
            }

            var newItemData = new SelectableItemData(label, data);
            var newObj = Instantiate(_itemTemplate, transform, true).gameObject;
            newObj.gameObject.SetActive(true);
            newObj.gameObject.name = label;
            var button = newObj.GetComponent<Button>();
            if (button == null) button = newObj.GetComponentInChildren<Button>();
            button.transform.GetChild(0).GetComponent<Text>().text = label;
            Items.Add(new SelectableItem(newItemData, button));
            var index = Items.Count - 1;
            button.onClick.AddListener(() => HandleSelected(index));
        }

        public void ClearAllItems()
        {
            if (!_initialized) Initialize();

            foreach (var item in Items) {
                Destroy(item.Button.gameObject);
            }

            Items.Clear();
        }

        public void DeselectAll()
        {
            if (!_initialized) Initialize();

            foreach (var item in Items) item.Button.interactable = true;

            CurrentSelection = null;
        }

        private void HandleSelected(int index)
        {
            if (!_initialized) Initialize();

            if (index < 0 || index >= Items.Count) {
                Debug.LogError($"Index of {index} doesn't fit in buttons list with count {Items.Count}!");
                return;
            }

            DeselectAll();
            Items[index].Button.interactable = false;
            CurrentSelection = Items[index].Data;
            OnItemSelected?.Invoke(Items[index].Button, Items[index].Data);
        }

    }

}