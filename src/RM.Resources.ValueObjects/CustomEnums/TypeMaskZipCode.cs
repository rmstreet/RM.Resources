
namespace RM.Resources.ValueObjects.CustomEnums
{
    public class TypeMaskZipCode
    {
        // TODO: TypeMaskZipCode - Adicionar internacionalização

        private TypeMaskZipCode(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        #region Values
        /// <summary>
        /// #####-###
        /// </summary>
        public static TypeMaskZipCode Mask_Default => new TypeMaskZipCode("#####-###"); 
        #endregion

        #region Operators
        public static bool operator ==(TypeMaskZipCode left, TypeMaskZipCode right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(TypeMaskZipCode left, TypeMaskZipCode right)
        {
            return !(left == right);
        }
        #endregion

        #region Parses
        public static implicit operator TypeMaskZipCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return new TypeMaskZipCode(input);
        }

        public static implicit operator string(TypeMaskZipCode typeMask) => typeMask.ToString();

        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            var compareTo = obj as TypeMaskZipCode;

            if (compareTo is null) return false;
            if (ReferenceEquals(this, compareTo)) return true;

            return Value.Equals(compareTo.Value);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1369) + Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
        #endregion

    }
}

