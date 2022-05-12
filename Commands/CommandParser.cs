using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace EnglishTrainerBot
{
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
