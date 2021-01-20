using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Burse_Bot
{
    internal static class BotCommands
    {
        private static string outputshop;
        private static string outputshopInterval;

        public static async Task MessageReplyText(TelegramBotClient bot, Message msg, DB db)
        {
            var text = msg?.Text;
            var username = $"{msg.Chat.FirstName} {msg.Chat.LastName}";

            switch (text)
            {
                case "/start":

                    if (msg.Chat.Username == null)
                    {
                        await bot.SendTextMessageAsync(msg.Chat.Id, @"Задайте имя пользователя телеграмм!
1. Перейдите в настройки
2. Выберите графу 'Аккаунт'
3. Задайте имя пользователя
После этих процедур нажмите сюда /start");

                        return;
                    }

                    try
                    {
                        await db.RegSQL(msg.Chat.Username, Convert.ToString(msg.Chat.Id));
                        await db.input(Convert.ToString(msg.Chat.Id));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Пользователь {msg.Chat.FirstName} {msg.Chat.LastName} с логином {msg.Chat.Username} уже есть в БД");
                    }
                    finally
                    {

                        var keyboard = new ReplyKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            new KeyboardButton("Купить"),
                            new KeyboardButton("Продать")
                        },
                        new[]
                        {
                            new KeyboardButton("Отзывы"),
                            new KeyboardButton("Услуги гаранта")
                        }
                    }
                        );

                        await bot.SendTextMessageAsync(msg.Chat.Id, $@"
Приветствую тебя,<b> {username} </b>!
Данный бот позволяет продавать или покупать промокоды различных магазинов у пользователей",
    parseMode: ParseMode.Html,
    replyMarkup: keyboard);
                    }
                        break;

                case "Услуги гаранта":

                    await bot.SendTextMessageAsync(msg.From.Id, @"<i>Услуги гаранта сделок при покупке или продаже кодов или бонусов</i>

Контакт гаранта: @YURAFRALAV

Вариант №1 - стоимость услуги 10% от сделки
Гарант проверит код продавца и сообщит вам о результате проверки ещё до того, как вы заплатили за код.

Вариант №2 - стоимость услуги 15% от сделки
Гарант принимает деньги на свой счет и передаёт их продавцу только после того, когда покупатель подтвердил использование кода/бонусов. Лучше всего подходит для продажи/покупки бонусов.

<i>Прежде чем связываться с гарантом, вы должны договориться о подобной сделке с покупателем/продавцом. После согласия обеих сторон напишите гаранту и изложите суть сделки.</i>",
parseMode: ParseMode.Html);

                    break;

                case "Отзывы":

                    var inlineFeed = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Оставить отзыв")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Получить отзывы")
                        }
                    });

                    await bot.SendTextMessageAsync(msg.Chat.Id, "Выберите вариант",
                        replyMarkup: inlineFeed);

                    break;

                case "Купить":

                    var inlineBuy = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Бонусы М.Видео")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Бонусы Эльдорадо")
                        }
                    });

                    await bot.SendTextMessageAsync(msg.Chat.Id, "Выберите магазин",
                        replyMarkup: inlineBuy);

                    break;

                case "Продать":

                    var inline = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("М.Видео")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Эльдорадо")
                        }
                    }
                        );

                    await bot.SendTextMessageAsync(msg.Chat.Id, "Выберите магазин",
                        replyMarkup: inline);

                    break;
            }
        }

        public static async Task OutputShop(TelegramBotClient bot, CallbackQuery callbackdata, DB db, string ShopName)
        {
            var inlineCount = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("10 - 30"),
                    InlineKeyboardButton.WithCallbackData("30 - 50"),
                    InlineKeyboardButton.WithCallbackData("50 - 100")
                } 
            });

            outputshop = await db.OutPutBonus(ShopName) ?? "Предложения пока отсутствуют";

            await bot.SendTextMessageAsync(callbackdata.From.Id, outputshop, replyMarkup: inlineCount);
        }

        public static async Task OutputShopInteraval(TelegramBotClient bot, CallbackQuery callbackdata, DB db, string ShopName, int from, int before)
        {
            outputshopInterval = await db.OutPutBonusInterval(ShopName, from, before) ?? "Предложения пока отсутствуют";

            await bot.SendTextMessageAsync(callbackdata.From.Id, outputshopInterval);
        }


    }
}
