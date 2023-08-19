using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace InvoiceApi.Models
{
    public class MyContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Configuration.GetConnectionString("MyDatabase");

            string host = "bilbgv4omq9o1q5vecxd-mysql.services.clever-cloud.com";
            string database = "bilbgv4omq9o1q5vecxd";
            string user = "uqfpwqhhw0yli0aq";
            string password = "rGOnpxR7IgxPkTdhJMRK";
            int port = 3306;

            optionsBuilder.UseMySQL($"server={host};port={port};database={database};user={user};password={password};SslMode=none");
        }
    }
}