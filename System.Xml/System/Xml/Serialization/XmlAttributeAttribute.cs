﻿using System;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Specifies that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> must serialize the class member as an XML attribute.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class XmlAttributeAttribute : Attribute
	{
		private string attributeName;

		private string dataType;

		private Type type;

		private XmlSchemaForm form;

		private string ns;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributeAttribute" /> class.</summary>
		public XmlAttributeAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributeAttribute" /> class and specifies the name of the generated XML attribute.</summary>
		/// <param name="attributeName">The name of the XML attribute that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> generates. </param>
		public XmlAttributeAttribute(string attributeName)
		{
			this.attributeName = attributeName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributeAttribute" /> class.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> used to store the attribute. </param>
		public XmlAttributeAttribute(Type type)
		{
			this.type = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlAttributeAttribute" /> class.</summary>
		/// <param name="attributeName">The name of the XML attribute that is generated. </param>
		/// <param name="type">The <see cref="T:System.Type" /> used to store the attribute. </param>
		public XmlAttributeAttribute(string attributeName, Type type)
		{
			this.attributeName = attributeName;
			this.type = type;
		}

		/// <summary>Gets or sets the name of the XML attribute.</summary>
		/// <returns>The name of the XML attribute. The default is the member name.</returns>
		public string AttributeName
		{
			get
			{
				if (this.attributeName == null)
				{
					return string.Empty;
				}
				return this.attributeName;
			}
			set
			{
				this.attributeName = value;
			}
		}

		/// <summary>Gets or sets the XSD data type of the XML attribute generated by the <see cref="T:System.Xml.Serialization.XmlSerializer" />.</summary>
		/// <returns>An XSD (XML Schema Document) data type, as defined by the World Wide Web Consortium (www.w3.org) document named "XML Schema: DataTypes".</returns>
		public string DataType
		{
			get
			{
				if (this.dataType == null)
				{
					return string.Empty;
				}
				return this.dataType;
			}
			set
			{
				this.dataType = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the XML attribute name generated by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> is qualified.</summary>
		/// <returns>One of the <see cref="T:System.Xml.Schema.XmlSchemaForm" /> values. The default is XmlForm.None.</returns>
		public XmlSchemaForm Form
		{
			get
			{
				return this.form;
			}
			set
			{
				this.form = value;
			}
		}

		/// <summary>Gets or sets the XML namespace of the XML attribute.</summary>
		/// <returns>The XML namespace of the XML attribute.</returns>
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		/// <summary>Gets or sets the complex type of the XML attribute.</summary>
		/// <returns>The type of the XML attribute.</returns>
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XAA ");
			KeyHelper.AddField(sb, 1, this.ns);
			KeyHelper.AddField(sb, 2, this.attributeName);
			KeyHelper.AddField(sb, 3, this.form.ToString(), XmlSchemaForm.None.ToString());
			KeyHelper.AddField(sb, 4, this.dataType);
			KeyHelper.AddField(sb, 5, this.type);
			sb.Append('|');
		}
	}
}
