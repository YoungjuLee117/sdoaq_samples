using System;
using System.Reflection;

namespace SDOAQNet.Tool
{
    public delegate bool FuncTryParse<T>(string s, out T result);
    public class ParseHelper<T>
    {
        public static readonly FuncTryParse<T> TryParse;

        static ParseHelper()
        {
            var type = typeof(T);

            if (type.IsEnum)
            {
                TryParse = (string s, out T result) =>
                {
                    try
                    {
                        object parsed = Enum.Parse(typeof(T), s, true);
                        result = (T)parsed;
                        return true;
                    }
                    catch
                    {
                        result = default(T);
                        return false;
                    }
                };
                return;
            }
            else if (type == typeof(string))
            {
                TryParse = (string s, out T result) =>
                {
                    result = (T)(object)s;
                    return true;
                };
            }
            else
            {
                var tryParseMethod = type.GetMethod(
                    "TryParse",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { typeof(string), type.MakeByRefType() },
                    null);

                if (tryParseMethod != null)
                {
                    TryParse = (string s, out T result) =>
                    {
                        object[] args = new object[] { s, null };
                        bool success = (bool)tryParseMethod.Invoke(null, args);
                        result = success ? (T)args[1] : default(T);
                        return success;
                    };
                    return;
                }
                else
                {
                    TryParse = (string s, out T result) =>
                    {
                        result = default(T);
                        return false;
                    };
                }
            }
        }
    }
}
