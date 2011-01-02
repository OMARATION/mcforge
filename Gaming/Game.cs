using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MCForge.Gaming
{
    public abstract class Game
    {
        // Name of the player who is running the game
        public string GameOperator;
        public Game(string op)
        {
            this.GameOperator = op;
        }

        // List of teams
        // For FFA games, each player has their own 'team'
        public List<Team> Teams = new List<Team>();

        // Set cea.Cancel to 'true' if the death was handled by the game
        public abstract void OnPlayerDeath(Player p, CancelEventArgs cea);
        public abstract void OnGameStart();
        public abstract void OnGameEnd();

        public virtual void OnPlayerJoin(Player p)
        {
            //PlayerList;
            // Subscribe to notifications about the player dying
            p.OnDeath += new Player.OnDeathHandler(OnPlayerDeath);
        }

        public virtual void OnPlayerLeave(Player p)
        {
            // Unsubscribe to death notifications
            p.OnDeath -= new Player.OnDeathHandler(OnPlayerDeath);
        }

        public void Start()
        {
            OnGameStart();
        }
        public void End()
        {
            foreach (Team team in Teams)
                foreach (Player p in team.Players)
                    OnPlayerLeave(p);

            OnGameEnd();
        }

    }
}
