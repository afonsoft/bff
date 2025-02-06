using System.Text;

namespace System
{
    public static class ExceptionExtension
    {
        private static void ExplodeInnerException(Exception ex, ref StringBuilder sb)
        {
            if (ex.InnerException == null)
            {
                return;
            }
            sb.AppendLine("InnerException:");
            sb.AppendFormat("Exception: {0}. ", ex.Message);
            sb.AppendFormat("Callstack: {0}.\n", ex.StackTrace);

            ExceptionExtension.ExplodeInnerException(ex.InnerException, ref sb);
        }

        /// <summary>
        /// FormatException
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatException(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            stringBuilder.AppendFormat("\nException: {0}. ", ex.Message);
            stringBuilder.AppendFormat("Callstack: {0}.\n", ex.StackTrace);

            ExceptionExtension.ExplodeInnerException(ex, ref stringBuilder);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string. Ordering
        /// is important. If one of the postFixes is matched, others will not be tested.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (postFixes == null || postFixes.Length <= 0)
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="len"/> is bigger that string's length
        /// </exception>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }
    }
}