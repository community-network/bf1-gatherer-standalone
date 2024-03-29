﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gather_standalone.GameReader
{
    internal class CurrentServerReader
    {
        public List<Structs.PlayerList> PlayerLists_All { get; private set; }
        public List<Structs.PlayerList> PlayerLists_Team1 { get; private set; }
        public List<Structs.PlayerList> PlayerLists_Team2 { get; private set; }
        public ObservableCollection<Structs.PlayerList> ListBox_PlayerList_Team1 { get; private set; }
        public ObservableCollection<Structs.PlayerList> ListBox_PlayerList_Team2 { get; private set; }

        public long GameId { get; private set; }
        public string ServerName { get; private set; }
        public float ServerTime { get; private set; }
        public string ServerDate { get; private set; }
        public DateTime RefreshTime { get; private set; }
        public bool hasResults { get; private set; }

        public int ServerScoreTeam1 { get; private set; }
        public int Team1ScoreFromKill { get; private set; }
        public int Team1ScoreFromFlags { get; private set; }

        public int ServerScoreTeam2 { get; private set; }
        public int Team2ScoreFromKill { get; private set; }
        public int Team2ScoreFromFlags { get; private set; }

        public string player_vehicle { get; private set; }

        public long PSender { get; private set; }
        public long PContent { get; private set; }
        public string ChatSender { get; private set; }
        public string ChatContent { get; private set; }


        public CurrentServerReader()
        {
            PlayerLists_All = new List<Structs.PlayerList>();
            PlayerLists_Team1 = new List<Structs.PlayerList>();
            PlayerLists_Team2 = new List<Structs.PlayerList>();

            ListBox_PlayerList_Team1 = new ObservableCollection<Structs.PlayerList>();
            ListBox_PlayerList_Team2 = new ObservableCollection<Structs.PlayerList>();

            hasResults = false;
            Refresh();
        }

        public void Refresh()
        {
            if (Memory.Initialize())
            {
                var serverInfoAddr = Memory.Read<long>(Memory.GetBaseAddress() + Offsets.ServerScore_Offset, Offsets.ServerScoreTeam);
                // Team 1
                ServerScoreTeam1 = Memory.Read<int>(serverInfoAddr + 0x2B0);
                Team1ScoreFromKill = Memory.Read<int>(serverInfoAddr + 0x2B0 + 0x60);
                Team1ScoreFromFlags = Memory.Read<int>(serverInfoAddr + 0x2B0 + 0x100);
                // Team 2
                ServerScoreTeam2 = Memory.Read<int>(serverInfoAddr + 0x2B0 + 0x08);
                Team2ScoreFromKill = Memory.Read<int>(serverInfoAddr + 0x2B0 + 0x68);
                Team2ScoreFromFlags = Memory.Read<int>(serverInfoAddr + 0x2B0 + 0x108);


                GameId = Memory.Read<long>(Memory.GetBaseAddress() + Offsets.ServerID_Offset, Offsets.ServerID);
                ServerTime = Memory.Read<float>(Memory.GetBaseAddress() + Offsets.ServerTime_Offset, Offsets.ServerTime);
                ServerName = Memory.ReadString(Memory.GetBaseAddress() + Offsets.ServerName_Offset, Offsets.ServerName, 64);
                ServerName = string.IsNullOrEmpty(ServerName) ? "" : ServerName;
                ServerDate = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
                // player data

                for (int i = 0; i < 74; i++)
                {
                    List<Dictionary<string, string>> WeaponSlot = new List<Dictionary<string, string>>();
                    var pClientPlayerBA = Player.GetPlayerById(i);
                    if (!Memory.IsValid(pClientPlayerBA))
                        continue;

                    var platoonTag = Memory.ReadString(pClientPlayerBA + 0x2151, 16); // Platoon Tag
                    var fullName = Memory.ReadString(pClientPlayerBA + 0x2156, 64); // Name including platoon tag.
                    var platoonUrl = Memory.ReadString(pClientPlayerBA + 0x21DE, 256); // Platoon URL
                    var platoonName = Memory.ReadString(pClientPlayerBA + 0x2270, 64); // Platoon Name
                    var playerName = Memory.ReadString(pClientPlayerBA + 0x40, 64); // Name
                    var m_teamId = Memory.Read<int>(pClientPlayerBA + 0x1C34); // Player currentt team
                    var m_playerIndex = Memory.Read<byte>(pClientPlayerBA + 0x1D7C); // Player server index
                    var spectator = Memory.Read<byte>(pClientPlayerBA + 0x1C31);
                    var personaId = Memory.Read<long>(pClientPlayerBA + 0x38); // PersonaId
                    var squadID = Memory.Read<int>(pClientPlayerBA + 0x1E50); // Unknown

                    // Class info
                    var classInfoAddr = Memory.Read<long>(Memory.Read<long>(pClientPlayerBA + 0x11A8) + 0x28);
                    var classId = Memory.ReadString(classInfoAddr, 64);
                    var length = classId.Length;
                    var className = Memory.ReadString(classInfoAddr + (length * 0x1) + 0x1, 64);
                    length = length + className.Length;
                    var classKit = Memory.ReadString(classInfoAddr + (length * 0x1) + 0x2, 64);
                    length = length + classKit.Length;
                    var classInfo1 = Memory.ReadString(classInfoAddr + (length * 0x1) + 0x3, 64);
                    length = length + classInfo1.Length;
                    var classInfo2 = Memory.ReadString(classInfoAddr + (length * 0x1) + 0x4, 64);

                    var playerClassIcons = Statics.getPlayerClass(classId);

                    var pClientVehicleEntity = Memory.Read<long>(pClientPlayerBA + 0x1D38);
                    if (Memory.IsValid(pClientVehicleEntity))
                    {
                        var pVehicleEntityData = Memory.Read<long>(pClientVehicleEntity + 0x30);
                        player_vehicle = Memory.ReadString(Memory.Read<long>(pVehicleEntityData + 0x2F8), 64);
                    }
                    else
                    {

                        player_vehicle = null;

                        var pClientSoldierEntity = Memory.Read<long>(pClientPlayerBA + 0x1D48);
                        var pClientSoldierWeaponComponent = Memory.Read<long>(pClientSoldierEntity + 0x698);
                        var m_handler = Memory.Read<long>(pClientSoldierWeaponComponent + 0x8A8);

                        for (int j = 0; j < 8; j++)
                        {
                            var offset0 = Memory.Read<long>(m_handler + j * 0x8);

                            offset0 = Memory.Read<long>(offset0 + 0x4A30);
                            offset0 = Memory.Read<long>(offset0 + 0x20);
                            offset0 = Memory.Read<long>(offset0 + 0x38);
                            offset0 = Memory.Read<long>(offset0 + 0x20);

                            var weapon_id = Memory.ReadString(offset0, 64);
                            if (weapon_id != "")
                            {
                                WeaponSlot.Add(Statics.getItem(weapon_id));
                            }
                        }
                    }


                    PlayerLists_All.Add(new Structs.PlayerList()
                    {
                        teamId = m_teamId,
                        mark = m_playerIndex,
                        platoon = new Structs.Platoon()
                        { 
                            icon = platoonUrl,
                            name = platoonName,
                            tag = platoonTag
                        },
                        player_class = new Structs.PlayerClass()
                        {
                            class_id = classId,
                            class_name = className,
                            class_kit = classKit,
                            class_info1 = classInfo1,
                            class_info2 = classInfo2,
                            class_icons = playerClassIcons,
                        },
                        Spectator = spectator,
                        squad_id = squadID,
                        squad_name = Statics.getSquadName(squadID),
                        rank = 0,
                        name = playerName,
                        player_id = personaId,
                        kills = 0,
                        deaths = 0,
                        score = 0,
                        vehicle = Statics.getItem(player_vehicle),
                        weapons = WeaponSlot
                    });
                }

                // scoreboard data

                var pClientScoreBA = Memory.Read<long>(Memory.GetBaseAddress() + 0x39EB8D8);
                pClientScoreBA = Memory.Read<long>(pClientScoreBA + 0x68);

                for (int i = 0; i < 74; i++)
                {
                    pClientScoreBA = Memory.Read<long>(pClientScoreBA);
                    var pClientScoreOffset = Memory.Read<long>(pClientScoreBA + 0x10);
                    if (!Memory.IsValid(pClientScoreBA))
                        continue;

                    var Mark = Memory.Read<byte>(pClientScoreOffset + 0x300);
                    var Rank = Memory.Read<int>(pClientScoreOffset + 0x304);
                    var Kill = Memory.Read<int>(pClientScoreOffset + 0x308);
                    var Dead = Memory.Read<int>(pClientScoreOffset + 0x30C);
                    var Score = Memory.Read<int>(pClientScoreOffset + 0x314);

                    int index = PlayerLists_All.FindIndex(val => val.mark == Mark);
                    if (index != -1)
                    {
                        PlayerLists_All[index].rank = Rank;
                        PlayerLists_All[index].kills = Kill;
                        PlayerLists_All[index].deaths = Dead;
                        PlayerLists_All[index].score = Score;
                    }
                }

                // Team data collation

                foreach (var item in PlayerLists_All)
                {
                    if (item.teamId == 1)
                    {
                        PlayerLists_Team1.Add(item);
                    }
                    else if (item.teamId == 2)
                    {
                        PlayerLists_Team2.Add(item);
                    }
                }

                //PlayerLists_Team1.Sort((a, b) => b.Score.CompareTo(a.Score));
                //PlayerLists_Team2.Sort((a, b) => b.Score.CompareTo(a.Score));

                PlayerLists_Team1 = PlayerLists_Team1.OrderByDescending(o => o.score).ToList();
                PlayerLists_Team2 = PlayerLists_Team2.OrderByDescending(o => o.score).ToList();

                int count = 1;
                for (int i = 0; i < PlayerLists_Team1.Count; i++)
                {
                    PlayerLists_Team1[i].index = count++;
                    ListBox_PlayerList_Team1.Add(PlayerLists_Team1[i]);
                }

                count = 1;
                for (int i = 0; i < PlayerLists_Team2.Count; i++)
                {
                    PlayerLists_Team2[i].index = count++;
                    ListBox_PlayerList_Team2.Add(PlayerLists_Team2[i]);
                }

                RefreshTime = DateTime.Now;
                hasResults = true;

                string sender = GameReader.Chat.GetLastChatSender(out long pSender);
                string content = GameReader.Chat.GetLastChatContent(out long pContent);

                PSender = pSender;
                PContent = pContent;

                ChatSender = sender;
                ChatContent = content;

                Memory.CloseHandle();
            }
            else
            {
                hasResults = false;
            }
        }
    }
}
