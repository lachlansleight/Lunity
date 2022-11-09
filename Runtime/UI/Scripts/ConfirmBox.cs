using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lunity
{
    public class ConfirmBox : MonoBehaviour
    {
        /// Creates a new confirm box on top of everything else in the parent RectTransform and provides a callback for the Confirm and Cancel buttons
        public static void ConfirmWithCallback(RectTransform parent, Action<bool> onResponse, string title, string message, string yes = "Confirm", string no = "Cancel")
        {
            var newObj = Instantiate(Resources.Load<GameObject>("ConfirmBox")).GetComponent<ConfirmBox>();
            newObj.Initialize(parent, title, message, yes, no, onResponse);
        }

        public void Initialize(RectTransform parent, string title, string message, string yes, string no, Action<bool> onResponse)
        {
            //make child of parent and ensure it covers the entire rect
            var rt = (RectTransform) transform;
            rt.SetParentNeutral(parent);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            transform.SetAsLastSibling();
            
            //populate text boxes
            transform.Find("Panel/Title").GetComponent<Text>().text = title;
            transform.Find("Panel/Message").GetComponent<Text>().text = message;
            transform.Find("Panel/Buttons/Cancel/Text").GetComponent<Text>().text = no;
            transform.Find("Panel/Buttons/Confirm/Text").GetComponent<Text>().text = yes;
            
            //assign callbacks
            transform.Find("Panel/Buttons/Cancel").GetComponent<Button>().onClick.AddListener(() => {
                onResponse?.Invoke(false);
                Destroy(gameObject);
            });
            transform.Find("Panel/Buttons/Confirm").GetComponent<Button>().onClick.AddListener(() => {
                onResponse?.Invoke(true);
                Destroy(gameObject);
            });
        }
    }
}