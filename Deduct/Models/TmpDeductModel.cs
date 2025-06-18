namespace Deduct.Models
{
    public class TmpDeductModel
    {
        public int RunDeduct { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string ListNo { get; set; } = string.Empty;
        public string LotNo { get; set; } = string.Empty;
        public string JobBarcode { get; set; } = string.Empty;
        public string Article { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public decimal DeductQty { get; set; }
        public decimal QcQty { get; set; }
        public string Doc_No { get; set; } = string.Empty;
        public string DocNo { get; set; } = string.Empty;
        public int EmpCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool ChkS { get; set; }
    }
}
