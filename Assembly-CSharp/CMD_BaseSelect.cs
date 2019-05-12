﻿using FarmData;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_BaseSelect : CMD
{
	private const string CMD_NAME = "CMD_BaseSelect";

	public static CMD_BaseSelect instance;

	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	private BtnSort btnSort;

	[SerializeField]
	private UILabel btnSortText;

	[SerializeField]
	private GameObject goBTN_EVITEM_LIST;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private GUIMonsterIcon leftLargeMonsterIcon;

	[SerializeField]
	[Header("キャンペーンラベル")]
	private CampaignLabel campaignLabel;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	private UILabel ngTX_SORT_DISP;

	[SerializeField]
	[Header("キャラクターのステータスPanel")]
	private StatusPanel statusPanel;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_BTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_DECIDE;

	[SerializeField]
	private UISprite ngSPR_CHIP;

	[SerializeField]
	private UILabel ngTXT_CHIP;

	[SerializeField]
	private UILabel ngTXT_MEAT;

	[SerializeField]
	private UILabel ngTXT_HQ_MEAT;

	[SerializeField]
	private GameObject goPlateClustar;

	[SerializeField]
	private GameObject goPlateMeat;

	private GameObject goLeftLargeMonsterIcon;

	private int statusPage = 1;

	[SerializeField]
	private List<GameObject> goStatusPanelPage;

	[SerializeField]
	private UILabel switchingBtnText;

	public static CMD_BaseSelect.BASE_TYPE BaseType { get; set; }

	public static CMD_BaseSelect.ELEMENT_TYPE ElementType { get; set; }

	public static MonsterData DataChg { get; set; }

	public static CMD_BaseSelect CreateChipChipInstalling(Action<int> callback = null)
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.CHIP;
		CMD_BaseSelect.ElementType = CMD_BaseSelect.ELEMENT_TYPE.BASE;
		return GUIMain.ShowCommonDialog(callback, "CMD_BaseSelect") as CMD_BaseSelect;
	}

	protected override void Awake()
	{
		CMD_BaseSelect.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		base.Awake();
		PartyUtil.ActMIconShort = new Action<MonsterData>(this.ActMIconShort);
		this.monsterMedalList.SetActive(false);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.clBTN_DECIDE.activeCollider = false;
		this.ngTX_BTN_DECIDE.text = StringMaster.GetString("SystemButtonDecision");
		this.btnSortText.text = StringMaster.GetString("SystemSortButton");
		this.switchingBtnText.text = StringMaster.GetString("PartyStatusChange2");
		CMD_BaseSelect.instance = this;
	}

	protected override void WindowClosed()
	{
		CMD_BaseSelect.instance = null;
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (null != monsterDataMng)
		{
			monsterDataMng.PushBackAllMonsterPrefab();
		}
		base.WindowClosed();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.SetCommonUI();
		base.StartCoroutine(this.InitBaseSelect(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitBaseSelect(Action<int> f, float sizeX, float sizeY, float aT)
	{
		bool success = false;
		this.chipBaseSelect.ClearChipIcons();
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
		{
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(false);
			task.Add(MonsterDataMng.Instance().GetPicturebookData(DataMng.Instance().RespDataCM_Login.playerInfo.userId, false));
			yield return base.StartCoroutine(task.Run(delegate
			{
				success = true;
			}, delegate(Exception nop)
			{
				success = false;
			}, null));
			if (success)
			{
				this.goPlateMeat.SetActive(false);
				this.goPlateClustar.SetActive(true);
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
				this.campaignLabel.Refresh();
				this.btnSort.IsEvolvePage = true;
				base.PartsTitle.SetTitle(StringMaster.GetString("BaseSelect-04"));
				NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Chip");
			}
			this.goBTN_EVITEM_LIST.SetActive(true);
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.MEAL:
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo meatExpUpData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp);
			if (meatExpUpData == null)
			{
				APIRequestTask task = DataMng.Instance().RequestCampaign(10001, false);
				yield return base.StartCoroutine(task.Run(delegate
				{
					meatExpUpData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp);
					success = true;
				}, delegate(Exception nop)
				{
					success = false;
				}, null));
			}
			else
			{
				success = true;
			}
			if (meatExpUpData != null)
			{
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[]
				{
					GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp
				};
			}
			else
			{
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
				this.campaignLabel.Refresh();
			}
			this.goPlateMeat.SetActive(true);
			this.goPlateClustar.SetActive(false);
			base.PartsTitle.SetTitle(StringMaster.GetString("BaseSelect-03"));
			NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Meat");
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.LABO:
		{
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			this.campaignLabel.Refresh();
			string elementTypeName = string.Empty;
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				elementTypeName = StringMaster.GetString("SystemBase");
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				elementTypeName = StringMaster.GetString("SystemPartner");
			}
			base.PartsTitle.SetTitle(string.Format(StringMaster.GetString("BaseSelect-08"), elementTypeName));
			NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Chip");
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.CHIP:
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			this.campaignLabel.Refresh();
			base.PartsTitle.SetTitle(StringMaster.GetString("ChipInstalling-02"));
			this.chipBaseSelect.InitBaseSelect();
			break;
		default:
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			global::Debug.LogError("=========================================== CMD_BaseSelect TYPE設定がない!");
			break;
		}
		if (success)
		{
			this.ChipNumUpdate();
			this.InitMonsterList(true);
			this.ShowChgInfo();
			base.ShowDLG();
			base.Show(f, sizeX, sizeY, aT);
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
			base.ClosePanel(false);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	public void ChipNumUpdate()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
			this.ngTXT_MEAT.text = playerInfo.meatNum;
			this.ngTXT_HQ_MEAT.text = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001).ToString();
			this.ngTXT_MEAT.color = ((int.Parse(playerInfo.meatNum) > 0) ? Color.white : Color.red);
			break;
		case CMD_BaseSelect.BASE_TYPE.LABO:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			switch (CMD_BaseSelect.BaseType)
			{
			case CMD_BaseSelect.BASE_TYPE.EVOLVE:
				tutorialObserver.StartSecondTutorial("second_tutorial_evolution", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
				});
				goto IL_DB;
			case CMD_BaseSelect.BASE_TYPE.CHIP:
				tutorialObserver.StartSecondTutorial("second_tutorial_chip_equipment", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
				});
				goto IL_DB;
			}
			GUIMain.BarrierOFF();
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
			IL_DB:;
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE && farmRoot != null && farmRoot.DigimonManager != null)
		{
			farmRoot.DigimonManager.RefreshDigimonGameObject(true, delegate
			{
				this.CloseAndFarmCamOn(animation);
			});
		}
		else
		{
			this.CloseAndFarmCamOn(animation);
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
		{
			FarmCameraControlForCMD.On();
			base.ClosePanel(animation);
			RestrictionInput.EndLoad();
		}
		else
		{
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				FarmCameraControlForCMD.On();
				base.ClosePanel(animation);
				RestrictionInput.EndLoad();
				return;
			}
			APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
			base.StartCoroutine(task.Run(delegate
			{
				ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge();
				FarmCameraControlForCMD.On();
				this.ClosePanel(animation);
				RestrictionInput.EndLoad();
			}, delegate(Exception nop)
			{
				FarmCameraControlForCMD.On();
				this.ClosePanel(animation);
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void ShowChgInfo()
	{
		this.statusPanel.SetEnable(CMD_BaseSelect.DataChg != null);
		if (CMD_BaseSelect.DataChg != null)
		{
			this.monsterBasicInfo.SetMonsterData(CMD_BaseSelect.DataChg);
			this.monsterStatusList.SetValues(CMD_BaseSelect.DataChg, false);
			this.monsterLeaderSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterUniqueSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterSuccessionSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterResistanceList.SetValues(CMD_BaseSelect.DataChg);
			this.monsterMedalList.SetValues(CMD_BaseSelect.DataChg.userMonster);
			this.StatusPageChange(false);
		}
		else
		{
			this.chipBaseSelect.ClearChipIcons();
			this.monsterBasicInfo.ClearMonsterData();
			this.monsterStatusList.ClearValues();
			this.monsterMedalList.SetActive(false);
			this.RequestStatusPage(1);
		}
	}

	private void OnTouchSort()
	{
	}

	private void OnTouchDecide()
	{
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			GUIMain.ShowCommonDialog(delegate(int i)
			{
				if (this.leftLargeMonsterIcon != null && CMD_BaseSelect.DataChg != null)
				{
					this.leftLargeMonsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
				}
			}, "CMD_Evolution");
			break;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
		{
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			if (!userFacilityList.Exists((UserFacility x) => x.facilityId == 1))
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.LeadBuildMeat), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("BaseSelect-05");
				cmd_Confirm.Info = StringMaster.GetString("BaseSelect-06");
				cmd_Confirm.BtnTextYes = StringMaster.GetString("BaseSelect-07");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				CMD_MealExecution.DataChg = CMD_BaseSelect.DataChg;
				GUIMain.ShowCommonDialog(null, "CMD_MealExecution");
				CMD_MealExecution.UpdateParamCallback = delegate()
				{
					MonsterDataMng.Instance().RefreshMonsterDataList();
					this.InitMonsterList(true);
					this.ShowChgInfo();
					this.CheckChip();
				};
			}
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.LABO:
		{
			MonsterData partnerDigimon = CMD_Laboratory.instance.partnerDigimon;
			if (CMD_BaseSelect.DataChg == null)
			{
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					if (partnerDigimon != null)
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("Laboratory-04");
						cmd_ModalMessage.Info = StringMaster.GetString("Laboratory-06");
						return;
					}
					CMD_Laboratory.instance.RemoveBaseDigimon();
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					CMD_Laboratory.instance.RemovePartnerDigimon();
				}
			}
			base.SetForceReturnValue(1);
			this.ClosePanel(true);
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.CHIP:
			CMD_ChipSphere.DataChg = CMD_BaseSelect.DataChg;
			this.chipBaseSelect.OnTouchDecide(delegate
			{
				this.SetSelectedCharChg();
			});
			break;
		default:
			global::Debug.LogError("=========================================== CMD_BaseSelect TYPE設定がない!");
			break;
		}
	}

	private void SetCommonUI()
	{
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (this.goEFC_RIGHT != null)
		{
			this.goSelectPanelMonsterIcon.transform.SetParent(this.goEFC_RIGHT.transform);
		}
		Vector3 localPosition = this.goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this.goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanelMonsterIcon.ListWindowViewRect = ConstValue.GetRectWindow2();
	}

	public void InitMonsterList(bool initLoc = true)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.ClearSortMessAll();
		monsterDataMng.ClearLevelMessAll();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		bool isEvolvePage = false;
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			isEvolvePage = true;
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.CAN_EVOLVE);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.EVOLVE;
			monsterDataMng.MyCluster = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			break;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
			break;
		case CMD_BaseSelect.BASE_TYPE.LABO:
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.RESEARCH_TARGET);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
			break;
		default:
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
			break;
		}
		monsterDataMng.SetDimmAll(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		list = monsterDataMng.SortMDList(list, isEvolvePage);
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		monsterDataMng.SetSelectOffAll();
		monsterDataMng.ClearDimmMessAll();
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
		{
			MonsterData baseDigimon = CMD_Laboratory.instance.baseDigimon;
			MonsterData partnerDigimon = CMD_Laboratory.instance.partnerDigimon;
			List<MonsterData> deckMonsterDataList = monsterDataMng.GetDeckMonsterDataList(false);
			for (int i = 0; i < deckMonsterDataList.Count; i++)
			{
				PartyUtil.SetDimIcon(true, deckMonsterDataList[i], StringMaster.GetString("CharaIcon-04"), false);
			}
			this.PrepareLaboBaseIcon(baseDigimon);
			this.PrepareLaboPartnerIcon(partnerDigimon);
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				CMD_BaseSelect.DataChg = baseDigimon;
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				CMD_BaseSelect.DataChg = partnerDigimon;
			}
			this.SetSelectedCharChg();
		}
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
		{
			foreach (MonsterData monsterData in list)
			{
				if (monsterData.userMonster.IsLocked)
				{
					PartyUtil.SetDimIcon(true, monsterData, string.Empty, true);
				}
			}
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				this.PrepareLaboBaseIcon(CMD_BaseSelect.DataChg);
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				this.PrepareLaboPartnerIcon(CMD_BaseSelect.DataChg);
			}
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEAL && CMD_BaseSelect.DataChg != null)
		{
			if (CMD_BaseSelect.DataChg.userMonster.level.ToInt32() >= CMD_BaseSelect.DataChg.monsterM.maxLevel.ToInt32())
			{
				this.SetEmpty();
			}
			else
			{
				this.SetSelectedCharChg();
			}
		}
	}

	private void RemoveIcon(MonsterData md)
	{
		if (CMD_BaseSelect.DataChg == null)
		{
			return;
		}
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			PartyUtil.SetDimIcon(false, CMD_BaseSelect.DataChg, string.Empty, false);
			MonsterDataMng.Instance().SortMDList(new List<MonsterData>
			{
				CMD_BaseSelect.DataChg
			}, true);
		}
		else
		{
			PartyUtil.SetDimIcon(false, CMD_BaseSelect.DataChg, string.Empty, false);
		}
		this.SetEmpty();
		this.CheckChip();
	}

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			PartyUtil.SetDimIcon(false, CMD_BaseSelect.DataChg, string.Empty, false);
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
			{
				MonsterDataMng.Instance().SortMDList(new List<MonsterData>
				{
					CMD_BaseSelect.DataChg
				}, true);
			}
		}
		CMD_BaseSelect.DataChg = tappedMonsterData;
		this.SetSelectedCharChg();
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
		{
			bool flag = false;
			bool isCheckDim = true;
			if (tappedMonsterData == CMD_BaseSelect.DataChg || tappedMonsterData == CMD_Laboratory.instance.baseDigimon || tappedMonsterData == CMD_Laboratory.instance.partnerDigimon)
			{
				flag = true;
				isCheckDim = false;
			}
			List<MonsterData> deckMonsterDataList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
			foreach (MonsterData monsterData in deckMonsterDataList)
			{
				if (monsterData == tappedMonsterData)
				{
					isCheckDim = false;
				}
			}
			CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
			{
				PartyUtil.SetLock(tappedMonsterData, isCheckDim);
			}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
			if (flag)
			{
				cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Laboratory;
			}
		}
		else
		{
			CMD_CharacterDetailed.DataChg = tappedMonsterData;
			CMD_CharacterDetailed cmd_CharacterDetailed2 = GUIMain.ShowCommonDialog(delegate(int i)
			{
				PartyUtil.SetLock(tappedMonsterData, false);
				if (this.leftLargeMonsterIcon != null && tappedMonsterData == CMD_CharacterDetailed.DataChg)
				{
					this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
				}
			}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
			{
				cmd_CharacterDetailed2.Mode = CMD_CharacterDetailed.LockMode.Evolution;
			}
		}
	}

	public void SetEmpty()
	{
		CMD_BaseSelect.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		this.ShowChgInfo();
		if (this.goLeftLargeMonsterIcon != null)
		{
			UnityEngine.Object.DestroyImmediate(this.goLeftLargeMonsterIcon);
		}
		this.goMN_ICON_CHG.SetActive(true);
	}

	private void SetSelectedCharChg()
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			if (this.goLeftLargeMonsterIcon != null)
			{
				UnityEngine.Object.DestroyImmediate(this.goLeftLargeMonsterIcon);
			}
			if (this.goMN_ICON_CHG == null)
			{
				return;
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
			this.leftLargeMonsterIcon = monsterDataMng.MakePrefabByMonsterData(CMD_BaseSelect.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goLeftLargeMonsterIcon = this.leftLargeMonsterIcon.gameObject;
			this.goLeftLargeMonsterIcon.SetActive(true);
			this.leftLargeMonsterIcon.Data = CMD_BaseSelect.DataChg;
			this.chipBaseSelect.SetSelectedCharChg(CMD_BaseSelect.DataChg);
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
			{
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					this.PrepareLaboBaseIcon(CMD_BaseSelect.DataChg);
					this.leftLargeMonsterIcon.SetTouchAct_S(delegate(MonsterData i)
					{
						PartyUtil.SetDimIcon(false, CMD_BaseSelect.DataChg, string.Empty, false);
						this.SetEmpty();
						this.CheckChip();
					});
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					this.PrepareLaboPartnerIcon(CMD_BaseSelect.DataChg);
					this.leftLargeMonsterIcon.SetTouchAct_S(delegate(MonsterData i)
					{
						PartyUtil.SetDimIcon(false, CMD_BaseSelect.DataChg, string.Empty, false);
						this.SetEmpty();
						this.CheckChip();
					});
				}
			}
			else
			{
				GUIMonsterIcon guimonsterIcon = PartyUtil.SetDimIcon(true, CMD_BaseSelect.DataChg, StringMaster.GetString("SystemSelect"), false);
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.RemoveIcon));
				this.leftLargeMonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.RemoveIcon));
			}
			this.leftLargeMonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			this.leftLargeMonsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = this.leftLargeMonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = this.leftLargeMonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(this.leftLargeMonsterIcon.transform, add);
			}
			this.goMN_ICON_CHG.SetActive(false);
			this.ShowChgInfo();
			this.CheckChip();
		}
	}

	private void PrepareLaboBaseIcon(MonsterData md)
	{
		if (md == null)
		{
			return;
		}
		GUIMonsterIcon guimonsterIcon = PartyUtil.SetDimIcon(true, md, StringMaster.GetString("SystemBase"), false);
		guimonsterIcon.SetTouchAct_S(delegate(MonsterData i)
		{
			PartyUtil.SetDimIcon(false, md, string.Empty, false);
			this.SetEmpty();
			this.CheckChip();
		});
	}

	private void PrepareLaboPartnerIcon(MonsterData md)
	{
		if (md == null)
		{
			return;
		}
		GUIMonsterIcon guimonsterIcon = PartyUtil.SetDimIcon(true, md, "1", false);
		guimonsterIcon.SetTouchAct_S(delegate(MonsterData i)
		{
			PartyUtil.SetDimIcon(false, md, string.Empty, false);
			this.SetEmpty();
			this.CheckChip();
		});
	}

	private void CheckChip()
	{
		bool decideButton = true;
		if (CMD_BaseSelect.DataChg == null)
		{
			decideButton = false;
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEAL)
		{
			if (int.Parse(CMD_BaseSelect.DataChg.userMonster.level) >= int.Parse(CMD_BaseSelect.DataChg.monsterM.maxLevel))
			{
				decideButton = false;
			}
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			if (!userFacilityList.Exists((UserFacility x) => x.facilityId == 1))
			{
				decideButton = true;
			}
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			decideButton = false;
			GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution[] monsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM.monsterEvolutionM;
			foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution evolution in monsterEvolutionM)
			{
				if (evolution.baseMonsterId == CMD_BaseSelect.DataChg.monsterM.monsterId)
				{
					decideButton = true;
					break;
				}
			}
		}
		this.SetDecideButton(decideButton);
	}

	public void SetDecideButton(bool isExecutable)
	{
		if (isExecutable)
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Blue";
			this.ngTX_DECIDE.color = Color.white;
			this.clBTN_DECIDE.activeCollider = true;
		}
		else
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Gray";
			this.ngTX_DECIDE.color = Color.gray;
			this.clBTN_DECIDE.activeCollider = false;
		}
	}

	private DataMng.ExperienceInfo GetExpInfo()
	{
		DataMng dataMng = DataMng.Instance();
		int meat_num = 1;
		int expFromMeat = dataMng.GetExpFromMeat(meat_num);
		int exp = int.Parse(CMD_BaseSelect.DataChg.userMonster.ex) + expFromMeat;
		return dataMng.GetExperienceInfo(exp);
	}

	public void StatusPageChangeTap()
	{
		this.StatusPageChange(true);
	}

	private void StatusPageChange(bool pageChange)
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			if (pageChange)
			{
				if (this.statusPage < this.goStatusPanelPage.Count)
				{
					this.statusPage++;
				}
				else
				{
					this.statusPage = 1;
				}
			}
			int num = 1;
			foreach (GameObject gameObject in this.goStatusPanelPage)
			{
				if (num == this.statusPage)
				{
					gameObject.SetActive(true);
				}
				else
				{
					gameObject.SetActive(false);
				}
				num++;
			}
		}
	}

	private void RequestStatusPage(int requestPage)
	{
		this.statusPage = requestPage;
		if (this.statusPage >= this.goStatusPanelPage.Count || this.statusPage < 1)
		{
			this.statusPage = 1;
		}
		int num = 1;
		foreach (GameObject gameObject in this.goStatusPanelPage)
		{
			if (num == this.statusPage)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
	}

	private void LeadBuildMeat(int index)
	{
		if (index == 0)
		{
			base.SetCloseAction(new Action<int>(this.CloseAllCommonDialog));
			this.ClosePanel(true);
		}
	}

	private void CloseAllCommonDialog(int noop)
	{
		GUIManager.CloseAllCommonDialog(delegate(int nop)
		{
			GUIFace.ForceHideDigiviceBtn_S();
			GUIFace.ForceHideFacilityBtn_S();
			GUIMain.ShowCommonDialog(null, "CMD_FacilityShop");
		});
	}

	public void OnTouchedEvoltionItemListBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_EvolutionItemList");
	}

	public enum BASE_TYPE
	{
		None,
		EVOLVE,
		MEAL,
		LABO,
		CHIP
	}

	public enum ELEMENT_TYPE
	{
		BASE,
		PARTNER
	}
}