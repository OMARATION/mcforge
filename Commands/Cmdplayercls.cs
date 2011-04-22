using System;

namespace MCForge
{
    public class CmdPlayercls : Command
    {
        public override string name { get { return "playercls"; } }
        public override string shortcut { get { return "pcls"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }

        public override void Use(Player p, string message)
        {
           int i = 0;
           for (i = 0; i < 20; i++)
           {
               Player.SendMessage(p, ".");
           }
           Player.SendMessage(p, "%4Player Chat cleared.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/playercls - Clears Chat for the user.");
        }
    }
}