using System.Collections.Generic;
using Telegram.Bot.Types;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Представляет собой объект чата
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Объект Телеграм-чата
        /// </summary>
        private Chat telegramChat;

        /// <summary>
        /// Список сообщений 
        /// </summary>
        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public long GetId() => telegramChat.Id;

        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;

    }
}
