using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ServerConsole
{
    internal class CatFacts
    {
        public static async Task<string> GetFactAsync()
        {
            var catClient = new HttpClient();
            var catCts = new CancellationTokenSource();
            var catFact = await catClient.GetFromJsonAsync<DTO.CatFactDto>("https://catfact.ninja/fact", catCts.Token);
            return catFact?.Fact ?? "No facts.";
        }
    }
}
