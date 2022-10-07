using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using gather_standalone.Properties;

namespace gather_standalone
{
    internal class Api
    {
        public static void PostPlayerlist(GameReader.CurrentServerReader current_server_reader, Guid guid)
        {
            var payload = new
            {
                guid = guid.ToString(),
                serverinfo = new
                {
                    name = current_server_reader.ServerName,
                    gameId = current_server_reader.GameId
                },
                teams = new
                {
                    team1 = current_server_reader.PlayerLists_Team1,
                    team2 = current_server_reader.PlayerLists_Team2,
                    scoreteam1 = current_server_reader.ServerScoreTeam1,
                    scoreteam2 = current_server_reader.ServerScoreTeam2,
                    scoreteam1FromKills = current_server_reader.Team1ScoreFromKill,
                    scoreteam2FromKills = current_server_reader.Team2ScoreFromKill,
                    scoreteam1FromFlags = current_server_reader.Team1ScoreFromFlags,
                    scoreteam2FromFlags = current_server_reader.Team2ScoreFromFlags,
                }
            };
            string dataString = JsonConvert.SerializeObject(payload);
            string jwtData = Jwt.Create(guid, dataString);
            string stringPayload = JsonConvert.SerializeObject(new { data = jwtData });
            StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = new HttpClient().PostAsync("https://api.gametools.network/seederplayerlist/bf1", httpContent).Result;
            _ = httpResponse.Content.ReadAsStringAsync().Result;
        }
    }
}
