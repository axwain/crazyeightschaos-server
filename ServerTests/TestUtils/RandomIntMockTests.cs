using CrazyEights.Tests.TestUtils.RandomIntMock;

namespace CrazyEights.Tests.TestUtils
{
    /**
     * It may seem going overboard to have tests for mock classes, but this will help to have some proof
     * that the mocks return the expected values for testing
     */
    [TestFixture]
    public class RandomIntMockTests
    {
        [Test, Description("Returns the current value of the counter")]
        public void RandomCounterNumberMock_Next()
        {
            var randomMock = new RandomCounterNumberMock();
            int[] testArray = new int[6];
            int[] expectedArray = { 0, 1, 2, 3, 4, 5 };

            for (var i = 0; i < expectedArray.Length; i++)
            {
                testArray[i] = randomMock.Next(expectedArray.Length);
            }

            Assert.That(testArray, Is.EquivalentTo(expectedArray), "should return current counter value");
        }

        [Test, Description("Gives only odd numbers numbers duplicated in their previous even index")]
        public void RandomOddNumbersMock_Next()
        {
            var randomMock = new RandomOddNumbersMock();
            int[] testArray = new int[6];
            int[] expectedArray = { 1, 1, 3, 3, 5, 5 };

            for (var i = 0; i < expectedArray.Length; i++)
            {
                testArray[i] = randomMock.Next(expectedArray.Length);
            }

            Assert.That(testArray, Is.EquivalentTo(expectedArray), "should return duplicated odd numbers");
        }
    }
}