using System;
using ht1.Solver;

namespace ht1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Warehouse Route Planner!");

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
                    }
                    else if (command.Type == CommandType.SetRequest)
                    {
                        // Store request
                    }
                    else if (command.Type == CommandType.ProcessRequest)
                    {
                        // Run solver
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
                    Console.WriteLine(Command.GetCommandErrorMessage());
                    Console.WriteLine(Command.GetHelpText());
                }
                input = Console.ReadLine();
            }
            while (!string.IsNullOrEmpty(input));
        }
    }
}
