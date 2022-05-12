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

        public async Task RunCommand(Conversation chat, string text)
        {
            if (!chat.dictionary.Any())
            {
                await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Словарь пуст!");
            }
            else
            {
                var word = chat.dictionary.FirstOrDefault(o => o.Russian.ToUpper() == text.ToUpper());
                if(word == null)
                {
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), 
                        text: "Слово \"" + text + "\" не найдено в словаре!");
                }
                else
                {
                    chat.dictionary.Remove(word);
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(),
                        text: "Слово \"" + text + "\" удалено из словаря");
                }
            }
        }
    }
}
