﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_MonsterParamPop : CMD
{
	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterBasicInfoExpGauge monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistance;

	[SerializeField]
	private MonsterLeaderSkill leaderSkill;

	[SerializeField]
	private MonsterLearnSkill learnSkill1;

	[SerializeField]
	private MonsterLearnSkill learnSkill2;

	[SerializeField]
	private MonsterStatusChangeValueList statusChangeValue;

	[SerializeField]
	private MonsterMedalList monsterMedal;

	[SerializeField]
	private List<GameObject> chipObjList;

	[SerializeField]
	private List<GameObject> statusObjList;

	[SerializeField]
	private UILabel hpUpLabel;

	[SerializeField]
	private UILabel attackUpLabel;

	[SerializeField]
	private UILabel defenseUpLabel;

	[SerializeField]
	private UILabel spAttackUpLabel;

	[SerializeField]
	private UILabel spDefenseUpLabel;

	[SerializeField]
	private UILabel speedUpLabel;

	private int pageCnt;

	public void MonsterDataSet(MonsterData mData, DataMng.ExperienceInfo experienceInfo, int chipSlotNum)
	{
		this.monsterStatusList.ClearValues();
		this.monsterStatusList.SetValues(mData, true);
		this.monsterBasicInfo.ClearMonsterData();
		this.monsterBasicInfo.SetMonsterData(mData, experienceInfo);
		this.monsterResistance.ClearValues();
		this.monsterResistance.SetValues(mData);
		this.leaderSkill.ClearSkill();
		this.leaderSkill.SetSkill(mData);
		this.learnSkill1.ClearSkill();
		this.learnSkill1.SetSkill(mData);
		this.learnSkill2.ClearSkill();
		this.learnSkill2.SetSkill(mData);
		this.SetMedalParameter(this.hpUpLabel, mData.userMonster.hpAbility, mData.Base_HP(int.Parse(mData.userMonster.level)));
		this.SetMedalParameter(this.attackUpLabel, mData.userMonster.attackAbility, mData.Base_ATK(int.Parse(mData.userMonster.level)));
		this.SetMedalParameter(this.defenseUpLabel, mData.userMonster.defenseAbility, mData.Base_DEF(int.Parse(mData.userMonster.level)));
		this.SetMedalParameter(this.spAttackUpLabel, mData.userMonster.spAttackAbility, mData.Base_SATK(int.Parse(mData.userMonster.level)));
		this.SetMedalParameter(this.spDefenseUpLabel, mData.userMonster.spDefenseAbility, mData.Base_SDEF(int.Parse(mData.userMonster.level)));
		this.SetMedalParameter(this.speedUpLabel, mData.userMonster.speedAbility, mData.Base_SPD(int.Parse(mData.userMonster.level)));
		this.monsterMedal.SetActive(true);
		this.monsterMedal.SetValues(mData.userMonster);
		chipSlotNum += 5;
		for (int i = 0; i < this.chipObjList.Count; i++)
		{
			this.chipObjList[i].SetActive(false);
		}
		for (int j = 0; j < chipSlotNum; j++)
		{
			if (j >= this.chipObjList.Count)
			{
				break;
			}
			this.chipObjList[j].SetActive(true);
		}
	}

	private void SetMedalParameter(UILabel label, string ability, float baseStatus)
	{
		int num = 0;
		int.TryParse(ability, out num);
		float num2 = (float)num / 100f;
		int num3 = Mathf.FloorToInt(baseStatus * num2);
		if (num3 > 0)
		{
			label.text = num3.ToString("+0;-0");
		}
	}

	public void PageChange()
	{
		this.pageCnt++;
		if (this.pageCnt >= this.statusObjList.Count)
		{
			this.pageCnt = 0;
		}
		for (int i = 0; i < this.statusObjList.Count; i++)
		{
			this.statusObjList[i].SetActive(false);
		}
		this.statusObjList[this.pageCnt].SetActive(true);
	}

	public void ClosePopup()
	{
		this.ClosePanel(true);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
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
	}
}
