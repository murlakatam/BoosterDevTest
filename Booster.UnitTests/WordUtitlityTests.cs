using System.Linq;
using Booster.BlazorApp.Server.Data;
using NUnit.Framework;

namespace Booster.UnitTests
{
    public class WordUtitlityTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseWordsFromText_Empty()
        {
            var result = WordUtitlity.ParseWordsFromText(string.Empty);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void ParseWordsFromText_Null()
        {
            var result = WordUtitlity.ParseWordsFromText(null);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 0);
        }


        [Test]
        public void ParseWordsFromText_TwoWords_SplittedBySpace()
        {
            var result = WordUtitlity.ParseWordsFromText("Test data");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.ElementAtOrDefault(0)?.ToString(), "test");
            Assert.AreEqual(result.ElementAtOrDefault(1)?.ToString(), "data");
        }

        [Test]
        public void ParseWordsFromText_TwoWords_SplittedByComma()
        {
            var result = WordUtitlity.ParseWordsFromText("Test, data.");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.ElementAtOrDefault(0)?.ToString(), "test");
            Assert.AreEqual(result.ElementAtOrDefault(1)?.ToString(), "data");
        }

        [Test]
        public void ParseWordsFromText_OneWord()
        {
            var result = WordUtitlity.ParseWordsFromText("Test.");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.ElementAtOrDefault(0)?.ToString(), "test");
        }
    }
}