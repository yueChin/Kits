using System;
using System.Collections.Generic;

namespace Kits.ClientKit.Handlers.Collection
{
    public static partial class ListHandler
    {
        public static bool AnyFast<T>(this List<T> self) where T : UnityEngine.Object
        {
            for (int i = 0; i < self.Count; ++i)
            {
                if (self[i]) return true;
            }

            return false;
        }

        public static bool AnyFast<T>(this List<T> self, Predicate<T> predicate) where T : UnityEngine.Object
        {
            for (int i = 0; i < self.Count; ++i)
            {
                if (self[i] && predicate(self[i])) return true;
            }

            return false;
        }
    }
}