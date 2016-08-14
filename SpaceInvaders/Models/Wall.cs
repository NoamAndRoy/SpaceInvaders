using Infrastructure.ManagersInterfaces;
using Infrastructure.Models;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Wall : CollideableSprite
    {
        private const string k_TexturePath = "Barrier_44x32";

        public Wall(Game i_Game) 
            : base(i_Game, k_TexturePath)
        {
        }

        public override void Collided(ICollideable i_Collideable)
        {
            Bullet bullet = i_Collideable as Bullet;

            if (bullet != null && (bullet.BulletType == eBulletType.Player || bullet.BulletType == eBulletType.Enemy))
            {
                OnCollide(i_Collideable);
            }
        }
    }
}
