using CrazyEights.Utils;

namespace CrazyEights.Tests.Utils
{
    [TestFixture]
    public class Base15Tests
    {
        [Test]
        public void Encode_ValueZero_ReturnAllZeroes()
        {
            Assert.AreEqual(Base15.Encode(0), "00000000");
        }

        [Test]
        public void Encode_ValueMaxInt32_ReturnD87Z66C7()
        {
            Assert.AreEqual(Base15.Encode(Int32.MaxValue), "D87Z66C7");
        }

        [Test]
        public void Encode_Value50624_Return0000ZZZZ()
        {
            Assert.AreEqual(Base15.Encode(50624), "0000ZZZZ");
        }

        [Test]
        public void Encode_ValueLessThan0_Throws()
        {
            Assert.That(() => Base15.Encode(-1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("input"));
            Assert.That(() => Base15.Encode(-5000), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("input"));
            Assert.That(() => Base15.Encode(Int32.MinValue), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Decode_Value000000_Return0()
        {
            Assert.That(Base15.Decode("00000000"), Is.EqualTo(0));
        }

        [Test]
        public void Decode_Value0000ZZZZ_Return50624()
        {
            Assert.That(Base15.Decode("0000ZZZZ"), Is.EqualTo(50624));
        }

        [Test]
        public void Decode_ValueD87Z66C7_ReturnMaxInt32()
        {
            Assert.That(Base15.Decode("D87Z66C7"), Is.EqualTo(Int32.MaxValue));
        }

        [Test]
        public void Encode_Decode_RandomValue_ReturnSameValue()
        {
            var rand = new Random();
            var initialValue = rand.Next(Int32.MaxValue);
            var encodedValue = Base15.Encode(initialValue);
            Assert.That(Base15.Decode(encodedValue), Is.EqualTo(initialValue));
        }

        [Test]
        public void Decode_CodeNot8Characters_Throws()
        {
            Assert.That(() => Base15.Decode(string.Empty), Throws.TypeOf<ArgumentException>()
                .With.Property("ParamName").EqualTo("code"));
            Assert.That(() => Base15.Decode("00"), Throws.TypeOf<ArgumentException>()
                .With.Property("ParamName").EqualTo("code"));
            Assert.That(() => Base15.Decode("012345678"), Throws.TypeOf<ArgumentException>());
        }

        [Test, Description("Decode Throws if the provided code contains invalid characters")]
        public void Decode_InvalidCode_Throws()
        {
            Assert.That(() => Base15.Decode("0000000A"), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("code"));
            Assert.That(() => Base15.Decode("E0000000"), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("code"));
            Assert.That(() => Base15.Decode("0000G000"), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}