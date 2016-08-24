using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Models;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models;
using Infrastructure.Managers;

namespace SpaceInvaders
{
    public class Invaders : Game
    {
        private readonly CollisionManager r_CollisionManager;
        private readonly KeyboardManager r_KeyBoardManager;
        private readonly MouseManager r_MouseManager;

        private readonly Background r_Background;
        private readonly Player r_PlayerOne;
        private readonly Player r_PlayerTwo;
        private readonly MotherShip r_MotherShip;
        private readonly AlienMatrix r_Aliens;
        private readonly WallsLine r_Walls;

        private readonly PlayerShip r_PlayerShipOne;
        private readonly PlayerShip r_PlayerShipTwo;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;

        public Invaders() : base()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.Window.Title = "Invaders";
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;

            r_CollisionManager = new CollisionManager(this);
            r_KeyBoardManager = new KeyboardManager(this);
            r_MouseManager = new MouseManager(this);

            r_Background = new Background(this);
            r_MotherShip = new MotherShip(this);
            r_Aliens = new AlienMatrix(this);
            r_Aliens.AllDead += endGameByAlienMatrix;
            r_Aliens.ReachBottom += endGameByAlienMatrix;

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

        private void endGameByAlienMatrix(object sender, System.EventArgs e)
        {
            endGame();   
        }

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

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

        protected override void Update(GameTime gameTime)
        {
            if(r_PlayerOne.Souls == 0 && r_PlayerTwo.Souls == 0)
            {
                endGame();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }

        private void endGame()
        {
            string message;
            if (r_PlayerOne.PlayerScore != r_PlayerTwo.PlayerScore)
            {
                message = string.Format(
@"The winner is {0}!
{1} Score: {3}
{2} Score: {4}",
r_PlayerOne.PlayerScore > r_PlayerTwo.PlayerScore ? r_PlayerOne.PlayerName : r_PlayerTwo.PlayerName,
r_PlayerOne.PlayerName,
r_PlayerTwo.PlayerName,
r_PlayerOne.PlayerScore,
r_PlayerTwo.PlayerScore);
            }
            else
            {
                message = "Tie!!!";
            }

            System.Windows.Forms.MessageBox.Show(message);
            this.Exit();
        }
    }
}