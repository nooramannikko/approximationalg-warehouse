using System.Collections.Generic;

namespace ht1.Solver
{
    public class Request
    {
        public string Name { get; set; }
        public List<ItemBin> Targets { get; set; }

        public Request()
        {
            Name = null;
            Targets = new List<ItemBin>();
        }

        public Request(string req, string name)
        {
            Name = name;
            // Read and store request
        }
    }
}