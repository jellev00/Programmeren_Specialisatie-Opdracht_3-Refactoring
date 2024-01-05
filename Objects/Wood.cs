using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using EscapeFromTheWoods.MongoDB.Repo;
using EscapeFromTheWoods.MongoDB.Models;
using EscapeFromTheWoods.Objects;
using DnsClient.Protocol;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using ZstdSharp.Unsafe;

namespace EscapeFromTheWoods
{
    public class Wood
    {
        private const int drawingFactor = 8;
        private string path;
        //private DBwriter db;
        private MongoDBRepository db;
        private Random r = new Random(1);
        public int woodID { get; set; }
        public List<Tree> trees { get; set; }
        public List<Monkey> monkeys { get; private set; }
        private Map map;
        //private GridDataSet gridDataSet;

        private Dictionary<int, List<int>> monkeySteps;
        private List<string> logsMessage;
        private List<MonkeyRecords> recordsMonkeys;

        //private List<MonkeyRecords> records = new List<MonkeyRecords>();

        public Wood(int woodID, List<Tree> trees, Map map, string path, MongoDBRepository db)
        {
            this.woodID = woodID;
            this.trees = trees;
            this.monkeys = new List<Monkey>();
            this.map = map;
            this.path = path;
            this.db = db;
            //this.gridDataSet = g;

            this.monkeySteps = new Dictionary<int, List<int>>();
            this.logsMessage = new List<string>();
            this.recordsMonkeys = new List<MonkeyRecords>();
        }

        //public void ProcessWood()
        //{
        //    WriteWoodToDB();
        //    Escape();
        //}

        //public void PlaceMonkey(string monkeyName, int monkeyID)
        //{
        //    int treeNr;
        //    do
        //    {
        //        treeNr = r.Next(0, trees.Count - 1);
        //    }
        //    while (trees[treeNr].hasMonkey);
        //    Monkey m = new Monkey(monkeyID, monkeyName, trees[treeNr]);
        //    monkeys.Add(m);
        //    trees[treeNr].hasMonkey = true;
        //}

        public void PlaceMonkey(string monkeyName, int monkeyID)
        {
            int treeNr;
            do
            {
                treeNr = r.Next(0, trees.Count - 1);
            }
            while (trees[treeNr].hasMonkey);

            Monkey m = new Monkey(monkeyID, monkeyName, trees[treeNr]);

            // Voeg de aap toe aan de lijst op basis van alfabetische volgorde op naam
            int index = monkeys.BinarySearch(m, new MonkeyNameComparer());
            if (index < 0)
            {
                index = ~index;
            }

            monkeys.Insert(index, m);

            trees[treeNr].hasMonkey = true;
        }

        // Comparer klasse om de alfabetische volgorde te definiëren
        public class MonkeyNameComparer : IComparer<Monkey>
        {
            public int Compare(Monkey x, Monkey y)
            {
                int nameComparison = string.Compare(x.name, y.name, StringComparison.OrdinalIgnoreCase);

                // Als de namen gelijk zijn, behoud de oorspronkelijke volgorde op monkeyID
                return nameComparison == 0 ? x.monkeyID.CompareTo(y.monkeyID) : nameComparison;
            }
        }


        //public async Task EscapeAsync(Map map)
        //{
        //    List<List<Tree>> routes = new List<List<Tree>>();
        //    foreach (Monkey m in monkeys)
        //    {
        //        routes.Add(await EscapeMonkeyAsync(m, map));
        //    }
        //    WriteEscaperoutesToBitmap(routes);
        //}

        public async Task EscapeAsync(Map map)
        {
            List<List<Tree>> routes = new List<List<Tree>>();

            //foreach (Monkey m in monkeys)
            //{
            //    var route = await EscapeMonkeyAsync(m, map);
            //    routes.Add(route);
            //}

            for (int i = 0; i < monkeys.Count; i++)
            {
                var route = await EscapeMonkeyAsync(monkeys[i], map);
                routes.Add(route);
            }

            await WriteEscaperoutesToBitmapAsync(routes);
            await createLogsForFile();
        }

