﻿using System;
using UnityEngine;

public sealed class BattleResultDropInfo : MonoBehaviour
{
	[SerializeField]
	[Header("アイテムのグレーアウトの色")]
	private Color itemGrayOutColor = new Color32(70, 70, 70, byte.MaxValue);

	[SerializeField]
	[Header("箱のアイコン")]
	private UISprite[] boxIcons;

	[Header("ドロップアイテム")]
	[SerializeField]
	private PresentBoxItem[] dropItemItems;

	[SerializeField]
	[Header("運の文字アイコン")]
	private UISprite[] luckIcons;

	[Header("マルチ運プレイヤーアイコン")]
	[SerializeField]
	private GameObject[] goLuckPlayerIcons;

	[SerializeField]
	[Header("マルチの文字アイコン")]
	private UISprite[] multiIcons;

	[Header("ライン達")]
	[SerializeField]
	private GameObject[] lines;

	private int rewardNum;

	private void Start()
	{
		this.HideItems();
		global::Debug.Log(this.itemGrayOutColor);
	}

	private void HideItems()
	{
		foreach (GameObject go in this.lines)
		{
			NGUITools.SetActiveSelf(go, false);
		}
		for (int j = 0; j < this.dropItemItems.Length; j++)
		{
			NGUITools.SetActiveSelf(this.boxIcons[j].gameObject, false);
			NGUITools.SetActiveSelf(this.dropItemItems[j].gameObject, false);
		}
	}

	public void Init()
	{
		foreach (GameObject go in this.lines)
		{
			NGUITools.SetActiveSelf(go, true);
		}
	}

	private enum BoxType
	{
		NONE,
		NORMAL,
		RARE
	}

	private struct DropInfo
	{
		public int categoryId;

		public int num;
	}

	public class LuckDropUserInfo
	{
		public int no;

		public string userId;

		public string userName;

		public string leaderMonsterId;

		public int leaderMonsterLuckNum;
	}
}
