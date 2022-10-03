namespace CityTraffic 
{
    public class Node
    {
        public readonly int City;

        public Node(int city)
        {
            City = city;
            Next = new List<Node>();
        }

        public int Cost { get; set; }
        public List<Node> Next { get; set; }
    }
}