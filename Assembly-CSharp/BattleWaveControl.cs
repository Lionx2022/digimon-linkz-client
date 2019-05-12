﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleWaveControl : BattleFunctionBase
{
	public void CharacterWaveReset(int waveNumber, bool isRecover = false)
	{
		this.BattleWaveChangeResourcesCleaning();
		base.battleStateData.enemies = base.battleStateData.preloadEnemies[waveNumber];
		base.battleStateData.leaderEnemyCharacter = base.battleStateData.enemies[base.hierarchyData.leaderCharacter];
		int cameraType = base.hierarchyData.batteWaves[waveNumber].cameraType;
		string spawnPointId = ResourcesPath.ReservedID.SpawnPoints.GetSpawnPointId(base.battleStateData.enemies.Length, base.battleMode, cameraType);
		SpawnPointParams @object = base.battleStateData.preloadSpawnPoints.GetObject(spawnPointId);
		base.battleStateData.ApplySpawnPoint(@object);
		base.stateManager.threeDAction.HideAllPreloadEnemiesAction();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < base.battleStateData.enemies.Length; i++)
		{
			string prefabId = base.battleStateData.enemies[i].characterStatus.prefabId;
			int num = 0;
			if (dictionary.ContainsKey(prefabId))
			{
				Dictionary<string, int> dictionary3;
				Dictionary<string, int> dictionary2 = dictionary3 = dictionary;
				string key2;
				string key = key2 = prefabId;
				int num2 = dictionary3[key2];
				dictionary2[key] = num2 + 1;
				num = dictionary[prefabId];
			}
			else
			{
				dictionary.Add(prefabId, 0);
			}
			this.WaveResetEnemyResetupFunction(prefabId, num, i);
			int index = base.battleStateData.playerCharacters.Length + i;
			base.stateManager.uiControl.ApplyCharacterHudBoss(index, base.hierarchyData.batteWaves[waveNumber].enemiesBossFlag[i]);
			base.stateManager.uiControl.ApplyCharacterHudContent(index, base.battleStateData.enemies[i]);
			base.stateManager.uiControl.ApplyCharacterHudReset(index);
		}
		base.stateManager.uiControl.ApplyBigBossCharacterHudBoss(false);
		base.stateManager.uiControl.ApplyBigBossCharacterHudContent(base.battleStateData.enemies[0]);
		base.stateManager.uiControl.ApplyBigBossCharacterHudReset();
		for (int j = 0; j < base.battleStateData.playerCharacters.Length; j++)
		{
			this.WaveResetPlayerResetupFunction(j);
			CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[j];
			if (!characterStateControl.isDied)
			{
				if (!isRecover)
				{
					if (characterStateControl.WaveCountInitialize(base.hierarchyData.batteWaves[waveNumber].hpRevivalPercentage))
					{
						base.battleStateData.isRoundStartHpRevival[j] = true;
					}
				}
				if (waveNumber == 0)
				{
					base.stateManager.uiControl.ApplyCharacterHudBoss(j, false);
					base.stateManager.uiControl.ApplyCharacterHudContent(j, characterStateControl);
					base.stateManager.uiControl.ApplyCharacterHudReset(j);
				}
			}
		}
	}

	private void BattleWaveChangeResourcesCleaning()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	private void WaveResetEnemyResetupFunction(string prefabId, int num, int i)
	{
		if (base.isSkipAction)
		{
			return;
		}
		CharacterParams @object = base.battleStateData.preloadEnemiesParams.GetObject(prefabId, num.ToString());
		@object.gameObject.SetActive(true);
		@object.Initialize(base.hierarchyData.cameraObject.camera3D);
		@object.PlayIdleAnimation();
		@object.transform.position = base.battleStateData.enemiesSpawnPoint[i].position;
		@object.transform.rotation = base.battleStateData.enemiesSpawnPoint[i].rotation;
		base.battleStateData.enemies[i].CharacterParams = @object;
		base.battleStateData.enemies[i].InitializeAp();
		ThreeDHoldPressButton threeDHoldPressButton = @object.gameObject.GetComponent<ThreeDHoldPressButton>();
		if (threeDHoldPressButton == null)
		{
			threeDHoldPressButton = @object.gameObject.AddComponent<ThreeDHoldPressButton>();
			threeDHoldPressButton.camera3D = base.hierarchyData.cameraObject.camera3D;
		}
		else
		{
			threeDHoldPressButton.onHoldWaitPress.Clear();
			threeDHoldPressButton.onDisengagePress.Clear();
		}
		threeDHoldPressButton.waitPressCall = 0.2f;
		BattleInputUtility.AddEvent(threeDHoldPressButton.onHoldWaitPress, new Action<int>(base.stateManager.input.OnShowEnemyDescription3D), i);
		BattleInputUtility.AddEvent(threeDHoldPressButton.onDisengagePress, new Action(base.stateManager.input.OnHideEnemyDescriotion3D));
		@object.gameObject.SetActive(false);
	}

	private void WaveResetPlayerResetupFunction(int i)
	{
		if (base.isSkipAction)
		{
			return;
		}
		Transform transform = base.battleStateData.playerCharactersSpawnPoint[i];
		CharacterParams characterParams = base.battleStateData.playerCharacters[i].CharacterParams;
		AlwaysEffectParams alwaysEffectParams = base.battleStateData.revivalReservedEffect[i];
		characterParams.transform.position = transform.position;
		characterParams.transform.rotation = transform.rotation;
		characterParams.gameObject.SetActive(false);
		alwaysEffectParams.transform.position = transform.position;
		alwaysEffectParams.transform.rotation = transform.rotation;
	}

	public void RoundCountingFunction(CharacterStateControl[] totalCharacters, int[] apRevivals)
	{
		for (int i = 0; i < totalCharacters.Length; i++)
		{
			CharacterStateControl characterStateControl = totalCharacters[i];
			if (!characterStateControl.isDied)
			{
				bool flag = false;
				if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep) && characterStateControl.currentSufferState.onSleep.GetSleepGetupOccurrence(false))
				{
					characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Sleep);
				}
				if (characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.ApRevival))
				{
					int revivalAp = characterStateControl.currentSufferState.onApRevival.GetRevivalAp(characterStateControl.maxAp);
					characterStateControl.ap += revivalAp;
					characterStateControl.upAp += revivalAp;
					flag = true;
				}
				if (apRevivals != null)
				{
					int num = apRevivals[i];
					if (num > 0)
					{
						flag = true;
						characterStateControl.ap += num;
						characterStateControl.upAp += num;
					}
				}
				if (characterStateControl.StartMyRoundApUp() || flag)
				{
					int num2;
					if (characterStateControl.isEnemy)
					{
						num2 = base.battleStateData.playerCharacters.Length + characterStateControl.myIndex;
					}
					else
					{
						num2 = characterStateControl.myIndex;
					}
					base.battleStateData.isRoundStartApRevival[num2] = true;
				}
				characterStateControl.currentSufferState.RoundCount();
			}
			else
			{
				characterStateControl.ApZero();
			}
			characterStateControl.HateReset();
		}
		base.battleStateData.totalRoundNumber++;
		if (!base.battleStateData.GetCharactersDeath(true))
		{
			base.battleStateData.currentRoundNumber++;
		}
		base.stateManager.uiControl.ApplyWaveAndRound(base.battleStateData.currentWaveNumber, base.battleStateData.currentRoundNumber);
	}
}