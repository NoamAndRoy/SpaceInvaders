using Infrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Models
{
    public class BaseGame : Game
    {
        private readonly CollisionManager r_CollisionManager;
        private readonly KeyboardManager r_KeyBoardManager;
        private readonly MouseManager r_MouseManager;

        protected GraphicsDeviceManager m_Graphics;
        protected SpriteBatch m_SpriteBatch;

        public BaseGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            r_CollisionManager = new CollisionManager(this);
            r_KeyBoardManager = new KeyboardManager(this);
            r_MouseManager = new MouseManager(this);
        }

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }

        public virtual void EndGame()
        {
            this.Exit();
        }
    }
}
