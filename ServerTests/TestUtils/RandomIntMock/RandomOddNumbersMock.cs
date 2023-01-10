namespace CrazyEights.Tests.TestUtils.RandomIntMock
{
    public class RandomOddNumbersMock : AbstractRandomIntMock
    {
        public override int Next(int maxValue)
        {
            if (Counter >= maxValue)
            {
                Reset();
            }
            var next = ((Counter + 2) / 2) * 2 - 1;
            Counter++;
            return next == maxValue ? next - 1 : next;
        }
    }
}