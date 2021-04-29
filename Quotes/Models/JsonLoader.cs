using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Quotes.Models
{
    public static class JsonLoader
    {
        public static Daily Download()
        {
            try
            {
                Daily daily;

                var wc = new WebClient();
                using (var stream = new MemoryStream(wc.DownloadData("https://www.cbr-xml-daily.ru/daily_json.js")))
                {
                    daily = JsonSerializer.Deserialize<Daily>(new ReadOnlySpan<byte>(stream.ToArray()));
                }

                daily.InitBindingData();

                return daily;
            }
            catch (Exception)
            {
                return new Daily();
            }
        }
    }
}
