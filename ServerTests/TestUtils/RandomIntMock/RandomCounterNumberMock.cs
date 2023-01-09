namespace CrazyEights.Tests.TestUtils.RandomIntMock
{
    public class RandomCounterNumberMock : AbstractRandomIntMock
    {
        public override int Next(int maxValue)
        {
            if (Counter >= maxValue)
            {
                Reset();
            }

            return Counter++;
        }
    }
}