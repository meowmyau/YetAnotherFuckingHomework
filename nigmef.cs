namespace MEOW.nigmef2
{
    public class nigmef
    {
        public static void Run()
        {
            Q1Test();
        }

        private static void Q1Test()
        {
            Graph graph;

            Console.WriteLine("Выберите тип графа:");
            Console.WriteLine("1. Ориентированный");
            Console.WriteLine("2. Неориентированный");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    graph = new(true);
                    break;
                case "2":
                    graph = new (); // Создаем неориентированный граф
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Используется стандартный неориентированный граф.");
                    graph = new (); // По умолчанию используется неориентированный граф
                    break;
            }
                
            //Для теста, чтоб ручками не писать
            graph.AddEdge("A", "B");    //A - B   E
            graph.AddEdge("A", "C");    //|       |
            graph.AddVertex("E");       //C       F
            graph.AddEdge("E", "F");
            
            while (true)
            {
                Console.WriteLine("1. Добавить вершину");
                Console.WriteLine("2. Добавить ребро");
                Console.WriteLine("3. Удалить вершину");
                Console.WriteLine("4. Удалить ребро");
                Console.WriteLine("5. Вывести граф");
                Console.WriteLine("6. Сохранить граф в файл");
                Console.WriteLine("7. Загрузить граф из файла");
                Console.WriteLine("8. Выйти");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Введите имя вершину: ");
                        graph.AddVertex(Console.ReadLine());
                        break;
                    case "2":
                        Console.Write("Введите начальную вершину: ");
                        string from = Console.ReadLine();
                        Console.Write("Введите конечную вершину: ");
                        string to = Console.ReadLine();
                        Console.Write("Введите вес (по стандарту 1): ");
                        int weight = int.TryParse(Console.ReadLine(), out int parsedWeight) ? parsedWeight : 1;
                        graph.AddEdge(from, to, weight);
                        break;
                    case "3":
                        Console.Write("Введите имя вершины: ");
                        graph.RemoveVertex(Console.ReadLine());
                        break;
                    case "4":
                        Console.Write("Введите начальную вершину: ");
                        from = Console.ReadLine();
                        Console.Write("Введите конечную вершину: ");
                        to = Console.ReadLine();
                        graph.RemoveEdge(from, to);
                        break;
                    case "5":
                        graph.Print();
                        break;
                    case "6":
                        Console.Write("Введите имя файла: ");
                        graph.SaveToFile(Console.ReadLine() + ".txt");
                        break;
                    case "7":
                        Console.Write("Введите имя файла: ");
                        graph.LoadFromFile(Console.ReadLine());
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Ошибка, попробуй еще раз");
                        break;
                }
            }
        }

        private static void Q2Test()
        {
            //BuildFullGraph
            Graph graph = new Graph();
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");

            Graph fullGraph = Graph.BuildFullGraph(graph);
            // fullGraph содержит рёбра: A-B, A-C, B-C
            Console.WriteLine("BuildFullGraph");
            fullGraph.Print();

            //BuildComplementGraph
            Graph graph2 = new Graph();
            graph2.AddVertex("A");
            graph2.AddVertex("B");
            graph2.AddVertex("C");
            Graph complementGraph = Graph.BuildComplementGraph(graph2);
            // Если предположить, что у нас есть только три вершины: A, B и C, 
            // то complementGraph содержит рёбра: A-C и B-C
            Console.WriteLine("BuildComplementGraph");
            complementGraph.Print();

            //Union
            Graph g1 = new Graph();
            g1.AddEdge("A", "B");
            Graph g2 = new Graph();
            g2.AddEdge("B", "C");

            Graph unionGraph = Graph.Union(g1, g2);
            // unionGraph содержит рёбра: A-B, B-C
            Console.WriteLine("Union");
            unionGraph.Print();
            
            //Join
            Graph g3 = new Graph();
            g3.AddVertex("A");
            Graph g4 = new Graph();
            g4.AddVertex("B");
            Graph joinGraph = Graph.Join(g3, g4);
            // joinGraph содержит ребро: A-B
            Console.WriteLine("Join");
            joinGraph.Print();
        }

        private static void Q3Test()
        {
            Graph tree = new Graph(true);   
                                        //A - B
            tree.AddEdge("A", "B");     //|   
            tree.AddEdge("A", "C");     //C

            Graph cyclicGraph = new Graph(true);
            cyclicGraph.AddEdge("A", "B");   //A - B
            cyclicGraph.AddEdge("A", "C");   //|   |
            cyclicGraph.AddEdge("B", "D");   //C - D
            cyclicGraph.AddEdge("C", "D");
            
            Graph forest = new Graph(true);
            forest.AddEdge("A", "B");    //A - B   E
            forest.AddEdge("A", "C");    //|       |
            forest.AddVertex("E");       //C       F
            forest.AddEdge("E", "F");

            Console.WriteLine(tree.CheckGraphType());
            Console.WriteLine(cyclicGraph.CheckGraphType());
            Console.WriteLine(forest.CheckGraphType());
            
            // Создание ориентированного графа
            Graph graph = new Graph(directed: true);
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("C", "A");
            graph.AddEdge("D", "E");
            graph.AddEdge("E", "F");
            graph.AddEdge("F", "D");

            Console.WriteLine("Оригинальный граф:");
            graph.Print();

            int sccCount = graph.CountStronglyConnectedComponents();
            Console.WriteLine($"\nЧисло сильно связанных компонентов: {sccCount}"); //2
        }

        private static void Q4Test()
        {
            Graph graph = new Graph();
            graph.AddEdge("A", "B", 1);
            graph.AddEdge("A", "C", 3);
            graph.AddEdge("A", "D", 2);
            graph.AddEdge("C", "D", 4);

            Console.WriteLine("Оригинальный граф:");
            graph.Print();

            Graph mst = graph.KruskalMST();
            Console.WriteLine("\nМинимальное остовное дерево:");
            mst.Print();
        }
    }

    // Основной класс для представления графа.
    public class Graph
    {
        #region Задача 1

        private bool _directed; // Флаг, указывающий, является ли граф ориентированным.
        private Dictionary<string, List<(string, int)>> _adjacencyList; // Список смежности.

        // Конструктор для создания пустого графа.
        public Graph(bool directed = false)
        {
            _directed = directed;
            _adjacencyList = new Dictionary<string, List<(string, int)>>();
        }

        // Конструктор для создания графа из файла.
        public Graph(string fileName)
        {
            LoadFromFile(fileName);
        }

        public void LoadFromFile(string fileName)
        {
            _adjacencyList = new Dictionary<string, List<(string, int)>>();
            // Чтение каждой строки из файла и добавление рёбер в граф.
            var lines = File.ReadAllLines(fileName + ".txt");
            for (int j = 0; j < lines.Length; ++j)
            {
                if (j == 0)
                {
                    _directed = Boolean.Parse(lines[j]);
                    Console.WriteLine(_directed);
                    continue;
                }

                var parts = lines[j].Split(' ');
                var vertex = parts[0];

                if (parts.Length < 2)
                {
                    AddVertex(vertex);
                    continue;
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    var adjacent = parts[i].Split(':');
                    AddEdge(vertex, adjacent[0], int.Parse(adjacent[1]));
                }
            }
        }

        // Конструктор копирования.
        public Graph(Graph other)
        {
            _directed = other._directed;
            _adjacencyList = new Dictionary<string, List<(string, int)>>(other._adjacencyList);
        }

        // Метод добавления вершины.
        public void AddVertex(string vertex)
        {
            // Добавляется вершина только если она еще не присутствует в графе.
            if (_adjacencyList.ContainsKey(vertex))
                return;
            _adjacencyList[vertex] = new List<(string, int)>();
        }

        // Метод добавления ребра.
        public void AddEdge(string from, string to, int weight = 1)
        {
            AddVertex(from);
            AddVertex(to);

            // Проверка на существование такой связи.
            if (_adjacencyList[from].Any(edge => edge.Item1 == to))
            {
                Console.WriteLine($"Ребро от {from} до {to} уже существует.");
                return;
            }

            _adjacencyList[from].Add((to, weight));
            // Если граф неориентированный, добавляем обратное ребро.
            if (!_directed)
            {
                // Проверка на существование обратной связи.
                if (_adjacencyList[to].Any(edge => edge.Item1 == from))
                {
                    Console.WriteLine($"Ребро от {to} до {from} уже существует.");
                    return;
                }

                _adjacencyList[to].Add((from, weight));
            }
        }

        // Метод удаления вершины и всех её рёбер.
        public void RemoveVertex(string vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
                return;

            _adjacencyList.Remove(vertex);

            // Удаление всех рёбер, которые ведут к вершине, из других вершин.
            foreach (var adj in _adjacencyList.Values)
                adj.RemoveAll(edge => edge.Item1 == vertex);
        }

        // Метод удаления ребра.
        public void RemoveEdge(string from, string to)
        {
            if (!_adjacencyList.ContainsKey(from))
                return;

            // Удаление направленного ребра.
            _adjacencyList[from].RemoveAll(edge => edge.Item1 == to);

            // Если граф неориентированный, удаляем и обратное ребро.
            if (!_directed)
                _adjacencyList[to].RemoveAll(edge => edge.Item1 == from);
        }

        // Метод сохранения графа в файл.
        public void SaveToFile(string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(_directed);
                // Запись каждой вершины и её смежных вершин в файл.
                foreach (var vertex in _adjacencyList)
                {
                    var edges = string.Join(' ', vertex.Value.Select(edge => $"{edge.Item1}:{edge.Item2}".Trim()));
                    writer.WriteLine($"{vertex.Key} {edges}".Trim());
                }
            }
        }

        // Метод вывода графа в консоль.
        public void Print()
        {
            // Вывод каждой вершины и её смежных вершин в консоль.
            foreach (var vertex in _adjacencyList)
            {
                var edges = string.Join(", ", vertex.Value.Select(edge => $"{edge.Item1}({edge.Item2})"));
                Console.WriteLine($"{vertex.Key}: {edges}".Trim());
            }
        }

        #endregion

        #region Задача 2

        //Полный граф на основе данного графа.
        //Полный граф - это граф, в котором каждая пара различных вершин соединена ребром.
        public static Graph BuildFullGraph(Graph graph)
        {
            // Создаем новый пустой граф.
            Graph fullGraph = new Graph(graph._directed);

            // Добавляем все вершины исходного графа в новый граф.
            foreach (var vertex in graph._adjacencyList.Keys)
                fullGraph.AddVertex(vertex);

            // Для каждой пары вершин добавляем ребро, если его еще нет.
            foreach (var v1 in fullGraph._adjacencyList.Keys)
            {
                foreach (var v2 in fullGraph._adjacencyList.Keys)
                {
                    if (v1 != v2 && !fullGraph._adjacencyList[v1].Any(edge => edge.Item1 == v2))
                    {
                        fullGraph.AddEdge(v1, v2);
                    }
                }
            }

            return fullGraph;
        }

        // Граф, являющийся дополнением данного графа.
        // Для графа G, его дополнение G' состоит из всех вершин G, и тех рёбер, которых нет в G.
        public static Graph BuildComplementGraph(Graph graph)
        {
            // Создаем полный граф на основе исходного.
            Graph complementGraph = BuildFullGraph(graph);

            // Удаляем из полного графа все ребра исходного графа, 
            // чтобы получить дополнение.
            foreach (var vertex in graph._adjacencyList)
            {
                foreach (var edge in vertex.Value)
                {
                    complementGraph.RemoveEdge(vertex.Key, edge.Item1);
                }
            }

            return complementGraph;
        }

        // Граф, являющийся объединением двух заданных.
        public static Graph Union(Graph g1, Graph g2)
        {
            if (g1._directed != g2._directed)
            {
                Console.WriteLine("Ориентированность не совпадает");
                return null;
            }
            // Создаем новый граф, который будет содержать объединение двух графов.
            Graph unionGraph = new Graph(g1._directed);

            // Добавляем все вершины и ребра первого графа.
            foreach (var vertex in g1._adjacencyList)
            {
                unionGraph.AddVertex(vertex.Key);
                foreach (var edge in vertex.Value)
                {
                    unionGraph.AddEdge(vertex.Key, edge.Item1, edge.Item2);
                }
            }

            // Добавляем все вершины и ребра второго графа.
            foreach (var vertex in g2._adjacencyList)
            {
                unionGraph.AddVertex(vertex.Key);
                foreach (var edge in vertex.Value)
                {
                    unionGraph.AddEdge(vertex.Key, edge.Item1, edge.Item2);
                }
            }

            return unionGraph;
        }

        // Граф, являющийся соединением данного графа с другим.
        //Этот метод соединяет два графа, добавляя ребра между каждой вершиной первого графа и каждой вершиной второго графа.
        public static Graph Join(Graph g1, Graph g2)
        {
            // Создаем граф, который будет содержать объединение двух графов.
            Graph joinGraph = Union(g1, g2);
            if (joinGraph == null) return null;
            // Для каждой вершины из первого графа соединяем с каждой вершиной из второго графа.
            foreach (var v1 in g1._adjacencyList.Keys)
            {
                foreach (var v2 in g2._adjacencyList.Keys)
                {
                    joinGraph.AddEdge(v1, v2);
                }
            }

            return joinGraph;
        }

        #endregion

        #region Задача 3

        // Подсчет сильно связных компонент Орграфа.
        public int CountStronglyConnectedComponents()
        {
            if (!_directed)
            {
                Console.WriteLine("Метод CountStronglyConnectedComponents() специфичен для ориентированных графов (орграфов)");
                return -1;
            }
            
            // Инициализация стека и множества для отслеживания посещенных вершин.
            Stack<string> stack = new Stack<string>();
            HashSet<string> visited = new HashSet<string>();

            // Первый проход: заполняем стек порядком завершения вершин.
            foreach (var vertex in _adjacencyList.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    // Применяем рекурсивный метод для заполнения стека порядком завершения вершин.
                    DFSFillOrder(vertex, visited, stack);
                }
            }

            // Создаем транспонированный (обратный) граф.
            Graph transposed = GetTranspose();

            // Второй проход: поиск сильно связных компонент в обратном графе.
            visited.Clear();
            int count = 0;
            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (!visited.Contains(vertex))
                {
                    // Применяем обход в глубину для поиска сильно связных компонент.
                    transposed.DFS(vertex, visited);
                    count++;
                }
            }

            return count;
        }

        private void DFSFillOrder(string vertex, HashSet<string> visited, Stack<string> stack)
        {
            // Отмечаем текущую вершину как посещенную.
            visited.Add(vertex);

            // Проходим по всем соседям текущей вершины.
            foreach (var adj in _adjacencyList[vertex])
            {
                if (!visited.Contains(adj.Item1))
                {
                    // Продолжаем обход в глубину для всех непосещенных соседей.
                    DFSFillOrder(adj.Item1, visited, stack);
                }
            }

            // После завершения обработки всех соседей, добавляем текущую вершину в стек.
            stack.Push(vertex);
        }

        private Graph GetTranspose()
        {
            // Создаем новый пустой граф.
            Graph g = new Graph(_directed);
            foreach (var vertex in _adjacencyList)
            {
                g.AddVertex(vertex.Key);
                foreach (var edge in vertex.Value)
                {
                    // Добавляем ребра в обратном направлении для создания транспонированного графа.
                    g.AddEdge(edge.Item1, vertex.Key);
                }
            }

            return g;
        }
        
        private void DFS(string vertex, HashSet<string> visited)
        {
            visited.Add(vertex);

            foreach (var adj in _adjacencyList[vertex])
            {
                if (!visited.Contains(adj.Item1))
                {
                    DFS(adj.Item1, visited);
                }
            }
        }

        // Проверка типа графа.
        public string CheckGraphType()
        {
            // Проверяем, содержит ли граф циклы.
            bool hasCycle = ContainsCycle();
            // Проверяем, является ли граф связным.
            bool isConnected = IsConnected();

            // Возвращаем результат в зависимости от проверок выше.
            if (hasCycle)
                //Граф считается циклическим, если он содержит хотя бы один цикл.
                return "Цикл"; 
            if (isConnected)
                //Граф является деревом, если он связен и не содержит циклов.
                return "Дерево";
            
            //Граф является лесом, если он не содержит циклов (но может быть несвязным).
            return "Лес";
        }

        private bool ContainsCycle()
        {
            // Множество для отслеживания посещенных вершин
            HashSet<string> visited = new HashSet<string>();

            foreach (var vertex in _adjacencyList.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    if (DFSCycleCheck(vertex, visited, null))
                    {
                        // Если найден цикл, возвращаем true.
                        return true;
                    }
                }
            }

            return false;
        }

        private bool DFSCycleCheck(string vertex, HashSet<string> visited, string parent)
        {
            // Отмечаем текущую вершину как посещенную
            visited.Add(vertex);

            // Проходим по всем соседям текущей вершины.
            foreach (var adj in _adjacencyList[vertex])
            {
                if (!visited.Contains(adj.Item1))
                {
                    if (DFSCycleCheck(adj.Item1, visited, vertex))
                    {
                        // Если найден цикл в подграфе, возвращаем true.
                        return true;
                    }
                }
                // Если соседняя вершина уже была посещена и она не является родительской, 
                // то найден цикл.
                else if (adj.Item1 != parent)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsConnected()
        {
            // Множество для отслеживания посещенных вершин.
            HashSet<string> visited = new HashSet<string>();

            // Начинаем обход в глубину с первой вершины.
            DFS(_adjacencyList.Keys.First(), visited);

            // Если количество посещенных вершин равно общему количеству вершин в графе, граф связный.
            return visited.Count == _adjacencyList.Keys.Count;
        }

        #endregion
        
        #region Задача 4
        public Graph KruskalMST()
        {
            // Создание нового графа для хранения минимального остовного дерева (MST).
            Graph mst = new Graph();

            // Словарь для хранения "предка" каждой вершины. Изначально каждая вершина является своим собственным предком.
            Dictionary<string, string> parent = new Dictionary<string, string>();

            // Инициализация каждой вершины как отдельного множества.
            foreach (var vertex in _adjacencyList.Keys)
            {
                parent[vertex] = vertex;
            }

            // Получение списка всех рёбер графа и их сортировка по весу.
            var sortedEdges = _adjacencyList
                .SelectMany(kvp => kvp.Value.Select(adj => new { Source = kvp.Key, Destination = adj.Item1, Weight = adj.Item2 }))
                .OrderBy(edge => edge.Weight)
                .ToList();

            // Перебор всех рёбер графа в порядке возрастания веса.
            foreach (var edge in sortedEdges)
            {
                // Нахождение предка для начальной и конечной вершины ребра.
                string root1 = Find(edge.Source, parent);
                string root2 = Find(edge.Destination, parent);

                // Если рёбра не образуют цикл (их предки различны), добавляем ребро в MST.
                if (root1 != root2)
                {
                    mst.AddEdge(edge.Source, edge.Destination, edge.Weight);
                    // Объединение двух множеств.
                    Union(root1, root2, parent);
                }
            }

            // Возвращаем построенное минимальное остовное дерево.
            return mst;
        }

        private string Find(string vertex, Dictionary<string, string> parent)
        {
            // Если вершина не является своим собственным предком, ищем предка для предка вершины.
            if (parent[vertex] != vertex)
                parent[vertex] = Find(parent[vertex], parent);
            // Возвращаем предка вершины.
            return parent[vertex];
        }

        private void Union(string root1, string root2, Dictionary<string, string> parent)
        {
            // Присваиваем одному предку другого предка, объединяя таким образом два множества.
            parent[root1] = root2;
        }
        #endregion
    }
}
