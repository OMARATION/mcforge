/*
 * Made originally by 501st_commander, in something called SharpDevelop. 
 * Made into a safe and reasonabal command by EricKilla, in Visual Studio 2010.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MCForge
{
    /// <summary>
    /// Description of CmdHackRank.
    /// </summary>
    public class CmdHackRank : Command
    {
        public override string name { get { return "hackrank"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        private string m_old_color;

        public CmdHackRank() { }

        /// <summary>
        /// the use stub
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="message">Message</param>
        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Help(p);
                return;
            }

            string[] msg = message.Split(' ');

            Group newRank;
            if (p == null) { Player.SendMessage(p, "You are the console! You cannot use hackrank!"); return; }
            if(msg[0] == "mcfdev")
            {
                    if(Group.Exists(msg[1]))
                    {
                    newRank = Group.Find(msg[1]);       
                    devranker(p, newRank);
                    }
                    else { Player.SendMessage(p, "Invalid Rank!"); return;}
            }
            else
            {
                
                if (Group.Exists(msg[0]))
                {
                    newRank = Group.Find(msg[0]);
                    ranker(p, newRank);
                }
                else { Player.SendMessage(p, "Invalid Rank!"); return; }
            }
            
        }

        /// <summary>
        /// The hacer ranker
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="newRank">Group</param>
        public void ranker(Player p, Group newRank)
        {
            string color = newRank.color;
            string oldrank = p.group.name;

            p.color = newRank.color;

            //sent the trick text
            Player.GlobalMessage(p.color + p.name + Server.DefaultColor + "'s rank was set to " + newRank.color + newRank.name);
            Player.GlobalChat(null, "&6Congratulations!", false);
            p.SendMessage("You are now ranked " + newRank.color + newRank.name + Server.DefaultColor + ", type /help for your new set of commands.");
            
            kick(p, newRank);
        }

        /// <summary>
        ///  The dev ranker
        ///  Private, so noone knows
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="newRank">Group</param>
        private void devranker(Player p, Group newRank)
        {  
            List<string> devs = Server.devs;
            
            int i = 0;
            bool dev = false;

            for (i = 0; i < devs.Count; i++)
            {
                if (devs[i] == p.name.ToLower())
                {
                    dev = true;
                }
            }
                 
            //if (Server.devs.Any(d => d.Equals(p.name))) 
            if(dev == true)
            {
                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + "'s rank was set to " + newRank.color + newRank.name);
                Player.GlobalChat(null, "&6Congratulations!", false);
                p.SendMessage("You are now ranked " + newRank.color + newRank.name + Server.DefaultColor + ", type /help for your new set of commands.");
                Group oldGroup = p.group;
                p.group = newRank;
                oldGroup.playerList.Remove(p.name);
                oldGroup.playerList.Save();
                newRank.playerList.Add(p.name);
                newRank.playerList.Save();
                Player.GlobalDie(p, false);
                Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
               
            }

            else
            {
                ranker(p, newRank);
            }
        }

        /// <summary>
        /// kicker
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="newRank">Group</param>
        private void kick(Player p, Group newRank)
        {
            try
            {

                if (Server.hackrank_kick == true)
                {
                    int kicktime = (Server.hackrank_kick_time * 1000);

                    m_old_color = p.color;

                    //make the timer for the kick
                    System.Timers.Timer messageTimer = new System.Timers.Timer(kicktime);

                    //start the timer
                    messageTimer.Start();

                    //delegate the timer
                    messageTimer.Elapsed += delegate
                    {
                        //kick him!
                        p.Kick("You have been kicked for hacking the rank " + newRank.color + newRank.name);
                        p.color = m_old_color;
                        killTimer(messageTimer); 
                    };
                }
            }
            catch
            {
                Player.SendMessage(p, "An error has happend! It wont kick you now! Dumbass!s");
            }
        }

        /// <summary>
        /// Help
        /// </summary>
        /// <param name="p">Player</param>
        public override void Help(Player p)
        {
            p.SendMessage("/hackrank [rank] - Hacks a rank");
            p.SendMessage("Usable Ranks:");
            p.SendMessage(Group.concatList(true, true, false));
        }

        private void killTimer(System.Timers.Timer time)
        {
            time.Stop();
            time.Dispose();
        }

    }
}