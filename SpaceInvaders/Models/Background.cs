using Infrastructure.Models;
using Infrastructure.Models.Screens;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Background : Sprite
    {
        private const string k_TexturePath = "BG_Space01_1024x768";

        public Background(GameScreen i_GameScreen)
            : base(i_GameScreen, k_TexturePath)
        {
            this.DrawOrder = int.MinValue;
            TintColor = new Color(0.4f, 0.4f, 0.4f);
        }
    }
}