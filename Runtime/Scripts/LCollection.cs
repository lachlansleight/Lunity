using System.Collections.Generic;

namespace Lunity
{
    public class LCollection
    {
        public static void AlignDictionaries<T, TU, TV>(Dictionary<T, TU> current, Dictionary<T, TV> target, out List<T> toAdd, out List<T> toUpdate, out List<T> toRemove)
        {
            toAdd = new List<T>();
            toUpdate = new List<T>();
            toRemove = new List<T>();
            
            var validIds = new List<T>();
            foreach (var (key, value) in target) {
                validIds.Add(key);
                if (current.ContainsKey(key)) {
                    toUpdate.Add(key);
                }  else {
                    toAdd.Add(key);
                }
            }
            foreach (var (key, value) in current) {
                if (!validIds.Contains(key)) {
                    toRemove.Add(key);
                }
            }
        }
    }
}