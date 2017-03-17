using System.Collections.Generic;

namespace ht1.Solver
{
    public class Layout
    {
        public List<ItemBin> ItemBins { get; set; }
        public bool Valid { get; set; }

        public Layout()
        {
            ItemBins = new List<ItemBin>();
            Valid = true;
        }

        public static Layout Create(string layoutDescription)
        {
            var result = new Layout();
            // Read layoutDescription line by line and save
            // Set Valid as false if there are errors on the way
            return result;
        }
    }

    public class ItemBin
    {
        public int Identifier { get; set; }
        public Coordinate Location { get; set; }

        public ItemBin()
        {
            Identifier = 0;
            Location = new Coordinate();
        }

        public ItemBin(int id, int x, int y)
        {
            Identifier = id;
            Location = new Coordinate(x, y);
        }
    }

    public class Coordinate
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

        public Coordinate()
        {
            XCoordinate = 0;
            YCoordinate = 0;
        }

        public Coordinate(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }
    }
}