/* Coded by GamezGalaxy (hypereddie10) */
using System;
using System.Threading;
using System.Timers;
using System.Collections.Generic;

namespace MCForge
{
    class CmdInfection : Command
    {
        #region Declaring
        //Command stuff...
        public override string name { get { return "infection"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdInfection() { }
        //CAN I GO BOOM!?!?!?!
        public static Boolean Theybecreepen;
        //Infection Core timer
        public static System.Timers.Timer Infect = new System.Timers.Timer(500);
        //Timer timer
        public static System.Timers.Timer timer = new System.Timers.Timer(1000);
        //Some random stuff...
        Random random = new Random();
        //WHOS INFECTED :D
        public static List<Player> infect = new List<Player>();
        //Game name??
        public string gamename = "";
        //WHATS THE GAME PLAYING ON ?!?!?!
        public static Level INFECTEDLEVEL;
        //It wont let me use time D:
        public static int minute;
        public static int seconds;
        public static string time1;
        #endregion
        #region command
        public override void Use(Player p, string message)
        {
			if (message.ToLower() == "STOP") { Player.SendMessage(p, "Stopping infection..."); Infect.Enabled = false; timer.Enabled = false; END(); }
            Player.SendMessage(p, "Starting Infection...");
            //Picks a random game
            if (random.Next(5) == 3)
            {
                gamename = "CREEPER";
                Theybecreepen = true;
            }
            else
            {
                gamename = "ZOMBIE";
                Theybecreepen = false;
            }
            minute = random.Next(5, 11);
            seconds = 60;
            timer.Elapsed += new ElapsedEventHandler(TIMERCORE);
            timer.Enabled = true;
            INFECTEDLEVEL = p.level;
            Player.GlobalMessage(gamename + " INFECTION HAS STARTED ON MAP " + p.level.name);
            //Gets player in the map
            Player.players.ForEach(delegate(Player player)
            {
                if (player.level == p.level)
                {
                    Server.People.Add(player);
                    player.oldtitle = player.title;
                    player.title = "Playing";
                    //player.SendPos(0xff, 3682, 4179, 3303, player.rot[0], 0);
                }
            });
            //wait, theres less than 3 players?
            if (Server.People.Count <= 3)
            {
                Player.SendMessage(p, "You can't play with only " + Server.People.Count);
                Player.SendMessage(p, "Ending the game...");
                END();
                return;
            }
            //Count down...
            Player.GlobalMessageLevel(p.level, "%4Choosing in...");
            int time = 10;
            while (time > 0)
            {
                Player.GlobalMessageLevel(p.level, "%4" + time);
                time--;
                Thread.Sleep(1000);
            }
            //Enable Defense TNT
            Server.AllowTNT = true;
            //declaring stuff...
            int firstinfect = random.Next(Server.People.Count);
            //pick someone
            Player.GlobalMessage("%4" + Server.People[firstinfect].name + " is infected RUN AWAY!!!");
            Server.People[firstinfect].oldname = Server.People[firstinfect].name;
            Player.GlobalDie(Server.People[firstinfect], true);
            Server.People[firstinfect].name = "zombie";
            Thread.Sleep(5);
            Player.GlobalSpawn(Server.People[firstinfect], Server.People[firstinfect].pos[0], Server.People[firstinfect].pos[1], Server.People[firstinfect].pos[2], Server.People[firstinfect].rot[0], Server.People[firstinfect].rot[1], false);
            infect.Add(Server.People[firstinfect]);
            Server.People.Remove(Server.People[firstinfect]);
            Thread.Sleep(500);
            //Start the infection timer
            Infect.Elapsed += new ElapsedEventHandler(INFECTCORE);
            Infect.Enabled = true;
        }
        #endregion
        #region TIMECORE
        public static void TIMERCORE(object sender, ElapsedEventArgs e)
        {
            seconds--;
            if (minute <= 0 && seconds <= 0) { Infect.Enabled = false; END(); return; }
            else if (seconds <= 0) { seconds = 60; minute--; Player.GlobalMessage("There is " + minute + ":" + seconds + " left in the infection game"); }
            time1 = "" + minute + ":" + seconds;
        }
        public static void SaveInfo()
        {
            Server.Time = "%2There is " + time1 + " minutes remaining in this round";
        }
        #endregion
        #region INFECTCORE
        public static void INFECTCORE(object sender, ElapsedEventArgs e)
        {
            //Whats the infected name?
            string name;
            if (Theybecreepen == true)
            {
                name = "creeper";
            }
            else
            {
                name = "zombie";
            }
            //Everyone whos infected
            infect.ForEach(delegate(Player player1)
            {
                //Everyone on the server
                Player.players.ForEach(delegate(Player player2)
                {
                    //if the player is the same as the infected player and the player's name doesnt = an infected name, and the player is playing and the player is touching an ifected player
                    if (player2.level == player1.level && player2.name != name && player2.title == "Playing" && Math.Abs((player2.pos[0] / 32) - (player1.pos[0] / 32)) <= 1 && Math.Abs((player2.pos[1] / 32) - (player1.pos[1] / 32)) <= 1 && Math.Abs((player2.pos[2] / 32) - (player1.pos[2] / 32)) <= 1)
                    {
                        //if its creeper infetion
                        if (Theybecreepen == true)
                        {
                            //KABOOM
                            player1.level.MakeExplosion((ushort)(player1.pos[0] / 32), (ushort)(player1.pos[1] / 32), (ushort)(player1.pos[2] / 32), 1);
                        }
                        //Send message
                        Player.GlobalMessage(player2.color + player2.name + " %2WAS EATED BY " + player1.color + player1.oldname);
                        //Give some advice
                        Player.SendMessage(player2, "&fYou can kill human " + name + " by placing tnt blocks.");
                        //Take away his lives
                        player2.lives--;
                        //Spawn
                        Command.all.Find("spawn").Use(player2, "");
                        //Alert of him of how many lives he has left
                        Player.SendMessage(player2, "%2You have " + player2.lives + " live(s) left!");
                        //O NOES HES DEAD
                        if (player2.lives == 0)
                        {
                            //Tell the public
                            Player.GlobalMessage("%4" + player2.name + " is infected!!");
                            //Save his name
                            player2.oldname = player2.name;
                            //KILL HIM
                            Player.GlobalDie(player2, true);
                            //Give him a new name
                            player2.name = name;
                            //Pause...(Just cause)
                            Thread.Sleep(5);
                            //Bring back to life
                            Player.GlobalSpawn(player2, player2.pos[0], player2.pos[1], player2.pos[2], player2.rot[0], player2.rot[1], false);
                            //O NOES HES INFECTED!!!
                            infect.Add(player2);
                            //Ok lets make him invincible
                            player2.invincible = true;
                            //And let the server know hes infected (Just because it says creeper doesnt mean it means he's a creeper >.>)
                            player2.Creeper = true;
                        }
                    }
                    //IS THIS PERSON ON NO TEAM!?!?!?!
                    else if (player2.title != "Playing" && player2.name != name && player2.level == INFECTEDLEVEL)
                    {
                        //Lets put him on the good team
                        Server.People.Add(player2);
                        player2.oldtitle = player2.title;
                        player2.title = "Playing";
                    }
                });
            });
            //Lets see if someone got pass my dead code
            //**Might not be needed**
            Player.players.ForEach(delegate(Player tester)
            {
                if (tester.lives <= 0 && tester.name != name)
                {
                    Server.People.Remove(tester);
                    Player.GlobalMessage("%4" + tester.name + " is infected!!");
                    tester.oldname = tester.name;
                    Player.GlobalDie(tester, true);
                    tester.name = name;
                    Thread.Sleep(5);
                    Player.GlobalSpawn(tester, tester.pos[0], tester.pos[1], tester.pos[2], tester.rot[0], tester.rot[1], false);
                    infect.Add(tester);
                    tester.invincible = true;
                    Thread.Sleep(500);
                    tester.Creeper = true;
                }
            });
            //IS EVERYONE DEAD?!?!
            if (Server.People.Count == 1)
            {
                Infect.Enabled = false;
                END();
            }
        }
        #endregion
        #region END
        public static void END() {
            //yay the game is over
            Player.GlobalMessage("The infection game ended!");
            //Who won??
            Player.GlobalMessage("The winners are");
            Player.players.ForEach(delegate(Player p)
            {
                if (p.title == "Playing")
                    Player.GlobalMessage(p.color + p.name);
                Thread.Sleep(500);
            });
            //ok lets reset everything...
            minute = 0;
            seconds = 0;
            timer.Enabled = false;
            Infect.Enabled = false;
            Theybecreepen = false;
        }
        #endregion
        #region TNT EXPLODE
        public static void Death(Player killer, Player zombie, ushort x, ushort y, ushort z)
        {
            Player.GlobalMessage(killer.color + killer.name + Server.DefaultColor + " EXPLODED " + zombie.color + zombie.oldname);
            Command.all.Find("spawn").Use(zombie, "");
            killer.level.MakeExplosion(x, y, z, 1);
        }
        #endregion
        #region help
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infection - Play infection on the current map you are on");
        }
        #endregion
    }
}
