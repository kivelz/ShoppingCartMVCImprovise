using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace IssShopCart.Models.ViewModel
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Username is required")]
       public string Username { get; set; }
       [Required(ErrorMessage = "Password Is Required")]
       [DataType(DataType.Password)]
       [MinLength(6, ErrorMessage = "Password consist of  6 characters")]
       public string Password { get; set; }
       public bool RememberMe { get; set; }
    }
}