using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IssShopCart.Models;
using IssShopCart.Models.ViewModel;

namespace IssShopCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //defining the cart list
            var cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();

            //check if cart is empty
            if (cart.Count == 0 || Session["Cart"] == null)
            {
                ViewBag.Message = "Your Cart is empty";
            }

            //calculate total and save it to view bag

            decimal total= 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.Grandtotal = total;


            return View(cart);
        }

        public ActionResult CartPartial()
        {
            CartVM model = new CartVM();

            int qty = 0;

            decimal price = 0m;

            if (Session["Cart"] != null)
            {
                var cartList = (List<CartVM>) Session["Cart"];

                foreach (var item in cartList)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }

                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0m;
            }

            return PartialView(model);
        }

        public async Task<ActionResult> AddToCart(int id)
        {
            //initialize cartviewmodel as list
            List<CartVM> cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();

            //initialize cartvm as model
            CartVM model = new CartVM();

            using (ShopModelContainer db = new ShopModelContainer())
            {
                //get Product from DTO using await
                Product product = await db.Products.FindAsync(id);

                var productInCart = cart.FirstOrDefault(x => x.id == id);

                //Check if product is already in Cart
                if (productInCart == null)
                {
                    //if Null add new item to cart
                    cart.Add(new CartVM()
                    {
                        id = product.Id,
                        Name = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.Image
                    });
                }
                else
                {
                    //if product exist increase quantity
                    productInCart.Quantity++;
                }

            }
            //Get total Quantity and price and add to model

            //constructors
            int qty = 0;
            decimal price = 0m;


            
            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Price;
            }

            //setting model.quantity from the value qty
            model.Quantity = qty;
            model.Price = price;

            //saving cart back to session
            Session["Cart"] = cart;


            //return as a partial view passing the model value in it.
            return PartialView(model);
        }

        public JsonResult Increament(int productId)
        {
            //define the list
             List<CartVM> cart = Session["Cart"] as List<CartVM>;

            using (ShopModelContainer db = new ShopModelContainer())
            {
                //get cartVm from cart list
                CartVM model = cart.FirstOrDefault(x => x.id == productId);
                //increase quantity
                model.Quantity++;

                //store result required data cannot be null
                var result =  new {qty = model.Quantity, price = model.Price};
                //return as json result and allow get method from javascript
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DecreamentProduct(int productId)
        {
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            using (ShopModelContainer db = new ShopModelContainer())
            {
                CartVM model = cart.FirstOrDefault(x => x.id == productId);

                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }

                var result = new {qty = model.Quantity, price = model.Price};

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void RemoveProduct(int productId)
        {
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            using (ShopModelContainer db = new ShopModelContainer())
            {
                CartVM model = cart.FirstOrDefault(x => x.id == productId);

                cart.Remove(model);
            }
            
        }

        [HttpPost]
        public void Checkout()
        {
            List<CartVM> cart = Session["Cart"] as List<CartVM>;

            string username = User.Identity.Name;

            int orderId = 0;

            using (ShopModelContainer db = new ShopModelContainer()) 
            {
                Order orderDTO = new Order();

                var q = db.Users.FirstOrDefault(x => x.UserName == username);
                int userId = q.Id;

                orderDTO.UserId = userId;
                orderDTO.CreatedAt = DateTime.Now;

                db.Orders.Add(orderDTO);

                db.SaveChanges();
                orderId = orderDTO.Id;

                Order_details orderdetails = new Order_details();

                foreach (var item in cart)
                {
                    orderdetails.OrderId = orderId;
                    orderdetails.UserId = userId;
                    orderdetails.ProductId = item.id;
                    orderdetails.Quantity = item.Quantity;
          

                    db.Order_details.Add(orderdetails);

                    db.SaveChanges();

                }
            }

            Session["Cart"] = null;
        }
    }
    
}