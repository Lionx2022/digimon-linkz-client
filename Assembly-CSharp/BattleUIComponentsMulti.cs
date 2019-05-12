﻿using System;

public class BattleUIComponentsMulti : BattleUIComponentsMultiBasic
{
	[NonSerialized]
	public UITimer continueTimer;

	[NonSerialized]
	public SharedAPMulti sharedApMulti;

	[NonSerialized]
	public EmotionSenderMulti emotionSenderMulti;

	[NonSerialized]
	public RemainingTurn remainingTurnRightDown;

	[NonSerialized]
	public RemainingTurn remainingTurnMiddle;
}
