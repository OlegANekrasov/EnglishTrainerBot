﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Объект этого класса обрабатывает полученные сообщения через метод Response
    /// </summary>
    public class BotMessageLogic
    {
        private Messenger messanger;
        private Dictionary<long, Conversation> chatList;
        private ITelegramBotClient botClient;
        public CommandParser commandParser;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            messanger = new Messenger(botClient);
            commandParser = new CommandParser(botClient);
            chatList = new Dictionary<long, Conversation>();
        }

        public async Task Response(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery != null)
            {
                var Id = update.CallbackQuery.Message.Chat.Id;

                if (!chatList.ContainsKey(Id))
                {
                    var newchat = new Conversation(update.CallbackQuery.Message.Chat);
                    chatList.Add(Id, newchat);
                }

                var chat = chatList[Id];

                if (chat.isTraningMode && !chat.TraningTypeSelected)
                {
                    switch (update.CallbackQuery.Data)
                    {
                        case "rustoeng":
                            chat.trainingType = TrainingType.RusToEng;
                            chat.TraningTypeSelected = true;
                            break;
                        case "engtorus":
                            chat.trainingType = TrainingType.RusToEng;
                            chat.TraningTypeSelected = true;
                            break;
                        default:
                            break;
                    }

                    await SendMessage(chat);
                }

                /*
                if (chat.isTraningMode && chat.TraningTypeSelected)
                {
                    await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "gggg");
                }
                */

                // Этот метод вызывается, чтобы «часики» на кнопке, которые обозначают ответ сервера,
                // пропали — мы как раз отвечаем от сервера, что запрос обработан. 
                await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }
            else
            {
                var Id = update.Message.Chat.Id;

                if (!chatList.ContainsKey(Id))
                {
                    var newchat = new Conversation(update.Message.Chat);
                    chatList.Add(Id, newchat);
                }

                var chat = chatList[Id];

                chat.AddMessage(update.Message);

                 await SendMessage(chat);
                //await SendTextMessage(chat);
                //await SendTextWithKeyBoard(chat);
            }
        }

        private async Task SendMessage(Conversation chat)
        {
            await messanger.MakeAnswer(chat);
        }

        /*
        private async Task SendTextMessage(Conversation chat)
        {
            var text = messanger.CreateTextMessage(chat);

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        private async Task SendTextWithKeyBoard(Conversation chat)
        {
            string text = messanger.CreateTextMessage(chat);
            InlineKeyboardMarkup keyboard = ReturnKeyBoard();
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }
        private InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton("Пушкин")
                {
                  Text = "Пушкин",
                  CallbackData = "pushkin"
                },

                new InlineKeyboardButton("Есенин")
                {
                  Text = "Есенин",
                  CallbackData = "esenin"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }
        */
    }
}
