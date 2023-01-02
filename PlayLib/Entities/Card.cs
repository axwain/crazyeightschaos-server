namespace CrazyEights.PlayLib.Entities
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
        public Suits SuiteId { get; private set; }
        public int Value { get; private set; }
        public Effects EffectId { get; private set; }
        public int EffectArg { get; private set; }

        public Card(Suits suit, int value, Effects effect, int effectArg)
        {
            SuiteId = suit;
            Value = value;
            EffectId = effect;
            EffectArg = effectArg;
        }

        public bool Equals(Card card)
        {
            return SuiteId == card.SuiteId && Value == card.Value;
        }
    }
}