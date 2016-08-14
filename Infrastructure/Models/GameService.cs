using Microsoft.Xna.Framework;

namespace Infrastructure.Models
{
    public class GameService : RegisteredComponent
    {
        public GameService(Game i_Game)
            : base(i_Game)
        {
            this.UpdateOrder = int.MinValue;
            registerService();
        }

        protected virtual void registerService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
