using System.Collections.Generic;

namespace ht1.Solver
{
    public class Route
    {
        public ItemBin StartPoint { get; set; }

        public double Length { get; set; }

        public Route()
        {
            StartPoint = new ItemBin();
            Length = 0;
        }

        public Route(ItemBin bin, double dist = 0)
        {
            StartPoint = bin;
            Length = dist;
        }
    }
}