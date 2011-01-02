using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MCForge.Gaming.CTF
{
    public class CTFGame : Game, ITeamGame
    {
        public List<CTFTeam> Teams = new List<CTFTeam>();

        public Level LevelPlayingOn;

        public int MaxPoints = 3;

        public bool gameOn = false;

        public bool FriendlyFire = false;

        public System.Timers.Timer onTeamCheck = new System.Timers.Timer(500);
        public System.Timers.Timer flagReturn = new System.Timers.Timer(1000);

        public int returnCount = 0;

        public void OnGameStart()
        {
            LevelPlayingOn.ChatLevel("Capture the flag game has started!");
            foreach (CTFTeam team in Teams)
            {
                ReturnFlag(null, team, false);
                foreach (Player p in team.Players)
                {
                    team.SpawnPlayer(p);
                }
            }

            onTeamCheck.Start();
            onTeamCheck.Elapsed += delegate
            {
                foreach (CTFTeam team in Teams)
                {
                    foreach (Player p in team.Players)
                    {
                        if (!p.loggedIn || p.level != LevelPlayingOn)
                        {
                            team.RemoveMember(p);
                        }
                    }
                }
            };

            flagReturn.Start();
            flagReturn.Elapsed += delegate
            {
                foreach (CTFTeam team in Teams)
                {
                    if (!team.flagishome && team.holdingFlag == null)
                    {
                        team.ftcount++;
                        if (team.ftcount > 30)
                        {
                            LevelPlayingOn.ChatLevel("The " + team.Name + " flag has returned to their base.");
                            team.ftcount = 0;
                            ReturnFlag(null, team, false);
                        }
                    }
                }
            };

            Thread flagThread = new Thread(new ThreadStart(delegate
            {
                while (gameOn)
                {
                    foreach (CTFTeam team in Teams)
                    {
                        team.Drawflag();
                    }
                    Thread.Sleep(200);
                }

            })); flagThread.Start();
        }

        public void OnGameEnd()
        {
            CTFTeam winTeam = Teams.OrderByDescending(t => t.Points).First();
            LevelPlayingOn.ChatLevel("The game has ended! " + winTeam.Name + " has won with " + winTeam.Points + " point(s)!");
            foreach (CTFTeam team in Teams)
            {
                ReturnFlag(null, team, false);
                foreach (Player p in team.Players)
                {
                    p.hasflag = null;
                    p.carryingFlag = false;

                }
                team.Points = 0;

            }

            gameOn = false;

        }

        public void GrabFlag(Player p, CTFTeam team)
        {
            if (p.carryingFlag) { return; }
            ushort x = (ushort)(p.pos[0] / 32);
            ushort y = (ushort)((p.pos[1] / 32) + 3);
            ushort z = (ushort)(p.pos[2] / 32);

            team.tempFlagblock.x = x; team.tempFlagblock.y = y; team.tempFlagblock.z = z; team.tempFlagblock.type = LevelPlayingOn.GetTile(x, y, z);

            LevelPlayingOn.Blockchange(x, y, z, CTFTeam.GetColorBlock(team.Color));

            LevelPlayingOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has stolen the " + team.Name + " flag!");
            p.hasflag = team;
            p.carryingFlag = true;
            team.holdingFlag = p;
            team.flagishome = false;

            if (p.aiming)
            {
                p.ClearBlockchange();
                p.aiming = false;
            }
        }

        public void CaptureFlag(Player p, CTFTeam playerTeam, CTFTeam capturedTeam)
        {
            playerTeam.Points++;
            LevelPlayingOn.Blockchange(capturedTeam.tempFlagblock.x, capturedTeam.tempFlagblock.y, capturedTeam.tempFlagblock.z, capturedTeam.tempFlagblock.type);
            LevelPlayingOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has captured the " + capturedTeam.Name + " flag!");

            if (playerTeam.Points >= MaxPoints)
            {
                End();
                return;
            }

            LevelPlayingOn.ChatLevel(playerTeam.Name + " now has " + playerTeam.Name + " point(s).");
            p.hasflag = null;
            p.carryingFlag = false;
            ReturnFlag(null, capturedTeam, false);
        }

        public void DropFlag(Player p, CTFTeam team)
        {
            LevelPlayingOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has dropped the " + team.Name + " flag!");
            ushort x = (ushort)(p.pos[0] / 32);
            ushort y = (ushort)((p.pos[1] / 32) - 1);
            ushort z = (ushort)(p.pos[2] / 32);

            LevelPlayingOn.Blockchange(team.tempFlagblock.x, team.tempFlagblock.y, team.tempFlagblock.z, team.tempFlagblock.type);

            team.flagLocation[0] = x;
            team.flagLocation[1] = y;
            team.flagLocation[2] = z;

            p.hasflag = null;
            p.carryingFlag = false;

            team.holdingFlag = null;
            team.flagishome = false;
        }
        public void ReturnFlag(Player p, CTFTeam team, bool verbose)
        {
            if (p != null && p.spawning) { return; }
            if (verbose)
            {
                if (p != null)
                {
                    LevelPlayingOn.ChatLevel(p.color + p.prefix + p.name + Server.DefaultColor + " has returned the " + team.Name + " flag!");
                }
                else
                {
                    LevelPlayingOn.ChatLevel("The " + team.Name + " flag has been returned.");
                }
            }
            team.holdingFlag = null;
            team.flagLocation[0] = team.flagBase[0];
            team.flagLocation[1] = team.flagBase[1];
            team.flagLocation[2] = team.flagBase[2];
            team.flagishome = true;
        }

        public void AddTeam(string color)
        {
            char teamCol = (char)color[1];

            CTFTeam workteam = new CTFTeam();

            workteam.Color = teamCol;
            workteam.Points = 0;
            workteam.mapOn = LevelPlayingOn;
            char[] temp = c.Name("&" + teamCol).ToCharArray();
            temp[0] = char.ToUpper(temp[0]);
            string tempstring = new string(temp);
            workteam.Name = "&" + teamCol + tempstring + " team" + Server.DefaultColor;

            Teams.Add(workteam);

            LevelPlayingOn.ChatLevel(workteam.Name + " has been initialized!");
        }

        public void RemoveTeam(string color)
        {
            char teamCol = (char)color[1];

            CTFTeam workteam = Teams.Find(team => team.Color == teamCol);
            List<Player> storedP = new List<Player>();

            for (int i = 0; i < workteam.Players.Count; i++)
            {
                storedP.Add(workteam.Players[i]);
            }
            foreach (Player p in storedP)
            {
                workteam.RemoveMember(p);
            }


        }
        public void CreateTeam(string teamname, string color)
        {
            throw new NotImplementedException();
        }

        public bool Join(string playername, string teamname)
        {
            throw new NotImplementedException();
        }
    }
}
