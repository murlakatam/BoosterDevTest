using System.Collections.Generic;
using System.Linq;
using System.Text;
using Booster.BlazorApp.Server.Data;
using Booster.Shared.Domain;
using NUnit.Framework;

namespace Booster.UnitTests
{
    public class StatServiceTests
    {
        private StringBuilder _totalText = new StringBuilder("Something happens, brown fox jumps over the fence!");

        private Dictionary<char, int> _chars = new Dictionary<char, int>
        {
            {'a', 1}, {'b', 2}, {'c', 3 },{ 'd', 4 },{ 'e', 5}
        };

        private Dictionary<Word, int> _words = new Dictionary<Word, int>
        {
            {new Word("test"), 1}, 
            {new Word("bigger"), 1},
            {new Word("evenBigger"), 100},
            {new Word("theBiggestOne"), 1},
            {new Word("test2"), 1},
        };

        [Test]
        public void BuildCharsStats_Empty()
        {
            var target = new StatService();
            var result = target.BuildCharsStats(null, null);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalCharsInText, 0);
            Assert.IsNull(result.AllCharsOrderedByAppearance);
        }

        [Test]
        public void BuildCharsStats_Positive()
        {
            var target = new StatService();
            var result = target.BuildCharsStats(_totalText, _chars);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalCharsInText, _totalText.Length);
            Assert.IsNotNull(result.AllCharsOrderedByAppearance);
            Assert.AreEqual(result.AllCharsOrderedByAppearance.ElementAtOrDefault(0)?.AChar, 'e');
            Assert.AreEqual(result.AllCharsOrderedByAppearance.ElementAtOrDefault(4)?.AChar, 'a');
        }

        [Test]
        public void BuildWordsStats_Empty()
        {
            var target = new StatService();
            var result = target.BuildWordsStats(null);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalWordsInText, 0);
            Assert.IsNull(result.FiveSmallest);
            Assert.IsNull(result.FiveLargest);
            Assert.IsNull(result.TenMostUsed);
        }

        [Test]
        public void BuildWordsStats_Positive()
        {
            var target = new StatService();
            var result = target.BuildWordsStats(_words);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TotalWordsInText, 104);
            Assert.IsNotNull(result.FiveSmallest);
            Assert.AreEqual(result.FiveSmallest.ElementAtOrDefault(0)?.Text, "test");
            Assert.AreEqual(result.FiveLargest.ElementAtOrDefault(0)?.Text, "thebiggestone");
            Assert.AreEqual(result.TenMostUsed.ElementAtOrDefault(0)?.Text, "evenbigger");
        }

        
    }
}