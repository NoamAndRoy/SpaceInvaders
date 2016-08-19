using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Models;
using Infrastructure.Managers;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Invaders : Game
    {
        private readonly Background r_Background;
        private readonly PlayerShip r_PlayerOne;
        private readonly PlayerShip r_PlayerTwo;
        private readonly MotherShip r_MotherShip;
        private readonly AlienMatrix r_Aliens;
        private readonly WallsLine r_Walls;

        private readonly CollisionManager r_CollisionManager;
        private readonly KeyboardManager r_KeyBoardManager;
        private readonly MouseManager r_MouseManager;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;

        public Invaders()
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
            r_Walls = new WallsLine(this);

            r_PlayerOne = new PlayerShip(this, "Ship01_32x32");
            r_PlayerTwo = new PlayerShip(this, "Ship02_32x32");
        }

        protected override void Initialize()
        {
            base.Initialize();

            r_PlayerOne.Position = new Vector2(0f, GraphicsDevice.Viewport.Height - (r_PlayerOne.Height * 2 * 0.6f));
            r_PlayerOne.UseMouse = true;
            r_PlayerOne.MoveLeftKey = Keys.Left;
            r_PlayerOne.MoveRightKey = Keys.Right;
            r_PlayerOne.ShootKey = Keys.Space;

            r_PlayerTwo.Position = new Vector2(r_PlayerOne.Width, GraphicsDevice.Viewport.Height - (r_PlayerOne.Height * 2 * 0.6f));
            r_PlayerTwo.UseMouse = false;
            r_PlayerTwo.MoveLeftKey = Keys.S;
            r_PlayerTwo.MoveRightKey = Keys.F;
            r_PlayerTwo.ShootKey = Keys.E;
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

            r_Walls.YPosition = r_PlayerOne.TopLeftPosition.Y - Content.Load<Texture2D>(Wall.k_SpritesPath + Wall.k_TexturePath).Height;

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }

        public void EndGame()
        {
            string message = string.Format(
@"The winner is {0}!
P1 Score: {1}
P2 Score: {2}",
r_PlayerOne.PlayerScore > r_PlayerTwo.PlayerScore ? "P1" : "p2",
r_PlayerOne.PlayerScore,
r_PlayerTwo.PlayerScore);

            System.Windows.Forms.MessageBox.Show(message);
            this.Exit();
        }
    }
}