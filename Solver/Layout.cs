using System.Collections.Generic;
using System.IO;

namespace ht1.Solver
{
    public class Layout
    {
        public List<ItemBin> ItemBins { get; set; }
        public bool Valid { get; set; }

        public List<DistanceItem> DistanceTable { get; set; }

        public Layout()
        {
            ItemBins = new List<ItemBin>();
            Valid = true;
            DistanceTable = new List<DistanceItem>();
        }

        public static Layout Create(string filePath)
        {
            var result = new Layout();
            // Add warehouse robot as "ItemBin"
            result.ItemBins.Add(new ItemBin(0, 0, 0));

            // Read layout description line by line and save
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var parts = Helpers.SafeSplit(line, ' ');
                    if (parts.Count != 3)
                    {
                        // The file contains some invalid lines
                        result.Valid = false;
                        break;
                    }
                    else
                    {
                        int id;
                        bool numeric = int.TryParse(parts[0], out id);
                        int x;
                        numeric = int.TryParse(parts[1], out x);
                        int y;
                        numeric = int.TryParse(parts[2], out y);
                        if (!numeric)
                        {
                            // The file contains some invalid lines
                            result.Valid = false;
                            break;
                        }
                        else
                        {
                            // Add item and read new line
                            result.ItemBins.Add(new ItemBin(id, x, y));
                            line = reader.ReadLine();
                        }
                    }
                }
            }

            result.CreateDistanceTable();
            return result;
        }

        private void CreateDistanceTable()
        {
            DistanceTable = new List<DistanceItem>();
            for (int i = 0; i < ItemBins.Count; i++)
            {
                for (int j = i + 1; j < ItemBins.Count; j++)
                {
                    DistanceTable.Add(new DistanceItem(ItemBins[i], ItemBins[j]));
                }
            }
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

        public double GetSquareDistance(Coordinate target)
        {
            var xDistance = System.Math.Abs(XCoordinate - target.XCoordinate);
            var yDistance = System.Math.Abs(YCoordinate - target.YCoordinate);
            return System.Math.Pow(xDistance, 2) + System.Math.Pow(yDistance, 2);
        }
    }

    public class DistanceItem
    {
        public ItemBin Item1 { get; set; }
        public ItemBin Item2 { get; set; }

        public double SquareDistance { get; set; }

        public double Distance { get; set; }

        public DistanceItem()
        {
            Item1 = new ItemBin();
            Item2 = new ItemBin();
            SquareDistance = 0;
            Distance = 0;
        }

        public DistanceItem(ItemBin bin1, ItemBin bin2)
        {
            Item1 = bin1;
            Item2 = bin2;
            SquareDistance = Item1.Location.GetSquareDistance(Item2.Location);
            Distance = System.Math.Sqrt(SquareDistance);
        }
    }
}