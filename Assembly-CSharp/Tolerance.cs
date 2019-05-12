﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tolerance
{
	[SerializeField]
	private Strength _none;

	[SerializeField]
	private Strength _red;

	[SerializeField]
	private Strength _blue;

	[SerializeField]
	private Strength _yellow;

	[SerializeField]
	private Strength _green;

	[SerializeField]
	private Strength _white;

	[SerializeField]
	private Strength _black;

	[SerializeField]
	private Strength _poison;

	[SerializeField]
	private Strength _confusion;

	[SerializeField]
	private Strength _paralysis;

	[SerializeField]
	private Strength _sleep;

	[SerializeField]
	private Strength _stun;

	[SerializeField]
	private Strength _skillLock;

	[SerializeField]
	private Strength _instantDeath;

	public Tolerance(Tolerance tolerance)
	{
		this._none = tolerance.none;
		this._red = tolerance.red;
		this._blue = tolerance.blue;
		this._yellow = tolerance.yellow;
		this._green = tolerance.green;
		this._white = tolerance.white;
		this._black = tolerance.black;
		this._poison = tolerance.poison;
		this._confusion = tolerance.confusion;
		this._paralysis = tolerance.paralysis;
		this._sleep = tolerance.sleep;
		this._stun = tolerance.stun;
		this._skillLock = tolerance.skillLock;
		this._instantDeath = tolerance.instantDeath;
	}

	public Tolerance(Strength noneValue, Strength redValue, Strength blueValue, Strength yellowValue, Strength greenValue, Strength whiteValue, Strength blackValue, Strength poisonValue, Strength confusionValue, Strength paralysisValue, Strength sleepValue, Strength stunValue, Strength skillLockValue, Strength instantDeathValue)
	{
		this._none = noneValue;
		this._red = redValue;
		this._blue = blueValue;
		this._yellow = yellowValue;
		this._green = greenValue;
		this._white = whiteValue;
		this._black = blackValue;
		this._poison = poisonValue;
		this._confusion = confusionValue;
		this._paralysis = paralysisValue;
		this._sleep = sleepValue;
		this._stun = stunValue;
		this._skillLock = skillLockValue;
		this._instantDeath = instantDeathValue;
	}

	public static Tolerance GetNutralTolerance()
	{
		return new Tolerance(Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None, Strength.None);
	}

	public Strength none
	{
		get
		{
			return this._none;
		}
	}

	public Strength red
	{
		get
		{
			return this._red;
		}
	}

	public Strength blue
	{
		get
		{
			return this._blue;
		}
	}

	public Strength yellow
	{
		get
		{
			return this._yellow;
		}
	}

	public Strength green
	{
		get
		{
			return this._green;
		}
	}

	public Strength white
	{
		get
		{
			return this._white;
		}
	}

	public Strength black
	{
		get
		{
			return this._black;
		}
	}

	public Strength poison
	{
		get
		{
			return this._poison;
		}
	}

	public Strength confusion
	{
		get
		{
			return this._confusion;
		}
	}

	public Strength paralysis
	{
		get
		{
			return this._paralysis;
		}
	}

	public Strength sleep
	{
		get
		{
			return this._sleep;
		}
	}

	public Strength stun
	{
		get
		{
			return this._stun;
		}
	}

	public Strength skillLock
	{
		get
		{
			return this._skillLock;
		}
	}

	public Strength instantDeath
	{
		get
		{
			return this._instantDeath;
		}
	}

	public Strength GetAttributeStrength(global::Attribute attribute)
	{
		switch (attribute)
		{
		case global::Attribute.Red:
			return this.red;
		case global::Attribute.Blue:
			return this.blue;
		case global::Attribute.Yellow:
			return this.yellow;
		case global::Attribute.Green:
			return this.green;
		case global::Attribute.White:
			return this.white;
		case global::Attribute.Black:
			return this.black;
		default:
			return this.none;
		}
	}

	public Strength GetAffectEffectStrength(AffectEffect affectEffect)
	{
		if (!Tolerance.OnInfluenceToleranceAffectEffect(affectEffect))
		{
			global::Debug.LogError("Unknown Type : " + affectEffect.ToString());
			return Strength.None;
		}
		switch (affectEffect)
		{
		case AffectEffect.Paralysis:
			return this.paralysis;
		case AffectEffect.Poison:
			return this.poison;
		case AffectEffect.Sleep:
			return this.sleep;
		case AffectEffect.SkillLock:
			return this.skillLock;
		case AffectEffect.Confusion:
			return this.confusion;
		case AffectEffect.Stun:
			return this.stun;
		}
		return this.instantDeath;
	}

	public List<ConstValue.ResistanceType> GetAttributeStrengthList()
	{
		List<ConstValue.ResistanceType> list = new List<ConstValue.ResistanceType>();
		if (this.none == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.NOTHINGNESS);
		}
		if (this.red == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.FIRE);
		}
		if (this.blue == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.WATER);
		}
		if (this.yellow == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.THUNDER);
		}
		if (this.green == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.NATURE);
		}
		if (this.white == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.LIGHT);
		}
		if (this.black == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.DARK);
		}
		if (this.poison == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.POISON);
		}
		if (this.confusion == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.CONFUSION);
		}
		if (this.paralysis == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.PARALYSIS);
		}
		if (this.sleep == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.SLEEP);
		}
		if (this.stun == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.STUN);
		}
		if (this.skillLock == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.SKILL_LOCK);
		}
		if (this.instantDeath == Strength.Strong)
		{
			list.Add(ConstValue.ResistanceType.DEATH);
		}
		return list;
	}

	public override string ToString()
	{
		return string.Format("[Tolerance: none={0}, red={1}, blue={2}, yellow={3}, green={4}, white={5}, black={6}, poison={7}, confusion={8}, paralysis={9}, sleep={10}, stun={11}, skillLock={12}, instantDeath={13}]", new object[]
		{
			this.none,
			this.red,
			this.blue,
			this.yellow,
			this.green,
			this.white,
			this.black,
			this.poison,
			this.confusion,
			this.paralysis,
			this.sleep,
			this.stun,
			this.skillLock,
			this.instantDeath
		});
	}

	public static bool OnInfluenceToleranceAffectEffect(AffectEffect affectEffect)
	{
		return affectEffect == AffectEffect.Poison || affectEffect == AffectEffect.Paralysis || affectEffect == AffectEffect.Sleep || affectEffect == AffectEffect.Stun || affectEffect == AffectEffect.SkillLock || affectEffect == AffectEffect.InstantDeath || affectEffect == AffectEffect.Confusion;
	}

	public int GetAttributeGoodWeak(global::Attribute attribute)
	{
		Strength attributeStrength = this.GetAttributeStrength(attribute);
		return attributeStrength.ToInverseInt();
	}

	private static int GetDifference(Strength based, Strength overrided)
	{
		return overrided.ToInt() - based.ToInt();
	}

	public static ToleranceShifter operator -(Tolerance overrideTolerance, Tolerance toleranceBase)
	{
		return new ToleranceShifter(Tolerance.GetDifference(toleranceBase.none, overrideTolerance.none), Tolerance.GetDifference(toleranceBase.red, overrideTolerance.red), Tolerance.GetDifference(toleranceBase.blue, overrideTolerance.blue), Tolerance.GetDifference(toleranceBase.yellow, overrideTolerance.yellow), Tolerance.GetDifference(toleranceBase.green, overrideTolerance.green), Tolerance.GetDifference(toleranceBase.white, overrideTolerance.white), Tolerance.GetDifference(toleranceBase.black, overrideTolerance.black), Tolerance.GetDifference(toleranceBase.poison, overrideTolerance.poison), Tolerance.GetDifference(toleranceBase.confusion, overrideTolerance.confusion), Tolerance.GetDifference(toleranceBase.paralysis, overrideTolerance.paralysis), Tolerance.GetDifference(toleranceBase.sleep, overrideTolerance.sleep), Tolerance.GetDifference(toleranceBase.stun, overrideTolerance.stun), Tolerance.GetDifference(toleranceBase.skillLock, overrideTolerance.skillLock), Tolerance.GetDifference(toleranceBase.instantDeath, overrideTolerance.instantDeath));
	}
}