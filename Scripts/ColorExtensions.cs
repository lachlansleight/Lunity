using UnityEngine;

namespace Lunity
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }
    }
}