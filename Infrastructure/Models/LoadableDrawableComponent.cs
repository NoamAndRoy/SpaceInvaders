using System;
using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework;

namespace Infrastructure.Models
{
    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        protected readonly string r_AssetName;

        public event EventHandler<EventArgs> PositionChanged;

        public event EventHandler<EventArgs> SizeChanged;

        public event EventHandler<EventArgs> RotationChanged;

        public string AssetName
        {
            get { return r_AssetName; }
        }

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, EventArgs.Empty);
            }
        }

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
            this.r_AssetName = i_AssetName;
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