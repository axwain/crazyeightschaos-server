namespace CrazyEights.Tests.TestUtils.RandomIntMock
{
    public abstract class AbstractRandomIntMock
    {
        protected int Counter { get; set; }

        public AbstractRandomIntMock()
        {
            Reset();
        }

        public void Reset()
        {
            Counter = 0;
        }

        public abstract int Next(int maxValue);
    }
}