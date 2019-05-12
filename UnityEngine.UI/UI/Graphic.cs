﻿using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI.CoroutineTween;

namespace UnityEngine.UI
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CanvasRenderer))]
	public abstract class Graphic : UIBehaviour, ICanvasElement
	{
		protected static Material s_DefaultUI = null;

		protected static Texture2D s_WhiteTexture = null;

		[FormerlySerializedAs("m_Mat")]
		[SerializeField]
		protected Material m_Material;

		[SerializeField]
		private Color m_Color = Color.white;

		[SerializeField]
		private bool m_RaycastTarget = true;

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private CanvasRenderer m_CanvasRender;

		[NonSerialized]
		private Canvas m_Canvas;

		[NonSerialized]
		private bool m_VertsDirty;

		[NonSerialized]
		private bool m_MaterialDirty;

		[NonSerialized]
		protected UnityAction m_OnDirtyLayoutCallback;

		[NonSerialized]
		protected UnityAction m_OnDirtyVertsCallback;

		[NonSerialized]
		protected UnityAction m_OnDirtyMaterialCallback;

		[NonSerialized]
		protected static Mesh s_Mesh;

		[NonSerialized]
		private static readonly VertexHelper s_VertexHelper = new VertexHelper();

		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		protected Graphic()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			this.m_ColorTweenRunner.Init(this);
			this.useLegacyMeshGeneration = true;
		}

		public static Material defaultGraphicMaterial
		{
			get
			{
				if (Graphic.s_DefaultUI == null)
				{
					Graphic.s_DefaultUI = Canvas.GetDefaultCanvasMaterial();
				}
				return Graphic.s_DefaultUI;
			}
		}

		public Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_Color, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		public bool raycastTarget
		{
			get
			{
				return this.m_RaycastTarget;
			}
			set
			{
				this.m_RaycastTarget = value;
			}
		}

		protected bool useLegacyMeshGeneration { get; set; }

		public virtual void SetAllDirty()
		{
			this.SetLayoutDirty();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		public virtual void SetLayoutDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			if (this.m_OnDirtyLayoutCallback != null)
			{
				this.m_OnDirtyLayoutCallback();
			}
		}

		public virtual void SetVerticesDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_VertsDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyVertsCallback != null)
			{
				this.m_OnDirtyVertsCallback();
			}
		}

		public virtual void SetMaterialDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_MaterialDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyMaterialCallback != null)
			{
				this.m_OnDirtyMaterialCallback();
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (base.gameObject.activeInHierarchy)
			{
				if (CanvasUpdateRegistry.IsRebuildingLayout())
				{
					this.SetVerticesDirty();
				}
				else
				{
					this.SetVerticesDirty();
					this.SetLayoutDirty();
				}
			}
		}

		protected override void OnBeforeTransformParentChanged()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (!this.IsActive())
			{
				return;
			}
			this.CacheCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
			this.SetAllDirty();
		}

		public int depth
		{
			get
			{
				return this.canvasRenderer.absoluteDepth;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				RectTransform result;
				if ((result = this.m_RectTransform) == null)
				{
					result = (this.m_RectTransform = base.GetComponent<RectTransform>());
				}
				return result;
			}
		}

		public Canvas canvas
		{
			get
			{
				if (this.m_Canvas == null)
				{
					this.CacheCanvas();
				}
				return this.m_Canvas;
			}
		}

		private void CacheCanvas()
		{
			List<Canvas> list = ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				this.m_Canvas = list[0];
			}
			ListPool<Canvas>.Release(list);
		}

		public CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_CanvasRender == null)
				{
					this.m_CanvasRender = base.GetComponent<CanvasRenderer>();
				}
				return this.m_CanvasRender;
			}
		}

		public virtual Material defaultMaterial
		{
			get
			{
				return Graphic.defaultGraphicMaterial;
			}
		}

		public virtual Material material
		{
			get
			{
				return (!(this.m_Material != null)) ? this.defaultMaterial : this.m_Material;
			}
			set
			{
				if (this.m_Material == value)
				{
					return;
				}
				this.m_Material = value;
				this.SetMaterialDirty();
			}
		}

		public virtual Material materialForRendering
		{
			get
			{
				List<Component> list = ListPool<Component>.Get();
				base.GetComponents(typeof(IMaterialModifier), list);
				Material material = this.material;
				for (int i = 0; i < list.Count; i++)
				{
					material = (list[i] as IMaterialModifier).GetModifiedMaterial(material);
				}
				ListPool<Component>.Release(list);
				return material;
			}
		}

		public virtual Texture mainTexture
		{
			get
			{
				return Graphic.s_WhiteTexture;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.CacheCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
			if (Graphic.s_WhiteTexture == null)
			{
				Graphic.s_WhiteTexture = Texture2D.whiteTexture;
			}
			this.SetAllDirty();
		}

		protected override void OnDisable()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.canvasRenderer != null)
			{
				this.canvasRenderer.Clear();
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			Canvas canvas = this.m_Canvas;
			this.CacheCanvas();
			if (canvas != this.m_Canvas)
			{
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
				GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
			}
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (this.canvasRenderer.cull)
			{
				return;
			}
			if (update == CanvasUpdate.PreRender)
			{
				if (this.m_VertsDirty)
				{
					this.UpdateGeometry();
					this.m_VertsDirty = false;
				}
				if (this.m_MaterialDirty)
				{
					this.UpdateMaterial();
					this.m_MaterialDirty = false;
				}
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		protected virtual void UpdateMaterial()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.canvasRenderer.materialCount = 1;
			this.canvasRenderer.SetMaterial(this.materialForRendering, 0);
			this.canvasRenderer.SetTexture(this.mainTexture);
		}

		protected virtual void UpdateGeometry()
		{
			if (this.useLegacyMeshGeneration)
			{
				this.DoLegacyMeshGeneration();
			}
			else
			{
				this.DoMeshGeneration();
			}
		}

		private void DoMeshGeneration()
		{
			if (this.rectTransform != null && this.rectTransform.rect.width >= 0f && this.rectTransform.rect.height >= 0f)
			{
				this.OnPopulateMesh(Graphic.s_VertexHelper);
			}
			else
			{
				Graphic.s_VertexHelper.Clear();
			}
			List<Component> list = ListPool<Component>.Get();
			base.GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(Graphic.s_VertexHelper);
			}
			ListPool<Component>.Release(list);
			Graphic.s_VertexHelper.FillMesh(Graphic.workerMesh);
			this.canvasRenderer.SetMesh(Graphic.workerMesh);
		}

		private void DoLegacyMeshGeneration()
		{
			if (this.rectTransform != null && this.rectTransform.rect.width >= 0f && this.rectTransform.rect.height >= 0f)
			{
				this.OnPopulateMesh(Graphic.workerMesh);
			}
			else
			{
				Graphic.workerMesh.Clear();
			}
			List<Component> list = ListPool<Component>.Get();
			base.GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(Graphic.workerMesh);
			}
			ListPool<Component>.Release(list);
			this.canvasRenderer.SetMesh(Graphic.workerMesh);
		}

		protected static Mesh workerMesh
		{
			get
			{
				if (Graphic.s_Mesh == null)
				{
					Graphic.s_Mesh = new Mesh();
					Graphic.s_Mesh.name = "Shared UI Mesh";
					Graphic.s_Mesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return Graphic.s_Mesh;
			}
		}

		[Obsolete("Use OnPopulateMesh instead.", true)]
		protected virtual void OnFillVBO(List<UIVertex> vbo)
		{
		}

		[Obsolete("Use OnPopulateMesh(VertexHelper vh) instead.", false)]
		protected virtual void OnPopulateMesh(Mesh m)
		{
			this.OnPopulateMesh(Graphic.s_VertexHelper);
			Graphic.s_VertexHelper.FillMesh(m);
		}

		protected virtual void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			Color32 color = this.color;
			vh.Clear();
			vh.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.SetAllDirty();
		}

		public virtual void SetNativeSize()
		{
		}

		public virtual bool Raycast(Vector2 sp, Camera eventCamera)
		{
			if (!base.isActiveAndEnabled)
			{
				return false;
			}
			Transform transform = base.transform;
			List<Component> list = ListPool<Component>.Get();
			bool flag = false;
			while (transform != null)
			{
				transform.GetComponents<Component>(list);
				for (int i = 0; i < list.Count; i++)
				{
					ICanvasRaycastFilter canvasRaycastFilter = list[i] as ICanvasRaycastFilter;
					if (canvasRaycastFilter != null)
					{
						bool flag2 = true;
						CanvasGroup canvasGroup = list[i] as CanvasGroup;
						if (canvasGroup != null)
						{
							if (!flag && canvasGroup.ignoreParentGroups)
							{
								flag = true;
								flag2 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
							}
							else if (!flag)
							{
								flag2 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
							}
						}
						else
						{
							flag2 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
						}
						if (!flag2)
						{
							ListPool<Component>.Release(list);
							return false;
						}
					}
				}
				transform = transform.parent;
			}
			ListPool<Component>.Release(list);
			return true;
		}

		public Vector2 PixelAdjustPoint(Vector2 point)
		{
			if (!this.canvas || !this.canvas.pixelPerfect)
			{
				return point;
			}
			return RectTransformUtility.PixelAdjustPoint(point, base.transform, this.canvas);
		}

		public Rect GetPixelAdjustedRect()
		{
			if (!this.canvas || !this.canvas.pixelPerfect)
			{
				return this.rectTransform.rect;
			}
			return RectTransformUtility.PixelAdjustRect(this.rectTransform, this.canvas);
		}

		public void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			this.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, true);
		}

		private void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
		{
			if (this.canvasRenderer == null || (!useRGB && !useAlpha))
			{
				return;
			}
			if (this.canvasRenderer.GetColor().Equals(targetColor))
			{
				return;
			}
			ColorTween.ColorTweenMode tweenMode = (!useRGB || !useAlpha) ? ((!useRGB) ? ColorTween.ColorTweenMode.Alpha : ColorTween.ColorTweenMode.RGB) : ColorTween.ColorTweenMode.All;
			ColorTween info = new ColorTween
			{
				duration = duration,
				startColor = this.canvasRenderer.GetColor(),
				targetColor = targetColor
			};
			info.AddOnChangedCallback(new UnityAction<Color>(this.canvasRenderer.SetColor));
			info.ignoreTimeScale = ignoreTimeScale;
			info.tweenMode = tweenMode;
			this.m_ColorTweenRunner.StartTween(info);
		}

		private static Color CreateColorFromAlpha(float alpha)
		{
			Color black = Color.black;
			black.a = alpha;
			return black;
		}

		public void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			this.CrossFadeColor(Graphic.CreateColorFromAlpha(alpha), duration, ignoreTimeScale, true, false);
		}

		public void RegisterDirtyLayoutCallback(UnityAction action)
		{
			this.m_OnDirtyLayoutCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyLayoutCallback, action);
		}

		public void UnregisterDirtyLayoutCallback(UnityAction action)
		{
			this.m_OnDirtyLayoutCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyLayoutCallback, action);
		}

		public void RegisterDirtyVerticesCallback(UnityAction action)
		{
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyVertsCallback, action);
		}

		public void UnregisterDirtyVerticesCallback(UnityAction action)
		{
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyVertsCallback, action);
		}

		public void RegisterDirtyMaterialCallback(UnityAction action)
		{
			this.m_OnDirtyMaterialCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyMaterialCallback, action);
		}

		public void UnregisterDirtyMaterialCallback(UnityAction action)
		{
			this.m_OnDirtyMaterialCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyMaterialCallback, action);
		}

		virtual bool IsDestroyed()
		{
			return base.IsDestroyed();
		}

		virtual Transform get_transform()
		{
			return base.transform;
		}
	}
}
