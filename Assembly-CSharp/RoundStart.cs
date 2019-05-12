﻿using Master;
using System;
using UnityEngine;

public class RoundStart : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[SerializeField]
	[Header("ApHpUp_Rootのスキナー")]
	private UIComponentSkinner apHpUpRootSkinner;

	[Header("ラウンドのローカライズ")]
	[SerializeField]
	private UILabel roundLocalize;

	[Header("AP UPローカライズ(片方の時)")]
	[SerializeField]
	private UILabel onlyApUpLocalize;

	[Header("AP UPローカライズ(両方)")]
	[SerializeField]
	private UILabel apUpLocalize;

	[Header("HP回復ローカライズ")]
	[SerializeField]
	private UILabel hpRecoverLocalize;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.onlyApUpLocalize.text = StringMaster.GetString("BattleNotice-03");
		this.apUpLocalize.text = StringMaster.GetString("BattleNotice-03");
		this.hpRecoverLocalize.text = StringMaster.GetString("BattleNotice-04");
	}

	public void ApplyRoundStartRevivalText(bool onRevivalAp, bool onRevivalHp)
	{
		if (onRevivalAp)
		{
			if (onRevivalHp)
			{
				this.apHpUpRootSkinner.SetSkins(2);
			}
			else
			{
				this.apHpUpRootSkinner.SetSkins(0);
			}
		}
		else if (onRevivalHp)
		{
			this.apHpUpRootSkinner.SetSkins(1);
		}
		else
		{
			this.apHpUpRootSkinner.SetSkins(3);
		}
	}

	public void ApplyWaveAndRound(int round)
	{
		string text = string.Format(StringMaster.GetString("BattleUI-36"), round);
		this.roundLocalize.text = text;
	}
}