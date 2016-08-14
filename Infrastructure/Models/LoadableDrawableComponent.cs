using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        protected string m_AssetName;

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }

        public event EventHandler<EventArgs> PositionChanged;
        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SizeChanged;
        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> RotationChanged;
        protected virtual void OnRotationChanged()
        {
            if (RotationChanged != null)
            {
                RotationChanged(this, EventArgs.Empty);
            }
        }

        public LoadableDrawableComponent(Game i_Game, string i_AssetName)
            : base(i_Game)
        {
            this.AssetName = i_AssetName;
            this.UpdateOrder = int.MaxValue;
            this.DrawOrder = int.MaxValue;

            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (this is ICollideable)
            {
                ICollisionManager collisionManager = this.Game.Services.GetService(typeof(ICollisionManager)) as ICollisionManager;

                if (collisionManager != null)
                {
                    collisionManager.AddComponent(this as ICollideable);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}