﻿using System;
using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	/// <summary>
	///   <para>Identifies a specific game instance.</para>
	/// </summary>
	[DefaultValue(18446744073709551615UL)]
	public enum SourceID : ulong
	{
		/// <summary>
		///   <para>Invalid SourceID.</para>
		/// </summary>
		Invalid = 18446744073709551615UL
	}
}