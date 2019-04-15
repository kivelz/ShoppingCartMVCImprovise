using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebGrease.Css;

namespace IssShopCart.Models.ViewModel
{
    public class UserVm
    {
        public UserVm()
        {

        }
        public UserVm(User row)
        {
            UserName = row.UserName;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Password = row.Password;

        }
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public  string LastName { get; set; }
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field cannot be empty")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password required 6 characters")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't Match")]
        public string ConfirmPassword { get; set; }
    }
}