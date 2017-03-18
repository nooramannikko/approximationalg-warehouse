using System.Collections.Generic;
using System.IO;

namespace ht1.Solver
{
    public class Request
    {
        public string Name { get; set; }
        public bool Valid { get; set; }
        public List<int> Targets { get; set; }

        public Request()
        {
            Name = null;
            Valid = true;
            Targets = new List<int>();
        }

        public static Request Create(string filePath, string name)
        {
            var result = new Request();
            result.Name = name;
            // Read and store request
            string line = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(line))
            {
                var parts = Helpers.SafeSplit(line, ' ');
                foreach (var item in parts)
                {
                    int num;
                    if (int.TryParse(item, out num))
                    {
                        result.Targets.Add(num);
                    }
                    else
                    {
                        // The file contains some invalid data
                        result.Valid = false;
                        break;
                    }
                }
            }
            return result;
        }
    }
}