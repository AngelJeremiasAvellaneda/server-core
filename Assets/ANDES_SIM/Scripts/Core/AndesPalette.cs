using UnityEngine;

namespace AndesSim
{
    // Paleta única del proyecto. Todos los paneles y efectos deben tomar colores de aquí.
    public static class AndesPalette
    {
        public static readonly Color DeepBlue = new Color(0.015f, 0.055f, 0.13f, 0.90f);
        public static readonly Color DeepBlueSolid = new Color(0.010f, 0.035f, 0.09f, 1f);
        public static readonly Color Cyan = new Color(0.10f, 0.86f, 1.00f, 1f);
        public static readonly Color ElectricBlue = new Color(0.28f, 0.56f, 1.00f, 1f);
        public static readonly Color TextBlueWhite = new Color(0.88f, 0.95f, 1.00f, 1f);
        public static readonly Color Amber = new Color(1.00f, 0.68f, 0.16f, 1f);
        public static readonly Color Red = new Color(1.00f, 0.22f, 0.24f, 1f);
        public static readonly Color Green = new Color(0.24f, 0.90f, 0.52f, 1f);

        public static Color Alpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
