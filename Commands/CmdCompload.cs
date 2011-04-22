﻿using System;

namespace MCForge
{
    using System;
    public class CmdCompLoad : Command
    {
        public override string name { get { return "compload"; } }
        public override string shortcut { get { return "cml"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p) { Player.SendMessage(p, "/compload <command> - Compiles AND loads <command> for use (shortcut = /cml)"); }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            else
            {
                Command.all.Find("compile").Use(p, message);
                Command.all.Find("cmdload").Use(p, message);
                Command.all.Find("help").Use(p, message);
            }
        }
    }
}