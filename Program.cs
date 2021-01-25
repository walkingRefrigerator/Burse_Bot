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

                Console.ReadLine();

                telegrambot.StopListening();
            }
        }
    }
}
