/*
	Auto-generated command skeleton class.

	Use this as a basis for custom commands implemented via the MCDerp scripting framework.
	File and class should be named a specific way.  For example, /update is named 'CmdUpdate.cs' for the file, and 'CmdUpdate' for the class.
*/

// Add any other using statements you need up here, of course.
// As a note, MCDerp is designed for .NET 3.5.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MCForge
{
	public class CmdInformationbot : Command
	{
		// The command's name, in all lowercase.  What you'll be putting behind the slash when using it.
		public override string name { get { return "informationbot"; } }

		// Command's shortcut (please take care not to use an existing one, or you may have issues.
		public override string shortcut { get { return "ib"; } }

		// Determines which submenu the command displays in under /help.
		public override string type { get { return "other"; } }

		// Determines whether or not this command can be used in a museum.  Block/map altering commands should be made false to avoid errors.
		public override bool museumUsable { get { return false; } }

		// Determines the command's default rank.  Valid values are:
		// LevelPermission.Nobody, LevelPermission.Banned, LevelPermission.Guest
		// LevelPermission.Builder, LevelPermission.AdvBuilder, LevelPermission.Operator, LevelPermission.Admin
		public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

		// This is where the magic happens, naturally.
		// p is the player object for the player executing the command.  message is everything after the command invocation itself.
        //public CmdInformationbot() { }
		public override void Use(Player p, string message)
		{
            string rankup = "You need to build and either earn money and buy a rank or get promoted by an op.";
            string money = "You get money if the server by building if they use an economy system.";
            
            if (!File.Exists("text/infobot.txt"))
            {
                File.Create("text/infobot.txt");
                StreamWriter w = new StreamWriter("text/infobot.txt");

            }
            if (message == " ")
            {
            Command.all.Find("botadd").Use(p, "information_bot");
            Player.SendMessage(p, "Ask anything to the bot he will help you");
            Command.all.Find("botset").Use(p, "information_bot follow");
            return;
            }
            

		}

		// This one controls what happens when you use /help [commandname].
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/informationbot [topic] - A helpful auto bot. Ask it anything.");
            Player.SendMessage(p, "Available topics:");
		}
	}
}