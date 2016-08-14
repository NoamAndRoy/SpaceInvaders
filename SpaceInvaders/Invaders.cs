using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.Models;
using SpaceInvaders.Models;

namespace SpaceInvaders
{
    public class Invaders : BaseGame
    {
        private readonly Background r_Background;
        private readonly PlayerShip r_Player;
        private readonly MotherShip r_MotherShip;
        private readonly AlienMatrix r_Aliens;

        public Invaders()
        {
            this.Window.Title = "Invaders";
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;

            r_Background = new Background(this);
            r_Player = new PlayerShip(this);
            r_MotherShip = new MotherShip(this);
            r_Aliens = new AlienMatrix(this);
        }

        public override void EndGame()
        {
            System.Windows.Forms.MessageBox.Show("Your score is " + r_Player.PlayerScore);
            base.EndGame();
        }
    }
}