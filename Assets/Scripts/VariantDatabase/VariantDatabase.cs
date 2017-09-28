using Nett;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shiki {

    internal class Variant {
        public string prefab { get; set; }
    }

    internal class Item {
        public Dictionary<string, Variant> variant { get; set; }
        public string prefab { get; set; }
        public string name { get; set; }

        public Item() {
            this.variant = new Dictionary<string, Variant>();
        }

        public string PrefabForSeason(string season) {
            Variant seasonVariant;
            // See if there's a variant for the given season
            if(this.variant.TryGetValue(season, out seasonVariant)) {
                // If there is, use its prefab
                return seasonVariant.prefab;
            }
            // Otherwise use the default.
            return this.prefab;
        }
    }

    internal class Root {
        public List<Item> item { get; set; }
    }

    public class VariantDatabase {
        internal Dictionary<string, Item> items;

        public VariantDatabase() {
            this.items = new Dictionary<string, Item>();
        }

        public void Load(string path) {
            Root root = Toml.ReadFile<Root>(path);
            foreach(var item in root.item) {
                this.items.Add(item.name, item);
            }
        }

        public void LoadFromString(string content) {
            Root root = Toml.ReadString<Root>(content);
            foreach(var item in root.item) {
                this.items.Add(item.name, item);
            }
        }

        private GameObject InstantiateItem(Item item, string season) {
            var prefabName = item.PrefabForSeason(season);
            if(String.IsNullOrEmpty(prefabName)) {
                throw new Exception(string.Format("No prefab provided for item {0} in season {1}", item.name, season));
            }
            return Shiki.Loader.LoadPrefabInstance(prefabName);
        }

        public GameObject LoadItem(string itemName, string season) {
            Item item;
            if(!this.items.TryGetValue(itemName, out item)) {
                throw new Exception("Item not found: " + itemName);
            }
            return this.InstantiateItem(item, season);
        }

        public string GetPrefabName(string itemName, string season) {
            Item item;
            if(!this.items.TryGetValue(itemName, out item)) {
                throw new Exception("Item not found: " + itemName);
            }
            return item.PrefabForSeason(season);
        }

        public int ItemCount() {
            return this.items.Count;
        }
    }
}