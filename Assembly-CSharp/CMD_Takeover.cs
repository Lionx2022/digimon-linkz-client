﻿using Master;
using System;
using UnityEngine;

public class CMD_Takeover : CMD
{
	[SerializeField]
	private TakeoverMenu menu;

	public static CMD_Takeover.MODE currentMode;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Initialize();
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void Initialize()
	{
		if (base.PartsTitle != null)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("TakeOverTitle"));
		}
		this.menu.gameObject.SetActive(true);
		if (CMD_Takeover.currentMode == CMD_Takeover.MODE.INPUT)
		{
			this.menu.Initialize(TakeoverMenu.MODE.Input);
		}
		else if (CMD_Takeover.currentMode == CMD_Takeover.MODE.ISSUE)
		{
			this.menu.Initialize(TakeoverMenu.MODE.Issue);
		}
	}

	public enum MODE
	{
		NON,
		INPUT,
		ISSUE
	}
}
