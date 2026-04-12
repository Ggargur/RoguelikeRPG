using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public class PaletteSwapRendererFeature : ScriptableRendererFeature
    {
        [Serializable]
        public class Settings
        {
            public Shader shader;
            public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            public SpritePalette4SO sourcePalette;
            public SpritePalette4SO defaultTargetPalette;

            [Range(0f, 0.1f)]
            public float matchEpsilon = 0.002f;
        }

        private static readonly int SrcColor0Id = Shader.PropertyToID("_SrcColor0");
        private static readonly int SrcColor1Id = Shader.PropertyToID("_SrcColor1");
        private static readonly int SrcColor2Id = Shader.PropertyToID("_SrcColor2");
        private static readonly int SrcColor3Id = Shader.PropertyToID("_SrcColor3");

        private static readonly int DstColor0Id = Shader.PropertyToID("_DstColor0");
        private static readonly int DstColor1Id = Shader.PropertyToID("_DstColor1");
        private static readonly int DstColor2Id = Shader.PropertyToID("_DstColor2");
        private static readonly int DstColor3Id = Shader.PropertyToID("_DstColor3");

        private static readonly int MatchEpsilonId = Shader.PropertyToID("_MatchEpsilon");

        public Settings settings = new Settings();

        private PaletteSwapRenderPass _pass;
        private Material _material;

        private static SpritePalette4SO s_RuntimeTargetPalette;

        public static void SetRuntimeTargetPalette(SpritePalette4SO palette)
        {
            s_RuntimeTargetPalette = palette;
        }

        public static void ClearRuntimeTargetPalette()
        {
            s_RuntimeTargetPalette = null;
        }

        public override void Create()
        {
            EnsureResources();
        }

        private bool EnsureResources()
        {
            if (settings.shader == null)
                settings.shader = Shader.Find("Hidden/Rendering/PaletteSwapBlit4");

            if (settings.shader == null)
            {
                Debug.LogWarning("PaletteSwapRendererFeature: shader not found. Assign Hidden/Rendering/PaletteSwapBlit4.");
                return false;
            }

            if (_material == null || _material.shader != settings.shader)
            {
                Dispose(true);
                _material = CoreUtils.CreateEngineMaterial(settings.shader);
            }

            _pass ??= new PaletteSwapRenderPass();
            _pass.renderPassEvent = settings.renderPassEvent;
            return _material != null;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!EnsureResources())
                return;

            SpritePalette4SO targetPalette = s_RuntimeTargetPalette != null ? s_RuntimeTargetPalette : settings.defaultTargetPalette;
            if (settings.sourcePalette == null || targetPalette == null)
                return;

            _pass.Setup(_material, settings.sourcePalette, targetPalette, settings.matchEpsilon);
            renderer.EnqueuePass(_pass);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_material);
            _material = null;
        }

        private class PaletteSwapRenderPass : ScriptableRenderPass
        {
            private Material _material;
            private readonly MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();

            private SpritePalette4SO _sourcePalette;
            private SpritePalette4SO _targetPalette;
            private float _matchEpsilon;

            public PaletteSwapRenderPass() { }

            public void Setup(Material material, SpritePalette4SO sourcePalette, SpritePalette4SO targetPalette, float matchEpsilon)
            {
                _material = material;
                _sourcePalette = sourcePalette;
                _targetPalette = targetPalette;
                _matchEpsilon = matchEpsilon;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                if (_material == null || _sourcePalette == null || _targetPalette == null)
                    return;

                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                if (resourceData.isActiveTargetBackBuffer)
                    return;

                TextureHandle source = resourceData.activeColorTexture;
                if (!source.IsValid())
                    return;

                TextureDesc destinationDesc = renderGraph.GetTextureDesc(source);
                destinationDesc.name = "_PaletteSwapColor";
                destinationDesc.clearBuffer = false;
                TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

                _propertyBlock.Clear();
                _propertyBlock.SetColor(SrcColor0Id, _sourcePalette.Color0);
                _propertyBlock.SetColor(SrcColor1Id, _sourcePalette.Color1);
                _propertyBlock.SetColor(SrcColor2Id, _sourcePalette.Color2);
                _propertyBlock.SetColor(SrcColor3Id, _sourcePalette.Color3);

                _propertyBlock.SetColor(DstColor0Id, _targetPalette.Color0);
                _propertyBlock.SetColor(DstColor1Id, _targetPalette.Color1);
                _propertyBlock.SetColor(DstColor2Id, _targetPalette.Color2);
                _propertyBlock.SetColor(DstColor3Id, _targetPalette.Color3);
                _propertyBlock.SetFloat(MatchEpsilonId, _matchEpsilon);

                var blitParams = new RenderGraphUtils.BlitMaterialParameters(source, destination, _material, 0, _propertyBlock);
                renderGraph.AddBlitPass(blitParams, "Palette Swap 4");

                resourceData.cameraColor = destination;
            }
        }
    }
}


