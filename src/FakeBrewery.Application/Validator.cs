using System;

namespace FakeBrewery.Application
{
    internal static class Validator
    {
        public static bool IsNull(object value)
        {
            return value == null;
        }

        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsGreaterOrEqualThanZero(double value)
        {
            return value >= 0;
        }

        public static bool IsGreaterThanZero(double value)
        {
            return value > 0;
        }

        public static bool IsGreaterOrEqualThanZero(int value)
        {
            return value >= 0;
        }

        public static bool IsGreaterThanZero(int value)
        {
            return value > 0;
        }

        public static bool IsEmptyGuid(Guid value)
        {
            return value == Guid.Empty;
        }
    }
}