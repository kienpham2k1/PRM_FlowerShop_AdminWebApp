using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace PRM_FlowerShop.Pages.FlowerShop.FlowerPage
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public DetailsModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

      public Flower Flower { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            if (id == null || _context.Flowers == null)
            {
                return NotFound();
            }

            var flower = await _context.Flowers.FirstOrDefaultAsync(m => m.FlowerId == id);
            if (flower == null)
            {
                return NotFound();
            }
            else 
            {
                Flower = flower;
            }
            return Page();
        }
    }
}
