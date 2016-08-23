using Microsoft.Xna.Framework;

namespace Infrastructure.Models
{
    public class RegisteredComponent : GameComponent
    {
        public new BaseGame Game { get { return (BaseGame)base.Game; } }

        public RegisteredComponent(Game i_Game)
            : base(i_Game)
        {
            this.UpdateOrder = int.MaxValue;
            Game.Components.Add(this);
        }
    }
}
