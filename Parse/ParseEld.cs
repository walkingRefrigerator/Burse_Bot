using AngleSharp.Parser.Html;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;

namespace Burse_Bot
{
    public class ParseEld
    {
        private string Response = null;
        private const string LinkEld = "https://www.eldorado.ru/actions.php?type=online";

        public void GetPageEld()
        {
            HttpRequest request = new HttpRequest();

            request.CharacterSet = Encoding.UTF8;
            request.AddHeader(HttpHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Accept-Language", "ru,ru-RU;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cache-Control", "max-age=0");
            request.AddHeader("Host", "www.eldorado.ru");
            request.AddHeader("Referer", "https://www.eldorado.ru/actions.php?type=online");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.KeepAlive = true;
            request.UserAgent = Http.OperaUserAgent();

            request.AllowAutoRedirect = true;

            HttpResponse response = request.Get(LinkEld);
            
            Response = response.ToString();
        }

        public List<Variable.ParseInfo> ParsTover()
        {
            List<Variable.ParseInfo> parses = new List<Variable.ParseInfo>();
            HtmlParser Hp = new HtmlParser();

            var Doc = Hp.Parse(Response);

            #region habr parse
            //foreach (var pap in Doc.QuerySelectorAll("ul.content-list.content-list_posts.shortcuts_items>li.content-list__item.content-list__item_post.shortcuts_item>article.post.post_preview"))
            //{
            //    parses.Add(new Variable.ParseInfo()
            //    {

            //        TitlePaper = pap.QuerySelector("h2.post__title>a.post__title_link").TextContent,
            //        Url = pap.QuerySelector("h2.post__title>a").GetAttribute("href"),
            //        TagePaper = pap.QuerySelector("ul.post__hubs.inline-list").TextContent,
            //        PathImage = pap?.QuerySelector("ul.content-list.content-list_posts.shortcuts_items  div div  img[src]")?.GetAttribute("src"),
            //        Description = pap.QuerySelector("ul li div.post__text.post__text-html.post__text").TextContent
            //    }
            //    );

            //}

            #endregion

            #region edlorado parse


            foreach (var pap in Doc.QuerySelectorAll("div.promotion__content div.promotion__promotions-group a.promotion__promotion"))
            {
                parses.Add(new Variable.ParseInfo()
                {
                    TitlePaper = pap?.QuerySelector("a.promotion__promotion div.promotion__promotion-title")?.TextContent,
                    Url = pap?.GetAttribute("href"),
                    TagePaper = pap?.QuerySelector("a.promotion__promotion div.promotion__promotion-date[data-date]")?.TextContent,
                    PathImage = pap?.QuerySelector("a.promotion__promotion img.promotion__promotion-img[src]")?.GetAttribute("src"),
                    Description = pap?.QuerySelector("a.promotion__promotion div.promotion__promotion-info")?.TextContent
                }
                );
            }

            parses.RemoveAt(parses.Count - 1);
            #endregion

            return parses;
        }
    }
}
