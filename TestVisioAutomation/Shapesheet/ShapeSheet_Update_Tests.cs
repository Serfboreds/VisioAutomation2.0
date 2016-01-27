using Microsoft.VisualStudio.TestTools.UnitTesting;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace TestVisioAutomation.Shapesheet
{
    [TestClass]
    public class ShapeSheet_Update_Tests : VisioAutomationTest
    {
        private static readonly VA.ShapeSheet.SRC src_pinx = VA.ShapeSheet.SRCConstants.PinX;
        private static readonly VA.ShapeSheet.SRC src_piny = VA.ShapeSheet.SRCConstants.PinY;
        private static readonly VA.ShapeSheet.SRC src_linepat = VA.ShapeSheet.SRCConstants.LinePattern;

        [TestMethod]
        public void ShapeSheet_Update_Formulas_MultipleShapes()
        {
            var page1 = this.GetNewPage();

            var shape1 = page1.DrawRectangle(-1, -1, 0, 0);
            var shape2 = page1.DrawRectangle(-1, -1, 0, 0);
            var shape3 = page1.DrawRectangle(-1, -1, 0, 0);


            // Set the formulas
            var update = new VA.ShapeSheet.Update();
            update.SetFormula(shape1.ID16, ShapeSheet_Update_Tests.src_pinx, 0.5);
            update.SetFormula(shape1.ID16, ShapeSheet_Update_Tests.src_piny, 0.5);
            update.SetFormula(shape2.ID16, ShapeSheet_Update_Tests.src_pinx, 1.5);
            update.SetFormula(shape2.ID16, ShapeSheet_Update_Tests.src_piny, 1.5);
            update.SetFormula(shape3.ID16, ShapeSheet_Update_Tests.src_pinx, 2.5);
            update.SetFormula(shape3.ID16, ShapeSheet_Update_Tests.src_piny, 2.5);
            update.Execute(page1);

            // Verify that the formulas were set
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_pinx = query.AddCell(ShapeSheet_Update_Tests.src_pinx, "PinX");
            var col_piny = query.AddCell(ShapeSheet_Update_Tests.src_piny, "PinY");

            var shapeids = new[] { shape1.ID, shape2.ID, shape3.ID };

            var rf = query.GetFormulas(page1, shapeids);
            var rr = query.GetResults<double>(page1, shapeids);

            AssertVA.AreEqual("0.5 in", 0.5, rf[0][col_pinx], rr[0][col_pinx]);
            AssertVA.AreEqual("0.5 in", 0.5, rf[0][col_piny], rr[0][col_piny]);
            AssertVA.AreEqual("1.5 in", 1.5, rf[1][col_pinx], rr[1][col_pinx]);
            AssertVA.AreEqual("1.5 in", 1.5, rf[1][col_piny], rr[1][col_piny]);
            AssertVA.AreEqual("2.5 in", 2.5, rf[2][col_pinx], rr[2][col_pinx]);
            AssertVA.AreEqual("2.5 in", 2.5, rf[2][col_piny], rr[2][col_piny]);

            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Update_ResultsInt_SingleShape()
        {
            var page1 = this.GetNewPage();
            var shape1 = page1.DrawRectangle(0, 0, 1, 1);

            // Setup the modifications to the cell values
            var update = new VA.ShapeSheet.Update();
            update.SetResult(ShapeSheet_Update_Tests.src_linepat, 7, IVisio.VisUnitCodes.visNumber);
            update.Execute(shape1);

            // Build the query
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_linepat = query.AddCell(ShapeSheet_Update_Tests.src_linepat,"LinePattern");

            // Retrieve the values
            var data = query.GetCellData<double>(shape1);

            // Verify
            AssertVA.AreEqual("7", 7, data[col_linepat]);
            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Update_ResultsString_SingleShape()
        {
            var page1 = this.GetNewPage();
            var shape1 = page1.DrawRectangle(0, 0, 1, 1);

            // Setup the modifications to the cell values
            var update = new VA.ShapeSheet.Update();
            update.SetResult(ShapeSheet_Update_Tests.src_linepat, "7", IVisio.VisUnitCodes.visNumber);
            update.Execute(shape1);

            // Build the query
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_linepat = query.AddCell(ShapeSheet_Update_Tests.src_linepat, "LinePattern");

            // Retrieve the values
            var data = query.GetCellData<double>(shape1);

            // Verify
            AssertVA.AreEqual("7", 7, data[col_linepat]);
            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Update_ResultsDouble_MultipleShapes()
        {
            var page1 = this.GetNewPage();

            var shape1 = page1.DrawRectangle(-1, -1, 0, 0);
            var shape2 = page1.DrawRectangle(-1, -1, 0, 0);
            var shape3 = page1.DrawRectangle(-1, -1, 0, 0);


            // Set the formulas
            var update = new VA.ShapeSheet.Update();
            update.SetResult(shape1.ID16, ShapeSheet_Update_Tests.src_pinx, 0.5, IVisio.VisUnitCodes.visNumber);
            update.SetResult(shape1.ID16, ShapeSheet_Update_Tests.src_piny, 0.5, IVisio.VisUnitCodes.visNumber);
            update.SetResult(shape2.ID16, ShapeSheet_Update_Tests.src_pinx, 1.5, IVisio.VisUnitCodes.visNumber);
            update.SetResult(shape2.ID16, ShapeSheet_Update_Tests.src_piny, 1.5, IVisio.VisUnitCodes.visNumber);
            update.SetResult(shape3.ID16, ShapeSheet_Update_Tests.src_pinx, 2.5, IVisio.VisUnitCodes.visNumber);
            update.SetResult(shape3.ID16, ShapeSheet_Update_Tests.src_piny, 2.5, IVisio.VisUnitCodes.visNumber);
            update.Execute(page1);

            // Verify that the formulas were set
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_pinx = query.AddCell(ShapeSheet_Update_Tests.src_pinx,"PinX");
            var col_piny = query.AddCell(ShapeSheet_Update_Tests.src_piny, "PinY");

            var shapeids = new[] { shape1.ID, shape2.ID, shape3.ID };

            var rf = query.GetFormulas(page1, shapeids);
            var rr = query.GetResults<double>(page1, shapeids);

            AssertVA.AreEqual("0.5 in", 0.5, rf[0][col_pinx], rr[0][col_pinx]);
            AssertVA.AreEqual("0.5 in", 0.5, rf[0][col_piny], rr[0][col_piny]);
            AssertVA.AreEqual("1.5 in", 1.5, rf[1][col_pinx], rr[1][col_pinx]);
            AssertVA.AreEqual("1.5 in", 1.5, rf[1][col_piny], rr[1][col_piny]);
            AssertVA.AreEqual("2.5 in", 2.5, rf[2][col_pinx], rr[2][col_pinx]);
            AssertVA.AreEqual("2.5 in", 2.5, rf[2][col_piny], rr[2][col_piny]);

            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Update_ConsistencyChecking()
        {
            this.CheckHomogenousUpdates_FormulasResults();
            this.CheckHomogenousUpdates_Streams();
            this.CheckHomogenousUpdates_ResultTypes();
        }

        public void CheckHomogenousUpdates_FormulasResults()
        {
            var update1 = new VA.ShapeSheet.Update();
            update1.SetResult(ShapeSheet_Update_Tests.src_pinx, 5.0, IVisio.VisUnitCodes.visNumber);
            bool caught1 = false;
            try
            {
                update1.SetFormula(ShapeSheet_Update_Tests.src_pinx, "5.0");

            }
            catch (VA.AutomationException)
            {
                caught1 = true;
            }

            if (!caught1)
            {
                Assert.Fail();
            }
        }
        
        public void CheckHomogenousUpdates_Streams()
        {
            var update1 = new VA.ShapeSheet.Update();
            update1.SetResult(ShapeSheet_Update_Tests.src_pinx, 5.0, IVisio.VisUnitCodes.visNumber);
            bool caught1 = false;
            try
            {
                update1.SetResult(1, ShapeSheet_Update_Tests.src_pinx, 5.0, IVisio.VisUnitCodes.visNumber);

            }
            catch (VA.AutomationException)
            {
                caught1 = true;
            }

            if (!caught1)
            {
                Assert.Fail();
            }
        }

        public void CheckHomogenousUpdates_ResultTypes()
        {
            var page1 = this.GetNewPage();
            var shape1 = page1.DrawRectangle(0, 0, 1, 1);

            // Setup the modifications to the cell values
            var update = new VA.ShapeSheet.Update();
            update.SetResult(ShapeSheet_Update_Tests.src_linepat, "7", IVisio.VisUnitCodes.visNumber);
            update.SetResult(VA.ShapeSheet.SRCConstants.PinX, 2, IVisio.VisUnitCodes.visNumber);
            update.Execute(shape1);

            // Build the query
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_linepat = query.AddCell(ShapeSheet_Update_Tests.src_linepat, "LinePattern");
            var col_pinx = query.AddCell(VA.ShapeSheet.SRCConstants.PinX, "PinX");

            // Retrieve the values
            var data = query.GetCellData<double>(shape1);

            // Verify
            AssertVA.AreEqual("7", 7, data[col_linepat]);
            AssertVA.AreEqual("2 in", 2, data[col_pinx]);
            
            page1.Delete(0);
        }
    }
}