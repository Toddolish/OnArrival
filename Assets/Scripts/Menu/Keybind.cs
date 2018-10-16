using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keybind : MonoBehaviour
{
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
	private GameObject currentKey;

	public Sprite selected, normal;
	public Text up, left, down, right, jump, crouch, dash, interact;
	void Start()
	{
		keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
		keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
		keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
		keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
		keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
		keys.Add("Crouch", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "C")));
		keys.Add("Dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash", "E")));
		keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "F")));

		up.text = keys["Up"].ToString();
		down.text = keys["Down"].ToString();
		left.text = keys["Left"].ToString();
		right.text = keys["Right"].ToString();
		jump.text = keys["Jump"].ToString();
		crouch.text = keys["Crouch"].ToString();
		dash.text = keys["Dash"].ToString();
		interact.text = keys["Interact"].ToString();
	}
	private void OnGUI()
	{
		if (currentKey != null)
		{
			Event e = Event.current;
			if (e.isKey)
			{
				keys[currentKey.name] = e.keyCode;
				currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
				currentKey.GetComponent<Image>().sprite = normal;
				currentKey = null;
			} 
		}
	}
	public void ChangeKey(GameObject clicked)
	{
		FindObjectOfType<AudioManager>().Play("Click2");
		if (currentKey != null)
		{
			currentKey.GetComponent<Image>().sprite = normal;
		}
		currentKey = clicked;
		currentKey.GetComponent<Image>().sprite = selected;
	}

	public void SaveKeys()
	{
		FindObjectOfType<AudioManager>().Play("Pick");
		foreach (var key in keys)
		{
			PlayerPrefs.SetString(key.Key, key.Value.ToString());
		}
		PlayerPrefs.Save();
	}
}
