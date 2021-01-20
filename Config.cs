namespace Burse_Bot
{
    internal static class Config
    {
        private const string TelegramBotTokenKey = "1489146239:AAELSSu3JPXl9Sse-EME1jnj01JUC3EN3U4";

        public static string TelegramBotToken { get; }

        static Config()
        {
            TelegramBotToken = TelegramBotTokenKey;
        }
    }
}
