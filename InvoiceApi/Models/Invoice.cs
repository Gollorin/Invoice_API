using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApi.Models
{
    [Table("tbInvoices")]
    public class Invoice
    {
        [Key]
        public int UUID { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastUpdate { get; set; }
        public decimal Price { get; set; }
        public string SupplierName { get; set; }
        public string SupplierICO { get; set; }
        public string CustomerName { get; set; }
        public string CustomerICO { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PaymentTo { get; set; }
        public DateTime MakingIn { get; set; }
        public PaymentType Payment { get; set; }

        public enum PaymentType
        {
            Command,
            Card,
            Cash
        }
    }
}
