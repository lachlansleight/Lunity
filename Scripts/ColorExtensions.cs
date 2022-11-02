using UnityEngine;

namespace Lunity
{
    public static class ColorExtensions
    {
        /// Returns a new Color that is a copy of the source color, but with the specified alpha value
        public static Color SetAlpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        /// Creates a new two-stop gradient
        public static Gradient CreateGradient(float t0, Color c0, float a0, float t1, Color c1, float a1)
        {
            var g = new Gradient
            {
                alphaKeys = new[] {new GradientAlphaKey(a0, t0), new GradientAlphaKey(a1, t1)},
                colorKeys = new[] {new GradientColorKey(c0, t0), new GradientColorKey(c1, t1)}
            };
            return g;
        }
        
        /// Creates a new three-stop gradient
        public static Gradient CreateGradient(float t0, Color c0, float a0, float t1, Color c1, float a1, float t2, Color c2, float a2)
        {
            var g = new Gradient
            {
                alphaKeys = new[] {new GradientAlphaKey(a0, t0), new GradientAlphaKey(a1, t1), new GradientAlphaKey(a2, t2)},
                colorKeys = new[] {new GradientColorKey(c0, t0), new GradientColorKey(c1, t1), new GradientColorKey(c2, t2)}
            };
            return g;
        }
        
        /// Creates a new four-stop gradient
        public static Gradient CreateGradient(float t0, Color c0, float a0, float t1, Color c1, float a1, float t2, Color c2, float a2, float t3, Color c3, float a3)
        {
            var g = new Gradient
            {
                alphaKeys = new[] {new GradientAlphaKey(a0, t0), new GradientAlphaKey(a1, t1), new GradientAlphaKey(a2, t2), new GradientAlphaKey(a3, t3)},
                colorKeys = new[] {new GradientColorKey(c0, t0), new GradientColorKey(c1, t1), new GradientColorKey(c2, t2), new GradientColorKey(c3, t3)}
            };
            return g;
        }
    }
}