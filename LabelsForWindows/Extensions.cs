using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabelsForWindows {

    public static class Extensions {

        public static T[] Add<T>(this T[] target, params T[] items) {
            // Validate the parameters
            if (target == null) {
                target = new T[] { };
            }
            if (items == null) {
                items = new T[] { };
            }

            // Join the arrays
            T[] result = new T[target.Length + items.Length];
            target.CopyTo(result, 0);
            items.CopyTo(result, target.Length);
            return result;
        }
    }
}
