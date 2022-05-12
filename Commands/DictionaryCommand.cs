using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    public class DictionaryCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;

        public DictionaryCommand(ITelegramBotClient botClient)
        {
            CommandText = "/dictionary";
            this.botClient = botClient;
        }

        public async Task RunCommand(Conversation chat)
        {
            if (!chat.dictionary.Any())
            {
                await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Словарь пуст!");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var word in chat.dictionary.OrderBy(o => o.English))
                {
                    sb.Append(word.English + " - " + word.Russian + " - " + word.Theme + "\n");
                }

                await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: sb.ToString());
            }
        }
    }
}
