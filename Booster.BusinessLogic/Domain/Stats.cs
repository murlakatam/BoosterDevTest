using System.Collections.Generic;

namespace Booster.Shared.Domain
{
    public class Stats
    {
        public class WordsStats
        {
            public long TotalWordsInText { get; set; }

            public List<Word> FiveLargest { get; set; }

            public List<Word> FiveSmallest { get; set; }

            public List<Word> TenMostUsed { get; set; }
        }

        public class CharsStats
        {
            public class CharAppearance
            {
                public char AChar { get; set; }
                public int AppearedTimes { get; set; }
            }
            public long TotalCharsInText { get; set; }
            
            public List<CharAppearance> AllCharsOrderedByAppearance { get; set; }
        }

        public WordsStats Words { get; set; }

        public CharsStats Chars { get; set; }

    }
}
