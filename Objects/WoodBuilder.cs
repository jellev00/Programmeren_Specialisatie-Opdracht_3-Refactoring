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
        //public static Wood GetWood(int size,Map map,string path,DBwriter db)
        //{
        //    Random r = new Random(100);
        //    List<Tree> trees = new List<Tree>();
        //    int n = 0;
        //    while(n<size)
        //    {
        //        Tree t = new Tree(IDgenerator.GetTreeID(),r.Next(map.xmin,map.xmax),r.Next(map.ymin,map.ymax));
        //        if (!trees.Contains(t)) { trees.Add(t); n++; }
        //    }
        //    Wood w = new Wood(IDgenerator.GetWoodID(),trees,map,path,db);
        //    return w;
        //}

        //public static Wood GetWood(int size, Map map, string path, MongoDBRepository db)
        //{
        //    Random r = new Random();
        //    List<Tree> trees = new List<Tree>();
        //    int n = 0;
        //    while (n < size)
        //    {
        //        Tree t = new Tree(IDgenerator.GetTreeID(), r.Next(map.xmin, map.xmax), r.Next(map.ymin, map.ymax));
        //        if (!trees.Contains(t)) { trees.Add(t); n++; }
        //    }
        //    Wood w = new Wood(IDgenerator.GetWoodID(), trees, map, path, db);
        //    return w;
        //}

        //public static Wood GetWood(int size, Map map, string path, MongoDBRepository db)
        //{
        //    Random r = new Random();
        //    Dictionary<int, Tree> treeDictionary = new Dictionary<int, Tree>();
        //    int n = 0;
        //    while (n < size)
        //    {
        //        Tree t = new Tree(IDgenerator.GetTreeID(), r.Next(map.xmin, map.xmax), r.Next(map.ymin, map.ymax));
        //        if (!treeDictionary.ContainsKey(t.treeID))
        //        {
        //            treeDictionary.Add(t.treeID, t);
        //            n++;
        //        }
        //    }

        //    //MapBoundary mB1 = new MapBoundary(map.xmin, map.xmax, map.ymin, map.ymax);
        //    //GridDataSet g1 = new GridDataSet(50, mB1);

        //    Wood w = new Wood(IDgenerator.GetWoodID(), treeDictionary.Values.ToList(), map, path, db);
        //    return w;
        //}

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
