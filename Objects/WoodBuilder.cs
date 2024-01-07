using EscapeFromTheWoods.MongoDB.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeFromTheWoods
{
    public static class WoodBuilder
    {
        public static Wood GetWood(int size, Map map, string path, MongoDBRepository db)
        {
            Random r = new Random();
            // bomen opslaan met hun ID
            Dictionary<int, Tree> treeDictionary = new Dictionary<int, Tree>();

            // n: Dit zijn de aantal bomen
            int n = 0;

            while (n < size)
            {
                // nieuwe boom aanmaken op een willekeurige positie binnen de mee gegeven map grenzen
                Tree t = new Tree(IDgenerator.GetTreeID(), r.Next(map.xmin, map.xmax), r.Next(map.ymin, map.ymax));

                // kijken of de ID al in de dictionary zit. Zo niet voeg hem toe
                if (!treeDictionary.ContainsKey(t.treeID))
                {
                    treeDictionary.Add(t.treeID, t);
                    n++; // aantal bomen verhogen
                }
            }

            Wood w = new Wood(IDgenerator.GetWoodID(), treeDictionary.Values.ToList(), map, path, db);
            return w;
        }
    }
}
