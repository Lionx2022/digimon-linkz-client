﻿using System;

namespace UnityEngine.StyleSheets
{
	[Serializable]
	internal struct StyleValueHandle
	{
		[SerializeField]
		private StyleValueType m_ValueType;

		[SerializeField]
		internal int valueIndex;

		internal StyleValueHandle(int valueIndex, StyleValueType valueType)
		{
			this.valueIndex = valueIndex;
			this.m_ValueType = valueType;
		}

		public StyleValueType valueType
		{
			get
			{
				return this.m_ValueType;
			}
			internal set
			{
				this.m_ValueType = value;
			}
		}
	}
}
