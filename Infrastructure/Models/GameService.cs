using Microsoft.Xna.Framework;

namespace Infrastructure.Models
{
    public class GameService : RegisteredComponent
    {
        public GameService(BaseGame i_BaseGame)
            : base(i_BaseGame)
        {
            this.UpdateOrder = int.MinValue;
        }

        protected virtual void registerService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
