using System;
using System.Collections.Generic;
using System.Linq;
using Booster.Shared.Domain;

namespace Booster.BlazorApp.Server.Data
{
    public class WordUtitlity
    {
        public static IEnumerable<Word> ParseWordsFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Enumerable.Empty<Word>();
            }

            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();

            return text.Split(new char[0] , StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Word(x.Trim(punctuation))); 
        }
    }
}
