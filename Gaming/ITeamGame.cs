using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Gaming
{
    interface ITeamGame
    {
        void CreateTeam(string teamname, string color);
        void AddPlayer(string playername, string teamname);
    }
}
