
namespace RM.Resources.ValueObjects.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Método que remover toda ocorrência que dá match no pattern informado. 
        /// </summary>
        /// <param name="text">String onde será aplicado.</param>
        /// <param name="pattern">Pattern regex que será usado para match de remoção.</param>
        /// <returns>String com a remoção das ocorrências que deram match.</returns>
        public static string RemoveAllDifferentPattern(this string text, string pattern)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, pattern, "");
        }

        /// <summary>
        /// Método que aplica uma máscara em uma string. Ex.: (##) # ####-####; 
        /// Obs: Se a string for maior que a máscara, o valor será cortado.
        /// </summary> 
        /// <param name="str">String onde será aplicado</param>
        /// <param name="mask">Máscara a ser utilizada. Utilize # onde deseja que o sejá substituido pelo elemento da string.</param>
        /// <returns>String com a mascará aplicada.</returns>
        public static string SetMask(this string str, string mask)
        {
            var sb = new System.Text.StringBuilder();

            var indexStr = 0;

            for (int i = 0; i < mask.Length; i++)
            {
                if ('#'.Equals(mask[i]) && indexStr < str.Length)
                {
                    sb.Append(str[indexStr]);
                    indexStr += 1;
                }
                else
                    sb.Append(mask[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Método pega apenas o que estiver dando match no pattern.
        /// </summary>
        /// <param name="text">String onde será aplicado</param>
        /// <param name="pattern">Pattern regex que será usado para match.</param>
        /// <returns>String apenas com o que deu match.</returns>
        public static string TakeAllMatchPattern(this string text, string pattern)
        {
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (System.Text.RegularExpressions.Match m in regex.Matches(text))
            {
                sb.Append(m.Value);
            }
            return sb.ToString();
        }

    }
}
