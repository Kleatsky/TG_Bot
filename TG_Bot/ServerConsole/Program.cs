using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ServerConsole
{
    internal class Program
    {
        private static string _filePath = @"Token.txt";//файл с токеном
        private static string _token;

        private static void OnHandleUpdateStarted(object sender, string message)
        {
            if (sender == null) return;
            Console.WriteLine($"Началась обработка сообщения '{message}'");
        }
        private static void OnHandleUpdateCompleted(object sender, string message)
        {
            if (sender == null) return;
            Console.WriteLine($"Закончилась обработка сообщения '{message}'");
        }
        static async Task Main(string[] args)
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var lines = File.ReadAllLines(_filePath);
                    _token = lines[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
            else
            {
                return;
            }

            var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient(_token);


            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            UpdateHandler updateHandler = new UpdateHandler();
            updateHandler.OnHandleUpdateStarted += OnHandleUpdateStarted;//подписка с обработкой через OnHandleUpdateStarted
            updateHandler.OnHandleUpdateCompleted += OnHandleUpdateCompleted;//подписка с обработкой через OnHandleUpdateCompleted

            bot.StartReceiving(
                updateHandler: updateHandler.HandleUpdateAsync,
                errorHandler: updateHandler.HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            //Цикл ожидания завершения программы
            while (true)
            {
                Console.WriteLine("Press 'A' button to close program.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.A)
                {
                    await cts.CancelAsync();

                    try
                    {
                        updateHandler.OnHandleUpdateStarted -= OnHandleUpdateStarted;//Отписка от события
                        updateHandler.OnHandleUpdateCompleted -= OnHandleUpdateCompleted;//Отписка от события
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error while unsubscribe: " + e.Message);
                    }
                    Console.WriteLine("Closing program.");
                    return;
                }
                else
                {
                    var me = await bot.GetMe();
                    Console.WriteLine($"Name: {me.FirstName} {me.LastName} | {me.Username} id: {me.Id} " +
                        $"Premial: {me.IsPremium} LanguageCode: {me.LanguageCode}");
                }
            }
        }
    }
}
