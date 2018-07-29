
namespace RM.Resources.ValueObjects
{
    using RM.Resources.ValueObjects.CustomEnums;
    using RM.Resources.ValueObjects.Extensions;
    using RegexPatterns = Constants.RegexCustomPattern;
    public class ZipCode
    {
        // TODO: ZipCode - Adicionar internacionalização

        private ZipCode(string number)
        {
            if (!string.IsNullOrEmpty(number))
            {
                var cleanNumber = number.
                    Replace(" ", string.Empty).
                    RemoveAllDifferentPattern(RegexPatterns.PatternOnlyNumber);

                if (cleanNumber.Length == 8) // TODO: ZipCode - Tamanho de caracteres fixo, melhorar isso.
                {
                    Number = cleanNumber;
                    NumberWithMask = Number.SetMask(TypeMaskZipCode.Mask_Default);
                }
            }
        }

        public string Number { get; private set; }
        public string NumberWithMask { get; private set; }

        public ZipCode ApplyMask(TypeMaskZipCode typeMask)
        {
            NumberWithMask = Number?.SetMask(typeMask.Value);
            return this;
        }
        public bool IsValid() => !string.IsNullOrEmpty(Number);

        #region Parses
        public static implicit operator ZipCode(string input) => new ZipCode(input);

        public static implicit operator string(ZipCode zipCode) => zipCode.Number;
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            var compareTo = obj as ZipCode;

            if (compareTo is null) return false;
            if (ReferenceEquals(this, compareTo)) return true;

            return Number.Equals(compareTo.Number) && (NumberWithMask.Equals(compareTo.NumberWithMask));
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1683) + Number.GetHashCode() + (NumberWithMask?.GetHashCode() ?? 0);
        }
        public override string ToString()
        {
            return Number;
        }
        #endregion
    }
}
