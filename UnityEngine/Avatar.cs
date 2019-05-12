﻿using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Avatar definition.</para>
	/// </summary>
	public sealed class Avatar : Object
	{
		private Avatar()
		{
		}

		/// <summary>
		///   <para>Return true if this avatar is a valid mecanim avatar. It can be a generic avatar or a human avatar.</para>
		/// </summary>
		public extern bool isValid { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Return true if this avatar is a valid human avatar.</para>
		/// </summary>
		public extern bool isHuman { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetMuscleMinMax(int muscleId, float min, float max);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetParameter(int parameterId, float value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetAxisLength(int humanId);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Quaternion GetPreRotation(int humanId);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Quaternion GetPostRotation(int humanId);

		internal Quaternion GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
		{
			return Avatar.INTERNAL_CALL_GetZYPostQ(this, humanId, ref parentQ, ref q);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_GetZYPostQ(Avatar self, int humanId, ref Quaternion parentQ, ref Quaternion q);

		internal Quaternion GetZYRoll(int humanId, Vector3 uvw)
		{
			return Avatar.INTERNAL_CALL_GetZYRoll(this, humanId, ref uvw);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion INTERNAL_CALL_GetZYRoll(Avatar self, int humanId, ref Vector3 uvw);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Vector3 GetLimitSign(int humanId);
	}
}
