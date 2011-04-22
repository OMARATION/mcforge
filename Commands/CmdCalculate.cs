using System;
using System.IO;
using System.Threading;

namespace MCForge
{
    public class CmdCalculate : Command
    {
        public override string name { get { return "calculate"; } }
        public override string shortcut { get { return "calc"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            if (ValidChar(message.Split(' ')[0]) && ValidChar(message.Split(' ')[2]))
            {
                if (message.Split(' ')[1] == "square" && message.Split(' ').Length == 2)
                {
                    float result = float.Parse(message.Split(' ')[0]) * float.Parse(message.Split(' ')[0]);
                    Player.SendMessage(p, "The answer: %aThe square of " + message.Split(' ')[0] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "root" && message.Split(' ').Length == 2)
                {
                    double result = Math.Sqrt(double.Parse(message.Split(' ')[0]));
                    Player.SendMessage(p, "The answer: %aThe root of " + message.Split(' ')[0] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "pi" && message.Split(' ').Length == 2)
                {
                    double result = int.Parse(message.Split(' ')[0]) * Math.PI;
                    Player.SendMessage(p, "The answer: %a" + message.Split(' ')[0] + " x PI" + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "x" && message.Split(' ').Length == 3)
                {
                    float result = float.Parse(message.Split(' ')[0]) * float.Parse(message.Split(' ')[2]);
                    Player.SendMessage(p, "The answer: %a" + message.Split(' ')[0] + " x " + message.Split(' ')[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "+" && message.Split(' ').Length == 3)
                {
                    float result = float.Parse(message.Split(' ')[0]) + float.Parse(message.Split(' ')[2]);
                    Player.SendMessage(p, "The answer: %a" + message.Split(' ')[0] + " + " + message.Split(' ')[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "-" && message.Split(' ').Length == 3)
                {
                    float result = float.Parse(message.Split(' ')[0]) - float.Parse(message.Split(' ')[2]);
                    Player.SendMessage(p, "The answer: %a" + message.Split(' ')[0] + " - " + message.Split(' ')[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (message.Split(' ')[1] == "/" && message.Split(' ').Length == 3)
                {
                    float result = float.Parse(message.Split(' ')[0]) / float.Parse(message.Split(' ')[2]);
                    Player.SendMessage(p, "The answer: %a" + message.Split(' ')[0] + " / " + message.Split(' ')[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                else { Player.SendMessage(p, "There is no such method"); }
            }
            else { Player.SendMessage(p, "You can't calculate letters"); }
        }
        public override void Help(Player p)
        {
            //Help message
            Player.SendMessage(p, "/calculate <num1> <method> <num2> - Calculates <num1> <method> <num2>");
            Player.SendMessage(p, "methods with 3 fillins: /, x, -, +");
            Player.SendMessage(p, "/calculate <num1> <method> - Calculates <num1> <method>");
            Player.SendMessage(p, "methods with 2 fillins: square, root, pi");
        }
        public static bool ValidChar(string chr)
        {
            string allowedchars = "01234567890.,";
            foreach (char ch in chr) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }
    }
}