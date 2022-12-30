namespace CrazyEights.Entities
{
    public enum Suits
    {
        Wild = 0,
        Spade,
        Club,
        Heart,
        Diamond,
        Star
    }

    public enum Effects
    {
        None = 0,
        DrawCards,
        SkipTurn,
        ReverseTurnOrder,
        InterchangeHands
    }

    public class Card
    {
        public Suits SuiteId { get; set; }
        public int Value { get; set; }
        public Effects EffectId { get; set; }
        public int EffectArg { get; set; }
    }
}