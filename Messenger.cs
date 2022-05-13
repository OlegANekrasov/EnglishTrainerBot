using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace EnglishTrainerBot
{
    public class Messenger
    {
        private ITelegramBotClient botClient;
        private CommandParser commandParser;

        public Messenger(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            commandParser = new CommandParser(botClient);
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastmessage = chat.GetLastMessage();

            if (chat.isTraningMode && chat.TraningTypeSelected)
            {
                var cmd = commandParser.GetCommand("/training");
                
                if(!chat.isSetWords)
                {
                    ((TrainingCommand)cmd).SetWords(chat, chat.dictionary);
                    chat.isSetWords = true;
                }
                if (lastmessage == "/stop") //|| ((TrainingCommand)cmd).isEndArray()
                {
                    chat.isTraningMode = false;
                    chat.TraningTypeSelected = false;
                    chat.isSetWords = false;
                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Тренировка завершена!");
                    return;
                }

                await ((TrainingCommand)cmd).RunCommand(chat, lastmessage);
            }

            if (chat.isDictionaryMode)
            {
                var cmd = commandParser.GetCommand("/addword");
                if (((AddWordCommand)cmd).AddState == AddingState.Finish)
                {
                    chat.isDictionaryMode = false;
                }
                else
                {
                    ((AddWordCommand)cmd).NextAddingState();
                    await ((AddWordCommand)cmd).RunCommand(chat, lastmessage);
                }
            }

            if (!chat.isDictionaryMode && !chat.isTraningMode)
            {
                string commandStr = lastmessage;
                int i = lastmessage.IndexOf(" ");
                if (i != -1)
                {
                    commandStr = lastmessage.Substring(0, i);
                }

                var command = commandParser.GetCommand(commandStr);
                if (command != null)
                {
                    if (command is AbstractCommand cmd)
                    {
                        switch (cmd.CommandText)
                        {
                            case "/addword":
                                chat.isDictionaryMode = true;
                                ((AddWordCommand)cmd).AddState = AddingState.Russian;
                                await ((AddWordCommand)cmd).RunCommand(chat);
                                break;
                            case "/dictionary":
                                await ((DictionaryCommand)cmd).RunCommand(chat);
                                break;
                            case "/deleteword":
                                await ((DeleteWordCommand)cmd).RunCommand(chat, lastmessage.Substring(i).Trim());
                                break;
                            case "/training":
                                if (chat.dictionary.Any())
                                {
                                    chat.isTraningMode = true;
                                    await SendTextWithKeyBoard(chat);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: "Словарь пуст!");
                                }
                                break;
                        }
                    }
                }
            }
        }

        private async Task SendTextWithKeyBoard(Conversation chat)
        {
            InlineKeyboardMarkup keyboard = ReturnKeyBoard();
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), 
                text: "Выберите тип тренировки. Для окончания тренировки введите команду /stop", replyMarkup: keyboard);
        }

        private InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton("С русского на английский")
                {
                  Text = "С русского на английский",
                  CallbackData = "rustoeng"
                },

                new InlineKeyboardButton("С английского на русский")
                {
                  Text = "С английского на русский",
                  CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }
    }
}
