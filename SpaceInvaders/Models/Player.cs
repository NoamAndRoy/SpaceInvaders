using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Player
    { 
        private readonly string m_PlayerName;

        public int PlayerScore { get; set; }

        public Vector2 ScorePosition { get; set; }

        public Vector2 SoulsPostion { get; set; }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }

        public Player(string i_PlayerName)
        {
            m_PlayerName = i_PlayerName;
        }
    }
}
