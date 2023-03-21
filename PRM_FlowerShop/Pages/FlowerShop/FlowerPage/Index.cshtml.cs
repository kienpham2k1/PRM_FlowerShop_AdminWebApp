using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using DataAccess.DTO;
using System.Drawing.Printing;

namespace PRM_FlowerShop.Pages.FlowerShop.FlowerPage
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public IndexModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

        public IList<Flower> Flower { get; set; } = default!;
        public PaginatedList<Flower> FlowerPaginatedList { get; set; }

        public ActionResult OnGet(int? pageIndex, string search_name)
        {
            //if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            IQueryable<Flower> pageList = from flower
                                      in _context.Flowers
                                         .Include(flower => flower.OrderDetails)
                                         where flower.FowerName.Contains(search_name?? "")
                                     select flower;
            //if (_context.Flowers != null)
            //{
            //    Flower = await _context.Flowers.ToListAsync();
            //}
            int pageSize = 5;
            FlowerPaginatedList = PaginatedList<Flower>.ToPagedList(pageList, pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
