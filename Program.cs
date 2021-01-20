using System;

namespace Burse_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TelegramBotEdt telegrambot = new TelegramBotEdt(Config.TelegramBotToken))
            {

                telegrambot.StartListening();

                telegrambot.SetUpTimer(new TimeSpan(22, 40, 10));

                Console.ReadLine();

                telegrambot.StopListening();
            }
        }
    }
}
