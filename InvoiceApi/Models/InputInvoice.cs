namespace InvoiceApi.Models
{
    public class InputInvoice
    {
        public decimal Price { get; set; }
        public string SupplierName { get; set; }
        public string SupplierICO { get; set; }
        public string CustomerName { get; set; }
        public string CustomerICO { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PaymentTo { get; set; }
        public DateTime MakingIn { get; set; }
        public Invoice.PaymentType Payment { get; set; }
    }
}
