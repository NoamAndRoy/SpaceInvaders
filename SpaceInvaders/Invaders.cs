using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Models;
using Infrastructure.Managers;

namespace SpaceInvaders
{
    public class Invaders : Game
    {
        private readonly Background r_Background;
        private readonly PlayerShip r_Player;
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
            r_Player = new PlayerShip(this);
            r_MotherShip = new MotherShip(this);
            r_Aliens = new AlienMatrix(this);
            r_Walls = new WallsLine(this);
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

            r_Walls.YPosition = r_Player.TopLeftPosition.Y - Content.Load<Texture2D>(Wall.k_SpritesPath + Wall.k_TexturePath).Height;

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
            System.Windows.Forms.MessageBox.Show("Your score is " + r_Player.PlayerScore);
            this.Exit();
        }
    }
}