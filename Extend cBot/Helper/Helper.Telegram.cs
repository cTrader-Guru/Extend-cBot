using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace cAlgo.Guru.Helper
{

    struct TelegramResponse
    {
        public bool ok { get; set; }

    }

    public class Telegram
    {

        private readonly string _botToken;
        private readonly List<string> _chatIds;
    
        public Telegram(string botToken, string chatIdsByComma)
        {

            if (string.IsNullOrWhiteSpace(botToken))
                throw new ArgumentException("Telegram Bot Token can't be null");
            if (string.IsNullOrWhiteSpace(chatIdsByComma))
                throw new ArgumentException("Telegram ChatIDs can't be null");

            _botToken = botToken.Trim();
            _chatIds = chatIdsByComma
                .Trim()
                .Split(",")
                .Where(x => x.Trim().Length > 0)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

        }

        public async Task<List<string>> SendMessageAsync(string message)
        {

            List<string> unsended = new List<string>();

            foreach (var chatId in _chatIds) {

                string url = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={message}";
                using HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TelegramResponse apiResponseJson = JsonSerializer.Deserialize<TelegramResponse>(apiResponse);

                    if (!apiResponseJson.ok) unsended.Add(chatId);

                }
                else
                {

                    unsended.Add(chatId);

                }

            }

            return unsended;

        }

        public List<string> SendMessage(string message)
        {

            return SendMessageAsync(message).Result;

        }

    }

}
