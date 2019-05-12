﻿using System;
using UnityEngine;

public sealed class TutorialScene : GUIScreen
{
	[SerializeField]
	private UISprite backgroundBlackOrWhite;

	[SerializeField]
	private GameObject blueBG;

	private GameObject warpBackground;

	[SerializeField]
	private UITexture digitalWorldBG;

	[SerializeField]
	private UITexture digitalWorldCollapseBG;

	protected override void Start()
	{
		base.Start();
		GUICollider.EnableAllCollider(base.gameObject.name);
	}

	public void SetBackGround(TutorialScene.BackGroundType type, Action completed)
	{
		if (type == TutorialScene.BackGroundType.WARP)
		{
			GameObject original = AssetDataMng.Instance().LoadObject("Cutscenes/Tutorial", null, true) as GameObject;
			this.warpBackground = UnityEngine.Object.Instantiate<GameObject>(original);
			this.warpBackground.transform.parent = base.transform;
			this.warpBackground.transform.localScale = Vector3.one;
			this.warpBackground.transform.localPosition = Vector3.zero;
			Resources.UnloadUnusedAssets();
			if (completed != null)
			{
				completed();
			}
		}
		else if (null != this.warpBackground)
		{
			TutorialController component = this.warpBackground.GetComponent<TutorialController>();
			component.endFlg = true;
			component.EndCallBack = delegate()
			{
				this.SetBackGround(type, completed);
			};
			this.warpBackground = null;
		}
		else
		{
			if (type == TutorialScene.BackGroundType.BLACK || type == TutorialScene.BackGroundType.WHITE)
			{
				this.backgroundBlackOrWhite.gameObject.SetActive(true);
				this.backgroundBlackOrWhite.color = ((type != TutorialScene.BackGroundType.BLACK) ? Color.white : Color.black);
			}
			else
			{
				this.backgroundBlackOrWhite.gameObject.SetActive(false);
			}
			if (type == TutorialScene.BackGroundType.BLUE)
			{
				this.blueBG.gameObject.SetActive(true);
			}
			else
			{
				for (int i = 0; i < this.blueBG.transform.childCount; i++)
				{
					Transform child = this.blueBG.transform.GetChild(i);
					UnityEngine.Object.Destroy(child.gameObject);
				}
				this.blueBG.gameObject.SetActive(false);
			}
			this.digitalWorldBG.gameObject.SetActive(TutorialScene.BackGroundType.DIGITAL_WORLD == type);
			this.digitalWorldCollapseBG.gameObject.SetActive(TutorialScene.BackGroundType.DIGITAL_WORLD_COLLAPSE == type);
			if (completed != null)
			{
				completed();
			}
		}
	}

	public void ShakeBackGround(float intensity, float decay, Action completed)
	{
		ObjectShake component = base.gameObject.GetComponent<ObjectShake>();
		if (null != component)
		{
			component.ResetPosition();
			component.StartShake(intensity, decay, completed);
		}
	}

	public void ShakeStopBackGround(float decay, Action completed)
	{
		ObjectShake component = base.gameObject.GetComponent<ObjectShake>();
		if (null != component)
		{
			component.StopShake(decay, completed);
		}
	}

	public void SuspendShakeBackGround()
	{
		ObjectShake component = base.gameObject.GetComponent<ObjectShake>();
		if (null != component)
		{
			component.SuspendShake();
		}
	}

	public enum BackGroundType
	{
		NONE,
		BLACK,
		WHITE,
		BLUE,
		WARP,
		DIGITAL_WORLD,
		DIGITAL_WORLD_COLLAPSE
	}
}