        //public async Task EscapeAsync(Map map)
        //{
        //    List<List<Tree>> routes = new List<List<Tree>>();
        //    var escapeTasks = monkeys.Select(m => EscapeMonkeyAsync(m, map)).ToList();
        //    await Task.WhenAll(escapeTasks).ConfigureAwait(false);
        //    routes.AddRange(escapeTasks.Select(t => t.Result));
        //    await WriteEscaperoutesToBitmapAsync(routes).ConfigureAwait(false);
        //}

        //private async Task writeRouteToDBAsync(Monkey monkey, List<Tree> route)
        //{
        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");

        //    List<MonkeyRecords> records = new List<MonkeyRecords>();
        //    for (int j = 0; j < route.Count; j++)
        //    {
        //        records.Add(new MonkeyRecords
        //        {
        //            MonkeyID = monkey.monkeyID,
        //            MonkeyName = monkey.name,
        //            WoodID = woodID,
        //            Seqnr = j,
        //            TreeID = route[j].treeID,
        //            X = route[j].x,
        //            Y = route[j].y
        //        });
        //    }

        //    await db.InsertMonkeyRecordAsync(records);

        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
        //}

        //private async Task writeRouteToDBAsync(Monkey monkey, List<Tree> route)
        //{
        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");

        //    List<MonkeyRecords> records = new List<MonkeyRecords>();
        //    for (int j = 0; j < route.Count; j++)
        //    {
        //        records.Add(new MonkeyRecords
        //        {
        //            MonkeyID = monkey.monkeyID,
        //            MonkeyName = monkey.name,
        //            WoodID = woodID,
        //            Seqnr = j,
        //            TreeID = route[j].treeID,
        //            X = route[j].x,
        //            Y = route[j].y
        //        });
        //    }

        //    await db.InsertMonkeyRecordAsync(records);

        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
        //}

        private async Task writeRouteToDBAsync(Monkey monkey, List<Tree> route)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");

            if (!monkeySteps.ContainsKey(monkey.monkeyID))
            {
                monkeySteps[monkey.monkeyID] = new List<int>();
            }

            List<MonkeyRecords> records = new List<MonkeyRecords>();
            List<Logs> logs = new List<Logs>();
            for (int j = 0; j < route.Count; j++)
            {
                MonkeyRecords monkeyRecords = new MonkeyRecords
                {
                    MonkeyID = monkey.monkeyID,
                    MonkeyName = monkey.name,
                    WoodID = woodID,
                    Seqnr = j,
                    TreeID = route[j].treeID,
                    X = route[j].x,
                    Y = route[j].y
                };

                records.Add(monkeyRecords);
                recordsMonkeys.Add(monkeyRecords);

                logs.Add(new Logs
                {
                    WoodID = woodID,
                    MonkeyID = monkey.monkeyID,
                    Message = $"{monkey.name} is now in tree {route[j].treeID} at location ({route[j].x},{route[j].y})"
                });

                monkeySteps[monkey.monkeyID].Add(j);
            }

            await db.InsertMonkeyRecordAsync(records);
            await db.InsertLogAsync(logs);

            //Voeg de hele lijst records toe aan monkeySteps[monkey.monkeyID]
            if (monkeySteps.ContainsKey(monkey.monkeyID))
            {
                monkeySteps[monkey.monkeyID].AddRange(records.Select(record => record.Seqnr));
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
        }

        //private async Task createLogsForFile()
        //{
        //    for (int i = 0; i < monkeySteps.Count() + 1; i++)
        //    {
        //        foreach (var step in monkeySteps)
        //        {
        //            if (step.Value == i)
        //            {
        //                foreach (var monkey in monkeys)
        //                {
        //                    logsMessage.Add($"{monkey.name} is now in tree {monkey.tree.treeID} at location ({monkey.tree.x},{monkey.tree.y})");
        //                }
        //            }
        //        }
        //    }

