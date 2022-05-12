using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    public class TrainingCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;
        private Word[] words;
        private int currentIndex;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";
            this.botClient = botClient;
        }

        public void SetWords(List<Word> dictionary)
        {
            words = dictionary.ToArray();
            currentIndex = 0;
        }

        public async Task RunCommand(Conversation chat, string text)
        {
            
        }
    }
}
