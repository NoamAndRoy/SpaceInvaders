using System;
using Infrastructure.ManagersInterfaces;
using Microsoft.Xna.Framework;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models
{
    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        protected readonly GameScreen r_GameScreen;

        protected readonly string r_AssetName;

        public event EventHandler<EventArgs> PositionChanged;

        public event EventHandler<EventArgs> SizeChanged;

        public event EventHandler<EventArgs> RotationChanged;

        public event EventHandler<EventArgs> Disposed;

        public GameScreen GameScreen
        {
            get { return r_GameScreen; }
        }

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

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (Disposed != null)
            {
                Disposed.Invoke(sender, args);
            }
        }

        public LoadableDrawableComponent(GameScreen i_GameScreen, string i_AssetName)
            : base(i_GameScreen.Game)
        {
            this.r_GameScreen = i_GameScreen;
            this.r_AssetName = i_AssetName;
            this.UpdateOrder = int.MaxValue;
            this.DrawOrder = int.MaxValue;
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnDisposed(this, EventArgs.Empty);
        }
    }
}