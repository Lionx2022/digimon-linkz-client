﻿using EvolutionDiagram;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionRouteMap
{
	public class GUI_EvolutionRouteMapListItem : GUIListPartsWrapper
	{
		[SerializeField]
		private GUI_EvolutionRouteMapList listRoot;

		[SerializeField]
		private MonsterThumbnail monsterIcon;

		[SerializeField]
		private UILabel growStep;

		[SerializeField]
		private UILabel monsterName;

		private void OnPushed()
		{
			int idx = base.IDX;
			List<EvolutionDiagramData.IconMonster> routeMapData = this.listRoot.GetRouteMapData();
			if (routeMapData != null && idx < routeMapData.Count && routeMapData[idx] != null)
			{
				this.listRoot.OnChangeSelectMonster(routeMapData[idx]);
			}
		}

		protected override void OnUpdatedParts(int listPartsIndex)
		{
			List<EvolutionDiagramData.IconMonster> routeMapData = this.listRoot.GetRouteMapData();
			if (routeMapData != null && listPartsIndex < routeMapData.Count && routeMapData[listPartsIndex] != null)
			{
				EvolutionDiagramData.IconMonster iconMonster = routeMapData[listPartsIndex];
				this.monsterIcon.SetImage(iconMonster.singleData.iconId, iconMonster.groupData.growStep);
				this.growStep.text = CommonSentenceData.GetGrade(iconMonster.groupData.growStep);
				this.monsterName.text = iconMonster.groupData.monsterName;
			}
		}

		public override void InitParts()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.playSelectSE = true;
			this.touchBehavior = GUICollider.TouchBehavior.None;
			this.monsterIcon.Initialize();
		}
	}
}