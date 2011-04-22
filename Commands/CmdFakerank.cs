using System;

namespace MCForge
{
   public class CmdFakeRank : Command
   {
      public override string name { get { return "fakerank"; } }
      public override string shortcut { get { return "frk"; } } 
      public override string type { get { return "other"; } }
      public override bool museumUsable { get { return true; } }
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/fakerank <name> <rank> - Sends a fake rank change message.");
      }
      public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
      public override void Use(Player p, string message)
      {
         if (message == ""){Help(p); return;}
         Player plr = Player.Find(message.Split (' ')[0]);
         Group grp = Group.Find(message.Split (' ')[1]);
         if (plr == null)
         {
            Player.SendMessage(p, Server.DefaultColor + "Player not found!");
            return;
         }
         if (grp == null)
         {
             Player.SendMessage(p, Server.DefaultColor + "No rank entered.");
             return;
         }
         if (Group.GroupList.Contains(grp))
         {
             
             if (grp.name == "banned")
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + ".");
             }
             else
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + " is now " + grp.color + grp.name + Server.DefaultColor + ".");
                 Player.GlobalMessage("&6Congratulations!");
             }
         }
         else
         {
             Player.SendMessage(p, Server.DefaultColor + "Invalid Rank Entered!");
             return;
         }
       
         
          /*
         if(grp.name == "guest")
         {Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to %7guest.");
            Player.GlobalMessage("Congratulations!");
         }
         if(grp.name == "builder")
         {Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to %dbuilder.");
            Player.GlobalMessage("Congratulations!");
         }
         if(grp.name == "adv")
         {Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to %4advbuilder.");
            Player.GlobalMessage("Congratulations!");
         }
         if (grp.name == "op")
         {Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to %coperator.");
            Player.GlobalMessage("Congratulations!");
         }
         if(grp.name == "superop")
         {Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to %1superop.");
            Player.GlobalMessage("Congratulations!");
         }
         }*/
      }
   }
}
