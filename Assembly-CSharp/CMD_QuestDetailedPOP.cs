﻿using FarmData;
using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_QuestDetailedPOP : CMD
{
	private const float bonusChangeTime = 0.2f;

	private const int BOSS_ID = 2;

	private const int BOSS_ID_AP_FREE = 4;

	[Header("出現する敵リスト")]
	[SerializeField]
	private List<GameObject> goENCOUNT_MONS_LIST;

	[Header("ドロップするアイテムリスト")]
	[SerializeField]
	private List<PresentBoxItem> itemDROP_ITEM_LIST;

	[SerializeField]
	private GameObject goTEX_DROP;

	[SerializeField]
	private UILabel ngTX_A_STAGE_DNG_TITLE;

	[Header("ステージの数字のラベル")]
	[SerializeField]
	private UILabel ngTX_A_STAGE_DNG_NUMBER;

	[Header("コンテンツ・ルート (ヒナと吹き出し)")]
	[SerializeField]
	private GameObject goContentsROOT;

	[Header("ポイントコンテンツ・ルート (ボーナス)")]
	[SerializeField]
	private GameObject goPointContentsROOT;

	[SerializeField]
	[Header("詳細説明 (ヒナの吹き出し)")]
	private UILabel ngTX_A_STAGE_DNG_DESCRIPT;

	[SerializeField]
	[Header("ポイント・タイトルのリスト)")]
	private List<UILabel> bonusPointLabelList;

	[SerializeField]
	private GameObject changeBonusButton;

	[Header("取得できるEXPのタイトル")]
	[SerializeField]
	private UILabel getEXPTitleLabel;

	[SerializeField]
	[Header("取得できるEXP")]
	private UILabel getEXPLabel;

	[SerializeField]
	[Header("取得できるクラスタタイトル")]
	private UILabel getClusterTitleLabel;

	[Header("取得できるクラスタ数")]
	[SerializeField]
	private UILabel getClusterLabel;

	[SerializeField]
	[Header("消費するスタミナタイトル")]
	private UILabel getStaminaTitleLabel;

	[SerializeField]
	[Header("消費するスタミナ")]
	private UILabel getStaminaLabel;

	[SerializeField]
	[Header("バトルボタン")]
	private GameObject goBattleBtn;

	private UISprite spBattleBtn;

	[SerializeField]
	[Header("マルチバトルボタン")]
	private GameObject goMultiBattleBtn;

	[SerializeField]
	private RewardIconRoot rewardIconRoot;

	[SerializeField]
	[Header("初回報酬取得済みカラー")]
	private Color clearColor = Color.gray;

	private QuestData.WorldDungeonData d_data;

	private List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> epbList = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();

	private int epbListViewCnt;

	private TweenAlpha bonusPosTween;

	private bool bonusChange;

	[SerializeField]
	private GameObject bonusBaseObj;

	private bool isClosed;

	private List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset> dropAssetList;

	private static GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo;

	private string StageNum { get; set; }

	public static GameWebAPI.RespDataCP_Campaign.CampaignInfo CampaignInfo
	{
		set
		{
			CMD_QuestDetailedPOP.campaignInfo = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		base.StartCoroutine(this.Initialize(f, sizeX, sizeY, aT));
	}

	private IEnumerator Initialize(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.getEXPTitleLabel.text = StringMaster.GetString("QuestDetailsExp");
		this.getClusterTitleLabel.text = StringMaster.GetString("QuestDetailsTip");
		this.getStaminaTitleLabel.text = StringMaster.GetString("QuestDetailsCost");
		this.d_data = CMD_QuestTOP.instance.StageDataBk;
		this.StageNum = CMD_QuestTOP.instance.StageNumBk;
		this.ShowInfo();
		yield return null;
		int stamina = int.Parse(this.d_data.worldDungeonM.needStamina);
		if (CMD_QuestDetailedPOP.campaignInfo != null)
		{
			float fstamina = (float)stamina;
			stamina = Mathf.CeilToInt(fstamina * float.Parse(CMD_QuestDetailedPOP.campaignInfo.rate));
		}
		this.getStaminaLabel.text = stamina.ToString();
		this.InitializeRewardList();
		this.DispMultiBattleBtn();
		yield return null;
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		yield break;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		this.isClosed = true;
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void ShowInfo()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.d_data.dungeon;
		ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies = dungeon.encountEnemies;
		this.ngTX_A_STAGE_DNG_NUMBER.text = this.StageNum;
		this.ngTX_A_STAGE_DNG_TITLE.text = this.d_data.worldDungeonM.name;
		this.ngTX_A_STAGE_DNG_DESCRIPT.text = this.d_data.worldDungeonM.description;
		this.getEXPLabel.text = dungeon.exp.ToString();
		this.getClusterLabel.text = StringFormat.Cluster(dungeon.money);
		int num = 0;
		int i;
		for (i = 0; i < dungeon.encountEnemies.Length; i++)
		{
			if (num >= this.goENCOUNT_MONS_LIST.Count)
			{
				break;
			}
			GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy encountEnemy = dungeon.encountEnemies[i];
			if (encountEnemy.type == 2 || encountEnemy.type == 4)
			{
				string text = encountEnemy.monsterId.ToString();
				if (text != null && text != string.Empty)
				{
					MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(text);
					if (monsterData != null)
					{
						GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, this.goENCOUNT_MONS_LIST[num].transform.localScale, this.goENCOUNT_MONS_LIST[num].transform.localPosition, this.goENCOUNT_MONS_LIST[num].transform.parent, true, false);
						DepthController depthController = guimonsterIcon.GetDepthController();
						depthController.AddWidgetDepth(guimonsterIcon.transform, 40);
						guimonsterIcon.SetTouchAct_S(delegate(MonsterData tappedMonsterData)
						{
							this.ActCallBackEncMons(tappedMonsterData, encountEnemy.resistanceId);
						});
						this.goENCOUNT_MONS_LIST[num].SetActive(false);
						num++;
					}
				}
			}
		}
		if (this.d_data.dungeon.isExtraWave == 1 && num < this.goENCOUNT_MONS_LIST.Count)
		{
			GUIMonsterIcon guimonsterIcon2 = MonsterDataMng.Instance().MakeQuestionPrefab(this.goENCOUNT_MONS_LIST[num].transform.localScale, this.goENCOUNT_MONS_LIST[num].transform.localPosition, (int)(this.goENCOUNT_MONS_LIST[num].transform.localPosition.z + 35f), this.goENCOUNT_MONS_LIST[num].transform.parent);
			guimonsterIcon2.SetTouchAct_S(null);
			guimonsterIcon2.SetTouchAct_L(null);
		}
		while (i < this.goENCOUNT_MONS_LIST.Count)
		{
			this.goENCOUNT_MONS_LIST[i].SetActive(false);
			i++;
		}
		this.ShowDropItems();
		if (CMD_QuestTOP.AreaData.data.type == "3" || CMD_QuestTOP.AreaData.data.type == "4")
		{
			this.goContentsROOT.SetActive(false);
			this.goPointContentsROOT.SetActive(true);
			GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
			for (i = 0; i < respDataMA_EventPointBonusMaster.eventPointBonusM.Length; i++)
			{
				if (respDataMA_EventPointBonusMaster.eventPointBonusM[i].worldDungeonId == this.d_data.worldDungeonM.worldDungeonId && respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectType != "0")
				{
					this.epbList.Add(respDataMA_EventPointBonusMaster.eventPointBonusM[i]);
				}
			}
			this.epbList.Sort(new Comparison<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>(this.CompareEffType));
			for (int j = 0; j < this.bonusPointLabelList.Count; j++)
			{
				this.bonusPointLabelList[j].gameObject.SetActive(false);
				if (j < this.epbList.Count)
				{
					this.bonusPointLabelList[j].gameObject.SetActive(true);
					this.bonusPointLabelList[j].text = this.epbList[j].detail;
				}
			}
			if (this.epbList.Count > this.bonusPointLabelList.Count)
			{
				this.changeBonusButton.SetActive(true);
			}
			else
			{
				this.changeBonusButton.SetActive(false);
			}
		}
		else
		{
			this.goContentsROOT.SetActive(true);
			this.goPointContentsROOT.SetActive(false);
		}
	}

	private void ShowDropItems()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.d_data.dungeon;
		this.dropAssetList = new List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset>();
		for (int i = 0; i < dungeon.dropAssets.Length; i++)
		{
			if (dungeon.dropAssets[i].assetCategoryId != 4 && dungeon.dropAssets[i].assetCategoryId != 5)
			{
				this.dropAssetList.Add(dungeon.dropAssets[i]);
			}
		}
		int num = 0;
		for (int j = 0; j < this.dropAssetList.Count; j++)
		{
			if (j >= this.itemDROP_ITEM_LIST.Count)
			{
				global::Debug.LogError("======================================= CMD_QuestItemPOP Drop Disp Over!!");
				break;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goTEX_DROP);
			Transform transform = gameObject.transform;
			Transform transform2 = this.itemDROP_ITEM_LIST[j].transform;
			transform.parent = transform2.parent;
			transform.localScale = transform2.transform.localScale;
			transform.localPosition = transform2.localPosition;
			int index = j;
			GUICollider component = gameObject.GetComponent<GUICollider>();
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.ActCallBackDropItem(index);
			};
			UIWidget component2 = this.itemDROP_ITEM_LIST[j].GetComponent<UIWidget>();
			UIWidget component3 = gameObject.GetComponent<UIWidget>();
			component3.depth = component2.depth;
			string assetCategoryId = this.dropAssetList[j].assetCategoryId.ToString();
			string objectId = this.dropAssetList[j].assetValue.ToString();
			this.itemDROP_ITEM_LIST[j].SetItem(assetCategoryId, objectId, "1", true, null);
			BoxCollider[] componentsInChildren = this.itemDROP_ITEM_LIST[j].GetComponentsInChildren<BoxCollider>();
			if (componentsInChildren != null)
			{
				foreach (BoxCollider boxCollider in componentsInChildren)
				{
					boxCollider.enabled = false;
				}
			}
			num++;
		}
		if (this.d_data.dungeon.isExtraWave == 1 && num < this.itemDROP_ITEM_LIST.Count)
		{
			GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakeQuestionPrefab(new Vector3(0.46f, 0.46f, 1f), this.itemDROP_ITEM_LIST[num].transform.localPosition, (int)(this.itemDROP_ITEM_LIST[num].transform.localPosition.z + 35f), this.itemDROP_ITEM_LIST[num].transform.parent);
			guimonsterIcon.SetTouchAct_S(null);
			guimonsterIcon.SetTouchAct_L(null);
		}
		this.goTEX_DROP.SetActive(false);
	}

	public void OnTapBonusChange()
	{
		if (this.bonusChange)
		{
			return;
		}
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_107", 0f, false, true, null, -1);
		this.bonusChange = true;
		base.StartCoroutine(this.BonusChangeAnima());
	}

	private IEnumerator BonusChangeAnima()
	{
		this.bonusPosTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 1f;
		this.bonusPosTween.to = 0f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		this.epbListViewCnt++;
		int count = this.epbList.Count / this.bonusPointLabelList.Count;
		if (this.epbList.Count % this.bonusPointLabelList.Count != 0)
		{
			count++;
		}
		int viewNum = this.epbListViewCnt % count;
		for (int i = 0; i < this.bonusPointLabelList.Count; i++)
		{
			this.bonusPointLabelList[i].gameObject.SetActive(false);
			if (i + viewNum * this.bonusPointLabelList.Count < this.epbList.Count)
			{
				this.bonusPointLabelList[i].gameObject.SetActive(true);
				this.bonusPointLabelList[i].text = this.epbList[i + viewNum * this.bonusPointLabelList.Count].detail;
			}
		}
		UnityEngine.Object.Destroy(this.bonusPosTween);
		this.bonusPosTween = null;
		this.bonusPosTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
		this.bonusPosTween.from = 0f;
		this.bonusPosTween.to = 1f;
		this.bonusPosTween.duration = 0.2f;
		this.bonusPosTween.PlayForward();
		yield return new WaitForSeconds(0.2f);
		this.bonusChange = false;
		yield break;
	}

	private int CompareEffType(GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus x, GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus y)
	{
		int num = int.Parse(x.effectType);
		int num2 = int.Parse(y.effectType);
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	private void ActCallBackEncMons(MonsterData md, int resistanceId)
	{
		if (this.isClosed)
		{
			return;
		}
		CMD_QuestMonsterPOP.MonsterData = md;
		CMD_QuestMonsterPOP.ResistanceId = resistanceId;
		GUIMain.ShowCommonDialog(null, "CMD_QuestMonsterPOP");
	}

	private void ActCallBackDropItem(int idx)
	{
		if (this.isClosed)
		{
			return;
		}
		MasterDataMng.AssetCategory assetCategoryId = (MasterDataMng.AssetCategory)this.dropAssetList[idx].assetCategoryId;
		string text = this.dropAssetList[idx].assetValue.ToString();
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(this.dropAssetList[idx].assetCategoryId.ToString());
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.DIGI_STONE:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		case MasterDataMng.AssetCategory.TIP:
			CMD_QuestItemPOP.Create(assetCategory);
			break;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(text);
			CMD_QuestItemPOP.Create(itemM);
			break;
		}
		case MasterDataMng.AssetCategory.SOUL:
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(text);
			CMD_QuestItemPOP.Create(soul);
			break;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(text);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(int.Parse(facilityConditionM.releaseId));
			CMD_QuestItemPOP.Create(facilityConditionM, text, facilityMasterByReleaseId);
			break;
		}
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(text);
			CMD_QuestItemPOP.Create(chipMainData);
			break;
		}
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			string ticketValue = text;
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM data = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => x.dungeonTicketId == ticketValue);
			CMD_QuestItemPOP.Create(data);
			break;
		}
		}
	}

	private void InitializeRewardList()
	{
		List<GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward> list = new List<GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward worldDungeonReward in MasterDataMng.Instance().RespDataMA_WorldDungeonRewardM.worldDungeonRewardM)
		{
			if (worldDungeonReward.worldDungeonId == this.d_data.worldDungeonM.worldDungeonId && worldDungeonReward.everyTimeFlg == "0")
			{
				list.Add(worldDungeonReward);
			}
		}
		switch (this.d_data.status)
		{
		case 2:
		case 3:
			this.rewardIconRoot.SetRewardList(list, Color.white);
			break;
		case 4:
			this.rewardIconRoot.SetRewardList(list, this.clearColor);
			break;
		}
	}

	private void DispMultiBattleBtn()
	{
		if (this.goMultiBattleBtn != null)
		{
			this.goMultiBattleBtn.SetActive(true);
		}
	}

	private void OnTapMultiBattleBtn()
	{
		GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseMultiBattleMenu), "CMD_MultiBattleParticipateMenu");
	}

	private void OnCloseMultiBattleMenu(int idx)
	{
		int forceReturnValue;
		switch (idx)
		{
		case 1:
			forceReturnValue = 10;
			goto IL_31;
		case 2:
			forceReturnValue = 20;
			goto IL_31;
		}
		forceReturnValue = 99;
		IL_31:
		base.SetForceReturnValue(forceReturnValue);
		base.ClosePanel(true);
	}
}