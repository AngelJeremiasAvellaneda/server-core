using UnityEngine;

namespace AndesSim
{
    // Crea materiales holográficos en tiempo de ejecución para no depender de archivos .mat externos.
    public static class MaterialFactory
    {
        private static Shader unlit;
        private static Shader transparent;

        public static Shader UnlitShader
        {
            get
            {
                if (unlit == null)
                {
                    unlit = Shader.Find("Universal Render Pipeline/Unlit");
                    if (unlit == null) unlit = Shader.Find("Unlit/Color");
                    if (unlit == null) unlit = Shader.Find("Sprites/Default");
                }
                return unlit;
            }
        }

        public static Shader TransparentShader
        {
            get
            {
                if (transparent == null)
                {
                    transparent = Shader.Find("Universal Render Pipeline/Unlit");
                    if (transparent == null) transparent = Shader.Find("Transparent/Diffuse");
                    if (transparent == null) transparent = Shader.Find("Sprites/Default");
                }
                return transparent;
            }
        }

        public static Material Create(Color color, bool transparent = false)
        {
            var shader = transparent ? TransparentShader : UnlitShader;
            var material = new Material(shader);
            SetColor(material, color);
            if (transparent)
            {
                material.SetFloat("_Surface", 1f);
                material.SetFloat("_Blend", 0f);
                material.SetOverrideTag("RenderType", "Transparent");
                material.renderQueue = 3000;
            }
            return material;
        }

        public static void SetColor(Material material, Color color)
        {
            if (material == null) return;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
        }

        public static Color GetColor(Material material)
        {
            if (material == null) return Color.white;
            if (material.HasProperty("_BaseColor")) return material.GetColor("_BaseColor");
            if (material.HasProperty("_Color")) return material.GetColor("_Color");
            return Color.white;
        }
    }
}
