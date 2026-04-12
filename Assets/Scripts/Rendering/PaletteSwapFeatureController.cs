using UnityEngine;

namespace Rendering
{
    public class PaletteSwapFeatureController : MonoBehaviour
    {
        [SerializeField] private SpritePalette4SO[] palettes;
        [SerializeField] private int startPaletteIndex;

        private void OnEnable()
        {
            ApplyPaletteByIndex(startPaletteIndex);
        }

        private void OnDisable()
        {
            PaletteSwapRendererFeature.ClearRuntimeTargetPalette();
        }

        public void ApplyPaletteByIndex(int index)
        {
            if (palettes == null || palettes.Length == 0)
                return;

            int clampedIndex = Mathf.Clamp(index, 0, palettes.Length - 1);
            startPaletteIndex = clampedIndex;

            SpritePalette4SO palette = palettes[clampedIndex];
            if (palette != null)
                PaletteSwapRendererFeature.SetRuntimeTargetPalette(palette);
        }

        public void ApplyNextPalette()
        {
            if (palettes == null || palettes.Length == 0)
                return;

            startPaletteIndex = (startPaletteIndex + 1) % palettes.Length;
            ApplyPaletteByIndex(startPaletteIndex);
        }
    }
}

