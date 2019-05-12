﻿using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Provides a callback mechanism for the type library converter to inform the caller of the status of the conversion, and involve the caller in the conversion process itself.</summary>
	[ComVisible(true)]
	[Guid("f1c3bf76-c3e4-11d3-88e7-00902754c43a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITypeLibImporterNotifySink
	{
		/// <summary>Notifies the caller that an event occured during the conversion of a type library.</summary>
		/// <param name="eventKind">An <see cref="T:System.Runtime.InteropServices.ImporterEventKind" /> value indicating the type of event. </param>
		/// <param name="eventCode">Indicates extra information about the event. </param>
		/// <param name="eventMsg">A message generated by the event. </param>
		void ReportEvent(ImporterEventKind eventKind, int eventCode, string eventMsg);

		/// <summary>Asks the user to resolve a reference to another type library.</summary>
		/// <returns>The assembly corresponding to <paramref name="typeLib" />.</returns>
		/// <param name="typeLib">The object implementing the ITypeLib interface that needs to be resolved. </param>
		Assembly ResolveRef([MarshalAs(UnmanagedType.Interface)] object typeLib);
	}
}