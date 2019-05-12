﻿using System;

namespace System
{
	/// <summary>Encapsulates a method that has four parameters and returns a value of the type specified by the <paramref name="TResult" /> parameter.</summary>
	/// <returns>The return value of the method that this delegate encapsulates.</returns>
	/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param>
	/// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T3">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T4">The type of the fourth parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
	public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}
