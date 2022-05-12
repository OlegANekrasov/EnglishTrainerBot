using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

            if(chat.isDictionaryMode)
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

            if (!chat.isDictionaryMode)
            {
                var command = commandParser.GetCommand(lastmessage);
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
                        }
                    }
                }
            }
        }

        /*
        public string CreateTextMessage(Conversation chat)
        {
            var text = "";
            var lastMessage = chat.GetLastMessage();

            var command = commandParser.GetCommand(lastMessage);
            if (command != null)
            {
                text = ((IChatTextCommand)command).ReturnText();
            }
            else
            {
                var delimiter = ",";
                text = "История ваших сообщений: " + string.Join(delimiter, chat.GetTextMessages().ToArray());
            }

            return text;
        }
        */
    }
}
