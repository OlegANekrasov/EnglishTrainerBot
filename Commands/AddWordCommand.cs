using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Команда добавления слов в словарь
    /// </summary>
    public class AddWordCommand : AbstractCommand
    {
        private ITelegramBotClient botClient; 
        private Word word;

        private AddingState addState;
        public AddingState AddState 
        {
            get { return addState; }
            set { addState = value; }
        }

        public void NextAddingState()
        {
            switch (addState)
            {
                case AddingState.Russian:
                    addState = AddingState.English;
                    break;
                case AddingState.English:
                    addState = AddingState.Theme;
                    break;
                case AddingState.Theme:
                    addState = AddingState.Finish;
                    break;
            }
        }

        public AddWordCommand(ITelegramBotClient botClient)
        {
            CommandText = "/addword";
            this.botClient = botClient;
        }

        public async Task RunCommand(Conversation chat, string text = "")
        {
            switch (addState)
            {
                case AddingState.Russian:
                    word = new Word();
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Введите русское значение слова");
                    break;
                case AddingState.English:
                    word.Russian = text;
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Введите английское значение слова");
                    break;
                case AddingState.Theme:
                    word.English = text;
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Введите тематику");
                    break;
                case AddingState.Finish:
                    word.Theme = text;
                    chat.dictionary.Add(word);
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), 
                                                        text: "Успешно! Слово " + word.English + " добавлено в словарь. ");
                    break;
            }
        }
    }
}
