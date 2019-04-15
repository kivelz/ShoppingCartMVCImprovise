using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace IssShopCart.Models.ViewModel
{
    public class CartVM
    {
       public  int id { get; set; }
       public string Name { get; set; }
       public int Quantity { get; set; }
       public decimal Price { get; set; }
       public decimal Total
       {
           get { return Quantity * Price; }
       }
       public string Image { get; set; }
       public System.Guid ActivationCode { get; set; }
    }
}