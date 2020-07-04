namespace Abaddon.Verification
{
    public class Ignorable<TBoardEntry>
    {
        public bool Ignore { get; }
        public TBoardEntry Value { get; }

        public Ignorable()
        {
            Ignore = true;
        }

        public Ignorable(TBoardEntry value)
        {
            Value = value;
            Ignore = false;
        }
    }
}