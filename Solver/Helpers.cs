using System.Collections.Generic;

namespace ht1.Solver
{
    public class Helpers
    {
        public static List<string> SafeSplit(string text, char separator)
        {
            var result = new List<string>();
            var parts = text.Split(separator);
            foreach (var s in parts)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    result.Add(s);
                }
            }
            return result;
        }
    }
}