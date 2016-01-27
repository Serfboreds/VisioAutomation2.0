using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisioAutomation.Extensions;
using VACONT = VisioAutomation.Shapes.Controls;
using VACUSTPROP = VisioAutomation.Shapes.CustomProperties;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace TestVisioAutomation.Shapesheet
{
    [TestClass]
    public class ShapeSheet_Query_Tests : VisioAutomationTest
    {
        public static VA.ShapeSheet.SRC cell_fg = VA.ShapeSheet.SRCConstants.FillForegnd;
        public static VA.ShapeSheet.SRC cell_bg = VA.ShapeSheet.SRCConstants.FillBkgnd;
        public static VA.ShapeSheet.SRC cell_pat = VA.ShapeSheet.SRCConstants.FillPattern;

        [TestMethod]
        public void ShapeSheet_Query_SectionCells_have_names()
        {
            var query = new VA.ShapeSheet.Query.CellQuery();

            var sec_char = query.AddSection(IVisio.VisSectionIndices.visSectionCharacter);
            Assert.AreEqual("Character", sec_char.Name);

            var sec_obj = query.AddSection(IVisio.VisSectionIndices.visSectionObject);
            Assert.AreEqual("Object", sec_obj.Name);

        }

        [TestMethod]
        public void ShapeSheet_Query_GetResults_SingleShape()
        {
            var doc1 = this.GetNewDoc();
            var page1 = doc1.Pages[1];
            VisioAutomationTest.SetPageSize(page1, this.StandardPageSize);

            // draw a simple shape
            var s1 = page1.DrawRectangle(this.StandardPageSizeRect);
            int s1_id = s1.ID;

            // format it with setformulas
            var fg_cell = s1.Cells["FillForegnd"];
            var bg_cell = s1.Cells["FillBkgnd"];
            var pat_cell = s1.Cells["FillPattern"];

            fg_cell.FormulaU = "RGB(255,0,0)";
            bg_cell.FormulaU = "RGB(0,0,255)";
            pat_cell.FormulaU = "40";

            // now retrieve the formulas with GetFormulas

            var src_fg = VA.ShapeSheet.SRCConstants.FillForegnd;
            var src_bg = VA.ShapeSheet.SRCConstants.FillBkgnd;
            var src_filpat = VA.ShapeSheet.SRCConstants.FillPattern;

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_fg = query.AddCell(src_fg, "FillForegnd");
            var col_bg = query.AddCell(src_bg, "FillBkgnd");
            var col_filpat = query.AddCell(src_filpat, "FillPattern");
            var sec_char = query.AddSection(IVisio.VisSectionIndices.visSectionCharacter);
            Assert.AreEqual("Character",sec_char.Name);
            var col_charcase = sec_char.AddCell(VA.ShapeSheet.SRCConstants.CharCase, "CharCase");
            var col_charcolor = sec_char.AddCell(VA.ShapeSheet.SRCConstants.CharColor, "CharColor");
            var col_chartrans = sec_char.AddCell(VA.ShapeSheet.SRCConstants.CharColorTrans, "CharColorTrans");

            var shapeids = new[] {s1_id};

            var formulas = query.GetFormulas(page1, shapeids);

            // now verify that the formulas were actually set
            Assert.AreEqual("RGB(255,0,0)", formulas[0][col_fg]);
            Assert.AreEqual("RGB(0,0,255)", formulas[0][col_bg]);
            Assert.AreEqual("40", formulas[0][col_filpat]);

            // now retrieve the results with GetResults as floats
            var float_results = query.GetResults<double>(page1,shapeids);
            Assert.IsNotNull(float_results);
            Assert.AreEqual(40.0, float_results[0][col_filpat]);

            // now retrieve the results with GetResults as ints
            var int_results = query.GetResults<int>(page1,shapeids);
            Assert.AreEqual(40, int_results[0][col_filpat]);

            // now retrieve the results with GetResults as strings

            var string_results = query.GetResults<string>(page1,shapeids);

            Assert.AreEqual("RGB(255, 0, 0)", string_results[0][col_fg]);
            Assert.AreEqual("RGB(0, 0, 255)", string_results[0][col_bg]);
            Assert.AreEqual("40", string_results[0][col_filpat]);

            page1.Delete(0);
            doc1.Close(true);
        }

        [TestMethod]
        public void ShapeSheet_Query_GetResults_MultipleShapes()
        {
            var page1 = this.GetNewPage();

            // draw a simple shape
            var s1 = page1.DrawRectangle(this.StandardPageSizeRect);
            int s1_id = s1.ID;

            // format it with setformulas
            var fg_cell = s1.Cells["FillForegnd"];
            var bg_cell = s1.Cells["FillBkgnd"];
            var pat_cell = s1.Cells["FillPattern"];

            fg_cell.ResultIU = 2.0; //red
            bg_cell.ResultIU = 3.0; //green
            pat_cell.ResultIU = 40.0;

            var src_fg = VA.ShapeSheet.SRCConstants.FillForegnd;
            var src_bg = VA.ShapeSheet.SRCConstants.FillBkgnd;
            var src_filpat = VA.ShapeSheet.SRCConstants.FillPattern;

            // now retrieve the formulas with GetFormulas

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_fg = query.AddCell(src_fg, "FillForegnd");
            var col_bg = query.AddCell(src_bg, "FillBkgnd");
            var col_filpat = query.AddCell(src_filpat, "FillPattern");

            var shapeids = new[] {s1_id};

            var formulas = query.GetFormulas(page1, shapeids);

            // now verify that the formulas were actually set
            Assert.AreEqual("2", formulas[0][col_fg]);
            Assert.AreEqual("3", formulas[0][col_bg]);
            Assert.AreEqual("40", formulas[0][col_filpat]);

            // now retrieve the results with GetResults as floats

            var float_results = query.GetResults<double>(page1,shapeids);
            Assert.AreEqual(2.0, float_results[0][col_fg]);
            Assert.AreEqual(3.0, float_results[0][col_bg]);
            Assert.AreEqual(40.0, float_results[0][col_filpat]);

            // now retrieve the results with GetResults as ints
            var int_results = query.GetResults<int>(page1,shapeids);

            Assert.AreEqual(2, int_results[0][col_fg]);
            Assert.AreEqual(3, int_results[0][col_bg]);
            Assert.AreEqual(40, int_results[0][col_filpat]);

            // now retrieve the results with GetResults as strings
            var string_results = query.GetResults<string>(page1,shapeids);
            Assert.AreEqual("2", string_results[0][col_fg]);
            Assert.AreEqual("3", string_results[0][col_bg]);
            Assert.AreEqual("40", string_results[0][col_filpat]);

            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Query_SectionRowHandling()
        {
            var page1 = this.GetNewPage();
            var s1 = page1.DrawRectangle(0, 0, 2, 2);
            var s2 = page1.DrawRectangle(2, 1, 3, 3);
            var s3 = page1.DrawRectangle(3, 1, 4, 2);
            var s4 = page1.DrawRectangle(4, -1, 5, 1);

            VACUSTPROP.CustomPropertyHelper.Set(s1, "S1P1", "1");
            VACUSTPROP.CustomPropertyHelper.Set(s2, "S2P1", "2");
            VACUSTPROP.CustomPropertyHelper.Set(s2, "S2P2", "3");
            //set nothing for s3
            VACUSTPROP.CustomPropertyHelper.Set(s4, "S3P1", "4");
            VACUSTPROP.CustomPropertyHelper.Set(s4, "S3P2", "5");
            VACUSTPROP.CustomPropertyHelper.Set(s4, "S3P3", "6");

            var query = new VA.ShapeSheet.Query.CellQuery();

            var prop_sec = query.AddSection(IVisio.VisSectionIndices.visSectionProp);
            var value_col = prop_sec.AddCell(VA.ShapeSheet.SRCConstants.Prop_Value,"Value");

            var shapeids = new[] { s1.ID, s2.ID, s3.ID, s4.ID };

            var data = query.GetCellData<double>(
                page1,
                shapeids);

            Assert.AreEqual(4, data.Count);
            Assert.AreEqual(1, data[0].Sections[prop_sec].Count);
            Assert.AreEqual(2, data[1].Sections[prop_sec].Count);
            Assert.AreEqual(0, data[2].Sections[prop_sec].Count);
            Assert.AreEqual(3, data[3].Sections[prop_sec].Count);

            AssertVA.AreEqual("\"1\"", 1.0, data[0].Sections[prop_sec][0][0]);
            AssertVA.AreEqual("\"2\"", 2.0, data[1].Sections[prop_sec][0][0]);
            AssertVA.AreEqual("\"3\"", 3.0, data[1].Sections[prop_sec][1][0]);
            AssertVA.AreEqual("\"4\"", 4.0, data[3].Sections[prop_sec][0][0]);
            AssertVA.AreEqual("\"5\"", 5.0, data[3].Sections[prop_sec][1][0]);
            AssertVA.AreEqual("\"6\"", 6.0, data[3].Sections[prop_sec][2][0]);

            page1.Delete(0);
        }


        [TestMethod]
        public void ShapeSheet_Query_Demo_MultipleShapes()
        {
            var page1 = this.GetNewPage(new VisioAutomation.Drawing.Size(10, 10));

            // draw a simple shape
            var s1 = page1.DrawRectangle(0, 0, 2, 2);
            var s2 = page1.DrawRectangle(4, 4, 6, 6);
            var s3 = page1.DrawRectangle(5, 5, 7, 7);

            var shapeids = new List<int> { s1.ID, s2.ID, s3.ID };

            Assert.AreEqual(3, page1.Shapes.Count);

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_pinx = query.AddCell(VA.ShapeSheet.SRCConstants.PinX, "PinX");
            var col_piny = query.AddCell(VA.ShapeSheet.SRCConstants.PinY, "PinY");

            var rf = query.GetFormulas(page1, shapeids);
            var rr = query.GetResults<double>(page1, shapeids);

            var expected_formulas = new[,]
                                      {
                                          {"1 in", "1 in"},
                                          {"5 in", "5 in"},
                                          {"6 in", "6 in"}
                                      };

            var expected_results = new[,]
                                      {
                                          {1.0, 1.0},
                                          {5.0, 5.0},
                                          {6.0, 6.0}
                                      };


            for (int row = 0; row < rr.Count; row++)
            {
                for (int col = 0; col < query.CellColumns.Count; col++)
                {
                    Assert.AreEqual(expected_formulas[row,col], rf[row][col]);
                    Assert.AreEqual(expected_results[row,col], rr[row][col]);
                }
            }

            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Query_Demo_MultipleShapes_Verify_Out_Of_order()
        {
            var page1 = this.GetNewPage(new VisioAutomation.Drawing.Size(10, 10));

            // draw a simple shape
            var sa = page1.DrawRectangle(-1, -1, 0, 0);
            var s1 = page1.DrawRectangle(0, 0, 2, 2);
            var sb = page1.DrawRectangle(-1, -1, 0, 0);
            var s2 = page1.DrawRectangle(4, 4, 6, 6);
            var s3 = page1.DrawRectangle(5, 5, 7, 7);

            // notice that the shapes are created as 0, 1,2,3
            // but are queried as 2, 3, 1
            var shapeids = new List<int> { s2.ID, s3.ID, s1.ID };

            Assert.AreEqual(5, page1.Shapes.Count);

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_pinx = query.AddCell(VA.ShapeSheet.SRCConstants.PinX, "PinX");
            var col_piny = query.AddCell(VA.ShapeSheet.SRCConstants.PinY, "PinY");

            var rf = query.GetFormulas(page1, shapeids);
            var rr = query.GetResults<double>(page1, shapeids);

            var expected_formulas = new[,]
                                      {
                                          {"5 in", "5 in"},
                                          {"6 in", "6 in"},
                                          {"1 in", "1 in"}
                                      };

            var expected_results = new[,]
                                      {
                                          {5.0, 5.0},
                                          {6.0, 6.0},
                                          {1.0, 1.0}
                                      };


            for (int row = 0; row < rr.Count; row++)
            {
                for (int col = 0; col < query.CellColumns.Count; col++)
                {
                    Assert.AreEqual(expected_formulas[row, col], rf[row][col]);
                    Assert.AreEqual(expected_results[row, col], rr[row][col]);
                }
            }

            page1.Delete(0);
        }


        [TestMethod]
        public void ShapeSheet_Query_NonExistentSections()
        {
            var page1 = this.GetNewPage();
            var s1 = page1.DrawRectangle(0, 0, 2, 2);
            var s2 = page1.DrawRectangle(2, 1, 3, 3);
            var s3 = page1.DrawRectangle(3, 1, 4, 2);
            var s4 = page1.DrawRectangle(4, -1, 5, 1);

            var shapes = new[] {s1, s2, s3, s4};
            var shapeids = shapes.Select(s => s.ID).ToList();

            // First verify that none of the shapes have the controls section locally or otherwise
            foreach (var s in shapes)
            {
                Assert.AreEqual(0, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 1]);
                Assert.AreEqual(0, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 0]);
            }

            // Try to retrieve the control cells rows for each shape, every shape should return zero rows
            foreach (var s in shapes)
            {
                var r1 = VACONT.ControlCells.GetCells(s);
                Assert.AreEqual(0,r1.Count);
            }

            // Try to retrieve the control cells rows for all shapes at once, every shape should return a collection of zero rows
            var r2 = VACONT.ControlCells.GetCells(page1, shapeids);
            Assert.AreEqual(shapes.Count(),r2.Count);
            for (int i = 0; i < shapes.Count();i++)
            {
                Assert.AreEqual(0,r2[i].Count);
            }

            // Add a Controls row to shape2
            var cc = new VACONT.ControlCells();
            VACONT.ControlHelper.Add(s2, cc);

            // Now verify that none of the shapes *except s2* have the controls section locally or otherwise
            foreach (var s in shapes)
            {
                if (s != s2)
                {
                    Assert.AreEqual(0, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 1]);
                    Assert.AreEqual(0, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 0]);
                }
                else
                {
                    Assert.AreEqual(-1, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 1]);
                    Assert.AreEqual(-1, s.SectionExists[(short)IVisio.VisSectionIndices.visSectionControls, 0]);
                }
            }

            // Try to retrieve the control cells rows for each shape, every shape should return zero rows *except for s2*
            foreach (var s in shapes)
            {
                if (s != s2)
                {
                    var r1 = VACONT.ControlCells.GetCells(s);
                    Assert.AreEqual(0, r1.Count);
                }
                else
                {
                    var r1 = VACONT.ControlCells.GetCells(s);
                    Assert.AreEqual(1, r1.Count);
                }
            }

            // Try to retrieve the control cells rows for all shapes at once, every shape *except s2* should return a collection of zero rows
            var r3 = VACONT.ControlCells.GetCells(page1, shapeids);
            Assert.AreEqual(shapes.Count(), r3.Count);
            for (int i = 0; i < shapes.Count(); i++)
            {
                if (shapes[i] != s2)
                {
                    Assert.AreEqual(0, r3[i].Count);
                }
                else
                {
                    Assert.AreEqual(1, r3[i].Count);
                }
            }

            page1.Delete(0);
        }

        [TestMethod]
        public void ShapeSheet_Query_Demo_AllCellsAndSections()
        {
            var doc1 = this.GetNewDoc();
            var page1 = doc1.Pages[1];
            VisioAutomationTest.SetPageSize(page1, this.StandardPageSize);

            // draw simple shapes
            var s1 = page1.DrawRectangle(0,0,1,1);
            var s2 = page1.DrawRectangle(2,2,3,3);


            var query = new VA.ShapeSheet.Query.CellQuery();

            var name_to_src = VA.ShapeSheet.SRCConstants.GetSRCDictionary();
            var section_to_secquery = new Dictionary<short,VA.ShapeSheet.Query.SectionColumn>();

            foreach (var kv in name_to_src)
            {
                var name = kv.Key;
                var src = kv.Value;

                if (src.Section == (short) IVisio.VisSectionIndices.visSectionObject)
                {
                    query.AddCell(src, name);
                }
                else if ((src.Section == (short) IVisio.VisSectionIndices.visSectionFirst)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionFirstComponent)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionLast)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionInval)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionNone)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionFirst)
                         || (src.Section == (short) IVisio.VisSectionIndices.visSectionLastComponent)
                    )
                {
                    //skip
                }
                else
                {
                    VA.ShapeSheet.Query.SectionColumn sec;
                    if (!section_to_secquery.ContainsKey(src.Section))
                    {
                        sec = query.AddSection((IVisio.VisSectionIndices)src.Section);
                        section_to_secquery[src.Section] = sec;
                    }
                    else
                    {
                        sec = section_to_secquery[src.Section];
                    }
                    sec.AddCell(src.Cell, name);
                }
            }

            var formulas1 = query.GetFormulas(s1);
            var formulas2 = query.GetFormulas(page1,new [] {s1.ID,s2.ID});

            doc1.Close(true);
        }

        [TestMethod]
        public void ShapeSheet_Query_TestDuplicates()
        {
            // Ensure that duplicate cells are caught
            var q1 = new VA.ShapeSheet.Query.CellQuery();
            q1.AddCell(VA.ShapeSheet.SRCConstants.PinX, "PinX");

            bool caught_exc1 = false;
            try
            {
                q1.AddCell(VA.ShapeSheet.SRCConstants.PinX, "PinX");
            }
            catch (VA.AutomationException)
            {
                caught_exc1 = true;
            }

            Assert.IsTrue(caught_exc1);

            // Ensure that duplicate sections are caught

            var q2 = new VA.ShapeSheet.Query.CellQuery();
            q2.AddSection(IVisio.VisSectionIndices.visSectionObject);

            bool caught_exc2 = false;
            try
            {
                q2.AddSection(IVisio.VisSectionIndices.visSectionObject);
            }
            catch (VA.AutomationException)
            {
                caught_exc2 = true;
            }

            Assert.IsTrue(caught_exc2);

            // Ensure that Duplicates in Section Queries Are caught - 
            var q3 = new VA.ShapeSheet.Query.CellQuery();
            var sec = q3.AddSection(IVisio.VisSectionIndices.visSectionObject);
            sec.AddCell(VA.ShapeSheet.SRCConstants.PinX.Cell,"PinX");
            bool caught_exc3 = false;
            try
            {
                sec.AddCell(VA.ShapeSheet.SRCConstants.PinX.Cell, "PinX");
            }
            catch (VA.AutomationException)
            {
                caught_exc3 = true;
            }

            Assert.IsTrue(caught_exc3);
        }
    }
}
