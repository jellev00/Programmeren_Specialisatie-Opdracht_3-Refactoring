using System;
using System.Collections.Generic;

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
            // kijken of de boom binnen de grenzen zit van de map
            if ((tree.x < MapBoundary.MinX) || (tree.x > MapBoundary.MaxX) || (tree.y < MapBoundary.MinY) || (tree.y > MapBoundary.MaxY))
            {
                throw new Exception("Out of bounds");
            }

            // Bepaal de i en j indexwaarde van het roostergebied waarin de boom valt.
            int i = (int)((tree.x - MapBoundary.MinX) / Delta);
            int j = (int)((tree.y - MapBoundary.MinY) / Delta);

            // kijken of i & j binnen de grenzen zitten
            i = Math.Max(0, Math.Min(i, NX - 1));
            j = Math.Max(0, Math.Min(j, NY - 1));

            // boom toevoegen aan de rooster
            GridData[i][j].Add(tree);
        }

        public GridDataSet(int delta, MapBoundary mapBoundary)
        {
            // grote van elke rooster
            Delta = delta;
            MapBoundary = mapBoundary;

            // de aantal roosters langs de x & y as bereken aandehand van de map granzen en delta
            NX = (int)(mapBoundary.DeltaX / delta) + 1;
            NY = (int)(mapBoundary.DeltaY / delta) + 1;

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
            // uit de lijst van bomen die mee gegeven wordt de bomen toevoegen aan de rooster
            foreach (Tree tree in data)
            {
                AddTree(tree);
            }
        }
    }
}
