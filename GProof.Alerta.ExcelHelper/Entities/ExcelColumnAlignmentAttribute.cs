using System;
using OfficeOpenXml.Style;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class ExcelColumnAlignmentAttribute: Attribute
    {
        public ExcelColumnAlignmentAttribute(ExcelHorizontalAlignment alignment)
        {
            Alignment = alignment;
        }

        public ExcelHorizontalAlignment Alignment { get; set; }

        public string Properties => ExcelFormat.HorizontalAlignment;
    }
}
