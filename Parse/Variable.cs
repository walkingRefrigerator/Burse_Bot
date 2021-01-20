namespace Burse_Bot
{
    public class Variable
    {
        public class ParseInfo
        {
            private string url;
            private string pathimage;

            public string TagePaper { get; set; }
            public string Description { get; set; }
            public string TitlePaper { get; set; }
            public string PathImage
            {
                get
                {
                    return pathimage;
                }
                set
                {
                    pathimage = "https://static.eldorado.ru" + value;
                }
            }
            public string Url
            {
                get
                {
                    return url;
                }
                set
                {
                    url = "https://www.eldorado.ru" + value;
                }
            }


        }
    }
}
