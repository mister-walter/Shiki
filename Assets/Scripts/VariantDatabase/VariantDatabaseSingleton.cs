using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki {
    public static class VariantDatabaseSingleton {
        private static VariantDatabase database;

        private static void LoadDatabase() {
            var dbFile = Resources.Load<TextAsset>("VariantDatabase");
            database = new VariantDatabase();
            database.LoadFromString(dbFile.text);
        }

        public static VariantDatabase GetDatabase() {
            if(database == null)
                LoadDatabase();
            return database;
        }
    }
}
