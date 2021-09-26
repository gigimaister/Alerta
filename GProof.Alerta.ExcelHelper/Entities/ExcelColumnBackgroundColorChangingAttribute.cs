using System;
using System.Collections.Generic;
using OfficeOpenXml.Style;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class ExcelColumnBackgroundColorChangingAttribute : Attribute
    {
        public ExcelColumnBackgroundColorChangingAttribute()
        {
            ExcelFillStyle = ExcelFillStyle.Solid;
        }

        public ExcelFillStyle ExcelFillStyle { get; set; }

        public List<System.Drawing.Color> Colors = new List<System.Drawing.Color>
        {
            System.Drawing. Color.AntiqueWhite,
            System.Drawing.Color.Aquamarine,
            System.Drawing.Color.LightGoldenrodYellow,
            System.Drawing.Color.LightCyan,
            System.Drawing.Color.LightGreen,
            System.Drawing.Color.LightPink,
            System.Drawing.Color.LightBlue,
            //System.Drawing.Pr.ColorTranslator.FromHtml("#FFFFFF")
        };

        public string ExcelFillStyleProperties => ExcelFormat.ColorFillPattern;
        public string ColorProperties => ExcelFormat.BackgroundColor;
    }
}