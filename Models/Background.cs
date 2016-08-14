using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Background : Sprite
    {
        private const string k_TexturePath = "BG_Space01_1024x768";

        public Background(Game i_Game)
            : base(i_Game, k_TexturePath)
        {
            TintColor = new Color(0.4f, 0.4f, 0.4f);
        }
    }
}