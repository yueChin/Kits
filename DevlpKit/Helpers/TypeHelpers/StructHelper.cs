using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.TypeHelpers
{
	public static class StructHelper
	{
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }
}
}