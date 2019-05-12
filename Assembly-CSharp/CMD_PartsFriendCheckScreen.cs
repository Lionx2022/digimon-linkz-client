﻿using Master;
using System;
using UnityEngine;

public class CMD_PartsFriendCheckScreen : CMD
{
	public GameObject goMN_ICON;

	public GameObject goTX_NICKNAME;

	public GameObject goTX_EXP;

	public GameObject goTX_LAST_LOGIN;

	private UILabel ngTX_NICKNAME;

	private UILabel ngTX_EXP;

	private UILabel ngTX_LAST_LOGIN;

	public GameObject goTX_EXP_2;

	private UILabel ngTX_EXP_2;

	private static GameWebAPI.FriendList data;

	private static string exp;

	private GameObject goMN_ICON_2;

	private MonsterData md_favo;

	public static GameWebAPI.FriendList Data
	{
		get
		{
			return CMD_PartsFriendCheckScreen.data;
		}
		set
		{
			CMD_PartsFriendCheckScreen.data = value;
		}
	}

	public static string Exp
	{
		get
		{
			return CMD_PartsFriendCheckScreen.exp;
		}
		set
		{
			CMD_PartsFriendCheckScreen.exp = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_NICKNAME = this.goTX_NICKNAME.GetComponent<UILabel>();
		this.ngTX_EXP = this.goTX_EXP.GetComponent<UILabel>();
		this.ngTX_LAST_LOGIN = this.goTX_LAST_LOGIN.GetComponent<UILabel>();
		this.ngTX_EXP_2 = this.goTX_EXP_2.GetComponent<UILabel>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowParam();
		this.ShowExp();
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_PartsFriendCheckScreen.data = null;
	}

	private void ShowParam()
	{
		this.md_favo = MonsterDataMng.Instance().CreateMonsterDataByMID(CMD_PartsFriendCheckScreen.data.monsterData.monsterId);
		GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(this.md_favo, this.goMN_ICON.transform.localScale, this.goMN_ICON.transform.localPosition, this.goMN_ICON.transform.parent, true, false);
		this.goMN_ICON_2 = guimonsterIcon.gameObject;
		this.goMN_ICON_2.SetActive(true);
		guimonsterIcon.Data = this.md_favo;
		guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.actMIconLong));
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.actMIconLong));
		UIWidget component = this.goMN_ICON.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		this.goMN_ICON.SetActive(false);
		this.ngTX_NICKNAME.text = CMD_PartsFriendCheckScreen.data.userData.nickname;
		this.ngTX_EXP.text = CMD_PartsFriendCheckScreen.data.userData.description;
		this.ngTX_LAST_LOGIN.text = StringMaster.GetString("Friend-12") + CMD_PartsFriendCheckScreen.data.userData.loginTime;
	}

	private void actMIconLong(MonsterData md)
	{
		CMD_FriendTop.instance.OpenFriendProfile();
	}

	private void ShowExp()
	{
		this.ngTX_EXP_2.text = CMD_PartsFriendCheckScreen.exp;
	}
}
