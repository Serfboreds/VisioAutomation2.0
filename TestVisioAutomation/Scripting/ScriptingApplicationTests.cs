using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestVisioAutomation.Scripting
{
    [TestClass]
    public class ScriptingApplicationTests : VisioAutomationTest
    {
        [TestMethod]
        public void Scripting_Test_Application_Window()
        {
            this.Scripting_Test_Resize_Application_Window1();
            this.Scripting_Test_Resize_Application_Window2();
            this.Scripting_Test_App_to_Front();
        }

        public void Scripting_Test_Resize_Application_Window1()
        {
            var client = this.GetScriptingClient();

            var old_size = client.Application.Window.GetSize();
            var desired_size = new Size(600, 800);

            client.Application.Window.SetSize(desired_size.Width, desired_size.Height);

            var actual_size = client.Application.Window.GetSize();
            Assert.AreEqual(desired_size, actual_size);
            client.Application.Window.SetSize(old_size.Width, old_size.Height);
            actual_size = client.Application.Window.GetSize();
            Assert.AreEqual(old_size, actual_size);
        }

        public void Scripting_Test_Resize_Application_Window2()
        {
            var client = this.GetScriptingClient();

            var doc = client.Document.New(10, 5);

            Assert.IsTrue(client.Document.HasActiveDocument);

            var pagesize = client.Page.GetSize();
            Assert.AreEqual(10.0, pagesize.Width);
            Assert.AreEqual(5.0, pagesize.Height);
            Assert.AreEqual(0, client.Selection.Get().Count);
            client.Draw.Rectangle(1, 1, 2, 2);
            Assert.AreEqual(1, client.Selection.Get().Count);

            client.Document.Close(true);
        }

        public void Scripting_Test_App_to_Front()
        {
            var client = this.GetScriptingClient();
            client.Application.Window.ToFront();
        }

        [TestMethod]
        public void Scripting_Undo_Scenarios()
        {
            var client = this.GetScriptingClient();
            var drawing = client.Document.New(8.5, 11);
            var page = client.Page.New(new VisioAutomation.Drawing.Size(8.5, 11), false);
            Assert.AreEqual(0, page.Shapes.Count);
            page.DrawRectangle(1, 1, 3, 3);
            Assert.AreEqual(1, page.Shapes.Count);
            client.Application.Undo();
            Assert.AreEqual(0, page.Shapes.Count);
            client.Document.Close(true);
        }

        [TestMethod]
        public void Scripting_CloseDocument_Scenarios()
        {
            var client = this.GetScriptingClient();
            var doc1 = client.Document.New(10, 5);
            var doc2 = client.Document.New(10, 5);
            var doc3 = client.Document.New(10, 5);

            client.Document.CloseAllWithoutSaving();

            Assert.IsFalse(client.Document.HasActiveDocument);
            var application = client.Application.Get();
            var documents = application.Documents;
            Assert.AreEqual(0, documents.Count);
        }
    }
}