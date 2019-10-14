using System;

namespace Booster.Shared.Domain
{
    public class Word : IEquatable<Word>
    {
        private string _word;

        public Word(string word)
        {

            _word = word?.ToLower();
        }

        public string Text
        {
            get => _word;
            set => _word = value;
        }

        public int Length
        {
            get => _word?.Length ?? 0;
        }

        public virtual bool Equals(Word other)
        {
            if (other == null)
                return false;
            return string.Equals(_word, other.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Word);
        }

        public override int GetHashCode()
        {
            return _word?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return _word;
        }
    }
}
