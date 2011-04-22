/*
Written by GamezGalaxy (hypereddie10)
*/
using System;

namespace MCForge
{
    public class CmdHigh5 : Command
    {
        public override string name { get { return "high5"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdHigh5() { }
        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Help(p);
            }
            else
            {
                Player player1 = Player.Find(message);
                if (player1 == null || player1.hidden == true)
                {
                    Player.SendMessage(p, "Could not find player specified.");
                }
                else
                {
                    Player.SendMessage(player1, p.name + " just highfived you");
                    Player.GlobalMessage(string.Concat((string[])new string[] { p.color, p.name, Server.DefaultColor, " just highfived " + player1.color + player1.name }));
                }
            }
        }

        // This one controls what happens when you use /help [commandname].
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/high5 - High five someone :D");
        }
    }
}