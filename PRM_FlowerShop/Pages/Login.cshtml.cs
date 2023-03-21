using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRM_FlowerShop.Pages
{
    public class LoginModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context; 
        public LoginModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }
        [BindProperty]
        public AccountLogin Account { get; set; }
        public void OnGet()
        {
        }
        public ActionResult OnPost() {
            if (ModelState.IsValid) {
                var admin = _context.Users.FirstOrDefault(admin => admin.Username.Equals(Account.userName) 
                && admin.UserPassword.Equals(Account.password));
                if (admin != null )
                {
                    if (admin.RoleId == 1) {
                        HttpContext.Session.SetInt32("role" , admin.RoleId.Value);
                        return RedirectToPage("/FlowerShop/FlowerPage/Index");
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "You dont have permision to do this!");
                        return Page();
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "User name or password invalid!");
                    return Page();
                }
            }
            return Page();
        }
    }
}
