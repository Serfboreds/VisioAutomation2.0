using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VACONNECT = VisioAutomation.Shapes.Connections;

namespace TestVisioAutomation.Connections
{
    [TestClass]
    public class Path_Test
    {
        [TestMethod]
        public void Path_TestTransitiveClosure0()
        {
            // v0->v0
            // doesn't yield any edges (nodes are implictly connected to themselves)
            var input = new List<VACONNECT.DirectedEdge<string, object>>
                {
                    new VACONNECT.DirectedEdge<string, object>("v0", "v0", null)
                };
            var output = VACONNECT.PathAnalysis.GetClosureFromEdges(input).ToList();
            Assert.AreEqual(0,output.Count);
        }

        [TestMethod]
        public void Path_TestTransitiveClosure1()
        {
            // v0->v0
            // v1->v1
            // doesn't yield any edges (nodes are implictly connected to themselves)
            var input = new List<VACONNECT.DirectedEdge<string, object>>();
            input.Add(new VACONNECT.DirectedEdge<string, object>("v0", "v0", null));
            input.Add(new VACONNECT.DirectedEdge<string, object>("v1", "v1", null));
            var output = VACONNECT.PathAnalysis.GetClosureFromEdges(input).ToList();
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
        public void Path_TestTransitiveClosure2()
        {
            // v0->v1
            // doesn't yield any edges (nodes are implictly connected to themselves)
            var input = new List<VACONNECT.DirectedEdge<string, object>>
                {
                    new VACONNECT.DirectedEdge<string, object>("v0", "v1", null)
                };
            var output = VACONNECT.PathAnalysis.GetClosureFromEdges(input).ToList();
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual("v0",output[0].From);
            Assert.AreEqual("v1", output[0].To);
        }


        [TestMethod]
        public void Path_TestTransitiveClosure3()
        {
            var input = new List<VACONNECT.DirectedEdge<string, object>>
                {
                    new VACONNECT.DirectedEdge<string, object>("v0", "v1", null),
                    new VACONNECT.DirectedEdge<string, object>("v1", "v2", null)
                };
            var output = VACONNECT.PathAnalysis.GetClosureFromEdges(input).ToList();
            Assert.AreEqual(3, output.Count);
            Assert.AreEqual("v0", output[0].From);
            Assert.AreEqual("v1", output[0].To);

            Assert.AreEqual("v0", output[1].From);
            Assert.AreEqual("v2", output[1].To);

            Assert.AreEqual("v1", output[2].From);
            Assert.AreEqual("v2", output[2].To);
            
        }

        [TestMethod]
        public void Path_TestTransitiveClosure4()
        {
            var input = new List<VACONNECT.DirectedEdge<string, object>>
                {
                    new VACONNECT.DirectedEdge<string, object>("v0", "v1", null),
                    new VACONNECT.DirectedEdge<string, object>("v1", "v2", null),
                    new VACONNECT.DirectedEdge<string, object>("v2", "v0", null)
                };
            var output = VACONNECT.PathAnalysis.GetClosureFromEdges(input).ToList();
            Assert.AreEqual(6, output.Count);
            Assert.AreEqual("v0", output[0].From);
            Assert.AreEqual("v1", output[0].To);

            Assert.AreEqual("v0", output[1].From);
            Assert.AreEqual("v2", output[1].To);

            Assert.AreEqual("v1", output[2].From);
            Assert.AreEqual("v0", output[2].To);

            Assert.AreEqual("v1", output[3].From);
            Assert.AreEqual("v2", output[3].To);

            Assert.AreEqual("v2", output[4].From);
            Assert.AreEqual("v0", output[4].To);

            Assert.AreEqual("v2", output[5].From);
            Assert.AreEqual("v1", output[5].To);

        }
    }
}