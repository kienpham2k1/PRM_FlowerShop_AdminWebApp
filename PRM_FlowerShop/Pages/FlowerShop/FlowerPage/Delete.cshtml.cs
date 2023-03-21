using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PRM_FlowerShop.Pages.FlowerShop.FlowerPage
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public DeleteModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            bool error =false;
            if (id == null || _context.Flowers == null)
            {
                return NotFound();
            }
            var flower = await _context.Flowers.FindAsync(id);

            if (flower != null)
            {
                try
                {
                    Flower = flower;
                    _context.Flowers.Remove(Flower);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    error = true;
                }
                if (error) {
                    ModelState.AddModelError(string.Empty, "Can not delete this flower because of be using in another order!");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
