﻿using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIScreenHome : GUIScreen
{
	private PartsMenu partsMenu;

	protected GameObject goFARM_ROOT;

	private bool isInfoShowed;

	private bool isStartGuidance;

	public static bool isManualScreenFadeIn;

	public bool isFinishedStartLoading;

	public static Action homeOpenCallback;

	private List<CampaignFacilityIcon> campaignFacilityIconList;

	public static bool enableBackKeyAndroid = true;

	private static bool isCacheBattled;

	public override void ShowGUI()
	{
		Time.timeScale = 1f;
		GameObject gameObject = GUIManager.LoadCommonGUI("Parts/PartsMenu", base.gameObject);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -300f);
		this.partsMenu = gameObject.GetComponent<PartsMenu>();
		base.ShowGUI();
		GUIFace.ForceShowDigiviceBtn_S();
		this.ServerRequest();
	}

	protected virtual void ServerRequest()
	{
		APIRequestTask task = APIUtil.Instance().RequestHomeData();
		base.StartCoroutine(task.Run(delegate
		{
			base.StartCoroutine(this.StartEvent());
		}, null, null));
	}

	protected virtual IEnumerator StartEvent()
	{
		yield return base.StartCoroutine(this.CreateHomeData());
		TipsLoading.Instance.StopTipsLoad(true);
		Loading.Invisible();
		if (!GUIScreenHome.isManualScreenFadeIn)
		{
			yield return base.StartCoroutine(this.StartScreenFadeIn(null));
		}
		this.isFinishedStartLoading = true;
		bool isPenaltyLevelTwo = false;
		yield return base.StartCoroutine(this.PenaltyCheck(delegate
		{
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			isPenaltyLevelTwo = true;
		}));
		if (isPenaltyLevelTwo)
		{
			yield break;
		}
		GUIManager.ResetTouchingCount();
		yield return base.StartCoroutine(this.ShowLoginBonusCampaign());
		yield return base.StartCoroutine(this.ShowLoginBonusNormal());
		GameWebAPI.RespDataUS_GetPlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo;
		GameWebAPI.RespDataCM_LoginBonus loginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (loginBonus != null && loginBonus.loginBonus != null && loginBonus.loginBonus.normal != null && loginBonus.loginBonus.normal.Length > 0 && playerInfo != null && playerInfo.playerInfo != null && playerInfo.playerInfo.loginCount == 3)
		{
			bool isReviewDialogClose = false;
			Action onFinishedAction = delegate()
			{
				isReviewDialogClose = true;
			};
			LeadReview.ShowReviewConfirm(LeadReview.MessageType.TOTAL_LOGIN_COUNT_3DAYS, onFinishedAction, false);
			while (!isReviewDialogClose)
			{
				yield return null;
			}
		}
		yield return base.StartCoroutine(this.CheckRecoverBattle());
		Loading.Display(Loading.LoadingType.LARGE, false);
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		Loading.Invisible();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		yield return base.StartCoroutine(tutorialObserver.StartGuidance(new Action<bool>(this.StartedGuidance)));
		GUIFace.SetFacilityAlertIcon();
		yield break;
	}

	private void StartedGuidance(bool isActionGuidance)
	{
		this.isStartGuidance = isActionGuidance;
		this.StartFarm();
	}

	public IEnumerator StartScreenFadeIn(Action finish = null)
	{
		GUIFadeControll.StartFadeIn(0f);
		GameObject fadeObj = GUIManager.LoadCommonGUI("Render2D/SquaresROOT", base.gameObject);
		while (fadeObj != null)
		{
			yield return null;
		}
		if (finish != null)
		{
			finish();
		}
		GUIScreenHome.isManualScreenFadeIn = false;
		yield break;
	}

	protected virtual IEnumerator CreateHomeData()
	{
		GUIPlayerStatus.RefreshParams_S(false);
		MonsterDataMng.Instance().GetMonsterDataList(true);
		MonsterDataMng.Instance().InitMonsterGO();
		this.MissionProcess();
		ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
		ClassSingleton<PartsMenuNewsIconAccessor>.Instance.artsMenuNewsIcon.NewsCheck();
		ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon.FrinedListCheck();
		if (ConstValue.IS_CHAT_OPEN != 1)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.gameObject.SetActive(false);
		}
		GUIFace.SetFacilityShopButtonBadge();
		this.DownloadMenuBanner();
		while (!TextureManager.instance.isLoadSaveData)
		{
			yield return null;
		}
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus != null && respDataCM_LoginBonus.loginBonus.campaign != null)
		{
			foreach (GameWebAPI.RespDataCM_LoginBonus.LoginBonus campaign in respDataCM_LoginBonus.loginBonus.campaign)
			{
				bool isLoadEnd = false;
				string path = CMD_LoginCampaign.GetBgPathForFTP(campaign.backgroundImg);
				TextureManager.instance.Load(path, delegate(Texture2D texture)
				{
					isLoadEnd = true;
				}, 30f, true);
				while (!isLoadEnd)
				{
					yield return null;
				}
			}
		}
		yield return base.StartCoroutine(this.CreateFarm());
		this.StartCacheBattle();
		this.StartCacheParty();
		LeadCapture.Instance.CheckCaptureUpdate();
		this.ShowCampaignFacilityIcon();
		yield break;
	}

	public void ShowCampaignFacilityIcon()
	{
		this.campaignFacilityIconList = new List<CampaignFacilityIcon>();
		CampaignFacilityIcon campaignFacilityIcon = CampaignFacilityIcon.Create(GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp, base.gameObject);
		if (campaignFacilityIcon != null)
		{
			this.campaignFacilityIconList.Add(campaignFacilityIcon);
		}
	}

	public void CloseAllCampaignFacilityIcon()
	{
		if (this.campaignFacilityIconList != null)
		{
			foreach (CampaignFacilityIcon campaignFacilityIcon in this.campaignFacilityIconList)
			{
				campaignFacilityIcon.Close();
			}
			this.campaignFacilityIconList.Clear();
		}
	}

	private void StartFarm()
	{
		ServerDateTime.isUpdateServerDateTime = true;
		FarmRoot instance = FarmRoot.Instance;
		instance.DigimonManager.AppaearanceDigimon(null);
		this.EnableFarmInput();
		this.CachePartyAll();
		if (ConstValue.IS_CHAT_OPEN == 1)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StartGetHistoryIdList();
		}
		GUIMain.BarrierOFF();
		this.ShowWebWindow();
		if (GUIScreenHome.homeOpenCallback != null)
		{
			GUIScreenHome.homeOpenCallback();
			GUIScreenHome.homeOpenCallback = null;
		}
	}

	private void MissionProcess()
	{
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.MissionTapCheck();
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge();
	}

	protected IEnumerator CreateFarm()
	{
		GameObject go = AssetDataMng.Instance().LoadObject("Farm/Fields/farm_01/FARM_ROOT", null, true) as GameObject;
		yield return null;
		this.goFARM_ROOT = UnityEngine.Object.Instantiate<GameObject>(go);
		go = null;
		Resources.UnloadUnusedAssets();
		yield return null;
		FarmRoot farmRoot = FarmRoot.Instance;
		yield return base.StartCoroutine(farmRoot.Initialize(base.GetComponent<FarmUI>()));
		yield break;
	}

	protected void EnableFarmInput()
	{
		FarmRoot instance = FarmRoot.Instance;
		InputControll input = instance.Input;
		if (null != input)
		{
			input.enabled = true;
		}
	}

	private IEnumerator PenaltyCheck(Action OnPenaltyLevelTwo)
	{
		GameWebAPI.RespDataMP_MyPage mypageData = DataMng.Instance().RespDataMP_MyPage;
		if (mypageData.penaltyUserInfo != null && (mypageData.penaltyUserInfo.penaltyLevel == "1" || mypageData.penaltyUserInfo.penaltyLevel == "2"))
		{
			bool isClose = false;
			Action<int> onClosedAction = delegate(int x)
			{
				if (mypageData.penaltyUserInfo.penaltyLevel == "2")
				{
					OnPenaltyLevelTwo();
				}
				isClose = true;
			};
			string title = StringMaster.GetString("PenaltyTitle");
			string message = mypageData.penaltyUserInfo.penalty.message;
			AlertManager.ShowAlertDialog(onClosedAction, title, message, AlertManager.ButtonActionType.Close, false);
			while (!isClose)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator ShowLoginBonusCampaign()
	{
		DataMng.Instance().ShowLoginBonusNumC = 0;
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus == null || respDataCM_LoginBonus.loginBonus.campaign == null)
		{
			yield return null;
		}
		else
		{
			int showNum = respDataCM_LoginBonus.loginBonus.campaign.Length;
			while (showNum > DataMng.Instance().ShowLoginBonusNumC)
			{
				bool isClose = false;
				Action<int> onClosedAction = delegate(int x)
				{
					isClose = true;
					DataMng.Instance().ShowLoginBonusNumC++;
				};
				GameWebAPI.RespDataCM_LoginBonus.LoginBonus loginBonus = respDataCM_LoginBonus.loginBonus.campaign[DataMng.Instance().ShowLoginBonusNumC];
				if (loginBonus.loginBonusId != "2")
				{
					GUIMain.ShowCommonDialog(onClosedAction, "CMD_LoginAnimator");
				}
				else
				{
					GUIMain.ShowCommonDialog(onClosedAction, "CMD_CampaignLogin");
				}
				while (!isClose)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	private IEnumerator ShowLoginBonusNormal()
	{
		DataMng.Instance().ShowLoginBonusNumN = 0;
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus == null || respDataCM_LoginBonus.loginBonus.normal == null)
		{
			yield return null;
		}
		else
		{
			int showNum = respDataCM_LoginBonus.loginBonus.normal.Length;
			while (showNum > DataMng.Instance().ShowLoginBonusNumN)
			{
				bool isClose = false;
				Action<int> action = delegate(int x)
				{
					isClose = true;
					DataMng.Instance().ShowLoginBonusNumN++;
				};
				GUIMain.ShowCommonDialog(action, "CMD_NormalLogin");
				while (!isClose)
				{
					yield return null;
				}
			}
			if (0 < showNum)
			{
				this.isInfoShowed = true;
				Partytrack.start(5789, "07e569a17368b4a04e0eed94ee2937f3");
			}
		}
		yield break;
	}

	public void SetActiveOfPartsMenu(bool active)
	{
		this.partsMenu.gameObject.SetActive(active);
	}

	private IEnumerator CheckRecoverBattle()
	{
		BattleNextBattleOption.ClearBattleMenuSettings();
		if (ClassSingleton<BattleDataStore>.Instance.IsBattleRecoverable)
		{
			bool isCancel = false;
			Action onBattleRecover = delegate()
			{
				GUIMain.BarrierOFF();
			};
			Action onCancelAction = delegate()
			{
				isCancel = true;
			};
			ClassSingleton<BattleDataStore>.Instance.OpenBattleRecoverConfirm(onBattleRecover, onCancelAction);
			while (!isCancel)
			{
				yield return null;
			}
		}
		yield break;
	}

	private void ShowWebWindow()
	{
		DataMng dataMng = DataMng.Instance();
		if (!this.isStartGuidance && (this.isInfoShowed || (null != dataMng && dataMng.IsPopUpInformaiton)))
		{
			CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
			cmdwebWindow.TitleText = StringMaster.GetString("InfomationTitle");
			cmdwebWindow.Url = WebAddress.EXT_ADR_INFO;
			if (null != dataMng)
			{
				dataMng.IsPopUpInformaiton = false;
			}
		}
	}

	private void DownloadMenuBanner()
	{
		GUIBannerPanel.Data = DataMng.Instance().RespData_BannerMaster;
		if (GUIBannerPanel.Data != null)
		{
			this.partsMenu.SetMenuBanner();
		}
	}

	protected override void Update()
	{
		this.UpdateBackKeyAndroid();
	}

	private void UpdateBackKeyAndroid()
	{
		if (GUIScreenHome.enableBackKeyAndroid && GUIManager.IsEnableBackKeyAndroid() && Input.GetKeyDown(KeyCode.Escape))
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.BackToTitle), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("BackKeyConfirmGoTitle");
			this.partsMenu.ForceHide(false);
			SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
		}
	}

	private void BackToTitle(int idx)
	{
		if (idx == 0)
		{
			GUIMain.BackToTOP("UITitle", 0.8f, 0.8f);
		}
	}

	protected void StartCacheBattle()
	{
		if (!GUIScreenHome.isCacheBattled)
		{
			this.CacheBattleAllClear();
			this.CacheBattleAll();
			GUIScreenHome.isCacheBattled = true;
		}
	}

	protected void StartCacheParty()
	{
		this.CachePartyAllClear();
		this.CachePartyFavorite();
	}

	private void CachePartyAllClear()
	{
		AssetDataCacheMng.Instance().DeleteCacheType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY);
	}

	private void CachePartyFavorite()
	{
		List<string> deckMonsterPathList = MonsterDataMng.Instance().GetDeckMonsterPathList(true);
		AssetDataCacheMng.Instance().RegisterCacheType(deckMonsterPathList, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, false);
	}

	private void CachePartyAll()
	{
		List<string> deckMonsterPathList = MonsterDataMng.Instance().GetDeckMonsterPathList(false);
		AssetDataCacheMng.Instance().RegisterCacheType(deckMonsterPathList, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, false);
	}

	private void CacheBattleAllClear()
	{
		AssetDataCacheMng.Instance().DeleteCacheType(AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON);
	}

	private void CacheBattleAll()
	{
		List<string> battleCommon = AssetDataCacheData.GetBattleCommon();
		AssetDataCacheMng.Instance().RegisterCacheType(battleCommon, AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON, false);
	}
}