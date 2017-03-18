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
        // Store Layout
        // Process requests

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
    }
}