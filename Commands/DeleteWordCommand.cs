using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    class DeleteWordCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;

        public DeleteWordCommand(ITelegramBotClient botClient)
        {
            CommandText = "/deleteword";
            this.botClient = botClient;
        }
    }
}
