﻿using Master;
using System;
using UnityEngine;

public class RoundStart : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[Header("ApHpUp_Rootのスキナー")]
	[SerializeField]
	protected UIComponentSkinner apHpUpRootSkinner;

	[SerializeField]
	[Header("ラウンドのローカライズ")]
	protected UILabel roundLocalize;

	[SerializeField]
	[Header("AP UPローカライズ(片方の時)")]
	protected UILabel onlyApUpLocalize;

	[Header("AP UPローカライズ(両方)")]
	[SerializeField]
	protected UILabel apUpLocalize;

	[SerializeField]
	[Header("HP回復ローカライズ(片方の時)")]
	protected UILabel onlyHpRecoverLocalize;

	[Header("HP回復ローカライズ(両方)")]
	[SerializeField]
	protected UILabel hpRecoverLocalize;

	private void Awake()
	{
		this.SetupLocalize();
	}

	protected void SetupLocalize()
	{
		this.onlyApUpLocalize.text = StringMaster.GetString("BattleNotice-03");
		this.apUpLocalize.text = StringMaster.GetString("BattleNotice-03");
		this.onlyHpRecoverLocalize.text = StringMaster.GetString("BattleNotice-04");
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
