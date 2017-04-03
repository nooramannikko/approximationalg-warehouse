using System.Collections.Generic;

namespace ht1.Solver
{
    public class WarehouseRouter
    {
        public Layout Layout { get; set; }
        public List<Request> Requests { get; set; }

        public WarehouseRouter()
        {
            Layout = new Layout();
            Requests = new List<Request>();
        }

        public bool SetLayout(Command command)
        {
            Layout = Layout.Create(command.CommandArgFile);
            return Layout.Valid;
        }

        public bool SetRequest(Command command)
        {
            var request = Request.Create(command.CommandArgFile, command.RequestName);
            // Remove previous requests with same name
            Requests.RemoveAll(req => req.Name == request.Name);
            Requests.Add(request);
            return request.Valid;
        }

        public bool FindRoute(Command command, out string route)
        {
            route = "";
            var result = new List<Route>();
            var request = Requests.Find(req => req.Name == command.RequestName);
            if (request == null)
            {
                // Request with given name doesn't exist
                return false;
            }

            if (command.ProcessMode == RequestProcessMode.NearestAddition)
            {
                // Find route with nearest addition algorithm
                result = NearestAddition(request);
            }
            else if (command.ProcessMode == RequestProcessMode.Bruteforce)
            {
                // Find route with brute force
            }
            foreach (var r in result)
            {
                route += r.StartPoint.Identifier.ToString() + " ";
            }
            return true;
        }

        private List<Route> NearestAddition(Request request)
        {
            // Initial result contains the starting position of the robot ang route length 0
            var result = new List<Route>() { new Route() };

            if (request.Targets.Count > 0)
            {
                // Create copy of targets so that the original version doesn't change
                var targets = new List<int>();
                request.Targets.ForEach(t => targets.Add(t));

                // Go through the current route and find the nearest requested bin to add
                // Add first target
                ItemBin currentBestBin = null;
                DistanceItem currentBestDistance = null;
                foreach (var target in targets)
                {
                    var bin = Layout.ItemBins.Find(b => b.Identifier == target);
                    var distanceItem = Layout.DistanceTable.Find(d => (d.Item1.Identifier == 0 && d.Item2.Identifier == target) || (d.Item1.Identifier == target && d.Item2.Identifier == 0));
                    if (bin != null && distanceItem != null && (currentBestDistance == null || distanceItem.SquareDistance < currentBestDistance.SquareDistance))
                    {
                        currentBestBin = bin;
                        currentBestDistance = distanceItem;
                    }
                }
                if (currentBestBin != null && currentBestDistance != null)
                {
                    // Update distance to adjacent bin
                    result[0].Length = currentBestDistance.Distance;
                    // Add new bin
                    result.Add(new Route(currentBestBin, currentBestDistance.Distance));
                    // Remove found target
                    targets.RemoveAll(t => t == currentBestBin.Identifier);
                    System.Console.WriteLine(currentBestDistance.Distance.ToString());
                }

                System.Console.WriteLine("Current route: " + result[0].StartPoint.Identifier.ToString() + "(" + result[0].Length.ToString() + ") "
                    + result[1].StartPoint.Identifier.ToString() + "(" + result[1].Length.ToString() + ")");

                var allTargetsAdded = (targets.Count == 0);
                while (!allTargetsAdded)
                {
                    currentBestBin = null;
                    DistanceItem distLeft = null;
                    DistanceItem distRight = null;
                    DistanceItem currentBestLeft = null;
                    DistanceItem currentBestRight = null;
                    double currentBestAddedDistance = -1;
                    int currentPosToAdd = -1;
                    for (int i = 0; i < result.Count; i++)
                    {
                        // For each pair of adjacent points in the route,
                        // find a target that increases the route distance the least
                        var currentRouteItem = result[i];
                        var nextRouteItem = (i + 1 >= result.Count) ? result[0] : result[i+1];
                        foreach (var target in targets)
                        {
                            // Fing the nearest target for this pair
                            distLeft = Layout.DistanceTable.Find(d => (d.Item1.Identifier == currentRouteItem.StartPoint.Identifier && d.Item2.Identifier == target) 
                                || (d.Item1.Identifier == target && d.Item2.Identifier == currentRouteItem.StartPoint.Identifier));
                            distRight = Layout.DistanceTable.Find(d => (d.Item1.Identifier == nextRouteItem.StartPoint.Identifier && d.Item2.Identifier == target) 
                                || (d.Item1.Identifier == target && d.Item2.Identifier == nextRouteItem.StartPoint.Identifier));
                            System.Console.WriteLine("Pair " + currentRouteItem.StartPoint.Identifier.ToString() + " " + nextRouteItem.StartPoint.Identifier.ToString() + "; Target: " + target);
                            System.Console.WriteLine("DistLeft: " + distLeft.Distance.ToString() + "; DistRight: " + distRight.Distance.ToString() + "; CurrentLength: " + currentRouteItem.Length.ToString());
                            if (currentBestAddedDistance < 0 || currentBestAddedDistance > (distLeft.Distance + distRight.Distance - currentRouteItem.Length))
                            {
                                currentPosToAdd = i;
                                currentBestAddedDistance = distLeft.Distance + distRight.Distance - currentRouteItem.Length;
                                currentBestBin = (distLeft.Item1.Identifier == target) ? distLeft.Item1 : distLeft.Item2;
                                currentBestLeft = distLeft;
                                currentBestRight = distRight;
                            }
                        }
                    }
                    if (currentBestBin != null && currentBestLeft != null && currentBestRight != null 
                        && currentBestAddedDistance > -1 && currentPosToAdd >= 0)
                    {
                        // Update distance to adjacent bin
                        result[currentPosToAdd].Length = currentBestLeft.Distance;
                        // Add new bin between i and i + 1
                        result.Insert(currentPosToAdd + 1, new Route(currentBestBin, currentBestRight.Distance));
                        // Remove found target
                        targets.RemoveAll(t => t == currentBestBin.Identifier);
                    }
                    allTargetsAdded = (targets.Count == 0);

                    var routeString = "";
                    foreach (var r in result)
                    {
                        routeString += r.StartPoint.Identifier.ToString() + " (" + r.Length.ToString() + ") ";
                    }
                    System.Console.WriteLine("Current route: " + routeString);
                }
            }
            return result;
        }
    }
}