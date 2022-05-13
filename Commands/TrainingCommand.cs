using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Команда проведения тренировок
    /// </summary>
    public class TrainingCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;
        private string[] words;
        private string currentWord;
        private int currentIndex;
        private int length;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";
            this.botClient = botClient;
        }

        public void SetWords(Conversation chat, List<Word> dictionary)
        {
            if(chat.trainingType == TrainingType.EngToRus)
                words = dictionary.Select(o => o.English).ToArray();
            else
                words = dictionary.Select(o => o.Russian).ToArray();

            this.length = words.Length;
            currentIndex = 0;
        }

        public async Task RunCommand(Conversation chat, string answer)
        {
            var text = "";
            var word = "Слово: ";
            if (currentIndex > 0)
            {
                var result = chat.CheckWord(currentWord, answer);
                if (result)
                {
                    text = "Правильно!\n";
                }
                else
                {
                    text = "Неправильно!\n";
                }
            }

            if(currentIndex < length)
            {
                currentWord = words[currentIndex++];
                word += currentWord;
                text += word;
            }
            else
            {
                text += "Тренировка завершена!";
                chat.isTraningMode = false;
                chat.TraningTypeSelected = false;
                chat.isSetWords = false;
            }

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
    }
}
