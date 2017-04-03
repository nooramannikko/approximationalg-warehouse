using System;
using ht1.Solver;

namespace ht1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warehouse Route Planner!");

            var router = new WarehouseRouter();

            // Read input and proceed accordingly
            var input = Console.ReadLine();
            do
            {
                Command command = Command.CreateCommand(input);
                if (command.IsValid())
                {
                    // Proceed
                    if (command.Type == CommandType.SetLayout)
                    {
                        // Store layout
                        bool success = router.SetLayout(command);
                        if (!success)
                        {
                            Console.WriteLine(Command.GetFileErrorMessage());
                        }
                        else
                        {
                            Console.WriteLine("Layout set successfully");
                        }
                    }
                    else if (command.Type == CommandType.SetRequest)
                    {
                        // Store request
                        bool success = router.SetRequest(command);
                        if (!success)
                        {
                            Console.WriteLine(Command.GetFileErrorMessage());
                        }
                        else
                        {
                            Console.WriteLine("Request stored successfully");
                        }
                    }
                    else if (command.Type == CommandType.ProcessRequest)
                    {
                        // Run solver
                        string route;
                        bool success = router.FindRoute(command, out route);
                        if (success)
                        {
                            Console.WriteLine(route);
                        }
                        else
                        {
                            Console.WriteLine("The given request doesn't exist");
                        }
                    }
                    else if (command.Type == CommandType.Help)
                    {
                        Console.WriteLine(Command.GetHelpText());
                    }
                    else if (command.Type == CommandType.Quit)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    // Invalid command, print error message and help text
                    Console.WriteLine(Command.GetCommandErrorMessage());
                    Console.WriteLine(Command.GetHelpText());
                }
                input = Console.ReadLine();
            }
            while (!string.IsNullOrEmpty(input));
        }
    }
}
