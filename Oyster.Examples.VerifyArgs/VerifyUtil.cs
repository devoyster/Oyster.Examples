using System;

namespace Oyster.Examples.VerifyArgs
{
    public static class VerifyUtil
    {
        public static void NotNull(object arg, string name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotEmpty(object arg, string name)
        {
            if (arg is string && ((string) arg).Length == 0)
            {
                throw new ArgumentException("Value can't be empty.", name);
            }
        }

        public static void NotNullOrEmpty(object arg, string name)
        {
            NotNull(arg, name);
            NotEmpty(arg, name);
        }
    }
}
