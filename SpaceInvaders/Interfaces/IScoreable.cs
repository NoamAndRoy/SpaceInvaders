namespace SpaceInvaders.Interfaces
{
    public interface IScoreable
    {
        int Score { get; }

        bool IsScoreAvailable { get; }
    }
}