using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Models
{
    public class Player : Sprite
    {
        public Player(Game i_Game, string i_TexturePath) : base(i_Game, i_TexturePath)
        {
        }
    }
}
