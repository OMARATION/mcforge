/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl) Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/
using System;
using System.IO;

namespace MCForge
{
    public class CmdSummon : Command
    {
        public override string name { get { return "summon"; } }
        public override string shortcut { get { return "s"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdSummon() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.ToLower() == "all")
            {
                try
                {
                    foreach (Player pl in Player.players)
                    {
                        if (pl.level == p.level && pl != p && p.group.Permission > pl.group.Permission)
                        {
                            unchecked { pl.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
                            pl.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
                        }
                    }
                }
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                }
                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " summoned everyone!");
                return;


            }

            Player who = Player.Find(message);
            if (who == null || who.hidden) { Player.SendMessage(p, "There is no player \"" + message + "\"!"); return; }
            if (p.level != who.level)
            {
                Command.all.Find("fetch").Use(p, who.name);
                return;
            }
            unchecked { who.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
            who.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/summon <player> - Summons a player to your position.");
            Player.SendMessage(p, "/summon all - Summons all players in the map");
        }
    }
}