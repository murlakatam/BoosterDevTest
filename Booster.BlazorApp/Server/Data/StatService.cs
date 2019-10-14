using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booster.Shared.Domain;

namespace Booster.BlazorApp.Server.Data
{
    public class StatService
    {
        private static StringBuilder _totalText = new StringBuilder();
        private static Dictionary<Word, int> _words = new Dictionary<Word, int>();
        private static Dictionary<char, int> _chars = new Dictionary<char, int>();
        public delegate Task StatServiceAsyncEventHandler(Stats e);
        public event StatServiceAsyncEventHandler StatsHasChanged; 

        public async Task UpdateStats(IAsyncEnumerable<string> stream)
        {
            await foreach (var text in stream)
            {
                _totalText.Append(text);
                AddWords(WordUtitlity.ParseWordsFromText(text));
                AddChars(text);
                var stats = BuildStats(_totalText, _words, _chars);
                StatsHasChanged?.Invoke(stats);
            }
        }

        public Stats BuildStats(StringBuilder totalText, Dictionary<Word, int> words, Dictionary<char, int> chars)
        {
            return new Stats { Words = BuildWordsStats(words), Chars = BuildCharsStats(totalText, chars) };
        }

        public Stats.CharsStats BuildCharsStats(StringBuilder totalText, Dictionary<char, int> chars)
        {
            var result = new Stats.CharsStats();

            if (chars != null && chars.Any())
            {
                //requirement: 
                // •	List showing all the characters used in the text and the number of times they appear sorted in descending order
                var charsByAppearance = chars.OrderByDescending(x => x.Value).ToArray();
                result.AllCharsOrderedByAppearance = charsByAppearance
                    .Select(x => new Stats.CharsStats.CharAppearance
                    {AChar = x.Key, AppearedTimes = x.Value}).ToList();
            }

            //requirement:
            //•	Total number of characters
            //(it doesn't say unique chars, or all chars, so I've assumed ALL
            result.TotalCharsInText = totalText?.Length ?? 0;
            
            return result;
        }

        public Stats.WordsStats BuildWordsStats(Dictionary<Word, int> words)
        {
            var result = new Stats.WordsStats();
            if (words != null && words.Any())
            {
                result.TotalWordsInText = words.Sum(x => x.Value);

                // requirement
                // •	The 5 largest and 5 smallest word 
                var wordsByLength = words.Select(x => x.Key).OrderByDescending(x => x.Length).ToArray();
                if (wordsByLength.Length <= 5)
                {
                    result.FiveLargest = wordsByLength.ToList();
                    result.FiveSmallest = wordsByLength.Reverse().ToList();
                }
                else
                {
                    result.FiveLargest = wordsByLength[..5].ToList();
                    result.FiveSmallest = wordsByLength[^5..].ToList();
                }

                // requirement
                //•	10 most frequently used words
                result.TenMostUsed = words.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key).ToList();
            }

            return result;
        }

        internal void AddWords(IEnumerable<Word> words)
        {
            foreach (var word in words)
            {
                AddAWord(word);
            }
        }


        internal void AddChars(string text)
        {
            foreach (var ch in text)
            {
                AddAChar(ch);
            }
        }

        internal void AddAWord(Word word)
        {
            if (!_words.ContainsKey(word))
            {
                _words.Add(word, 1);
            }
            else
            {
                _words[word] = ++_words[word];
            }
        }

        internal void AddAChar(char ch)
        {
            if (!_chars.ContainsKey(ch))
            {
                _chars.Add(ch, 1);
            }
            else
            {
                _chars[ch] = ++_chars[ch];
            }
        }
    }
}
