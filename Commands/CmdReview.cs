

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class CmdReview : Command
    {
        public override string name { get { return "review"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdReview() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            Player.SendMessage(p, "You requested that operators see your building. They should be coming soon.");
            Player.GlobalMessageOps("To Ops -" + p.color + p.name + "-" + Server.DefaultColor + " needs an op to look at their building!");

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/review - Sends a message to admins telling them you need your building looked at.");
        }
    }

}