        //    await writeLogsToFile(logsMessage);
        //}

        //private async Task createLogsForFile()
        //{
        //    foreach (var keyValue in monkeySteps)
        //    {
        //        var monkeyID = keyValue.Key;
        //        var stepList = keyValue.Value;

        //        foreach (var step in stepList)
        //        {
        //            for (int i = 0; i < step + 1; i++)
        //            {
        //                foreach (var monkey in monkeys)
        //                {
        //                    logsMessage.Add($"{monkey.name} is now in tree {monkey.tree.treeID} at location ({monkey.tree.x},{monkey.tree.y})");
        //                }
        //            }
        //        }
        //    }

        //    await writeLogsToFile(logsMessage);
        //}

        //private async Task createLogsForFile()
        //{
        //    int aantalSteps = 0;
        //    int maxSteps = 0;
        //    foreach (var keyValue in monkeySteps)
        //    {
        //        int steps = keyValue.Value.Count();

        //        if (steps > maxSteps)
        //        {
        //            maxSteps = steps;
        //        }

        //        aantalSteps += steps;
        //    }

        //    int i = 0;
        //    do
        //    {
        //        for (int j = 0; j < maxSteps + 1; j++)
        //        {
        //            foreach (var keyValue in monkeySteps)
        //            {
        //                var monkeyID = keyValue.Key;
        //                var stepList = keyValue.Value;

        //                foreach (var step in stepList)
        //                {
        //                    foreach (var monkey in monkeys)
        //                    {
        //                        if (monkey.monkeyID == monkeyID && step == j)
        //                        {
        //                            var record = recordsMonkeys.FirstOrDefault(r => r.MonkeyID == monkeyID && r.Seqnr == step);
        //                            logsMessage.Add($"{monkey.name} is now in tree {record.TreeID} at location ({record.X},{record.Y})");
        //                            i++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    while (i < aantalSteps);

        //    await writeLogsToFile(logsMessage);
        //}

        private async Task createLogsForFile()
        {
            int maxSteps = monkeySteps.Values.Max(list => list.Count);
            int i = 0;

            for (int j = 0; j < maxSteps; j++)
            {
                foreach (var keyValue in monkeySteps)
                {
                    var monkeyID = keyValue.Key;
                    var stepList = keyValue.Value;

                    if (j < stepList.Count)
                    {
                        var step = stepList[j];
                        var monkey = monkeys.FirstOrDefault(m => m.monkeyID == monkeyID);

                        if (monkey != null)
                        {
                            var record = recordsMonkeys.FirstOrDefault(r => r.MonkeyID == monkeyID && r.Seqnr == step);

                            if (record != null)
                            {
                                logsMessage.Add($"{monkey.name} is now in tree {record.TreeID} at location ({record.X},{record.Y})");
                                i++;
                            }
                        }
                    }
                }
            }

            await writeLogsToFile(logsMessage);
        }


        //private async Task createLogsForFile()
        //{
        //    int aantalSteps = 0;
        //    int maxSteps = 0;

        //    foreach (var steps in monkeySteps.Values)
        //    {
        //        int count = steps.Count;
        //        aantalSteps += count;

        //        if (count > maxSteps)
        //        {
        //            maxSteps = count;
        //        }
        //    }

        //    int i = 0;

        //    // Gebruik een HashSet voor snelle controle
        //    HashSet<int> monkeyIDs = new HashSet<int>(monkeys.Select(m => m.monkeyID));

        //    for (int j = 0; j < maxSteps + 1; j++)
        //    {
        //        foreach (var keyValue in monkeySteps)
        //        {
        //            var monkeyID = keyValue.Key;
        //            var stepList = keyValue.Value;

