using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Commands : MonoBehaviour
{
	private void Update()
	{
		this.PredictCommands();
		this.PlayerInput();
		this.suggestText.text = this.suggestedText;
	}

	private void PlayerInput()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.FillCommand();
		}
	}

	private void FillCommand()
	{
		if (!ChatBox.Instance.typing)
		{
			return;
		}
		if (this.suggestedText == "")
		{
			return;
		}
		this.inputField.text = this.suggestedText;
		this.inputField.stringPosition = this.inputField.text.Length;
	}

	private void PredictCommands()
	{
		if (!ChatBox.Instance.typing)
		{
			if (this.suggestText.text != "")
			{
				this.suggestedText = "";
				this.suggestText.text = "";
			}
			return;
		}
		this.suggestedText = "";
		string text = this.inputField.text;
		if (text.Length < 1)
		{
			return;
		}
		string text2 = text.Split(new char[]
		{
			' '
		}).Last<string>();
		if (text2.Length < 1)
		{
			return;
		}
		string a = text2[0].ToString() ?? "";
		string text3 = text2.Remove(0, 1);
		if (a == "/")
		{
			foreach (string text4 in ChatBox.Instance.commands)
			{
				if (text4.StartsWith(text3))
				{
					this.suggestedText = text;
					int num = text4.Length - text3.Length;
					this.suggestedText += text4.Substring(text4.Length - num);
					return;
				}
			}
		}
		string[] array = text.Split(Array.Empty<char>());
		if (array.Length < 2)
		{
			return;
		}
		int startIndex = text.IndexOf(" ", StringComparison.Ordinal) + 1;
		string text5 = text.Substring(startIndex);
		if (array[0] == "/kick")
		{
			foreach (Client client in Server.clients.Values)
			{
				if (client != null && client.player != null && client.player.username.ToLower().Contains(text5.ToLower()))
				{
					this.suggestedText = array[0] + " ";
					this.suggestedText += client.player.username;
					break;
				}
			}
		}
	}

	public TMP_InputField inputField;

	public TextMeshProUGUI suggestText;

	private string suggestedText;
}
