﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayStageEffect : BattleStateController
{
	private bool isShowBattleStageEffect;

	private Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>> countDictionary;

	public SubStatePlayStageEffect(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStatePlayHitAnimationAction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		this.isShowBattleStageEffect = false;
		this.countDictionary = new Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>>();
	}

	protected override IEnumerator MainRoutine()
	{
		foreach (EffectStatusBase.EffectTriggerType trigger in base.battleStateData.reqestStageEffectTriggerList)
		{
			IEnumerator function = this.PlayStageEffect(trigger);
			while (function.MoveNext())
			{
				yield return null;
			}
		}
		base.battleStateData.reqestStageEffectTriggerList.Clear();
		yield break;
	}

	protected override void DisabledThisState()
	{
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	private IEnumerator PlayStageEffect(EffectStatusBase.EffectTriggerType effectTriggerType)
	{
		bool isBigBoss = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1;
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, effectTriggerType);
		if (invocationList.Count == 0)
		{
			yield break;
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		string cameraKey = "0002_command";
		if (isBigBoss)
		{
			cameraKey = "BigBoss/0002_command";
		}
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		foreach (ExtraEffectStatus invocation in invocationList)
		{
			IEnumerator function = null;
			EffectStatusBase.ExtraEffectType effectType = (EffectStatusBase.ExtraEffectType)invocation.EffectType;
			EffectStatusBase.ExtraEffectType extraEffectType = effectType;
			if (extraEffectType != EffectStatusBase.ExtraEffectType.LeaderChange)
			{
				function = this.PlayDefaultEffect(invocation);
			}
			else
			{
				function = this.PlayLeaderChange(invocation);
			}
			while (function.MoveNext())
			{
				yield return null;
			}
			if (base.stateManager.IsLastBattleAndAllDeath())
			{
				break;
			}
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		yield break;
	}

	private IEnumerator ShowBattleStageEffect()
	{
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Stage);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
			this.isShowBattleStageEffect = true;
		}
		yield break;
	}

	private IEnumerator PlayLeaderChange(ExtraEffectStatus extraEffectStatus)
	{
		IEnumerator showBattleStageEffect = this.ShowBattleStageEffect();
		while (showBattleStageEffect.MoveNext())
		{
			yield return null;
		}
		int leaderindex = (int)extraEffectStatus.EffectValue;
		bool isRandom = leaderindex <= 0 || leaderindex > 3;
		if (isRandom)
		{
			leaderindex = UnityEngine.Random.Range(0, 3);
		}
		else
		{
			leaderindex--;
		}
		IEnumerator function = null;
		switch (base.battleMode)
		{
		case BattleMode.Single:
			function = this.SingleLeaderChangeSync(leaderindex);
			break;
		case BattleMode.Multi:
			function = this.MultiLeaderChangeSync(leaderindex);
			break;
		case BattleMode.PvP:
			function = this.PvPLeaderChangeSync(leaderindex);
			break;
		}
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator SingleLeaderChangeSync(int leaderindex)
	{
		base.battleStateData.ChangePlayerLeader(leaderindex);
		base.battleStateData.ChangeEnemyLeader(leaderindex);
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator MultiLeaderChangeSync(int leaderindex)
	{
		base.stateManager.uiControlMulti.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		if (base.stateManager.multiFunction.IsOwner)
		{
			IEnumerator sendCancelCharacterRevival = base.stateManager.multiFunction.SendLeaderChange(leaderindex);
			while (sendCancelCharacterRevival.MoveNext())
			{
				object obj = sendCancelCharacterRevival.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator waitAllPlayers = base.stateManager.multiFunction.WaitAllPlayers(TCPMessageType.LeaderChange);
			while (waitAllPlayers.MoveNext())
			{
				object obj2 = waitAllPlayers.Current;
				yield return obj2;
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlMulti.HideLoading();
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PvPLeaderChangeSync(int leaderindex)
	{
		base.stateManager.uiControlPvP.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		if (base.stateManager.pvpFunction.IsOwner)
		{
			IEnumerator sendCancelCharacterRevival = base.stateManager.pvpFunction.SendLeaderChange(leaderindex);
			while (sendCancelCharacterRevival.MoveNext())
			{
				object obj = sendCancelCharacterRevival.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator waitAllPlayers = base.stateManager.pvpFunction.WaitAllPlayers(TCPMessageType.LeaderChange);
			while (waitAllPlayers.MoveNext())
			{
				object obj2 = waitAllPlayers.Current;
				yield return obj2;
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlPvP.HideLoading();
		IEnumerator function = this.PlayLeaderChangeAnimation();
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayLeaderChangeAnimation()
	{
		base.stateManager.uiControl.ApplyBattleStartAction(true);
		base.stateManager.uiControl.ApplyBattleStartActionTitle(false);
		base.stateManager.uiControl.ApplyPlayerLeaderSkill(true, base.battleStateData.leaderCharacter.leaderSkillStatus.name, true);
		base.stateManager.uiControl.ApplyEnemyLeaderSkill(base.battleMode == BattleMode.PvP, base.battleStateData.leaderEnemyCharacter.leaderSkillStatus.name, true);
		base.stateManager.uiControl.ApplyAllMonsterButtonEnable(false);
		string cameraKey = string.Empty;
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			cameraKey = "0001_bossStart";
		}
		else
		{
			cameraKey = "0002_roundStart";
		}
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		base.stateManager.uiControl.ApplyBattleStartAction(false);
		base.stateManager.uiControl.ApplyAllMonsterButtonEnable(true);
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator PlayDefaultEffect(ExtraEffectStatus extraEffectStatus)
	{
		List<CharacterStateControl> targetList = new List<CharacterStateControl>();
		foreach (CharacterStateControl character in base.battleStateData.GetTotalCharacters())
		{
			if (!character.isDied && extraEffectStatus.IsHitExtraEffect(character, EffectStatusBase.ExtraEffectType.Skill))
			{
				targetList.Add(character);
			}
		}
		if (targetList.Count == 0)
		{
			yield break;
		}
		if (!this.isShowBattleStageEffect)
		{
			base.stateManager.uiControl.ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType.Stage);
			yield return null;
			while (base.stateManager.uiControl.IsBattleExtraEffect())
			{
				yield return null;
			}
			base.stateManager.uiControl.HideBattleExtraEffect();
			this.isShowBattleStageEffect = true;
		}
		string key = ((int)extraEffectStatus.EffectValue).ToString();
		SkillStatus status = base.hierarchyData.GetSkillStatus(key);
		IEnumerator playFunction = null;
		if (status != null)
		{
			playFunction = this.PlaySkill(status, targetList);
		}
		else
		{
			playFunction = this.PlayNotSkill(extraEffectStatus, targetList);
		}
		while (playFunction.MoveNext())
		{
			yield return null;
		}
		base.battleStateData.currentDeadCharacters = targetList.Where((CharacterStateControl item) => item.isDied).ToArray<CharacterStateControl>();
		base.SetState(typeof(SubStateEnemiesItemDroppingFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.battleStateData.SEStopFunctionCall();
		while (base.battleStateData.StopHitAnimationCall())
		{
			yield return null;
		}
		base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ApplyHideHitIcon();
		yield break;
	}

	private IEnumerator PlaySkill(SkillStatus status, List<CharacterStateControl> targetList)
	{
		this.countDictionary.Clear();
		foreach (AffectEffectProperty affectEffectProperty in status.affectEffect)
		{
			int hitNumber = 1;
			if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
			{
				hitNumber = affectEffectProperty.hitNumber;
			}
			for (int i = 0; i < hitNumber; i++)
			{
				IEnumerator playSkill = this.PlaySkill(affectEffectProperty, targetList);
				while (playSkill.MoveNext())
				{
					yield return null;
				}
			}
		}
		this.UpdateCount(SufferStateProperty.SufferType.CountGuard);
		this.UpdateCount(SufferStateProperty.SufferType.CountBarrier);
		this.UpdateCount(SufferStateProperty.SufferType.CountEvasion);
		yield break;
	}

	private IEnumerator PlaySkill(AffectEffectProperty affectEffectProperty, List<CharacterStateControl> targetList)
	{
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<int> skillResultDamages = new List<int>();
		List<Strength> skillResultStrength = new List<Strength>();
		List<AffectEffect> skillResultAffectEffect = new List<AffectEffect>();
		foreach (CharacterStateControl target in targetList)
		{
			if (!target.isDied)
			{
				int hitIconDigit = 0;
				bool isHit = affectEffectProperty.OnHit(target);
				Strength strength = Strength.None;
				AffectEffect affectEffect = affectEffectProperty.type;
				if (isHit)
				{
					if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
					{
						strength = target.tolerance.GetAttributeStrength(affectEffectProperty.attribute);
						if (strength == Strength.Invalid)
						{
							affectEffect = AffectEffect.Invalid;
						}
						else if (target.currentSufferState.onTurnBarrier.isActive)
						{
							affectEffect = AffectEffect.TurnBarrier;
						}
						else if (target.currentSufferState.onCountBarrier.isActive)
						{
							affectEffect = AffectEffect.CountBarrier;
							this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, target);
						}
						else if (target.currentSufferState.onTurnEvasion.isActive)
						{
							affectEffect = AffectEffect.TurnEvasion;
						}
						else if (target.currentSufferState.onCountEvasion.isActive)
						{
							affectEffect = AffectEffect.CountEvasion;
							this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, target);
						}
						else
						{
							float reduceDamageRate = SkillStatus.GetReduceDamageRate(target.currentSufferState);
							if (affectEffectProperty.powerType == PowerType.Fixable)
							{
								hitIconDigit = (int)((float)affectEffectProperty.damagePower * reduceDamageRate);
							}
							else
							{
								hitIconDigit = (int)(affectEffectProperty.damagePercent * (float)target.hp * reduceDamageRate);
							}
							if (strength != Strength.Drain)
							{
								target.hp -= hitIconDigit;
							}
							else
							{
								target.hp += hitIconDigit;
							}
							if (target.currentSufferState.onCountGuard.isActive)
							{
								this.AddCountDictionary(SufferStateProperty.SufferType.CountGuard, target);
							}
						}
					}
					else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffectProperty.type))
					{
						strength = target.tolerance.GetAffectEffectStrength(affectEffect);
						if (strength == Strength.Invalid)
						{
							affectEffect = AffectEffect.Invalid;
						}
						else if (target.currentSufferState.onTurnBarrier.isActive)
						{
							affectEffect = AffectEffect.TurnBarrier;
						}
						else if (target.currentSufferState.onCountBarrier.isActive)
						{
							affectEffect = AffectEffect.CountBarrier;
							this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, target);
						}
						else if (target.currentSufferState.onTurnEvasion.isActive)
						{
							affectEffect = AffectEffect.TurnEvasion;
						}
						else if (target.currentSufferState.onCountEvasion.isActive)
						{
							affectEffect = AffectEffect.CountEvasion;
							this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, target);
						}
						else if (affectEffectProperty.type == AffectEffect.InstantDeath)
						{
							target.Kill();
						}
						else
						{
							SufferStateProperty suffer = new SufferStateProperty(affectEffectProperty, base.battleStateData.currentLastGenerateStartTimingSufferState);
							target.currentSufferState.SetSufferState(suffer, null);
							base.battleStateData.currentLastGenerateStartTimingSufferState++;
						}
					}
					else
					{
						AffectEffect type = affectEffectProperty.type;
						if (type != AffectEffect.HpRevival)
						{
							base.stateManager.skillDetails.AddSufferStateOthers(target, affectEffectProperty);
						}
						else
						{
							hitIconDigit = base.stateManager.skillDetails.HpRevival(target, affectEffectProperty);
							skillResultDamages.Add(hitIconDigit);
						}
					}
				}
				skillResultTargets.Add(target);
				skillResultMisses.Add(!isHit);
				skillResultDamages.Add(hitIconDigit);
				skillResultStrength.Add(strength);
				skillResultAffectEffect.Add(affectEffect);
			}
		}
		List<HitIcon> hitIconlist = new List<HitIcon>();
		Vector3[] hitIconPositions = this.GetHitIconPositions(skillResultTargets);
		for (int i = 0; i < skillResultTargets.Count; i++)
		{
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPositions[i], skillResultAffectEffect[i], skillResultDamages[i], skillResultStrength[i], skillResultMisses[i], false, false, false, ExtraEffectType.Non);
			hitIconlist.Add(hitIcon);
		}
		base.battleStateData.SetPlayAnimationActionValues(null, skillResultTargets.ToArray(), affectEffectProperty.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconlist.ToArray(), affectEffectProperty, false, null);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private void AddCountDictionary(SufferStateProperty.SufferType key, CharacterStateControl value)
	{
		if (!this.countDictionary.ContainsKey(key))
		{
			this.countDictionary.Add(key, new List<CharacterStateControl>());
		}
		HaveSufferState currentSufferState = value.currentSufferState;
		if (currentSufferState.FindSufferState(key))
		{
			SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
			if (sufferStateProperty.isMultiHitThrough)
			{
				sufferStateProperty.currentKeepRound--;
				if (sufferStateProperty.currentKeepRound <= 0)
				{
					currentSufferState.RemoveSufferState(key);
				}
			}
			else if (this.countDictionary[key].Contains(value))
			{
				this.countDictionary[key].Add(value);
			}
		}
	}

	private void UpdateCount(SufferStateProperty.SufferType key)
	{
		List<CharacterStateControl> list = null;
		this.countDictionary.TryGetValue(key, out list);
		if (list == null || list.Count == 0)
		{
			return;
		}
		foreach (CharacterStateControl characterStateControl in list)
		{
			HaveSufferState currentSufferState = characterStateControl.currentSufferState;
			if (currentSufferState.FindSufferState(key))
			{
				SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
				if (!sufferStateProperty.isMultiHitThrough)
				{
					sufferStateProperty.currentKeepRound--;
					if (sufferStateProperty.currentKeepRound <= 0)
					{
						currentSufferState.RemoveSufferState(key);
					}
				}
			}
		}
	}

	private IEnumerator PlayNotSkill(ExtraEffectStatus extraEffectStatus, List<CharacterStateControl> targetList)
	{
		yield return null;
		yield break;
	}

	private Vector3[] GetHitIconPositions(List<CharacterStateControl> characters)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < characters.Count; i++)
		{
			Vector3 characterCenterPosition2DFunction = base.stateManager.uiControl.GetCharacterCenterPosition2DFunction(characters[i]);
			list.Add(characterCenterPosition2DFunction);
		}
		return list.ToArray();
	}
}
