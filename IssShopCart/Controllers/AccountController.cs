using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IssShopCart.Models;
using IssShopCart.Models.ViewModel;

namespace IssShopCart.Controllers
{
    public class AccountController : Controller
    {
        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserVm user)
        {
            //if formstate is not valid, return view
           if (!ModelState.IsValid)
            {
                return View("register", user);
            }

            //query SQL database to see if username is already taken. Can be used to check email also
            using (ShopModelContainer db = new ShopModelContainer())
            {
                if (db.Users.Any(x => x.UserName.Equals(user.UserName)))
                {
                    ModelState.AddModelError("", "Username" + user.UserName + " is already taken");
                    user.UserName = "";
                    return View("Register", user);
                }
                //if doesn't exist, set user form attribute and save to database
                User userDTO =  new User()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password
                };
                //db is define as database, Users is UserDTO which is to communicate with database and pass in userdto which is a new user
                db.Users.Add(userDTO);
                db.SaveChanges();
            }
            //set TEMPDATA to a Message - tempdata can only be used once per action can't be used to save object
              TempData["SM"] = "You have successfully registered";

            return View(user);

        }
        [HttpGet]
        public ActionResult Login()
        {
            //if user already login return view
           string username =   User.Identity.Name;
            return View();
        }

        [HttpPost]
        
        public ActionResult Login(LoginVm user)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Check if the user is valid
            bool isValid = false;

            //query sql database to find if user and password are the same
            using (ShopModelContainer db = new ShopModelContainer())
            {
                if (db.Users.Any(x => x.UserName.Equals(user.Username) && x.Password.Equals(user.Password)))
                {
                    //if yes set isValid to true
                    isValid = true;
                }
            }

            //If its not valid, which username or password or wrong
            if (!isValid)
            {
                //set model error message to invalid username or password
                ModelState.AddModelError("", "Invalid username or password.");
                return View(user);
            }
            else
            {
                //use libary to set cookie by usingname and if RememberMe is true
                FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                //if true, redirect formauthentication and redirecturl which is set in web.confirm Authentication mode
                return Redirect(FormsAuthentication.GetRedirectUrl(user.Username, user.RememberMe));
            }
            
        }

        public ActionResult UserNavPartial()
        {
            //setting username as user.identity.name which is username
            string username = User.Identity.Name;

            //setting UserNavPartial as userinfo
            Models.ViewModel.UserNavPartial userInfo;

            //query SQL database to see if username exist
            using (ShopModelContainer db = new ShopModelContainer())
            {
                User dto =  db.Users.FirstOrDefault(x => x.UserName == username);

                //if exist, set userInfo as firstname and last Name
                userInfo = new UserNavPartial()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName

                };
                return PartialView(userInfo);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Account/Login");
        }

        public ActionResult Orders()
        {
            //define list for orderforUserVM
            List<OrderForUserVM> purchaseHistory = new List<OrderForUserVM>();


            //Querying SQL database for list of orders
            using (ShopModelContainer db = new ShopModelContainer())
            {
                User user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                int userId = user.Id;

                //initialize list of orders
                List<OrderVm> orders = db.Orders.Where(x => x.UserId == userId).ToArray().Select(x => new OrderVm(x))
                    .ToList();

                //loop through all the items in orderVM
                foreach (var order in orders)
                {
                    //set dictornary to products and quantity
                    Dictionary<string, int> productsAndQuantity = new Dictionary<string, int>();
                    //define total
                    decimal total = 0m;

                   
               
                    //initialize list for orderDetailsDTO
                    List<Order_details> orderDetailDTO =
                        db.Order_details.Where(x => x.OrderId == order.OrderId).ToList();

                    foreach (var orderDetail in orderDetailDTO)
                    {
                        //get the product
                        Product product = db.Products.Where(x => x.Id == orderDetail.ProductId).FirstOrDefault();
                        //get product price
                        decimal price = product.Price;
                        //get product name
                        string productName = product.Name;
                    
                        //add product to dictonary
                        productsAndQuantity.Add(productName, orderDetail.Quantity);
                       
                        //get total
                        total += orderDetail.Quantity * price;

                        //get ActivationCode
                    

                    }

                    //add orders to orderforuser list
                    purchaseHistory.Add( new OrderForUserVM()
                    {
                        OrderNumber =  order.OrderId,
                        Total = total,
                        ProductsAndQty = productsAndQuantity,
                        CreatedAt = order.CreatedAt, 
                        });
                }
            }

            return View(purchaseHistory);
        }
    }
}