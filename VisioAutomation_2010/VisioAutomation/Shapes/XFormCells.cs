﻿using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Shapes
{
    public class XFormCells : ShapeSheet.CellGroups.CellGroup
    {
        public ShapeSheet.CellData<double> PinX { get; set; }
        public ShapeSheet.CellData<double> PinY { get; set; }
        public ShapeSheet.CellData<double> LocPinX { get; set; }
        public ShapeSheet.CellData<double> LocPinY { get; set; }
        public ShapeSheet.CellData<double> Width { get; set; }
        public ShapeSheet.CellData<double> Height { get; set; }
        public ShapeSheet.CellData<double> Angle { get; set; }

        public override IEnumerable<SRCFormulaPair> Pairs
        {
            get
            {
                yield return this.newpair(ShapeSheet.SRCConstants.PinX, this.PinX.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.PinY, this.PinY.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.LocPinX, this.LocPinX.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.LocPinY, this.LocPinY.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.Width, this.Width.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.Height, this.Height.Formula);
                yield return this.newpair(ShapeSheet.SRCConstants.Angle, this.Angle.Formula);
            }
        }

        public static IList<XFormCells> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = XFormCells.lazy_query.Value;
            return ShapeSheet.CellGroups.CellGroup._GetCells<XFormCells, double>(page, shapeids, query, query.GetCells);
        }

        public static XFormCells GetCells(IVisio.Shape shape)
        {
            var query = XFormCells.lazy_query.Value;
            return ShapeSheet.CellGroups.CellGroup._GetCells<XFormCells, double>(shape, query, query.GetCells);
        }

        private static System.Lazy<ShapeSheet.Query.Common.XFormCellsQuery> lazy_query = new System.Lazy<ShapeSheet.Query.Common.XFormCellsQuery>();

    }
}