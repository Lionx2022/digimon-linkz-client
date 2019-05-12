﻿using FarmData;
using Master;
using System;
using UnityEngine;

public sealed class CMD_UpperLimit : CMD
{
	[SerializeField]
	private UILabel message;

	[SerializeField]
	private UILabel reinforcementButtonLabel;

	[SerializeField]
	private UILabel houseButtonLabel;

	[SerializeField]
	private UILabel gardenButtonLabel;

	protected override void Awake()
	{
		base.Awake();
		this.reinforcementButtonLabel.text = StringMaster.GetString("ReinforcementTitle");
		this.houseButtonLabel.text = StringMaster.GetString("HouseTitle");
		this.gardenButtonLabel.text = StringMaster.GetString("GardenTitle");
	}

	public void SetType(CMD_UpperLimit.MessageType type)
	{
		int facilityID = 7;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityID);
		bool flag = false;
		for (int i = 0; i < Singleton<UserDataMng>.Instance.userFacilityList.Count; i++)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.userFacilityList[i];
			if (userFacility.facilityId == int.Parse(facilityMaster.facilityId))
			{
				flag = (userFacility.level >= int.Parse(facilityMaster.maxLevel));
				break;
			}
		}
		if (flag)
		{
			switch (type)
			{
			case CMD_UpperLimit.MessageType.GASHA:
				this.message.text = StringMaster.GetString("PossessionOverGasha");
				break;
			case CMD_UpperLimit.MessageType.PRESENTS:
				this.message.text = StringMaster.GetString("PossessionOverPresent");
				break;
			case CMD_UpperLimit.MessageType.QUEST:
				this.message.text = StringMaster.GetString("PossessionOverQuest");
				break;
			}
		}
		else
		{
			switch (type)
			{
			case CMD_UpperLimit.MessageType.GASHA:
				this.message.text = StringMaster.GetString("PossessionOverGashaUpgrade");
				break;
			case CMD_UpperLimit.MessageType.PRESENTS:
				this.message.text = StringMaster.GetString("PossessionOverPresentUpgrade");
				break;
			case CMD_UpperLimit.MessageType.QUEST:
				this.message.text = StringMaster.GetString("PossessionOverQuestUpgrade");
				break;
			}
		}
	}

	private void OnPushedReinforcementButton()
	{
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_ReinforcementTOP");
		});
	}

	private void OnPushedHouseButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SELL;
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_FarewellListRun");
		});
	}

	private void OnPushedGardenButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.GARDEN_SELL;
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_GardenList");
		});
	}

	public enum MessageType
	{
		GASHA,
		PRESENTS,
		QUEST
	}
}