using System;
using System.Collections.Generic;

namespace Utils {
    public static class JsonUtils {
        public static string GetString(this Dictionary<string, object> json, string key, string defaultValue = "") {
            if (!json.TryGetValue(key, out var value))
                return defaultValue;
            if (value is string stringValue)
                return stringValue;
            return defaultValue;
        }

        public static long GetLong(this Dictionary<string, object> json, string key, long defaultValue = 0) {
            if (!json.TryGetValue(key, out var value))
                return defaultValue;
            if (value is long longValue)
                return longValue;
            return defaultValue;
        }

        public static int GetInt(this Dictionary<string, object> json, string key, int defaultValue = 0) {
            if (!json.TryGetValue(key, out var value))
                return defaultValue;
            if (value is long longValue)
                return Convert.ToInt32(longValue);
            if (value is int intValue)
                return intValue;
            return defaultValue;
        }
    }
}