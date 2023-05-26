using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INF354ReportHomework.Models;
using Microsoft.EntityFrameworkCore;
using static INF354ReportHomework.ViewModels.Data;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace INF354ReportHomework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpPost]
        [Route("GetReportData")]
        public dynamic GetReportData([FromBody] DateVM Dates)
        {
            using var db = new NorthwindContext();

            var reportData = db.OrderDetails.Include(zz => zz.Product).Include(zz => zz.Order)
                .Where(zz => zz.Order.ShippedDate > Dates.startDate && zz.Order.ShippedDate < Dates.endDate
                && zz.Product.CategoryId == Dates.productCategoryID).ToList();

            return GetDynamicData(reportData);
        }

        public dynamic GetDynamicData(List<OrderDetails> reportData)
        {
            dynamic dynTableData = new ExpandoObject();
            dynTableData.reportData = null;

            var orders = reportData.GroupBy(zz => zz.Product.ProductName);
            List<dynamic> orderGroups = new List<dynamic>();

            foreach (var item in orders)
            {
                dynamic orderList = new ExpandoObject();

                orderList.ProductName = item.Key;
                orderList.AverageQuantityOrdered = Math.Round((double)item.Average(zz => zz.Quantity), 2);
                List<dynamic> productOrders = new List<dynamic>();
                foreach (var order in item)
                {
                    dynamic orderObject = new ExpandoObject();
                    orderObject.OrderDate = order.Order.OrderDate;
                    orderObject.OrderQuantity = order.Quantity;
                    orderObject.ShippedDate = order.Order.ShippedDate;

                    productOrders.Add(orderObject);
                }

                orderList.ProductOrders = productOrders;
                orderGroups.Add(orderList);
            }

            dynTableData.reportData = orderGroups;
            return dynTableData;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(AuthVM vm)
        {
            using var db = new NorthwindContext();
            string hashedPassword = this.ComputeSha256Hash(vm.Password);
            var user = db.UserLogin.Where(zz => zz.EmailAddress == vm.EmailAddress && zz.Password == hashedPassword).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(vm);
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(AuthVM vm)
        {
            using var db = new NorthwindContext();

            if (this.UserExists(vm.EmailAddress))
            {
                return Forbid();
            }

            var newUser = new UserLogin
            {
                EmailAddress = vm.EmailAddress,
                Password = this.ComputeSha256Hash(vm.Password),
                Name = vm.Name
            };

            try
            {
                db.UserLogin.Add(newUser);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // Get Product Categories
        [HttpGet]
        [Route("GetCategories")]
        public List<dynamic> GetCategories()
        {
            using var db = new NorthwindContext();

            var categories = db.Categories.ToList();

            return GetDynamicCategories(categories);
        }

        public List<dynamic> GetDynamicCategories(List<Categories> categories)
        {
            var dynamicCategories = new List<dynamic>();

            foreach (var category in categories)
            {
                dynamic dynamicCat = new ExpandoObject();
                dynamicCat.CategoryID = category.CategoryId;
                dynamicCat.CategoryName = category.CategoryName;

                dynamicCategories.Add(dynamicCat);
            }

            return dynamicCategories;
        }
        // Helper functions
        private bool UserExists(string EmailAddress)
        {
            using var db = new NorthwindContext();
            var user = db.UserLogin.Where(zz => zz.EmailAddress == EmailAddress).FirstOrDefault();

            return user != null;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
