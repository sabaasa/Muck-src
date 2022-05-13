using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPrefab : MonoBehaviour
{
	public void SetStat(KeyValuePair<string, int> s)
	{
		this.statName.text = s.Key + " |";
		this.statValue.text = string.Concat(s.Value);
	}

	public TextMeshProUGUI statName;

	public TextMeshProUGUI statValue;
}
