using System;
using System.Collections.Generic;


namespace MCForge.Gaming
{
    public class Team
    {
        public char Color;
        public int Points = 0;
        
        public List<Spawn> SpawnLocations = new List<Spawn>();
        public List<Player> Players = new List<Player>();
        
        public string Name = "";

        public void AddMember(Player p)
        {
            if (p.team != this)
            {
                if (p.carryingFlag) { p.spawning = true; mapOn.ctfgame.DropFlag(p, p.hasflag); p.spawning = false; }
                if (p.team != null) { p.team.RemoveMember(p); }
                p.team = this;
                Player.GlobalDie(p, false);
                p.CTFtempcolor = p.color;
                p.CTFtempprefix = p.prefix;
                p.color = "&" + Color;
                p.carryingFlag = false;
                p.hasflag = null;
                p.prefix = p.color + "[" + c.Name("&" + Color).ToUpper() + "] ";
                Players.Add(p);
                mapOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has joined the " + Name + ".");
                Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                if (mapOn.ctfgame.gameOn)
                {
                    SpawnPlayer(p);
                }
            }
        }

        public void RemoveMember(Player p)
        {
            if (p.team == this)
            {
                if (p.carryingFlag)
                {
                    mapOn.ctfgame.DropFlag(p, p.hasflag);
                }
                p.team = null;
                Player.GlobalDie(p, false);
                p.color = p.CTFtempcolor;
                p.prefix = p.CTFtempprefix;
                p.carryingFlag = false;
                p.hasflag = null;
                Players.Remove(p);
                mapOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has left the " + Name + ".");
                Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
            }
        }

        public void SpawnPlayer(Player p)
        {
            p.spawning = true;
            if (SpawnLocations.Count != 0)
            {
                Random random = new Random();
                int rnd = random.Next(0, SpawnLocations.Count);
                ushort x, y, z, rotx;

                x = SpawnLocations[rnd].x;
                y = SpawnLocations[rnd].y;
                z = SpawnLocations[rnd].z;

                ushort x1 = (ushort)((0.5 + x) * 32);
                ushort y1 = (ushort)((1 + y) * 32);
                ushort z1 = (ushort)((0.5 + z) * 32);
                rotx = SpawnLocations[rnd].rotx;
                unchecked
                {
                    p.SendSpawn((byte)-1, p.name, x1, y1, z1, (byte)rotx, 0);
                }
                p.health = 100;
            }
            else
            {
                ushort x = (ushort)((0.5 + mapOn.spawnx) * 32);
                ushort y = (ushort)((1 + mapOn.spawny) * 32);
                ushort z = (ushort)((0.5 + mapOn.spawnz) * 32);
                ushort rotx = mapOn.rotx;
                ushort roty = mapOn.roty;

                unchecked
                {
                    p.SendSpawn((byte)-1, p.name, x, y, z, (byte)rotx, (byte)roty);
                }
            }
            p.spawning = false;
        }

        public void AddSpawn(ushort x, ushort y, ushort z, ushort rotx, ushort roty)
        {
            Spawn workSpawn = new Spawn();
            workSpawn.x = x;
            workSpawn.y = y;
            workSpawn.z = z;
            workSpawn.rotx = rotx;
            workSpawn.roty = roty;

            SpawnLocations.Add(workSpawn);
        }

        

        public struct CatchPos { public ushort x, y, z; public byte type; }
        public struct Spawn { public ushort x, y, z, rotx, roty; }
    }
}