namespace ht1.Solver
{
    public enum CommandType
    {
        Unknown = 0,
        SetLayout = 1,
        SetRequest = 2,
        ProcessRequest = 3,
        Help = 4,
        Quit = 5
    }

    public enum RequestProcessMode
    {
        Unknown = 0,
        Bruteforce = 1,
        NearestAddition = 2
    }

    public class Command
    {
        public CommandType Type { get; set; }
        public string CommandArgFile { get; set; }
        public RequestProcessMode ProcessMode { get; set; }
        public string RequestName { get; set; }

        public Command()
        {
            Type = CommandType.Unknown;
            CommandArgFile = null;
            ProcessMode = RequestProcessMode.Unknown;
            RequestName = null;
        }

        public bool IsValid()
        {
            // Layout and request command require files as parameter
            if ((Type == CommandType.Unknown) || (Type == CommandType.ProcessRequest && ProcessMode == RequestProcessMode.Unknown)
                || (Type == CommandType.SetLayout && string.IsNullOrEmpty(CommandArgFile)
                || (Type == CommandType.SetRequest && string.IsNullOrEmpty(CommandArgFile) && string.IsNullOrEmpty(RequestName))))
            {
                return false;
            }
            return true;
        }

        public static Command CreateCommand(string commandText)
        {
            var result = new Command();
            // TODO: parse commandText
            if (!string.IsNullOrEmpty(commandText))
            {
                var parts = commandText.Split(' ');
                if (parts.Length > 0)
                {
                    string type = parts[0];
                    result.Type = GetCommandType(type);
                    if (result.Type == CommandType.SetLayout && parts.Length > 1)
                    {
                        // Requires file path
                        result.CommandArgFile = parts[1];
                    }
                    else if (result.Type == CommandType.SetRequest && parts.Length > 2)
                    {
                        // Requires file path and name
                        result.CommandArgFile = parts[1];
                        result.RequestName = parts[2];
                    }
                    else if (result.Type == CommandType.ProcessRequest && parts.Length > 2)
                    {
                        // Requires name and mode
                        result.RequestName = parts[1];
                        string mode = parts[2];
                        if (mode == "--bf")
                        {
                            result.ProcessMode = RequestProcessMode.Bruteforce;
                        }
                        else if (mode == "--na")
                        {
                            result.ProcessMode = RequestProcessMode.NearestAddition;
                        }
                    }
                }
            }
            return result;
        }

        public static string GetCommandErrorMessage()
        {
            return "Invalid command or arguments!";
        }

        public static string GetFileErrorMessage()
        {
            return "Invalid file contents!";
        }

        public static string GetHelpText()
        {
            return "The following commands are available:\n"
                + "help: Print this help text\n"
                + "layout [file path]: Set layout from given file path\n"
                + "request [file path] [name]: Set request from given file path with given name\n"
                + "run [name] --bf/--na: Run request of given name, --bf for bruteforce and --na for nearest addition\n"
                + "quit: Exit program";
        }

        private static CommandType GetCommandType(string type)
        {
            var result = CommandType.Unknown;
            if (type == "help")
            {
                result = CommandType.Help;
            }
            else if (type == "quit")
            {
                result = CommandType.Quit;
            }
            else if (type == "layout")
            {
                result = CommandType.SetLayout;
            }
            else if (type == "request")
            {
                result = CommandType.SetRequest;
            }
            else if (type == "run")
            {
                result = CommandType.ProcessRequest;
            }
            return result;
        }
    }
}