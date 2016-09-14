using System;
using SpaceInvaders.Interfaces;

namespace SpaceInvaders.Models
{
    class Score : IScoreable
    {
        public bool IsScoreAvailable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IScoreable.Score
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
