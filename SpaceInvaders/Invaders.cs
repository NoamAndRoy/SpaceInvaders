using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Models;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models;

namespace SpaceInvaders
{
    public class Invaders : BaseGame
    {
        private readonly Background r_Background;
        private readonly Player r_PlayerOne;
        private readonly Player r_PlayerTwo;
        private readonly MotherShip r_MotherShip;
        private readonly AlienMatrix r_Aliens;
        private readonly WallsLine r_Walls;

        private readonly PlayerShip r_PlayerShipOne;
        private readonly PlayerShip r_PlayerShipTwo;

        public Invaders() : base()
        { 
            this.Window.Title = "Invaders";
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;

            r_Background = new Background(this);
            r_MotherShip = new MotherShip(this);
            r_Aliens = new AlienMatrix(this);
            r_Walls = new WallsLine(this);


            r_PlayerShipOne = new PlayerShip(this, "Ship01_32x32");
            r_PlayerShipTwo = new PlayerShip(this, "Ship02_32x32");

            r_PlayerOne = new Player(this, "Player1");
            r_PlayerOne.TextColor = Color.Blue;
            r_PlayerOne.PlayerShip = r_PlayerShipOne;
            r_PlayerOne.YPosition = 10;

            r_PlayerTwo = new Player(this, "Player2");
            r_PlayerTwo.TextColor = Color.Green;
            r_PlayerTwo.PlayerShip = r_PlayerShipTwo;
            r_PlayerTwo.YPosition = r_PlayerOne.YPosition + 20;
        }

        protected override void Initialize()
        {
            base.Initialize();

            r_MotherShip.TintColor = Color.Red;

            r_PlayerShipOne.Position = new Vector2(0f, GraphicsDevice.Viewport.Height - (r_PlayerShipOne.Height * 2 * 0.6f));
            r_PlayerShipOne.UseMouse = true;
            r_PlayerShipOne.MoveLeftKey = Keys.Left;
            r_PlayerShipOne.MoveRightKey = Keys.Right;
            r_PlayerShipOne.ShootKey = Keys.Space;
                       
            r_PlayerShipTwo.Position = new Vector2(r_PlayerShipOne.Width, GraphicsDevice.Viewport.Height - (r_PlayerShipOne.Height * 2 * 0.6f));
            r_PlayerShipTwo.UseMouse = false;
            r_PlayerShipTwo.MoveLeftKey = Keys.S;
            r_PlayerShipTwo.MoveRightKey = Keys.F;
            r_PlayerShipTwo.ShootKey = Keys.E;

            r_Walls.YPosition = r_PlayerShipOne.TopLeftPosition.Y - (Content.Load<Texture2D>(Wall.k_SpritesPath + Wall.k_TexturePath).Height * 2);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void EndGame()
        {
            /*string message = string.Format(
@"The winner is {0}!
P1 Score: {1}
P2 Score: {2}",
r_PlayerOne.PlayerScore > r_PlayerTwo.PlayerScore ? "P1" : "p2",
r_PlayerOne.PlayerScore,
r_PlayerTwo.PlayerScore);*/

            //System.Windows.Forms.MessageBox.Show(message);
            base.EndGame();
        }
    }
}