using System;
using System.Collections.Generic;

namespace MCForge
{
    class AntiGrief
    {
        public static Boolean Check(Player p, byte b, int x, int y, int z)
        {
            try
            {
                if (IsBlock(p, x, y, z) == false && IsBreakable(b) == false && p.group.Permission < LevelPermission.Operator)
                    return false;
                else
                    return true;
            }
            catch { if (WorldBlock(p, x, y, z, b)) return false; else return true; }
        }
        public static string GetBlockOwner(Player p, int x, int y, int z) { if (p.level.antigrief[x, y, z] != null) return p.level.antigrief[x, y, z]; else return "World"; }
        public static void Record(Player p, int x, int y, int z) { p.level.antigrief[x, y, z] = GetTeam(p); }
        public static void Dispose(Level l) { l.antigrief = null; }
        public static string GetTeam(Player p)
        {
            string final = "";
            p.antigrief.ForEach(delegate(Player bla)
            {
                if (final == "")
                    final = bla.name;
                else
                    final = final + " " + bla.name;
            });
            return final;
        }
        public static void Create(Level l) { l.antigrief = new string[l.width, l.depth, l.height]; }
        public static void RemovePlayer(Player p, Player p1)
        {
            //Removes p from p1 teams
            try
            {
                if (p1 != null && p1 != p)
                {
                    p1.antigrief.Remove(p);
                    Player.SendMessage(p1, p.color + p.name + " %2left your team!");
                }
                else
                    return;
            }
            catch { }
        }
        public static void ClearPlayer(Player p)
        {
            try
            {
                p.antigrief.ForEach(delegate(Player friend)
                {
                    RemovePlayer(p, friend);
                });
                p.antigrief.Clear();
                p.antigrief.Add(p);
            }
            catch { }
        }
        public static string SendRequest(Player p, Player p1)
        {
            if (p.antigrief.Contains(p1))
                return "Already a team";
            else if (p1.request == true)
                return "Already waiting on another request";
            else
            {
                if (p1 != null)
                {
                    p1.SendMessage(p.name + p.color + " %2wants to team up with you!! Type yes or no to teamup!");
                    p1.request = true;
                    p1.requestname = p.name;
                    return "Request sent";
                }
                else
                    return "Not online";
            }
        }
        public static void Accept(Player p, Player p1)
        {
            p.antigrief.Add(p1);
            p1.antigrief.Add(p);
            p.SendMessage(p1.color + p1.name + " %2has been added to your team!!");
            p1.SendMessage(p.color + p.name + " %2has been added to your team!!");
            p.request = false;
            p1.request = false;
            p.requestname = "";
            p1.requestname = "";
        }
        public static void Reject(Player p, Player p1)
        {
            p.SendMessage(p1.color + p1.name + " %2has REJECTED your request!!");
            p1.SendMessage("%2request rejected...");
            p1.request = false;
            p1.requestname = "";
        }
        public static Boolean IsTeam(Player p, Player sender) { if (p.antigrief.Contains(sender)) return true; else return false; }
        public static Boolean IsBlock(Player p, int x, int y, int z) { if (p.level.antigrief[x, y, z].Contains(p.name)) return true; else return false; }
        public static Boolean IsBreakable(byte b) { if (b != Block.air && b != Block.MsgAir && b != Block.MsgBlack && b != Block.MsgLava && b != Block.MsgWater && b != Block.MsgWhite && b != Block.air_portal && b != Block.blue_portal && b != Block.lava_portal && b != Block.orange_portal && b != Block.water_portal && b != Block.zombiebody && b != Block.creeper && b != Block.door && b != Block.door2 && b != Block.door3 && b != Block.door4 && b != Block.door5 && b != Block.door6 && b != Block.door7 && b != Block.door8 && b != Block.door9 && b != Block.door10 && !Block.NeedRestart(b)) return false; else return true; }
        public static Boolean WorldBlock(Player p, int x, int y, int z, byte b) { try { if (p.level.antigrief[x, y, z] == null && IsBreakable(b) == false && p.group.Permission < LevelPermission.Operator) return true; else return false; } catch { if (IsBreakable(b) && p.group.Permission < LevelPermission.Operator) return true; else return false; } }
    }
}