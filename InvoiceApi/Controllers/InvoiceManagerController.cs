using InvoiceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace InvoiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceManagerController : ControllerBase
    {
        private MyContext _context = new MyContext();

        /// <summary>
        ///     Get a specific invoice by ID.
        /// </summary>
        /// 
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///     GET /api/InvoiceManager/GetBy/1
        ///     
        ///     Return 
        ///     {
        ///         UUID: 2,
        ///         etc...
        ///     }
        /// </remarks>
        /// 
        /// <response code="200">Retrive one invoice by given UUID</response>
        /// <response code="400">Server is down!</response>
        /// <response code="404">Invoice not found</response>
        /// 
        /// <param name="id">Value of specific ID of the invoice</param>
        [HttpGet("GetBy/{id}")]
        [SwaggerResponse(200, "Retrive one invoice by given UUID", typeof(Invoice))]
        public ActionResult<Invoice> Get(int id)
        {
            //if (_context.Database.CanConnect())
            //    return BadRequest("Server is down!");

            if (!TestConnection())
                return BadRequest("Server is down!");
            
            try
            {
                Invoice temp = _context.Invoices.Where(inv => inv.UUID == id).FirstOrDefault();

                if (temp == null)
                    return NotFound("Object with this id was not found");

                string result = JsonConvert.SerializeObject(temp);

                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound("Invoice Not Found");
            }
        }



        /// <summary>
        ///    Creates a new invoice entity using the provided JSON data and stores it in the database.
        /// </summary>
        /// 
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///     POST /api/InvoiceManager/CreateInvoice
        ///     
        ///     Return 
        ///     {
        ///         UUID: 2,
        ///         etc...
        ///     }
        /// </remarks>
        /// 
        /// <response code="200">Retrive actual made invoice by given JSON</response>
        /// <response code="400">Server is down! / Data types problem</response>
        /// 
        /// <param name="value">Data of specific invoice to add to the database</param>
        [HttpPost("CreateInvoice")]
        [SwaggerResponse(200, "Retrive actual made invoice by given JSON", typeof(Invoice))]
        public IActionResult Post([FromBody] InputInvoice value)
        {
            if (TestConnection())
                return BadRequest("Server is down!");

            if (!CheckIco(value.CustomerICO) || !CheckIco(value.SupplierICO))
                return BadRequest("Wrong ICO format");

            try
            {
                Invoice invoice = new Invoice()
                {
                    CreateAt = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    Price = value.Price,
                    SupplierName = value.SupplierName,
                    SupplierICO = value.SupplierICO,
                    CustomerName = value.CustomerName,
                    CustomerICO = value.CustomerICO,
                    InvoiceDate = value.InvoiceDate,
                    PaymentTo = value.PaymentTo,
                    MakingIn = value.MakingIn,
                    Payment = value.Payment,
                };

                _context.Invoices.Add(invoice);
                _context.SaveChanges();

                string result = JsonConvert.SerializeObject(invoice);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        ///    Updates an existing invoice record identified by its UUID with the provided JSON data.
        /// </summary>
        /// 
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///     PUT /api/InvoiceManager/UpdateBy/{id}
        ///     
        ///     Return 
        ///     {
        ///         UUID: 2,
        ///         etc...
        ///     }
        /// </remarks>
        /// 
        /// <response code="200">Retrive actual made invoice by given JSON</response>
        /// <response code="400">Server is down! / Data types problem</response>
        /// <response code="404">Invoice entity not found</response>
        /// 
        /// <param name="id">Value of specific ID of the invoice</param>
        /// <param name="value">Data of specific invoice to add to the database</param>
        [HttpPut("UpdateBy/{id}")]
        [SwaggerResponse(200, "Retrive actual made invoice by given JSON", typeof(Invoice))]
        public IActionResult Put(int id, [FromBody] InputInvoice value)
        {
            if (TestConnection())
                return BadRequest("Server is down!");

            if (!CheckIco(value.CustomerICO) || !CheckIco(value.SupplierICO))
                return BadRequest("Wrong ICO format");

            try
            {
                Invoice updt_invoice = _context.Invoices.Where(inv => inv.UUID == id).FirstOrDefault();

                if (updt_invoice == null)
                    return NotFound("Object with this id was not found");

                updt_invoice.LastUpdate = DateTime.Now;
                updt_invoice.SupplierName = value.SupplierName;
                updt_invoice.SupplierICO = value.SupplierICO;
                updt_invoice.CustomerName = value.CustomerName;
                updt_invoice.CustomerICO = value.CustomerICO;
                updt_invoice.InvoiceDate = value.InvoiceDate;
                updt_invoice.PaymentTo = value.PaymentTo;
                updt_invoice.MakingIn = value.MakingIn;

                _context.SaveChanges();

                string result = JsonConvert.SerializeObject(updt_invoice);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        /// <summary>
        ///    Deletes an existing invoice record identified by its UUID.
        /// </summary>
        /// 
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///     DELETE /api/InvoiceManager/DeleteBy/{id}
        ///     
        ///     Return 
        ///     {
        ///         UUID: 2,
        ///         etc...
        ///     }
        /// </remarks>
        /// 
        /// <response code="200">Retrive id of deleted invoice</response>
        /// <response code="400">Server is down! / Data types problem</response>
        /// <response code="404">Invoice entity not found</response>
        /// 
        /// <param name="id">Value of specific ID of the invoice</param>
        [HttpDelete("DeleteBy/{id}")]
        [SwaggerResponse(200, "Retrive ID of deleted invoice", typeof(Invoice))]
        public IActionResult Delete(int id)
        {
            if (TestConnection())
                return BadRequest("Server is down!");

            try
            {
                Invoice temp = _context.Invoices.Where(inv => inv.UUID == id).FirstOrDefault();

                if (temp == null)
                    return NotFound("Object with this id was not found");

                _context.Invoices.Remove(temp);
                _context.SaveChanges();

                return Ok(JsonConvert.SerializeObject(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        private bool TestConnection()
        {
            try
            {
                _context.Database.OpenConnection();
                _context.Database.CloseConnection();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }



        private bool CheckIco(string ico)
        {
            return Regex.IsMatch(ico, @"^\d{8}$");
        }
    }
}