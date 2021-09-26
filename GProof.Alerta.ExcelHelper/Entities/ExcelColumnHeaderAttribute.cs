using System;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class ExcelColumnHeaderAttribute: Attribute
    {
        public ExcelColumnHeaderAttribute(string header)
        {
            Header = header;
        }

        public string Header { get; set; }
    }
}
