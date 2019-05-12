﻿using System;
using UnityEngine;

public class TurnAction : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[SerializeField]
	[Header("トップのオブジェクト(未使用)")]
	private GameObject topGameObject;

	[Header("スキル名のスキナー")]
	[SerializeField]
	private UIComponentSkinner skillNameSkinner;

	[Header("スキル名のTween")]
	[SerializeField]
	private TweenTransform skinllNameTweenTransform;

	[Header("スキル名のテキスト")]
	[SerializeField]
	private UILabel skillNameText;

	public void ApplyTurnActionBarSwipeout(bool isReset)
	{
		this.skinllNameTweenTransform.ResetToBeginning();
		this.skinllNameTweenTransform.enabled = false;
		if (isReset)
		{
			return;
		}
		this.skinllNameTweenTransform.enabled = true;
		this.skinllNameTweenTransform.PlayForward();
	}

	public void ApplySkillName(bool isShow, string skillName = "", CharacterStateControl CharacterStateControl = null)
	{
		NGUITools.SetActiveSelf(this.skillNameText.gameObject, false);
		NGUITools.SetActiveSelf(this.skillNameText.gameObject, isShow);
		if (!isShow)
		{
			return;
		}
		base.gameObject.SetActive(isShow);
		this.skillNameText.text = skillName;
		this.skillNameSkinner.SetSkins((!CharacterStateControl.isEnemy) ? 0 : 1);
	}
}