namespace PMT
{
    internal struct MatchEvent : IEventData
    {
        public bool Match;
        public Gem Sender;
        public Gem Other;

        public MatchEvent(bool match, Gem sender, Gem other)
        {
            Match = match;
            Sender = sender;
            Other = other;
        }
    }
}
