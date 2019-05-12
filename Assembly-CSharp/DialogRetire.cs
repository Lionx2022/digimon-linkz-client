﻿using Master;
using System;
using UnityEngine;

public class DialogRetire : MonoBehaviour
{
	[SerializeField]
	[Header("Yesボタン")]
	private UIButton yesButton;

	[SerializeField]
	[Header("Noボタン")]
	private UIButton noButton;

	[Header("開く閉じるダイアログ")]
	[SerializeField]
	public UIOpenCloseDialog openCloseDialog;

	[Header("はいローカライズ")]
	[SerializeField]
	private UILabel yesLocalize;

	[Header("いいえローカライズ")]
	[SerializeField]
	private UILabel noLocalize;

	[SerializeField]
	[Header("リタイア/降参ローカライズ")]
	private UILabel retireLocalize;

	[SerializeField]
	[Header("リタイアメッセージローカライズ")]
	private UILabel retireMessageLocalize;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.yesLocalize.text = StringMaster.GetString("SystemButtonYes");
		this.noLocalize.text = StringMaster.GetString("SystemButtonNo");
		if (BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			this.retireLocalize.text = StringMaster.GetString("BattleUI-35");
			this.retireMessageLocalize.text = StringMaster.GetString("BattleUI-20");
		}
		else
		{
			this.retireLocalize.text = StringMaster.GetString("BattleUI-05");
			this.retireMessageLocalize.text = StringMaster.GetString("BattleUI-06");
		}
	}

	public void AddEvent(Action retireOkAction, Action retireCancelAction)
	{
		BattleInputUtility.AddEvent(this.yesButton.onClick, retireOkAction);
		BattleInputUtility.AddEvent(this.noButton.onClick, retireCancelAction);
	}
}