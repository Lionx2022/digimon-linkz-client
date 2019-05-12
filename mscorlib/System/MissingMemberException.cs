﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when there is an attempt to dynamically access a class member that does not exist.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class MissingMemberException : MemberAccessException
	{
		private const int Result = -2146233070;

		/// <summary>Holds the class name of the missing member.</summary>
		protected string ClassName;

		/// <summary>Holds the name of the missing member.</summary>
		protected string MemberName;

		/// <summary>Holds the signature of the missing member.</summary>
		protected byte[] Signature;

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMemberException" /> class.</summary>
		public MissingMemberException() : base(Locale.GetText("Cannot find the requested class member."))
		{
			base.HResult = -2146233070;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMemberException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error. </param>
		public MissingMemberException(string message) : base(message)
		{
			base.HResult = -2146233070;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMemberException" /> class with a specified error message and a reference to the inner exception that is the root cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">An instance of <see cref="T:System.Exception" /> that is the cause of the current Exception. If <paramref name="inner" /> is not a null reference (Nothing in Visual Basic), then the current Exception is raised in a catch block handling <paramref name="inner" />. </param>
		public MissingMemberException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233070;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMemberException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected MissingMemberException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.ClassName = info.GetString("MMClassName");
			this.MemberName = info.GetString("MMMemberName");
			this.Signature = (byte[])info.GetValue("MMSignature", typeof(byte[]));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMemberException" /> class with the specified class name and member name.</summary>
		/// <param name="className">The name of the class in which access to a nonexistent member was attempted. </param>
		/// <param name="memberName">The name of the member that cannot be accessed. </param>
		public MissingMemberException(string className, string memberName)
		{
			this.ClassName = className;
			this.MemberName = memberName;
			base.HResult = -2146233070;
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the class name, the member name, the signature of the missing member, and additional exception information.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> object is null. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("MMClassName", this.ClassName);
			info.AddValue("MMMemberName", this.MemberName);
			info.AddValue("MMSignature", this.Signature);
		}

		/// <summary>Gets the text string showing the class name, the member name, and the signature of the missing member.</summary>
		/// <returns>The error message string.</returns>
		/// <filterpriority>2</filterpriority>
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				string text = Locale.GetText("Member {0}.{1} not found.");
				return string.Format(text, this.ClassName, this.MemberName);
			}
		}
	}
}
