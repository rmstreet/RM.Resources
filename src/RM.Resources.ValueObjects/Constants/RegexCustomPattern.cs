
namespace RM.Resources.ValueObjects.Constants
{
    public static class RegexCustomPattern
    {
        /// <summary>
        /// Pattern regex que dá match em tudo que não é número, exceto +.
        /// </summary>
        public static string PatternOnlyNumber => @"[a-zA-Z \-éúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?";

        /// <summary>
        /// Pattern regex que dá match ao encontrar +55.
        /// </summary>
        public static string PatternRemovePlus55 => @"^(?:\+)[0-9]{2}";

        /// <summary>
        /// Pattern regex que remove a o zero se ele for o primeiro caracter.
        /// </summary>
        public static string PatternRemoveFirstZero => @"^0+(?=\d)";
    }
}
