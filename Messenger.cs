using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishTrainerBot
{
    public class Messenger
    {
        public string CreateTextMessage(Conversation chat, CommandParser commandParser)
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
                /*
                switch (chat.GetLastMessage())
                {
                    case "/saymehi":
                        text = "привет";
                        break;
                    case "/askme":
                        text = "как дела";
                        break;
                    default:
                        var delimiter = ",";
                        text = "История ваших сообщений: " + string.Join(delimiter, chat.GetTextMessages().ToArray());
                        break;
                }
                */
            return text;
        }
    }
}
