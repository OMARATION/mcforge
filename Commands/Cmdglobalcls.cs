using System;

namespace MCForge
{
    public class Cmdglobalcls : Command
    {
        public override string name { get { return "globalcls"; } }
        public override string shortcut { get { return "gcls"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message)
        {
            int i = 0;
            for (i = 0; i < 20; i++)
            {
                Player.GlobalMessage(".");
            }
            Player.GlobalMessage("%4Global Chat Cleared.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/globalcls - Cleares the chat for all users.");
        }
    }
}