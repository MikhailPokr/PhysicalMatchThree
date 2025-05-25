namespace PMT
{
    internal interface IGemChainSystem : IService
    {
        void Dismatch(Gem gem, Gem connection);
        void Match(Gem gem, Gem connection);
    }
}