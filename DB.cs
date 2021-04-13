using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace Burse_Bot
{
    internal class DB
    {
        //Данные для подключения к БД
        private readonly string conStr = ConfigurationManager.AppSettings.Get("Connect");

        private SqlDataReader readerBonus;
        private SqlDataReader readerBonusInterval;
        private SqlDataReader readerIdTel;
        private SqlDataReader readerFeedBack;
        private SqlDataReader readerAllID;

        private string oper = null;


        public async Task<string> FeedBackOutput(string username)
        {
            username = username.Replace("@", string.Empty);

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                string outFeed = null;
                int n = 1;

                var cmd = new SqlCommand($@"select Table_FeedB.Feedback, Table_FeedB.Poc 
FROM Table_FeedB Join Table_UserTeleg ON Table_UserTeleg.IdTeleg = Table_FeedB.IdProdav
wHERE Table_UserTeleg.UsernameTeleg = '{username}'", connection);

                readerFeedBack = await cmd.ExecuteReaderAsync();

                while (await readerFeedBack.ReadAsync())
                {
                    outFeed += $@"{n}. Покупатель: @{readerFeedBack.GetValue(1)}
Отзыв: {readerFeedBack.GetValue(0)}" + Environment.NewLine;
                }

                return outFeed;
            }
        }


        //Запрос на получение id всех пользователей
        public List<string> AllIDTeleg()
        {
            using (var connection = new SqlConnection(conStr))
            {
                connection.Open();

                var allId = new List<string>();

                var cmd = new SqlCommand($@"select Table_UserTeleg.IdTeleg From Table_UserTeleg", connection);

                readerAllID = cmd.ExecuteReader();

                while (readerAllID.Read())
                {
                    allId.Add(readerAllID.GetValue(0).ToString());
                }

                return allId;
            }
        }

        public async Task FeedBackAnswer(string idTeleg, string valFeed, string Pocupat, string FeedBack)
        {
            valFeed = valFeed == "Положительный" ? "GoodFeed" : "BadFeed";
            oper = valFeed == "GoodFeed" ? "+" : "-";

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                var cmd = new SqlCommand($@"insert into Table_FeedB (Feedback, Poc, IdProdav) VALUES ('{FeedBack}', '{Pocupat}', '{idTeleg}');
UPDATE Table_FeedCount Set {valFeed} = {valFeed} {oper} 1
where Table_FeedCount.IdTelegUser = '{idTeleg}'", connection);

                var rowAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        //дОДЕЛАТЬ
        public async Task<string> CheckUser(string username)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                string idtel = null;

                var cmd = new SqlCommand($@"select Table_UserTeleg.IdTeleg FROM Table_UserTeleg
Where Table_UserTeleg.UsernameTeleg = '{username}'", connection);

                readerIdTel = await cmd.ExecuteReaderAsync();

                while (await readerIdTel.ReadAsync())
                {
                    idtel = (string)readerIdTel.GetValue(0);
                }

                return idtel;
            }
        }

        public async Task input(string IdTeleg)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                var cmd = new SqlCommand($"insert into Table_FeedCount (IdTelegUser, GoodFeed, BadFeed) values ('{IdTeleg}', 0, 0)", connection);

                var rowAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task RegSQL(string logTeleg, string IdTeleg)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                var cmd = new SqlCommand($"INSERT INTO Table_UserTeleg (UsernameTeleg, IdTeleg) VALUES ('{logTeleg}', '{IdTeleg}')", connection);

                var rowAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task InsertPrice(string IdTeleg, int price, int bonus, string shop)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                var cmd = new SqlCommand($"INSERT INTO Table_BonusPrice (userTelID, Price, Bonus, Shop) VALUES ('{IdTeleg}', '{price}', '{bonus}', '{shop}')", connection);

                var rowAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        //Запрос на данные ассортименте бонусов выбраного магазина (всего 10 строк)
        public async Task<string> OutPutBonus(string shopname)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                string outputBonus = null;
                int n = 1;

                var cmd = new SqlCommand($@"select top 10 Table_UserTeleg.UsernameTeleg, Table_BonusPrice.Shop, Table_BonusPrice.Bonus, Table_BonusPrice.Price, 
Table_FeedCount.GoodFeed, Table_FeedCount.BadFeed  from 
Table_UserTeleg 
join Table_BonusPrice on Table_BonusPrice.userTelID = Table_UserTeleg.IdTeleg
join Table_FeedCount on Table_FeedCount.IdTelegUser = Table_UserTeleg.IdTeleg
where Table_BonusPrice.Shop = '{shopname}'
order by Table_BonusPrice.Bonus Desc, Table_BonusPrice.Price Asc", connection);

                readerBonus = await cmd.ExecuteReaderAsync();

                while (await readerBonus.ReadAsync())
                {
                    outputBonus += $@"{n}. @{readerBonus.GetValue(0)}({readerBonus.GetValue(4)}/{readerBonus.GetValue(5)}) 
Магазин: {readerBonus.GetValue(1)}, {readerBonus.GetValue(2)} бонусов за {readerBonus.GetValue(3)} р." + Environment.NewLine;
                    n++;
                }

                return outputBonus;
            }

        }


        //Вывод данных из бд в указаном интервале строк
        public async Task<string> OutPutBonusInterval(string shopname, int from, int before)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();

                string outputBonusInterval = null;
                int n = 1;

                var cmd = new SqlCommand($@"select Table_UserTeleg.UsernameTeleg, Table_BonusPrice.Shop, Table_BonusPrice.Bonus, Table_BonusPrice.Price, 
Table_FeedCount.GoodFeed, Table_FeedCount.BadFeed  from 
Table_UserTeleg 
join Table_BonusPrice on Table_BonusPrice.userTelID = Table_UserTeleg.IdTeleg
join Table_FeedCount on Table_FeedCount.IdTelegUser = Table_UserTeleg.IdTeleg
where Table_BonusPrice.Shop = '{shopname}'
order by Table_BonusPrice.Bonus Desc, Table_BonusPrice.Price Asc
OFFSET {from} ROWS
FETCH NEXT {before} ROWS ONLY", connection);

                readerBonusInterval = await cmd.ExecuteReaderAsync();

                while (await readerBonusInterval.ReadAsync())
                {
                    outputBonusInterval += $@"{n}. @{readerBonusInterval.GetValue(0)}({readerBonusInterval.GetValue(4)}/{readerBonusInterval.GetValue(5)}) 
Магазин: {readerBonusInterval.GetValue(1)}, {readerBonusInterval.GetValue(2)} бонусов за {readerBonusInterval.GetValue(3)} р." + Environment.NewLine;
                    n++;
                }

                return outputBonusInterval;
            }
        }
        public DB() { }
        
    }
}
