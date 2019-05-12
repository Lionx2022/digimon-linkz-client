﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsTitleBase : UtilForCMD
{
	private const string RETURN_SPRITE_NAME = "Common02_Btn_ReturnON";

	[SerializeField]
	private UILabel ngTXT_TITLE;

	[SerializeField]
	private GUICollider colBTN_RETURN;

	[SerializeField]
	private UISprite sprBTN_RETURN;

	[SerializeField]
	private GUICollider colBTN_CLOSE;

	private Action<int> actCallBackReturn;

	private Action<int> actCallBackClose;

	protected override void Awake()
	{
		this.colBTN_RETURN.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			if (flag)
			{
				this.ReturnAct();
			}
		};
		this.colBTN_CLOSE.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			if (flag)
			{
				this.CloseAct();
			}
		};
		base.Awake();
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	private void ReturnAct()
	{
		if (this.actCallBackReturn != null)
		{
			this.actCallBackReturn(0);
		}
	}

	private void CloseAct()
	{
		if (this.actCallBackClose != null)
		{
			this.actCallBackClose(0);
		}
	}

	public void SetTitle(string str)
	{
		this.ngTXT_TITLE.text = str;
	}

	public void SetReturnAct(Action<int> act)
	{
		this.colBTN_RETURN.activeCollider = true;
		this.sprBTN_RETURN.spriteName = "Common02_Btn_ReturnON";
		this.actCallBackReturn = act;
	}

	public void SetCloseAct(Action<int> act)
	{
		this.colBTN_CLOSE.activeCollider = true;
		this.actCallBackClose = act;
	}

	public void DisableCloseBtn(bool flg = true)
	{
		if (flg)
		{
			this.colBTN_CLOSE.gameObject.SetActive(false);
		}
		else
		{
			this.colBTN_CLOSE.gameObject.SetActive(true);
		}
	}

	public override void SetParamToCMD()
	{
		CMD cs = base.FindParentCMD();
		if (cs != null)
		{
			Dictionary<string, CommonDialog> dialogDic = GUIManager.GetDialogDic();
			cs.PartsTitle = this;
			cs.CanClosePanelRecursive = true;
			base.DisableCMD_CallBack(this.myTransform);
			int num = 0;
			foreach (string key in dialogDic.Keys)
			{
				CMD cmd = (CMD)dialogDic[key];
				if (cmd != null && cmd.useCMDAnim)
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.SetReturnAct(delegate(int i)
				{
					cs.ClosePanel(true);
				});
			}
			this.SetCloseAct(delegate(int i)
			{
				if (cs.useCMDAnim)
				{
					cs.closeAll();
				}
				else
				{
					cs.ClosePanelRecursive = true;
					cs.ClosePanel(true);
				}
			});
		}
	}
}
