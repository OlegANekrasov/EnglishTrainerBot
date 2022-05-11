using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Класс, отвечающий за работу клиента бота на верхнем уровне
    /// </summary>
    public class BotWorker
    {
        private ITelegramBotClient botClient;
        private CancellationTokenSource cts;
        private BotMessageLogic logic;

        /// <summary>
        /// Создает клиента бота
        /// </summary>
        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }

        /// <summary>
        /// Начинается ожидание сообщений и отправка ответов на них
        /// </summary>
        public void Start()
        {
            cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                // получать все типы обновлений
                AllowedUpdates = Array.Empty<UpdateType>() 
            };

            // StartReceiver не блокирует вызывающий поток. Получение выполняется в ThreadPool.
            botClient.StartReceiving(
                updateHandler: logic.Response,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        /// <summary>
        /// Завершает процесс
        /// </summary>
        public void Stop()
        {
            // Отправить запрос на отмену, чтобы остановить бота
            cts.Cancel();
        }

        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <param name="botClient"> botClient </param>
        /// <param name="exception"> exception </param>
        /// <param name="cancellationToken"> cancellationToken </param>
        /// <returns></returns>
        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
