
namespace RM.Resources.ValueObjects
{
    using RM.Resources.ValueObjects.CustomEnums;
    using RM.Resources.ValueObjects.Extensions;
    using RegexPatterns = Constants.RegexCustomPattern;

    public class Phone
    {
        // TODO: Phone - Adicionar internacionalização

        private Phone(string number)
        {
            if (!string.IsNullOrEmpty(number))
            {
                var cleanNumber = number.
                    Replace(" ", string.Empty).
                    RemoveAllDifferentPattern(RegexPatterns.PatternOnlyNumber).
                    RemoveAllDifferentPattern(RegexPatterns.PatternRemovePlus55).
                    RemoveAllDifferentPattern(RegexPatterns.PatternRemoveFirstZero);

                if (cleanNumber.Length == 10 || cleanNumber.Length == 11) // TODO: Phone - Tamanho de caracteres fixo, melhorar isso.
                {
                    Number = cleanNumber;
                    NumberWithMask = Number.SetMask(TypeMaskPhone.Mask2_DD_NineDigits);
                }
            }

        }

        public string Number { get; private set; }
        public string NumberWithMask { get; private set; }

        public Phone ApplyMask(TypeMaskPhone typeMask)
        {
            NumberWithMask = Number?.SetMask(typeMask.Value);
            return this;
        }

        public bool IsValid() => !string.IsNullOrEmpty(Number);

        #region Parses
        public static implicit operator Phone(string input) => new Phone(input);
        public static implicit operator string(Phone phone) => phone?.Number ?? default(string);
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            var compareTo = obj as Phone;

            if (compareTo is null) return false;
            if (ReferenceEquals(this, compareTo)) return true;

            return Number.Equals(compareTo.Number) && (NumberWithMask?.Equals(compareTo.NumberWithMask) ?? compareTo.NumberWithMask == null);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1621) + Number.GetHashCode() + (NumberWithMask?.GetHashCode() ?? 0);
        }
        public override string ToString()
        {
            return Number;
        }
        #endregion
    }
}
