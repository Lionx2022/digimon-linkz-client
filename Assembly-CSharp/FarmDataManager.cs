﻿using Facility;
using FarmData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FarmDataManager
{
	public const int DEFAULT_LIMIT_MEAT_MAX = 50;

	public const int LIMIT_BUILDING_COUNT = 2;

	public static List<FacilityM> facilityMaster;

	public static List<FacilityKeyM> facilityKeyMaster;

	public static List<FacilityUpgradeM> facilityUpgradeMaster;

	public static List<FacilityMeatFieldM> facilityMeatFieldMaster;

	public static List<FacilityChipM> facilityChipFieldMaster;

	public static List<MonsterFixidM> monsterFixidMaster;

	public static List<FacilityWarehouseM> facilityWarehouseMaster;

	public static List<FacilityExpUpM> facilityExpUpMaster;

	public static List<FacilityRestaurantM> facilityRestaurantMaster;

	public static List<FacilityHouseM> facilityHouseMaster;

	public static List<FacilityConditionM> facilityConditionMaster;

	public static List<FacilityExtraEffectM> facilityExtraEffectMaster;

	public static List<FacilityExtraEffectLevelM> facilityExtraEffectLevelMaster;

	private static FarmFacilityData facilityInfo;

	private static FarmFacilityAnimationData facilityAnimationData;

	private static RuntimeAnimatorController facilityAnimator;

	private static void InitList<T>(ref List<T> list) where T : class, new()
	{
		if (list == null)
		{
			list = new List<T>();
		}
		else
		{
			list.Clear();
		}
	}

	public static void SetResponseData(WebAPI.ResponseData response)
	{
		if (response == null)
		{
			return;
		}
		if (typeof(GameWebAPI.RespDataMA_GetFacilityM) == response.GetType())
		{
			GameWebAPI.RespDataMA_GetFacilityM respDataMA_GetFacilityM = (GameWebAPI.RespDataMA_GetFacilityM)response;
			FarmDataManager.facilityMaster = respDataMA_GetFacilityM.facilityM.Where((FacilityM x) => int.Parse(x.facilityId) != 6).ToList<FacilityM>();
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityKeyM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityKeyM>(ref FarmDataManager.facilityKeyMaster);
			FarmDataManager.facilityKeyMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityKeyM)response).facilityKeyM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityUpgradeM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityUpgradeM>(ref FarmDataManager.facilityUpgradeMaster);
			FarmDataManager.facilityUpgradeMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityUpgradeM)response).facilityUpgradeM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityMeatFieldM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityMeatFieldM>(ref FarmDataManager.facilityMeatFieldMaster);
			FarmDataManager.facilityMeatFieldMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityMeatFieldM)response).facilityMeatFieldM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityWarehouseM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityWarehouseM>(ref FarmDataManager.facilityWarehouseMaster);
			FarmDataManager.facilityWarehouseMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityWarehouseM)response).facilityWarehouseM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityExpUpM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityExpUpM>(ref FarmDataManager.facilityExpUpMaster);
			FarmDataManager.facilityExpUpMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityExpUpM)response).facilityExpUpM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityRestaurantM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityRestaurantM>(ref FarmDataManager.facilityRestaurantMaster);
			FarmDataManager.facilityRestaurantMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityRestaurantM)response).facilityRestaurantM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityHouseM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityHouseM>(ref FarmDataManager.facilityHouseMaster);
			FarmDataManager.facilityHouseMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityHouseM)response).facilityHouseM);
		}
		else if (typeof(GameWebAPI.RespDataMA_FacilityConditionM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityConditionM>(ref FarmDataManager.facilityConditionMaster);
			FarmDataManager.facilityConditionMaster.AddRange(((GameWebAPI.RespDataMA_FacilityConditionM)response).facilityConditionM);
		}
		else if (typeof(GameWebAPI.RespDataMA_FacilityExtraEffectM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityExtraEffectM>(ref FarmDataManager.facilityExtraEffectMaster);
			FarmDataManager.facilityExtraEffectMaster.AddRange(((GameWebAPI.RespDataMA_FacilityExtraEffectM)response).facilityExtraEffectM);
		}
		else if (typeof(GameWebAPI.RespDataMA_FacilityExtraEffectLevelM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityExtraEffectLevelM>(ref FarmDataManager.facilityExtraEffectLevelMaster);
			FarmDataManager.facilityExtraEffectLevelMaster.AddRange(((GameWebAPI.RespDataMA_FacilityExtraEffectLevelM)response).facilityExtraEffectLevelM);
		}
		else if (typeof(GameWebAPI.RespDataMA_GetFacilityChipM) == response.GetType())
		{
			FarmDataManager.InitList<FacilityChipM>(ref FarmDataManager.facilityChipFieldMaster);
			FarmDataManager.facilityChipFieldMaster.AddRange(((GameWebAPI.RespDataMA_GetFacilityChipM)response).facilityChipM);
		}
		else if (typeof(GameWebAPI.RespDataMA_MonsterFixidM) == response.GetType())
		{
			FarmDataManager.InitList<MonsterFixidM>(ref FarmDataManager.monsterFixidMaster);
			FarmDataManager.monsterFixidMaster.AddRange(((GameWebAPI.RespDataMA_MonsterFixidM)response).monsterFixedValueM);
		}
	}

	public static FacilityM GetFacilityMaster(int facilityID)
	{
		if (FarmDataManager.facilityMaster != null)
		{
			string b = facilityID.ToString();
			for (int i = 0; i < FarmDataManager.facilityMaster.Count; i++)
			{
				if (FarmDataManager.facilityMaster[i].facilityId == b)
				{
					return FarmDataManager.facilityMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityM GetFacilityMasterByReleaseId(int releaseId)
	{
		if (FarmDataManager.facilityMaster != null)
		{
			string b = releaseId.ToString();
			for (int i = 0; i < FarmDataManager.facilityMaster.Count; i++)
			{
				if (FarmDataManager.facilityMaster[i].releaseId == b)
				{
					return FarmDataManager.facilityMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityKeyM GetFacilityKeyMaster(string facilityKey)
	{
		if (FarmDataManager.facilityKeyMaster != null)
		{
			for (int i = 0; i < FarmDataManager.facilityMaster.Count; i++)
			{
				if (FarmDataManager.facilityKeyMaster[i].facilityKeyId == facilityKey)
				{
					return FarmDataManager.facilityKeyMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityUpgradeM GetFacilityUpgradeMaster(int facilityID, int level)
	{
		if (FarmDataManager.facilityUpgradeMaster != null)
		{
			string b = facilityID.ToString();
			string b2 = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityUpgradeMaster.Count; i++)
			{
				if (FarmDataManager.facilityUpgradeMaster[i].facilityId == b && FarmDataManager.facilityUpgradeMaster[i].level == b2)
				{
					return FarmDataManager.facilityUpgradeMaster[i];
				}
			}
		}
		return null;
	}

	public static string GetFacilityEffectDetail(int facilityID, int level)
	{
		string result = string.Empty;
		switch (facilityID)
		{
		case 1:
		{
			FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(level);
			result = facilityMeatFarmMaster.maxMeatNum;
			break;
		}
		case 2:
		{
			FacilityWarehouseM facilityStorehouseMaster = FarmDataManager.GetFacilityStorehouseMaster(level);
			result = facilityStorehouseMaster.limitMeatNum;
			break;
		}
		case 3:
		{
			FacilityRestaurantM facilityRestaurantM = FarmDataManager.GetFacilityRestaurantMaster(level);
			result = facilityRestaurantM.maxStamina;
			break;
		}
		default:
			switch (facilityID)
			{
			case 23:
				result = FarmDataManager.GetEvolutionEffectValue(level);
				break;
			case 25:
			{
				FacilityChipM facilityChipFarmMaster = FarmDataManager.GetFacilityChipFarmMaster(level);
				result = facilityChipFarmMaster.maxChipNum;
				break;
			}
			}
			break;
		case 7:
		{
			FacilityHouseM facilityDigiHouseMaster = FarmDataManager.GetFacilityDigiHouseMaster(level);
			result = facilityDigiHouseMaster.maxMonsterNum;
			break;
		}
		case 8:
		{
			FacilityExpUpM facilityDigimonGymMaster = FarmDataManager.GetFacilityDigimonGymMaster(level);
			result = facilityDigimonGymMaster.upRate;
			break;
		}
		}
		return result;
	}

	public static string GetFacilityDescription(int facilityId, int level)
	{
		FacilityM facilityM = FarmDataManager.GetFacilityMaster(facilityId);
		string result = string.Empty;
		if (facilityId == 23)
		{
			result = facilityM.description + FarmDataManager.GetEvolutionEffectDesctiption(level);
		}
		else
		{
			result = facilityM.description;
		}
		return result;
	}

	public static FacilityMeatFieldM GetFacilityMeatFarmMaster(int level)
	{
		if (FarmDataManager.facilityMeatFieldMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityMeatFieldMaster.Count; i++)
			{
				if (FarmDataManager.facilityMeatFieldMaster[i].level == b)
				{
					return FarmDataManager.facilityMeatFieldMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityChipM GetFacilityChipFarmMaster(int level)
	{
		if (FarmDataManager.facilityChipFieldMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityChipFieldMaster.Count; i++)
			{
				if (FarmDataManager.facilityChipFieldMaster[i].level == b)
				{
					return FarmDataManager.facilityChipFieldMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityWarehouseM GetFacilityStorehouseMaster(int level)
	{
		if (FarmDataManager.facilityWarehouseMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityWarehouseMaster.Count; i++)
			{
				if (FarmDataManager.facilityWarehouseMaster[i].level == b)
				{
					return FarmDataManager.facilityWarehouseMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityExpUpM GetFacilityDigimonGymMaster(int level)
	{
		if (FarmDataManager.facilityExpUpMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityExpUpMaster.Count; i++)
			{
				if (FarmDataManager.facilityExpUpMaster[i].level == b)
				{
					return FarmDataManager.facilityExpUpMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityRestaurantM GetFacilityRestaurantMaster(int level)
	{
		if (FarmDataManager.facilityRestaurantMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityRestaurantMaster.Count; i++)
			{
				if (FarmDataManager.facilityRestaurantMaster[i].level == b)
				{
					return FarmDataManager.facilityRestaurantMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityHouseM GetFacilityDigiHouseMaster(int level)
	{
		if (FarmDataManager.facilityHouseMaster != null)
		{
			string b = level.ToString();
			for (int i = 0; i < FarmDataManager.facilityHouseMaster.Count; i++)
			{
				if (FarmDataManager.facilityHouseMaster[i].level == b)
				{
					return FarmDataManager.facilityHouseMaster[i];
				}
			}
		}
		return null;
	}

	public static FacilityM[] GetFacilityShopGoods(FacilityType type)
	{
		List<FacilityM> list = new List<FacilityM>();
		int typeInt = (int)type;
		if (type == FacilityType.FACILITY)
		{
			list = FarmDataManager.facilityMaster.Where(delegate(FacilityM x)
			{
				int fid = int.Parse(x.facilityId);
				return int.Parse(x.type) == typeInt && !FarmDataManager.FacilityInfo.initBuild.Any((FarmFacilityData.FacilityID id) => id == (FarmFacilityData.FacilityID)fid);
			}).ToList<FacilityM>();
		}
		else
		{
			list = FarmDataManager.facilityMaster.Where(delegate(FacilityM x)
			{
				int fid = int.Parse(x.facilityId);
				return int.Parse(x.type) == typeInt && !FarmDataManager.FacilityInfo.initBuild.Any((FarmFacilityData.FacilityID id) => id == (FarmFacilityData.FacilityID)fid);
			}).ToList<FacilityM>();
		}
		list.Sort(new Comparison<FacilityM>(FarmDataManager.CompareFacility));
		return list.ToArray();
	}

	private static int CompareFacility(FacilityM x, FacilityM y)
	{
		int num = int.Parse(x.sort);
		int num2 = int.Parse(y.sort);
		return (num <= num2) ? ((num >= num2) ? 0 : -1) : 1;
	}

	public static FacilityConditionM[] GetFacilityCondition(string releaseId)
	{
		return FarmDataManager.facilityConditionMaster.Where((FacilityConditionM x) => x.releaseId == releaseId).ToArray<FacilityConditionM>();
	}

	public static FarmFacilityData FacilityInfo
	{
		get
		{
			return FarmDataManager.facilityInfo;
		}
		set
		{
			FarmDataManager.facilityInfo = value;
		}
	}

	public static FarmFacilityAnimationData FacilityAnimationData
	{
		get
		{
			return FarmDataManager.facilityAnimationData;
		}
		set
		{
			FarmDataManager.facilityAnimationData = value;
		}
	}

	public static RuntimeAnimatorController FacilityAnimator
	{
		get
		{
			return FarmDataManager.facilityAnimator;
		}
		set
		{
			FarmDataManager.facilityAnimator = value;
		}
	}

	public static APIRequestTask RequestUserNewFacility(FacilityType facilityType, Action<UserNewFacilityResponse> onSuccessed, bool requestRetry = true)
	{
		RequestFA_UserNewFacility request = new RequestFA_UserNewFacility
		{
			SetSendData = delegate(UserNewFacilityRequest param)
			{
				param.type = (int)facilityType;
			},
			OnReceived = onSuccessed
		};
		return new APIRequestTask(request, requestRetry);
	}

	private static string GetEvolutionEffectDesctiption(int level)
	{
		string facilityId = 23.ToString();
		string level2 = level.ToString();
		FacilityExtraEffectLevelM[] facilityExtraEffectLevelM = FarmDataManager.GetFacilityExtraEffectLevelM(facilityId, level2);
		FacilityExtraEffectM[] facilityExtraEffectMasterList = FarmDataManager.GetFacilityExtraEffectMasterList(facilityExtraEffectLevelM);
		string text;
		if (0 < facilityExtraEffectMasterList.Length)
		{
			text = facilityExtraEffectMasterList[0].effectValue;
			for (int i = 1; i < facilityExtraEffectMasterList.Length; i++)
			{
				text = text + "・" + facilityExtraEffectMasterList[i].detail;
			}
		}
		else
		{
			text = string.Empty;
		}
		return text;
	}

	private static string GetEvolutionEffectValue(int level)
	{
		string facilityId = 23.ToString();
		string level2 = level.ToString();
		FacilityExtraEffectLevelM[] facilityExtraEffectLevelM = FarmDataManager.GetFacilityExtraEffectLevelM(facilityId, level2);
		FacilityExtraEffectM[] facilityExtraEffectMasterList = FarmDataManager.GetFacilityExtraEffectMasterList(facilityExtraEffectLevelM);
		string result;
		if (1 < facilityExtraEffectMasterList.Length)
		{
			result = "0";
		}
		else if (facilityExtraEffectMasterList.Length == 1)
		{
			result = facilityExtraEffectMasterList[0].effectValue;
		}
		else
		{
			result = "0";
		}
		return result;
	}

	private static FacilityExtraEffectLevelM[] GetFacilityExtraEffectLevelM(string facilityId, string level)
	{
		return FarmDataManager.facilityExtraEffectLevelMaster.Where((FacilityExtraEffectLevelM x) => x.facilityId == facilityId && x.level == level).ToArray<FacilityExtraEffectLevelM>();
	}

	private static FacilityExtraEffectM[] GetFacilityExtraEffectMasterList(FacilityExtraEffectLevelM[] extraEffectLevelList)
	{
		List<FacilityExtraEffectM> list = new List<FacilityExtraEffectM>();
		for (int i = 0; i < extraEffectLevelList.Length; i++)
		{
			FacilityExtraEffectM facilityExtraEffectM = FarmDataManager.GetFacilityExtraEffectM(extraEffectLevelList[i].facilityExtraEffectId);
			if (facilityExtraEffectM != null)
			{
				list.Add(facilityExtraEffectM);
			}
		}
		return list.ToArray();
	}

	private static FacilityExtraEffectM GetFacilityExtraEffectM(string extraEffectId)
	{
		FacilityExtraEffectM result = null;
		for (int i = 0; i < FarmDataManager.facilityExtraEffectMaster.Count; i++)
		{
			if (FarmDataManager.facilityExtraEffectMaster[i].facilityExtraEffectId == extraEffectId)
			{
				result = FarmDataManager.facilityExtraEffectMaster[i];
				break;
			}
		}
		return result;
	}
}