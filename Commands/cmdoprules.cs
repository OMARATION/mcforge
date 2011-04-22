
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MCForge
{
    class CmdOPRules : Command
    {
        public override string name { get { return "oprules"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdOPRules() { }

        public override void Use(Player p, string message)
        {
            //Do you really need a list for this?
            List<string> oprules = new List<string>();
            if (!File.Exists("text/oprules.txt"))
            {
                File.WriteAllText("text/oprules.txt", "No oprules entered yet!");
            }
            StreamReader r = File.OpenText("text/oprules.txt");
            while (!r.EndOfStream)
                oprules.Add(r.ReadLine());

            r.Close();
            r.Dispose();

            Player who = null;
            if (message != "")
            {
                who = Player.Find(message);
                 if (p.group.Permission < who.group.Permission) { Player.SendMessage(p, "You cant send /oprules to another player!"); return; }
            }
            else
            {
                who = p;
            }

            if (who != null)
            {
                //if (who.level == Server.mainLevel && Server.mainLevel.permissionbuild == LevelPermission.Guest) { who.SendMessage("You are currently on the guest map where anyone can build"); }
                who.SendMessage("Server OPRules:");
                foreach (string s in oprules)
                    who.SendMessage(s);
            }
            else
            {
                Player.SendMessage(p, "There is no player \"" + message + "\"!");
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/oprules [player]- Displays server oprules to a player");
        }
    }
}
