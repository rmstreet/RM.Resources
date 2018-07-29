
namespace RM.Resources.ValueObjects.CustomEnums
{
    public class TypeMaskPhone
    {
        // TODO: TypeMaskPhone - Adicionar internacionalização

        private TypeMaskPhone(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        #region Values
        /// <summary>
        /// (##) # ####-####
        /// </summary>
        public static TypeMaskPhone Mask1_DD_NineDigits => new TypeMaskPhone("(##) # ####-####"); 
     

        /// <summary>
        /// (##) #####-####
        /// </summary>
        public static TypeMaskPhone Mask2_DD_NineDigits => new TypeMaskPhone("(##) #####-####");

        /// <summary>
        /// (##) ####-####
        /// </summary>
        public static TypeMaskPhone Mask1_DD_EightDigits => new TypeMaskPhone("(##) ####-####");
        #endregion

        #region Operators
        public static bool operator ==(TypeMaskPhone left, TypeMaskPhone right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(TypeMaskPhone left, TypeMaskPhone right)
        {
            return !(left == right);
        }
        #endregion

        #region Parses
        public static implicit operator TypeMaskPhone(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return new TypeMaskPhone(input);
        }

        public static implicit operator string(TypeMaskPhone typeMask) => typeMask.ToString();
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            var compareTo = obj as TypeMaskPhone;

            if (compareTo is null) return false;
            if (ReferenceEquals(this, compareTo)) return true;

            return Value.Equals(compareTo.Value);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1373) + Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
        #endregion

    }
}
