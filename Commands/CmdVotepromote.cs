using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdVotepromote : Command
    {

        public override string name { get { return "votepromote"; } }

        public override string shortcut { get { return "votep"; } }

        public override string type { get { return "mod"; } }

        public override bool museumUsable { get { return false; } }

        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message)
        {
            bool previousVote = false;
            bool alreadyListed = false;
            string playerName;
            int playerVotes;
            int amountOfKicks;
            string newRecord;
            //Player Owner;
            Player toBePromoted;
            //Owner = Player.Find("Windows_98SE"); No need for this, just use null and it'll use it like the console was using it. 

            //Read in the record of previous votes
            List<String> list = new List<string>();
            StreamReader reader = new StreamReader("extra/votes.dat");

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);          // Add to list.
            }
            reader.Close();

            for (int j = 0; j < list.Count; j++)
            {
                string[] param = list[j].Split('|');

                //Has person already been voted for?
                if (String.Compare(param[0].ToLower(), message.ToLower(), true) == 0)
                {
                    alreadyListed = true;
                    playerName = param[0];
                    toBePromoted = Player.Find(playerName);
                    playerVotes = Int32.Parse(param[1]);
                    amountOfKicks = Int32.Parse(param[2]);

                    //Has this person already voted?
                    for (int i = 3; i < param.Length; i++)
                    {
                        if (String.Compare(param[i].ToLower(), p.name.ToLower(), true) == 0)
                            previousVote = true;
                    }

                    if (previousVote == false)
                    {
                        playerVotes++;
                    }
                    else
                    {
                        Player.SendMessage(p, "You have already voted for this person.");
                    }

                    if (votesToPromote(playerVotes, p) == true)
                    {
                        if (toBePromoted.group.Permission > LevelPermission.Operator)
                        {
                            Command.all.Find("demote").Use(null, playerName);
                        }


                        //Promotes the player
                        Player.GlobalMessage("&a" + playerName + " &ehas accumulated " + playerVotes + " votes for promotion and was promoted!");
                        Command.all.Find("promote").Use(null, playerName);
                        amountOfKicks++;


                        //Reset the record so people who voted before can vote second time in future
                        playerVotes = 0;
                        newRecord = playerName + "|" + playerVotes + "|" + amountOfKicks;
                    }

                    //Not banned yet
                    else
                    {
                        Player.GlobalMessage("&a" + playerName + " &ehas accumulated " + playerVotes + " votes to be promoted!");


                        newRecord = playerName + "|" + playerVotes + "|" + amountOfKicks;
                        for (int m = 3; m < param.Length; m++)
                        {
                            newRecord += "|" + param[m];
                            //Command.all.Find("say").Use(p, newRecord);
                        }

                        if (previousVote == false)
                            newRecord += "|" + p.name;

                        //Command.all.Find("say").Use(p, "FINAL: " + newRecord);   
                    }
                    list[j] = newRecord;
                }
            }

            if (alreadyListed == true)
            {
                StreamWriter writer = new StreamWriter("extra/votes.dat", false);
                foreach (string newRequests in list)
                {
                    writer.WriteLine(newRequests);
                }
                writer.Flush();
                writer.Close();
            }
            //First time voted for
            else
            {
                StreamWriter writer = new StreamWriter("extra/votes.dat", true);
                writer.WriteLine(message + "|1|0|" + p.name);
                writer.Flush();
                writer.Close();
                Player.GlobalMessage("&a" + message + " &ehas accumulated 1 vote for promotion!");
            }
        }

        public bool votesToPromote(int votes, Player p)
        {
            //Determine the number of votes to promote
            int numOfPlayers;
            int numberOfVotesNeeded;

            //Get # of players
            numOfPlayers = Player.players.Count;

            numberOfVotesNeeded = (numOfPlayers / 2) + 1;

            Player.GlobalMessage("Votes needed to promote: (" + numberOfVotesNeeded + "/" + numOfPlayers + ")");
            if (votes >= numberOfVotesNeeded)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/votepromote [player] - submits a vote to promote someone.");
            Player.SendMessage(p, "Once a majority of the server votes for the person they are promoted.");
            Player.SendMessage(p, "You can only vote once.");
        }

    }
}



