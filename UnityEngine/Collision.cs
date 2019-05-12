﻿using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes a collision.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class Collision
	{
		internal Vector3 m_Impulse;

		internal Vector3 m_RelativeVelocity;

		internal Rigidbody m_Rigidbody;

		internal Collider m_Collider;

		internal ContactPoint[] m_Contacts;

		/// <summary>
		///   <para>The relative linear velocity of the two colliding objects (Read Only).</para>
		/// </summary>
		public Vector3 relativeVelocity
		{
			get
			{
				return this.m_RelativeVelocity;
			}
		}

		/// <summary>
		///   <para>The Rigidbody we hit (Read Only). This is null if the object we hit is a collider with no rigidbody attached.</para>
		/// </summary>
		public Rigidbody rigidbody
		{
			get
			{
				return this.m_Rigidbody;
			}
		}

		/// <summary>
		///   <para>The Collider we hit (Read Only).</para>
		/// </summary>
		public Collider collider
		{
			get
			{
				return this.m_Collider;
			}
		}

		/// <summary>
		///   <para>The Transform of the object we hit (Read Only).</para>
		/// </summary>
		public Transform transform
		{
			get
			{
				return (!(this.rigidbody != null)) ? this.collider.transform : this.rigidbody.transform;
			}
		}

		/// <summary>
		///   <para>The GameObject whose collider we are colliding with. (Read Only).</para>
		/// </summary>
		public GameObject gameObject
		{
			get
			{
				return (!(this.m_Rigidbody != null)) ? this.m_Collider.gameObject : this.m_Rigidbody.gameObject;
			}
		}

		/// <summary>
		///   <para>The contact points generated by the physics engine.</para>
		/// </summary>
		public ContactPoint[] contacts
		{
			get
			{
				return this.m_Contacts;
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			return this.contacts.GetEnumerator();
		}

		/// <summary>
		///   <para>The total impulse applied to this contact pair to resolve the collision.</para>
		/// </summary>
		public Vector3 impulse
		{
			get
			{
				return this.m_Impulse;
			}
		}

		[Obsolete("Use Collision.relativeVelocity instead.", false)]
		public Vector3 impactForceSum
		{
			get
			{
				return this.relativeVelocity;
			}
		}

		[Obsolete("Will always return zero.", false)]
		public Vector3 frictionForceSum
		{
			get
			{
				return Vector3.zero;
			}
		}

		[Obsolete("Please use Collision.rigidbody, Collision.transform or Collision.collider instead", false)]
		public Component other
		{
			get
			{
				return (!(this.m_Rigidbody != null)) ? this.m_Collider : this.m_Rigidbody;
			}
		}
	}
}