        //            foreach (var step in stepList)
        //            {
        //                // Controleer of monkeyID in de lijst zit voordat je de monkey zoekt
        //                if (monkeyIDs.Contains(monkeyID) && monkeys.Any(m => m.monkeyID == monkeyID && step == j))
        //                {
        //                    logsMessage.Add($"{monkeys.First(m => m.monkeyID == monkeyID).name} is now in tree {monkeys.First(m => m.monkeyID == monkeyID).tree.treeID} at location ({monkeys.First(m => m.monkeyID == monkeyID).tree.x},{monkeys.First(m => m.monkeyID == monkeyID).tree.y})");
        //                    i++;
        //                }
        //            }
        //        }
        //    }

        //    await writeLogsToFile(logsMessage);
        //}


        private async Task writeLogsToFile(List<string> log)
        {

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{woodID}:write logs to file {woodID} start");

            // Write logs to a text file
            string logFilePath = Path.Combine(path, $"Logs_{woodID}.txt");
            File.WriteAllLines(logFilePath, log);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{woodID}:write logs to file {woodID} end");
        }


        //private void writeLogsToFile(List<Logs> log)
        //{
        //    db.InsertLog(log);

        //    Console.ForegroundColor = ConsoleColor.Magenta;
        //    Console.WriteLine($"{woodID}:write logs to file {woodID} start");

        //    List<string> logsString = new List<string>();
        //    for (int l = 0; l < log.Count; l++)
        //    {
        //        string message = $"{log[l].Message}";
        //        logsString.Add(message);
        //    }

        //    // Sort logs by monkey name and time
        //    logsString.Sort();

        //    // Write logs to a text file
        //    string logFilePath = Path.Combine(path, $"Logs_{woodID}.txt");
        //    File.WriteAllLines(logFilePath, logsString);

        //    Console.ForegroundColor = ConsoleColor.Magenta;
        //    Console.WriteLine($"{woodID}:write logs to file {woodID} end");

        //    logs.Clear();
        //}

        //public void WriteEscaperoutesToBitmap(List<List<Tree>> routes)
        //{
        //    Console.ForegroundColor = ConsoleColor.Yellow;
        //    Console.WriteLine($"{woodID}:write bitmap routes {woodID} start");
        //    Color[] cvalues = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Cyan, Color.GreenYellow };
        //    Bitmap bm = new Bitmap((map.xmax - map.xmin) * drawingFactor, (map.ymax - map.ymin) * drawingFactor);
        //    Graphics g = Graphics.FromImage(bm);
        //    int delta = drawingFactor / 2;
        //    Pen p = new Pen(Color.Green, 1);
        //    foreach (Tree t in trees)
        //    {
        //        g.DrawEllipse(p, t.x * drawingFactor, t.y * drawingFactor, drawingFactor, drawingFactor);
        //    }
        //    int colorN = 0;
        //    foreach (List<Tree> route in routes)
        //    {
        //        int p1x = route[0].x * drawingFactor + delta;
        //        int p1y = route[0].y * drawingFactor + delta;
        //        Color color = cvalues[colorN % cvalues.Length];
        //        Pen pen = new Pen(color, 1);
        //        g.DrawEllipse(pen, p1x - delta, p1y - delta, drawingFactor, drawingFactor);
        //        g.FillEllipse(new SolidBrush(color), p1x - delta, p1y - delta, drawingFactor, drawingFactor);
        //        for (int i = 1; i < route.Count; i++)
        //        {
        //            g.DrawLine(pen, p1x, p1y, route[i].x * drawingFactor + delta, route[i].y * drawingFactor + delta);
        //            p1x = route[i].x * drawingFactor + delta;
        //            p1y = route[i].y * drawingFactor + delta;
        //        }
        //        colorN++;
        //    }
        //    bm.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);
        //    Console.ForegroundColor = ConsoleColor.Yellow;
        //    Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");
        //}

