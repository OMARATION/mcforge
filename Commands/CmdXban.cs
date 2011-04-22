using System;

namespace MCForge
{
   public class CmdXban : Command
   {
      
      public override string name { get { return "xban"; } }

      public override string shortcut { get { return ""; } }

      public override string type { get { return "other"; } }
   
      public override bool museumUsable { get { return false; } }

      public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

public CmdXban() { }      
public override void Use(Player p, string message)
               
      {

                      if (message == "") { Help(p); return; }

                      Player who = Player.Find(message.Split(' ')[0]);
                      if (Server.devs.Contains(message.ToLower()))
                      {
                          Player.SendMessage(p, "You can't ban a MCForge Developer!");
                          if (p != null)
                          {
                              Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " attempted to ban a MCForge Developer!");
                          }
                          else
                          {
                              Player.GlobalMessage(Server.DefaultColor + "The Console attempted to ban a MCForge Developer!");
                          }
                          return;
                      }
                      if (who != null) {

                     Command.all.Find("undo").Use(p, message + " all");
                     Command.all.Find("ban").Use(p, message); 
                     Command.all.Find("banip").Use(p, "@"+message);
                     Command.all.Find("kick").Use(p,message);
                     Command.all.Find("undo").Use(p,message+" all");

                                       }

                      else {

                           Command.all.Find("undo").Use(p, message + " all");
                           Command.all.Find("ban").Use(p,message);                           
                           Command.all.Find("banip").Use(p, "@"+message);
                           
                            }
                     
                     
                                         
                     
                         
      }

      
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/xban [name] - Bans, kicks, and undoes [name]");
      }
   }
}