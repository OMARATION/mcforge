
using System;

namespace MCForge
{
    public class CmdServerinfo : Command
    {
        public override string name { get { return "serverinfo"; } }
        public override string shortcut { get { return "si"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            Player.SendMessage(p, "&bThe Server-Name is: &e" + Server.name);
            Player.SendMessage(p, "&bThe Server is running on &eMCForge");
            Command.all.Find("devs").Use(p, "");
            Player.SendMessage(p, "&bThe Server-Money is Named: " + Server.moneys);
        }

        // This one controls what happens when you use /help [commandname].
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/serverinfo - Shows you information about the Servers Name, etc.");
            Player.SendMessage(p, "                Shortcut  =  si  ");
        }
    }
}

