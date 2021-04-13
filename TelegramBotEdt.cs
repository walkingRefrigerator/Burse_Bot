
using System;
using System.Collections.Generic;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Timers;




namespace Burse_Bot
{
    internal class TelegramBotEdt : IDisposable
    {
        private int Bonus;
        private int Price;
        private int FromInterval;
        private int BeforeInterval;
        private string Shop;
        private string IdTeleg;
        private string FeedOpe;
        private string FeedOutput;
        private readonly string _telegrambotToken;
        private DB db;
        private List<Variable.ParseInfo> parseInfos;
        private ParseEld parse;
        private List<string> listAllIdUser;
        private const double interval8Hours = 60000; //Задание времени отправки сообщений
        private System.Timers.Timer checkForTime = new System.Timers.Timer(interval8Hours);
        private TelegramBotClient botClient;

        //Наполнение констуктора
        public TelegramBotEdt(string telegrambottoken)
        {
            _telegrambotToken = telegrambottoken;

            //Создание сущности нашего бота
            botClient = new TelegramBotClient(_telegrambotToken) { Timeout = TimeSpan.FromSeconds(10) };

            db = new DB();
            parse = new ParseEld();
            parseInfos = new List<Variable.ParseInfo>();
            listAllIdUser = new List<string>();

            //Подписываем на события обработчики
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += Bot_CallbackQuery;

            //Создание таймера для парсера
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;
        }


