using System;

namespace MCForge
{
    public class CmdFakepay : Command
    {
        public override string name { get { return "fakepay"; } }
        public override string shortcut { get { return "fpay"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/fakerank <name> <amount> - Sends a fake give change message.");
        }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player who = Player.Find(message.Split(' ')[0]);
            int amount = int.Parse(message.Split(' ')[1]);
            if (who == null)
            {
                Player.SendMessage(p, Server.DefaultColor + "Player not found!");
                return;
            }
            if (amount < 0)
            {
                Player.SendMessage(p, Server.DefaultColor + "You can't fakegive someone negative.");
                return;
            }
            if (amount >= 16777215)
            {
                Player.SendMessage(p, "You can't fakegive someone over 16777215.");
                return;
            }
            else
            {
                Player.GlobalMessage(who.color + who.prefix + who.name + Server.DefaultColor + " was given " + amount + " " + Server.moneys);
                return;
            }
        }
    }
}

