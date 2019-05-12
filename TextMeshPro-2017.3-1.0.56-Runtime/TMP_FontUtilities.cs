﻿using System;
using System.Collections.Generic;

namespace TMPro
{
	public static class TMP_FontUtilities
	{
		private static List<int> k_searchedFontAssets;

		public static TMP_FontAsset SearchForGlyph(TMP_FontAsset font, int character, out TMP_Glyph glyph)
		{
			if (TMP_FontUtilities.k_searchedFontAssets == null)
			{
				TMP_FontUtilities.k_searchedFontAssets = new List<int>();
			}
			TMP_FontUtilities.k_searchedFontAssets.Clear();
			return TMP_FontUtilities.SearchForGlyphInternal(font, character, out glyph);
		}

		public static TMP_FontAsset SearchForGlyph(List<TMP_FontAsset> fonts, int character, out TMP_Glyph glyph)
		{
			return TMP_FontUtilities.SearchForGlyphInternal(fonts, character, out glyph);
		}

		private static TMP_FontAsset SearchForGlyphInternal(TMP_FontAsset font, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (font == null)
			{
				return null;
			}
			if (font.characterDictionary.TryGetValue(character, out glyph))
			{
				return font;
			}
			if (font.fallbackFontAssets != null && font.fallbackFontAssets.Count > 0)
			{
				int num = 0;
				while (num < font.fallbackFontAssets.Count && glyph == null)
				{
					TMP_FontAsset tmp_FontAsset = font.fallbackFontAssets[num];
					if (!(tmp_FontAsset == null))
					{
						int instanceID = tmp_FontAsset.GetInstanceID();
						if (!TMP_FontUtilities.k_searchedFontAssets.Contains(instanceID))
						{
							TMP_FontUtilities.k_searchedFontAssets.Add(instanceID);
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyphInternal(tmp_FontAsset, character, out glyph);
							if (tmp_FontAsset != null)
							{
								return tmp_FontAsset;
							}
						}
					}
					num++;
				}
			}
			return null;
		}

		private static TMP_FontAsset SearchForGlyphInternal(List<TMP_FontAsset> fonts, int character, out TMP_Glyph glyph)
		{
			glyph = null;
			if (fonts != null && fonts.Count > 0)
			{
				for (int i = 0; i < fonts.Count; i++)
				{
					TMP_FontAsset tmp_FontAsset = TMP_FontUtilities.SearchForGlyphInternal(fonts[i], character, out glyph);
					if (tmp_FontAsset != null)
					{
						return tmp_FontAsset;
					}
				}
			}
			return null;
		}
	}
}
