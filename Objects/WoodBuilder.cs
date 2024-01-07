using EscapeFromTheWoods.MongoDB.Repo;
using EscapeFromTheWoods.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EscapeFromTheWoods
{
    public static class WoodBuilder
    {
        public static Wood GetWood(int size, Map map, string path, MongoDBRepository db)
        {
            Random r = new Random();
            Dictionary<int, Tree> treeDictionary = new Dictionary<int, Tree>();
            int n = 0;

            while (n < size)
            {
                Tree t = new Tree(IDgenerator.GetTreeID(), r.Next(map.xmin, map.xmax), r.Next(map.ymin, map.ymax));

                if (!treeDictionary.ContainsKey(t.treeID))
                {
                    treeDictionary.Add(t.treeID, t);
                    n++;
                }
            }

            Wood w = new Wood(IDgenerator.GetWoodID(), treeDictionary.Values.ToList(), map, path, db);
            return w;
        }
    }
}
