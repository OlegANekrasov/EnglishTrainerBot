using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishTrainerBot
{
    public class CommandParser
    {
        private List<IChatCommand> Commands;

        public CommandParser()
        {
            Commands = new List<IChatCommand>();

            //Commands.Add(new AskMeCommand());
            //Commands.Add(new SayHiCommand());
        }

        public IChatCommand GetCommand(string commandText)
        {
            return Commands.FirstOrDefault(o => o.CheckMessage(commandText));
        }
    }
}