        private async Task WriteEscaperoutesToBitmapAsync(List<List<Tree>> routes)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} start");
            Color[] cvalues = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Cyan, Color.GreenYellow };
            Bitmap bm = new Bitmap((map.xmax - map.xmin) * drawingFactor, (map.ymax - map.ymin) * drawingFactor);
            Graphics g = Graphics.FromImage(bm);
            int delta = drawingFactor / 2;
            Pen p = new Pen(Color.Green, 1);
            foreach (Tree t in trees)
            {
                g.DrawEllipse(p, t.x * drawingFactor, t.y * drawingFactor, drawingFactor, drawingFactor);
            }
            int colorN = 0;
            foreach (List<Tree> route in routes)
            {
                int p1x = route[0].x * drawingFactor + delta;
                int p1y = route[0].y * drawingFactor + delta;
                Color color = cvalues[colorN % cvalues.Length];
                Pen pen = new Pen(color, 1);
                g.DrawEllipse(pen, p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                g.FillEllipse(new SolidBrush(color), p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                for (int i = 1; i < route.Count; i++)
                {
                    g.DrawLine(pen, p1x, p1y, route[i].x * drawingFactor + delta, route[i].y * drawingFactor + delta);
                    p1x = route[i].x * drawingFactor + delta;
                    p1y = route[i].y * drawingFactor + delta;
                }
                colorN++;
            }
            //bm.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");


            await Task.Run(() =>
            {
                bm.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);
            }).ConfigureAwait(false);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");
        }

        //public async Task WriteWoodToDBAsync()
        //{
        //    Console.ForegroundColor = ConsoleColor.Green;
        //    Console.WriteLine($"{woodID}:write db wood {woodID} start");
        //    List<WoodRecords> records = new List<WoodRecords>();
        //    foreach (Tree t in trees)
        //    {
        //        records.Add(new WoodRecords
        //        {
        //            WoodID = woodID,
        //            TreeID = t.treeID,
        //            X = t.x,
        //            Y = t.y
        //        });
        //    }

        //    await db.InsertWoodRecordAsync(records);

        //    Console.ForegroundColor = ConsoleColor.Green;
        //    Console.WriteLine($"{woodID}:write db wood {woodID} end");
        //}

        public async Task WriteWoodToDBAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{woodID}:write db wood {woodID} start");

            List<WoodRecords> records = new List<WoodRecords>();

            foreach (Tree t in trees)
            {
                records.Add(new WoodRecords
                {
                    WoodID = woodID,
                    TreeID = t.treeID,
                    X = t.x,
                    Y = t.y
                });
            }

            await db.InsertWoodRecordAsync(records);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{woodID}:write db wood {woodID} end");
        }

        public async Task<List<Tree>> EscapeMonkeyAsync(Monkey monkey, Map m)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{woodID}:start {woodID},{monkey.name}");

            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            trees.ForEach(x => visited.Add(x.treeID, false));

            List<Tree> route = new List<Tree>() { monkey.tree };


            MapBoundary mB = new MapBoundary(m.xmin, m.xmax, m.ymin, m.ymax);

            GridDataSet g2 = new GridDataSet(mB, 10, trees);

            //GridDataSet g = new GridDataSet(10, mB);

            int n = 15;

            do
            {
                visited[monkey.tree.treeID] = true;
                SortedList<double, List<Tree>> distanceToMonkey = new SortedList<double, List<Tree>>();
                (int i, int j) = FindCell(monkey.tree.x, monkey.tree.y, g2);
                ProcessCell(distanceToMonkey, monkey, i, j, n, g2, visited);

                int ring = 0;
                while (distanceToMonkey.Count < n)
                {
                    ring++;
                    ProcessRing(i, j, ring, distanceToMonkey, monkey, n, g2, visited);
                }

                ProcessRing(i, j, ring + 1, distanceToMonkey, monkey, n, g2, visited);

                // Search for the closest tree that hasn't been visited and doesn't have a monkey
                //foreach (Tree t in trees)
                //{
                //    if ((!visited[t.treeID]) && (!t.hasMonkey))
                //    {
                //        double d = Math.Sqrt(Math.Pow(t.x - monkey.tree.x, 2) + Math.Pow(t.y - monkey.tree.y, 2));
                //        if (distanceToMonkey.ContainsKey(d)) distanceToMonkey[d].Add(t);
                //        else distanceToMonkey.Add(d, new List<Tree>() { t });
                //    }
                //}

                // Calculate the distance to the map border (north, east, south, west)
                double distanceToBorder = (new List<double>()
                {
                    m.ymax - monkey.tree.y,
                    m.xmax - monkey.tree.x,
                    monkey.tree.y - m.ymin,
                    monkey.tree.x - m.xmin
                }).Min();

                // Check if there are no available trees or if the closest tree is closer to the border
                if (distanceToMonkey.Count == 0 || distanceToBorder < distanceToMonkey.First().Key)
                {
                    await writeRouteToDBAsync(monkey, route);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                    return route;
                }

                // Add the closest tree to the route
                route.Add(distanceToMonkey.First().Value.First());
                monkey.tree = distanceToMonkey.First().Value.First();
            }
            while (true);

            //return route;
        }

        private (int, int) FindCell(int x, int y, GridDataSet g)
        {
            if (!g.MapBoundary.WithinBounds(x, y))
            {
                throw new Exception("FindCell - out of bounds");
            }
            int i = (int)((x - g.MapBoundary.MinX) / g.Delta);
            int j = (int)((y - g.MapBoundary.MinY) / g.Delta);
            if (i == g.NX)
            {
                i--;
            }
            if (j == g.NY)
            {
                j--;
            }
            return (i, j);
        }

        private void ProcessCell(SortedList<double, List<Tree>> nn, Monkey m, int i, int j, int n, GridDataSet g, Dictionary<int, bool> v)
        {
            foreach (Tree t in g.GridData[i][j])
            {
                if (v[t.treeID] == false)
                {
                    double dsquare = Math.Sqrt(Math.Pow(t.x - m.tree.x, 2) + Math.Pow(t.y - m.tree.y, 2));
                    if ((nn.Count < n) || (dsquare < nn.Keys[nn.Count - 1]))
                    {
                        if (nn.ContainsKey(dsquare))
                        {
                            nn[dsquare].Add(t);
                        }
                        else
                        {
                            nn.Add(dsquare, new List<Tree>() { t });
                        }
                    }
                }
            }
        }


        private void ProcessRing(int i, int j, int ring, SortedList<double, List<Tree>> nn, Monkey m, int n, GridDataSet g, Dictionary<int, bool> v)
        {
            for (int gx = i - ring; gx <= j; gx++)
            {
                // onderste rij
                int gy = j - ring;
                if (IsValidCell(gx, gy, g))
                {
                    ProcessCell(nn, m, gx, gy, n, g, v);
                }

                // bovenste rij
                gy = j + ring;
                if (IsValidCell(gx, gy, g))
                {
                    ProcessCell(nn, m, gx, gy, n, g, v);
                }
            }

            for (int gy = j - ring + 1; gy <= j + ring - 1; gy++)
            {
                // linker kolom
                int gx = i - ring;
                if (IsValidCell(gx, gy, g))
                {
                    ProcessCell(nn, m, gx, gy, n, g, v);
                }

                // rechter kolom
                gx = i + ring;
                if (IsValidCell(gx, gy, g))
                {
                    ProcessCell(nn, m, gx, gy, n, g, v);
                }
            }
        }

        private bool IsValidCell(int i, int j, GridDataSet g)
        {
            if ((i < 0) || (i >= g.NX))
            {
                return false;
            }
            if ((j < 0) || (j >= g.NY))
            {
                return false;
            }
            return true;
        }
    }
}
