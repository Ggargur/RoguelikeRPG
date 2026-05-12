using UnityEngine;

namespace Rendering
{
    [CreateAssetMenu(fileName = "SpritePalette4", menuName = "Rendering/Sprite Palette 4")]
    public class SpritePalette4SO : ScriptableObject
    {
        [SerializeField] private Color color0 = Color.black;
        [SerializeField] private Color color1 = new Color(0.33f, 0.33f, 0.33f, 1f);
        [SerializeField] private Color color2 = new Color(0.66f, 0.66f, 0.66f, 1f);
        [SerializeField] private Color color3 = Color.white;

        public Color Color0 => color0;
        public Color Color1 => color1;
        public Color Color2 => color2;
        public Color Color3 => color3;
    }
}

