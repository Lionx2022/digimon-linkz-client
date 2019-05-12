﻿using Master;
using System;
using UnityEngine;

public class CMD_OtherTOP : CMD
{
	[SerializeField]
	private UILabel historyText;

	[SerializeField]
	private UILabel takeoverText;

	[SerializeField]
	private UILabel officialText;

	[SerializeField]
	private UILabel inquiryText;

	[SerializeField]
	private UILabel termsText;

	[SerializeField]
	private UILabel authorityText;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		SoundMng.Instance().PlayGameBGM("bgm_102");
		base.PartsTitle.SetTitle(StringMaster.GetString("InfomationOther"));
		base.Show(f, sizeX, sizeY, aT);
		this.historyText.text = StringMaster.GetString("OtherHistory");
		this.takeoverText.text = StringMaster.GetString("TakeOverTitle");
		this.officialText.text = StringMaster.GetString("OtherOfficialSite");
		this.inquiryText.text = StringMaster.GetString("InquiryTitle");
		this.termsText.text = StringMaster.GetString("AgreementTitle");
		this.authorityText.text = StringMaster.GetString("OtherRight");
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
		SoundMng.Instance().PlayGameBGM("bgm_201");
		this.CloseAndFarmCamOn(animation);
	}

	private void OnClickOfficialSite()
	{
		Application.OpenURL(ConstValue.OFFICIAL_SITE_URL);
	}

	private void OnClickedTermsOfUse()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		if (null != cmdwebWindow)
		{
			cmdwebWindow.TitleText = StringMaster.GetString("AgreementTitle");
			cmdwebWindow.Url = WebAddress.EXT_ADR_AGREE;
		}
	}

	private void OnClickedRightsExpression()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		if (null != cmdwebWindow)
		{
			cmdwebWindow.TitleText = StringMaster.GetString("OtherRight");
			cmdwebWindow.Url = WebAddress.EXT_ADR_COPY;
		}
	}

	private void OnClickedPurchaseHistory()
	{
		GUIMain.ShowCommonDialog(null, "CMD_History");
	}

	private void OnClickedTakeover()
	{
		CMD_Takeover.currentMode = CMD_Takeover.MODE.ISSUE;
		GUIMain.ShowCommonDialog(null, "CMD_TakeoverIssue");
	}

	private void OnClickedContact()
	{
		GUIMain.ShowCommonDialog(null, "CMD_inquiry");
	}
}