using System;
using System.Drawing;
using OfficeOpenXml.Style;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class ExcelColumnBackgroundColorAttribute: Attribute
    {
        public ExcelColumnBackgroundColorAttribute(ExcelFillStyle excelFillStyle, string color)
        {
            ExcelFillStyle = excelFillStyle;
            Color = Color.FromName(color);
        }

        public ExcelFillStyle ExcelFillStyle { get; set; }

        public Color Color { get; set; }

        public string ExcelFillStyleProperties => ExcelFormat.ColorFillPattern;
        public string ColorProperties => ExcelFormat.BackgroundColor;
    }
}
