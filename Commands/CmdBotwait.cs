﻿using System;

namespace MCForge
{
   public class CmdBotwait : Command
   {
      // The Command's name, in all lowercase.  What you'll be putting behind the slash when using it.
      public override string name { get { return "botwait"; } }

      // Command's shortcut (please take care not to use an existing one, or you may have issues.
      public override string shortcut { get { return "bw"; } }

      // Determines which submenu the Command displays in under /help.
      public override string type { get { return "other"; } }

      // Determines whether or not this Command can be used in a museum.  Block/map altering Commands should be made false to avoid errors.
      public override bool museumUsable { get { return false; } }

      // Determines the Command's default rank.  Valid values are:
      // LevelPermission.Nobody, LevelPermission.Banned, LevelPermission.Guest
      // LevelPermission.Builder, LevelPermission.AdvBuilder, LevelPermission.Operator, LevelPermission.Admin
      public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

      // This is where the magic happens, naturally.
      // p is the player object for the player executing the Command.  message is everything after the Command invocation itself.
      public override void Use(Player p, string message)
      {
            Command.all.Find("botadd").Use(p, message);
            Command.all.Find("botai").Use(p, "add wait");
            Command.all.Find("botset").Use(p, message+ " wait");
            Player.SendMessage(p, "'"+ message + "' Bot is Set to Wait!");
            Command.all.Find("botai").Use(p, "del wait");
      }

      // This one controls what happens when you use /help [Commandname].
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/botwait - Makes a Bot thats set to wait. If bot moves from position, remove and repeat.");
      }
   }
}