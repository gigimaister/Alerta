using System;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class ExcelColumnFormatAttribute: Attribute
    {
        public ExcelColumnFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; set; }

        public string Properties => ExcelFormat.NumberFormat;
    }
}
