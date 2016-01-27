using System.Collections.Generic;
using VACONNECT = VisioAutomation.Shapes.Connections;

namespace TestVisioAutomation.Connections
{
    public class ConnectivityMap
    {
        private readonly Dictionary<string, List<string>> dic;

        public ConnectivityMap(IList<VACONNECT.ConnectorEdge> edges)
        {
            this.dic = new Dictionary<string, List<string>>();
            foreach (var e in edges)
            {
                string fromtext = e.From.Text;
                if (!this.dic.ContainsKey(fromtext))
                {
                    this.dic[fromtext] = new List<string>();
                }
                var list = this.dic[fromtext];
                list.Add(e.To.Text);
            }
        }

        public bool HasConnectionFromTo(string f, string t)
        {
            return (this.dic[f].Contains(t));
        }

        public int CountConnectionsFrom(string f)
        {
            return this.dic[f].Count;
        }

        public int CountFromNodes()
        {
            return this.dic.Count;
        }
    }
}