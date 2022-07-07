﻿using System;
using System.Net;
using System.Web.Script.Serialization;
using gather_standalone.Properties;

namespace gather_standalone
{
    internal class Api
    {
        public static string GetGameId(GameReader.CurrentServerReader current_server_reader)
        {
            var post = new
            {
                name = current_server_reader.ServerName
            };
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string dataString = json_serializer.Serialize(post);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            return webClient.UploadString(new Uri("https://api.gametools.network/seedergameid/bf1"), "POST", dataString).Replace("\"", "");
        }

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
                    team2 = current_server_reader.PlayerLists_Team2,
                    scoreteam1 = current_server_reader.ServerScoreTeam1,
                    scoreteam2 = current_server_reader.ServerScoreTeam2
                }
            };
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string dataString = json_serializer.Serialize(post);
            WebClient webClient = new WebClient();
            string jwtData = Jwt.Create(guid, dataString);
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string postData = json_serializer.Serialize(new {data = jwtData});
            webClient.UploadString(new Uri("https://api.gametools.network/seederplayerlist/bf1"), "POST", postData);
        }
    }
}
