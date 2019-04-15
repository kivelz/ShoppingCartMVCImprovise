using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssShopCart.Models;

namespace IssShopCart.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string search)
        {
            List<Product> products = new List<Product>();

            using (ShopModelContainer db = new ShopModelContainer())
            {
                if (search == null)
                {
                    products = db.Products.ToList();
                }
                else
                {
                    products = db.Products.Where(x => x.Name.Contains(search) || x.Desc.Contains(search)).ToList();
                }
            }
                return View("index", products);
        }
    }
}