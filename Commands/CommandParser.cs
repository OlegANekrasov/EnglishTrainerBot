using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Список команд
    /// </summary>
    public class CommandParser
    {
        private List<IChatCommand> Commands;

        public CommandParser(ITelegramBotClient botClient)
        {
            Commands = new List<IChatCommand>(); 

            Commands.Add(new AddWordCommand(botClient));
            Commands.Add(new DictionaryCommand(botClient));
            Commands.Add(new DeleteWordCommand(botClient));
            Commands.Add(new TrainingCommand(botClient));
        }

        public IChatCommand GetCommand(string commandText)
        {
            return Commands.FirstOrDefault(o => o.CheckMessage(commandText));
        }
    }
}
