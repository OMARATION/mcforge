/*
 * /xmute - Made by GamezGalaxy (hypereddie10)
*/

using System;
using System.Threading;

namespace MCForge
{
        public class Cmdxmute : Command
        {
                public override string name { get { return "xmute"; } }
                public override string shortcut { get { return ""; } }
                public override string type { get { return "mod"; } }
                public override bool museumUsable { get { return false; } }
                public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
                public override void Use(Player p, string message)
                {
            Player muter = Player.Find(message.Split(' ')[0]);
            if (muter == null)
            {
                Player.SendMessage(p, "Couldn't find them!");
            }
            else
            {
                if (muter.group.Permission > p.group.Permission)
                {
                    Player.SendMessage(p, "You cannot xmute someone ranked higher than you!");
                }
                else
                {
                    Command.all.Find("mute").Use(p, muter.name);
                    int time = Convert.ToInt32(message.Split(' ')[1]);
                    Player.GlobalMessage(muter.color + muter.name + " has been muted for " + time + " seconds");
                    Player.SendMessage(muter, "You have been muted for " + time + " seconds");
                    Thread.Sleep(time * 1000);
                    Command.all.Find("mute").Use(p, muter.name);
                }
            }
                }

                // This one controls what happens when you use /help [commandname].
                public override void Help(Player p)
                {
                        Player.SendMessage(p, "/xmute <player> <seconds> - Mutes <player> for <seconds> seconds");
                }
        }
}


