﻿namespace gather_standalone.Structs
{
    internal class PlayerList
    {
        public int index { get; set; }
        public int teamId { get; set; }
        public byte mark { get; set; }

        public int rank { get; set; }
        public string name { get; set; }
        public long player_id { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int score { get; set; }
    }
}
