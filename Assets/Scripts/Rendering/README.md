# URP Palette Swap (4 Colors)

This setup adds a URP `ScriptableRendererFeature` that remaps exactly 4 source colors into 4 target colors using a full-screen pass.

## Files

- `Assets/Scripts/Rendering/SpritePalette4SO.cs`
- `Assets/Scripts/Rendering/PaletteSwapRendererFeature.cs`
- `Assets/Scripts/Rendering/PaletteSwapFeatureController.cs`
- `Assets/Shaders/PaletteSwapBlit4.shader`

## Setup

1. Create palettes via `Create > Rendering > Sprite Palette 4`.
2. Open your URP renderer asset (for this project likely `Assets/Settings/Renderer2D.asset`).
3. Add `PaletteSwapRendererFeature` to the renderer features list.
4. In the feature settings:
   - `Shader`: `Hidden/Rendering/PaletteSwapBlit4`
   - `Source Palette`: your original 4 sprite colors
   - `Default Target Palette`: replacement palette
   - `Match Epsilon`: e.g. `0.002`
5. Enter Play Mode.

## Runtime Palette Change

Add `PaletteSwapFeatureController` to a scene object, assign the `palettes` array, then call:

- `ApplyPaletteByIndex(int index)`
- `ApplyNextPalette()`

These update the target palette in real time by calling `PaletteSwapRendererFeature.SetRuntimeTargetPalette(...)`.

## Notes

- This pass affects the whole camera color output, not individual sprites.
- If palette matching misses colors, increase `Match Epsilon` slightly.

