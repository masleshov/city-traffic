using System.Text;

namespace CityTraffic
{
    internal class Program
    {
        private static Dictionary<int, int[]> _meta = new Dictionary<int, int[]>();

        private static void Main(string[] args)
        {
            FindMaxTraffic(new[] { "1:[5]", "4:[5]", "3:[5]", "5:[1,4,3,2]", "2:[5,15,7]", "7:[2,8]", "8:[7,38]", "15:[2]", "38:[8]" });
            FindMaxTraffic(new[] { "1:[5]", "2:[5]", "3:[5]", "4:[5]", "5:[1,2,3,4]" });
            FindMaxTraffic(new[] { "1:[5]", "2:[5,18]", "3:[5,12]", "4:[5]", "5:[1,2,3,4]", "18:[2]", "12:[3]" });
        }

        private static void FindMaxTraffic(string[] strArr)
        {
            Console.WriteLine($"Input: {string.Join(",", strArr)}");
            foreach (var str in strArr)
            {
                var colonIndex = str.IndexOf(':');
                var city = int.Parse(str.Substring(0, colonIndex));
                var neighbours = str.Substring(colonIndex + 1, str.Length - colonIndex - 1)
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty)
                    .Split(',')
                    .Select(num => Int32.Parse(num))
                    .ToArray();
                
                _meta.Add(city, neighbours);
            }

            var result = new List<string>();
            foreach (var city in _meta.OrderBy(pair => pair.Key))
            {
                result.Add($"{city.Key}:{GetCost(new Node(city.Key), null)}");
            }

            Console.WriteLine($"Result: {string.Join(",", result)}");
            _meta.Clear();
        }

        private static int GetCost(Node current, Node prev) 
        {
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current));
            }

            if (!_meta.TryGetValue(current.City, out var neighbours))
            {
                throw new NullReferenceException($"Internal error - city {current.City} info hasn't been provided");
            }

            for (int i = 0; i < neighbours.Length; i++)
            {
                // Way back, we do not need create a node for this case
                if (prev != null && neighbours[i] == prev.City)
                {
                    continue;
                }

                var node = new Node(neighbours[i]);
                current.Next.Add(node);
                node.Cost = GetCost(node, current);
            }

            // It could be optimized by removing LINQ (it does additional iterations inside)
            // but considering that we don't have big amounts of data, we can leave it in more readable view
            return current.Next.Count == 0
                    ? current.City
                    : prev == null
                        ? current.Next.Max(n => n.Cost)
                        : current.Next.Sum(n => n.Cost) + current.City;
        }
    }
}