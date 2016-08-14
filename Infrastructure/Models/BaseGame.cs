using Infrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public class BaseGame : Game
    {
        protected GraphicsDeviceManager m_Graphics;

        private readonly CollisionManager r_CollisionManager;
        private readonly KeyboardManager r_KeyBoardManager;
        private readonly MouseManager r_MouseManager;

        public SpriteBatch SpriteBatch { get; private set; }
        
        public BaseGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            r_CollisionManager = new CollisionManager(this);
            r_KeyBoardManager = new KeyboardManager(this);
            r_MouseManager = new MouseManager(this);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void Draw(GameTime i_GameTime)
        {
            SpriteBatch.Begin();
            base.Draw(i_GameTime);
            SpriteBatch.End();
        }

        public virtual void EndGame()
        {
            this.Exit();
        }
    }
}