using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickUiHeader : QuickUiSceneControl
{
	public string Text = "";

	public void Initialize(string text)
	{
		Text = text;

		var headerText = transform.Find("Label").GetComponent<Text>();
		headerText.text = Text;

		gameObject.name = "----" + text + "----";
	}
}