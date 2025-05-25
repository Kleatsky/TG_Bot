using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Telegram.Bot.TelegramBotClient;

namespace ServerConsole
{
    internal class UpdateHandler : IUpdateHandler
    {
        public delegate void MessageHandler(object sender, string message);
        public event MessageHandler OnHandleUpdateStarted;
        public event MessageHandler OnHandleUpdateCompleted;
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message != null)
            {

                var chatId = update.Message.Chat.Id;
                var messageText = update.Message?.Text ?? "ERROR";

                OnHandleUpdateStarted.Invoke(this, messageText);

                if (messageText == "/cat")
                {
                    string catFact = await CatFacts.GetFactAsync();
                    await bot.SendMessage(
                        chatId: chatId,
                        text: catFact,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    await bot.SendMessage(
                        chatId: chatId,
                        text: "Сообщение успешно принято",
                        cancellationToken: cancellationToken
                    );
                }

                OnHandleUpdateCompleted.Invoke(this, messageText);
            }
        }
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            await Task.Delay(2000, cancellationToken);
        }
    }
}
