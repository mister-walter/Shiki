using System;
using UnityEngine;

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
        private static readonly String[] AllSeasonsRepeated = { Winter, Spring, Summer, Fall, Winter, Spring, Summer, Fall };
        public static uint Distance(string startSeason, string endSeason) {
            var startIdx = Array.IndexOf(AllSeasonsRepeated, startSeason);
            var endIdx = Array.IndexOf(AllSeasonsRepeated, endSeason, startIdx);
            if(startIdx < 0 || endIdx < 0) {
                throw new ArgumentException(string.Format("one of the given strings is not a season: {0},{1}", startSeason, endSeason));
            }
            return (uint)Math.Abs(endIdx - startIdx);
        }
    }

    public static class LayerManager {
        public static int TeleportAreaLayer {
            get {
                return LayerMask.NameToLayer("TeleportAreas");
            }
        }
        public static int DefaultLayer {
            get {
                return LayerMask.NameToLayer("Default");
            }
        }
    }
}