        #region Парс сайта
        public void ParseEld()
        {
            //Получение данных с сайта
            parse.GetPage();
            parseInfos = parse.ParsTover();
            InputOnlineFile file = new InputOnlineFile(parseInfos[1].PathImage);

            //Получение id всех пользователей
            listAllIdUser = db.AllIDTeleg();

            //Рассылка рекламы для всех пользователей
            int n = 0;
            while (n != listAllIdUser.Count)
            {
                botClient.SendPhotoAsync(listAllIdUser[n], file,
                    $@"<b><a href='{parseInfos[0].Url}'>{parseInfos[0].TitlePaper}</a></b>
{parseInfos[0].TagePaper}
{parseInfos[0].Description}", ParseMode.Html);

                n++;

            }

        }


        #endregion

        #region Таймер
        //Подписка на таймер метода с парсингом
        public void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
            => ParseEld();

        #endregion


        //Запуск\остановка бота
        #region Старт\Стоп бота
        public void StartListening()
        {
            botClient.StartReceiving();
        }

        public void StopListening()
        {
            botClient.StopReceiving();
        }
        #endregion

        //Обработка действий пользователя
        #region Обработчики
        private async void Bot_CallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var callbackdata = e.CallbackQuery;

            #region Кнопки бонусов
            var inlineBonus = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("500"),
                    InlineKeyboardButton.WithCallbackData("750"),
                    InlineKeyboardButton.WithCallbackData("1000")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("1500"),
                    InlineKeyboardButton.WithCallbackData("1750"),
                    InlineKeyboardButton.WithCallbackData("2000")
                }
            });

            #endregion

            switch (callbackdata.Data)
            {
                #region Область с магазинами
                case "М.Видео":

                    Shop = callbackdata.Data;

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Выберите кол-во бонусов для продажи",
                        replyMarkup: inlineBonus);

                    break;

                case "Эльдорадо":

                    Shop = callbackdata.Data;

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Выберите кол-во бонусов для продажи",
                        replyMarkup: inlineBonus);

                    break;
                #endregion

                #region Область с бонусами
                case "500":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                case "750":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                case "1000":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                case "1500":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                case "1750":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                case "2000":

                    Bonus = Convert.ToInt32(callbackdata.Data);

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите стоимость в рублях");

                    botClient.OnMessage += Bot_0nMessagePrice;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;

                    break;

                #endregion

                #region Бонусы магазинов

                case "Бонусы М.Видео":
                    
                    Shop = "М.Видео";

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, @"Ссылка на магазин: https://www.mvideo.ru/");
                    await BotCommands.OutputShop(botClient, callbackdata, db, Shop);

                    break;

                case "Бонусы Эльдорадо":

                    Shop = "Эльдорадо";

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, @"Ссылка на магазин: https://www.eldorado.ru/");
                    await BotCommands.OutputShop(botClient, callbackdata, db, Shop);

                    break;

                #endregion

                #region Интервал кол-ва заказов

                case "10 - 30":

                    FromInterval = 11;
                    BeforeInterval = 30;

                    await BotCommands.OutputShopInteraval(botClient, callbackdata, db, Shop, FromInterval, BeforeInterval);

                    break;

                case "30 - 50":

                    FromInterval = 31;
                    BeforeInterval = 50;

                    await BotCommands.OutputShopInteraval(botClient, callbackdata, db, Shop, FromInterval, BeforeInterval);

                    break;

                case "50 - 100":

                    FromInterval = 51;
                    BeforeInterval = 100;

                    await BotCommands.OutputShopInteraval(botClient, callbackdata, db, Shop, FromInterval, BeforeInterval);

                    break;

                #endregion

                #region Область с отзывами

                case "Оставить отзыв":

                    var inlineFeedBack = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Положительный"),
                            InlineKeyboardButton.WithCallbackData("Отрицательный")
                        }
                    });

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Уточните какой отзыв",
                        replyMarkup: inlineFeedBack);

                    break;

                case "Получить отзывы":

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите логин пользователя");

                    botClient.OnCallbackQuery -= Bot_CallbackQuery;
                    botClient.OnMessage -= Bot_OnMessage;
                    botClient.OnMessage += Bot_OutputFeed;

                    break;

                case "Положительный":

                    FeedOpe = "Положительный";

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите имя пользователя");

                    botClient.OnMessage += Bot_FeedBackTo1;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;
                    botClient.OnMessage -= Bot_OnMessage;

                    break;

                case "Отрицательный":

                    FeedOpe = "Отрицательный";

                    await botClient.SendTextMessageAsync(callbackdata.From.Id, "Введите имя пользователя");

                    botClient.OnMessage += Bot_FeedBackTo1;
                    botClient.OnCallbackQuery -= Bot_CallbackQuery;
                    botClient.OnMessage -= Bot_OnMessage;

                    break;

                    #endregion

            }
        }

        private async void Bot_OutputFeed(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            try
            {
                FeedOutput =  await db.FeedBackOutput(msg.Text);
                await botClient.SendTextMessageAsync(msg.From.Id, FeedOutput);
            }
            catch(Exception)
            {
                await botClient.SendTextMessageAsync(msg.From.Id, "Введен не верный логин пользователя");
            }
            finally
            {
                botClient.OnMessage -= Bot_OutputFeed;
                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += Bot_CallbackQuery;
            }
        }

        private async void Bot_FeedBackTo1(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            var username = msg.From.Username;

            msg.Text = msg.Text.Replace("@", string.Empty);

            if (msg.Text == username && msg.Text != "YURAFRALAV")
            {
                await botClient.SendTextMessageAsync(msg.From.Id, "Вы не можете оставлять отзыв о себе!");

                botClient.OnCallbackQuery += Bot_CallbackQuery;
                botClient.OnMessage -= Bot_FeedBackTo1;
                botClient.OnMessage += Bot_OnMessage;

                return;
            }

            IdTeleg = await db.CheckUser(msg.Text);

            if (IdTeleg == null)
            {
                await botClient.SendTextMessageAsync(msg.From.Id, "Введенно неверное имя пользователя");
                botClient.OnCallbackQuery += Bot_CallbackQuery;
                botClient.OnMessage -= Bot_FeedBackTo1;
                botClient.OnMessage += Bot_OnMessage;

                return;
            }

            await botClient.SendTextMessageAsync(msg.From.Id, "Введите отзыв о пользователе (не более 250 символов)");

            botClient.OnMessage -= Bot_FeedBackTo1;
            botClient.OnMessage += Bot_FeedBackTo2;

        }

        private async void Bot_FeedBackTo2(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            try
            {
                await db.FeedBackAnswer(IdTeleg, FeedOpe, msg.From.Username, msg.Text);
                await botClient.SendTextMessageAsync(msg.From.Id, "Вас отзыв оставлен!");
            }
            catch(Exception ex)
            {
                Console.WriteLine($@"Ошибка у пользователя {msg.From.Username}
Последнее сообщение: {msg.Text}
{ex}");
                await botClient.SendTextMessageAsync(msg.From.Id, "Не удалось оставить отзыв");
            }
            finally
            {
                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += Bot_CallbackQuery;
                botClient.OnMessage -= Bot_FeedBackTo2;
            }

        }

        private async void Bot_0nMessagePrice(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            try
            {
                Price = Convert.ToInt32(msg.Text);

                await db.InsertPrice(Convert.ToString(msg.Chat.Id), Price, Bonus, Shop);
                await botClient.SendTextMessageAsync(msg.Chat.Id, $@"Ваше предложение опубликовано!
@{msg.Chat.Username} Магазин: {Shop}, {Bonus} бонусов за {Price} р. ");

                botClient.OnMessage -= Bot_0nMessagePrice;
                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += Bot_CallbackQuery;
            }
            catch(Exception)
            {
                await botClient.SendTextMessageAsync(msg.Chat.Id, "Вы не ввели цену!");
                botClient.OnMessage -= Bot_0nMessagePrice;
                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += Bot_CallbackQuery;
            }
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var msg = e.Message;

            if (msg.Text == null)
                return;

            switch (msg.Type)
            {
                case MessageType.Text:
                    await BotCommands.MessageReplyText(botClient, msg, db);
                    break;
            }

        }
        #endregion

        #region Освобождение ресурсов
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
