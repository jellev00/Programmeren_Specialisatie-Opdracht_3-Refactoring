using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Objects
{
    public class GridDataSet
    {
        public int Delta { get; set; }
        public int NX { get; set; }
        public int NY { get; set; }
        public List<Tree>[][] GridData { get; set; } 
        public MapBoundary MapBoundary { get; set; }

        public void AddTree(Tree tree)
        {
            if ((tree.x < MapBoundary.MinX) || (tree.x > MapBoundary.MaxX) || (tree.y < MapBoundary.MinY) || (tree.y > MapBoundary.MaxY))
            {
                throw new Exception("Out of bounds");
            }

            int i = (int)((tree.x - MapBoundary.MinX) / Delta);
            int j = (int)((tree.y - MapBoundary.MinY) / Delta);

            // Ensure i and j are within bounds
            i = Math.Max(0, Math.Min(i, NX - 1));
            j = Math.Max(0, Math.Min(j, NY - 1));

            GridData[i][j].Add(tree);
        }

        public GridDataSet(int delta, MapBoundary mapBoundary)
        {
            Delta = delta;
            MapBoundary = mapBoundary;
            NX = (int)(mapBoundary.DeltaX / delta) + 1;
            NY = (int)(mapBoundary.DeltaY / delta) + 1;

            // Initialize the GridData array
            GridData = new List<Tree>[NX][];
            for (int i = 0; i < NX; i++)
            {
                GridData[i] = new List<Tree>[NY];
                for (int j = 0; j < NY; j++)
                {
                    GridData[i][j] = new List<Tree>();
                }
            }
        }

        public GridDataSet(MapBoundary mapBoundary, int delta, List<Tree> data) : this(delta, mapBoundary)
        {
            foreach(Tree tree in data)
            {
                AddTree(tree);
            }
        }

    }
}
