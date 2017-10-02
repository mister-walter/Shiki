using System;

/// <summary>
/// Constants used within Shiki
/// </summary>
namespace Shiki.Constants {
    /// <summary>
    /// Contains the name of each scene.
    /// </summary>
    public static class SceneName {
        public const String Summer = "SummerScene";
        public const String Winter = "WinterScene";
        public const String Spring = "SpringScene";
        public const String Fall = "FallScene";
        public const String Village = "VillageScene";
        public const String Main = "MainScene";
    }

    /// <summary>
    /// Contains the name of each season.
    /// </summary>
    public static class SeasonName {
        public const String Summer = "Summer";
        public const String Winter = "Winter";
        public const String Spring = "Spring";
        public const String Fall = "Fall";
        public const String None = "None";
        public static readonly String[] AllSeasons = { Winter, Spring, Summer, Fall };
        public static uint Distance(string season1, string season2) {
            var idx1 = Array.IndexOf(AllSeasons, season1);
            var idx2 = Array.IndexOf(AllSeasons, season2);
            if(idx1 < 0 || idx2 < 0) {
                throw new ArgumentException(string.Format("one of the given strings is not a season: {0},{1}", season1, season2));
            }
            return (uint)Math.Abs(idx1 - idx2);
        }
    }
}