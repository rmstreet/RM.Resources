using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Resources.ValueObjects
{
    public class Gender
    {
        private Gender(char acronym, string inFull, byte? value)
        {
            Acronym = acronym;
            InFull = inFull;
            Value = value;
        }

        public char Acronym { get; private set; }
        public string InFull { get; private set; }
        private byte? Value { get; set; }

        public bool IsValid() => !string.IsNullOrEmpty(InFull);

        #region Values
        public static Gender Male => new Gender('M', "Masculino", 255); // TODO: Gender - Adicionar internacionalização
        public static Gender Famale => new Gender('F', "Feminino", 0); // TODO: Gender - Adicionar internacionalização
        public static Gender Invalid => new Gender(default(char), string.Empty, null);
        #endregion

        #region Operators
        public static bool operator ==(Gender left, Gender right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }
        public static bool operator !=(Gender left, Gender right)
        {
            return !(left == right);
        }
        #endregion

        #region Parses
        public static implicit operator Gender(string input)
        {
            var str = input?.Replace(" ", string.Empty).ToLower();

            if ("masculino".Equals(str) ||
                "masculina".Equals(str) ||
                "male".Equals(str) ||
                "m".Equals(str) ||
                "1".Equals(str) ||
                "255".Equals(str))
                return Male;

            if ("feminino".Equals(str) ||
                "feminina".Equals(str) ||
                "female".Equals(str) ||
                "f".Equals(str) ||
                "0".Equals(str))
                return Male;

            return Invalid;
        }
        public static implicit operator Gender(char input) => input.ToString();
        public static implicit operator string(Gender input) => input.Acronym.ToString();
        public static implicit operator char(Gender input) => input.Acronym;
        public static implicit operator byte? (Gender input) => input.Value;
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            var compareTo = obj as Gender;

            if (compareTo is null) return false;
            if (ReferenceEquals(this, compareTo)) return true;

            return Acronym.Equals(compareTo.Acronym) && InFull.Equals(compareTo.InFull);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1643) + Acronym.GetHashCode() + InFull.GetHashCode();
        }

        public override string ToString()
        {
            return InFull;
        }
        #endregion
    }
}
