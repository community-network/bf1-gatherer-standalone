﻿using System;
using System.Net;
using System.Web.Script.Serialization;
using gather_standalone.Properties;

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
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string dataString = json_serializer.Serialize(post);
            WebClient webClient = new WebClient();
            string jwtData = Jwt.Create(guid, dataString);
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string postData = json_serializer.Serialize(new {data = jwtData});
            webClient.UploadString(new Uri("https://api.gametools.network/seederplayerlist/bf1"), "POST", postData);

            Console.WriteLine(json_serializer.Serialize(current_server_reader));
        }
    }
}
