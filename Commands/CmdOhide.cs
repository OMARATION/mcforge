// /ohide - Written by Valek

using System;

namespace MCForge
{
    public class CmdOHide : Command
    {
        public override string name { get { return "ohide"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            message = message.Split(' ')[0];
            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player.");
                return;
            }
            if (who == p)
            {
                Player.SendMessage(p, "On yourself?  Really?  Just use /hide.");
                return;
            }
            if (who.group.Permission >= p.group.Permission)
            {
                Player.SendMessage(p, "Cannot use this on someone of equal or greater rank.");
                return;
            }
            Command.all.Find("hide").Use(who, "");
            Player.SendMessage(p, "Used /hide on " + who.color + who.name + Server.DefaultColor + ".");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ohide <player> - Hides/unhides the player specified.");
            Player.SendMessage(p, "Only works on players of lower rank.");
        }
    }
}
