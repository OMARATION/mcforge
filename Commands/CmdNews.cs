using System;
using System.IO;
namespace MCForge
{
    public class Cmdnews : Command
    {
        public override string name { get { return "news"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            string newsFile = "text/news.txt";
            if (!File.Exists(newsFile) || (File.Exists(newsFile) && File.ReadAllLines(newsFile).Length == -1))
            {
                StreamWriter SW = new StreamWriter(newsFile);
                SW.WriteLine("News have not been created. Put News in '" + newsFile + "'.");
                SW.Close();
                return;
            }
            string[] strArray = File.ReadAllLines(newsFile);
            if (message == "") { for (int j = 0; j < strArray.Length; j++) { Player.SendMessage(p, strArray[j]); } }
            else
            {
                string[] split = message.Split(' ');
                if (split[0] == "all") { if (p.group.Permission < LevelPermission.Operator) { Player.SendMessage(p, "You must be at least " + LevelPermission.Operator + " to send this to all players."); return; } for (int k = 0; k < strArray.Length; k++) { Player.GlobalMessage(strArray[k]); } return; }
                else
                {
                    Player player = Player.Find(split[0]);
                    if (player == null) { Player.SendMessage(p, "Could not find player \"" + split[0] + "\"!"); return; }
                    for (int l = 0; l < strArray.Length; l++) { Player.SendMessage(player, strArray[l]); }
                    Player.SendMessage(p, "The News were successfully sent to " + player.name + ".");
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/news - Shows server news.");
            Player.SendMessage(p, "/news <player> - Sends the News to <player>.");
            Player.SendMessage(p, "/news all - Sends the News to everyone.");
        }
    }
}