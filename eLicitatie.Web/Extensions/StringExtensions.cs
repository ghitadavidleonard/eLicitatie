using System;

namespace eLicitatie.Web.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string self) => string.IsNullOrEmpty(self);

        public static bool NotNullAndEmpty(this string self) => !IsNullOrEmpty(self);

        public static bool IsNullOrWhiteSpace(this string self)
        {
            if (self == null)
            {
                return true;
            }

            for (int i = 0; i < self.Length; i++)
            {
                if (!char.IsWhiteSpace(self[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool NotNullAndWhiteSpace(this string self) => !IsNullOrWhiteSpace(self);

        public static bool IsNullEmptyOrWhiteSpace(this string self) =>
            self.IsNullOrEmpty() || self.IsNullOrWhiteSpace();

        public static bool NotNullEmptyAndWhiteSpace(this string self) => !IsNullEmptyOrWhiteSpace(self);

        public static bool NotEquals(this string self, string value, StringComparison comparisonType = StringComparison.CurrentCulture) =>
            !self.Equals(value, comparisonType);
    }
}