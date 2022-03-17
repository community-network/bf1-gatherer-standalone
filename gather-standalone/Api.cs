using System;
using System.Net;
using System.Web.Script.Serialization;

namespace gather_standalone
{
    internal class Api
    {
        public static void PostPlayerlist(GameReader.CurrentServerReader current_server_reader, Guid guid)
        {
            var post = new
            {
                guid = guid.ToString(),
                serverinfo = new
                {
                    name = current_server_reader.ServerName
                },
                teams = new
                {
                    team1 = current_server_reader.PlayerLists_Team1,
                    team2 = current_server_reader.PlayerLists_Team2
                }
            };
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            var dataString = json_serializer.Serialize(post);
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            Int64 unixTimestamp = (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            webClient.Headers.Add("authentication", (unixTimestamp / 60 * 5963827110).ToString());
            webClient.UploadString(new Uri("https://api.gametools.network/seederplayerlist/bf1"), "POST", dataString);
        }
    }
}
