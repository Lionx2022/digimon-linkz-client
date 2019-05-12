﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public class TMP_SpriteAsset : TMP_Asset
	{
		private Dictionary<int, int> m_UnicodeLookup;

		private Dictionary<int, int> m_NameLookup;

		public static TMP_SpriteAsset m_defaultSpriteAsset;

		public Texture spriteSheet;

		public List<TMP_Sprite> spriteInfoList;

		[SerializeField]
		public List<TMP_SpriteAsset> fallbackSpriteAssets;

		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				if (TMP_SpriteAsset.m_defaultSpriteAsset == null)
				{
					TMP_SpriteAsset.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				return TMP_SpriteAsset.m_defaultSpriteAsset;
			}
		}

		private void OnEnable()
		{
		}

		private Material GetDefaultSpriteMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			Shader shader = Shader.Find("TextMeshPro/Sprite");
			Material material = new Material(shader);
			material.SetTexture(ShaderUtilities.ID_MainTex, this.spriteSheet);
			material.hideFlags = HideFlags.HideInHierarchy;
			return material;
		}

		public void UpdateLookupTables()
		{
			if (this.m_NameLookup == null)
			{
				this.m_NameLookup = new Dictionary<int, int>();
			}
			if (this.m_UnicodeLookup == null)
			{
				this.m_UnicodeLookup = new Dictionary<int, int>();
			}
			for (int i = 0; i < this.spriteInfoList.Count; i++)
			{
				int hashCode = this.spriteInfoList[i].hashCode;
				if (!this.m_NameLookup.ContainsKey(hashCode))
				{
					this.m_NameLookup.Add(hashCode, i);
				}
				int unicode = this.spriteInfoList[i].unicode;
				if (!this.m_UnicodeLookup.ContainsKey(unicode))
				{
					this.m_UnicodeLookup.Add(unicode, i);
				}
			}
		}

		public int GetSpriteIndexFromHashcode(int hashCode)
		{
			if (this.m_NameLookup == null)
			{
				this.UpdateLookupTables();
			}
			int result = 0;
			if (this.m_NameLookup.TryGetValue(hashCode, out result))
			{
				return result;
			}
			return -1;
		}

		public int GetSpriteIndexFromUnicode(int unicode)
		{
			if (this.m_UnicodeLookup == null)
			{
				this.UpdateLookupTables();
			}
			int result = 0;
			if (this.m_UnicodeLookup.TryGetValue(unicode, out result))
			{
				return result;
			}
			return -1;
		}

		public int GetSpriteIndexFromName(string name)
		{
			if (this.m_NameLookup == null)
			{
				this.UpdateLookupTables();
			}
			int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(name);
			return this.GetSpriteIndexFromHashcode(simpleHashCode);
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(TMP_SpriteAsset spriteAsset, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if (spriteAsset == null)
			{
				return null;
			}
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				int num = 0;
				while (num < spriteAsset.fallbackSpriteAssets.Count && spriteIndex == -1)
				{
					TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(spriteAsset.fallbackSpriteAssets[num], unicode, out spriteIndex);
					if (tmp_SpriteAsset != null)
					{
						return tmp_SpriteAsset;
					}
					num++;
				}
			}
			return null;
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(List<TMP_SpriteAsset> spriteAssets, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if (spriteAssets != null && spriteAssets.Count > 0)
			{
				for (int i = 0; i < spriteAssets.Count; i++)
				{
					TMP_SpriteAsset tmp_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(spriteAssets[i], unicode, out spriteIndex);
					if (tmp_SpriteAsset != null)
					{
						return tmp_SpriteAsset;
					}
				}
			}
			return null;
		}
	}
}
