using System;

namespace Burton.ExtensionMethods.Float
{
    public static class ExtensionMethods
    {
        public static float MapRange(this float Value, float From1, float To1, float From2, float To2)
        {
            return (Value - From1) / (To1 - From1) * (To2 - From2) + From2;
        }
    }
}
