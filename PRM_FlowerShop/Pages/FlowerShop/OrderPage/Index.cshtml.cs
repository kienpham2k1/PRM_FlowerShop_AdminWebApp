using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Enum;
using DataAccess.DTO;
using System.Diagnostics;

namespace PRM_FlowerShop.Pages.FlowerShop.OrderPage
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public IndexModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;
        public PaginatedList<Order> OrderPaginatedList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string search_name)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            if (_context.Orders != null)
            {
                List<string> statusList = Enum.GetValues(typeof(StatusOrder))
                                              .Cast<StatusOrder>()
                                              .Select(v => v.ToString())
                                              .ToList();

                ViewData["StatusList"] = statusList;
                int pageSize = 2;
                IQueryable<Order> orderList = from order
                                     in _context.Orders.Include(o => o.Customer)
                                                  .Include(flower => flower.OrderDetails)
                                              where order.Customer.Username.Contains(search_name ?? "")
                                              select order;

                Order = await _context.Orders
                .Include(o => o.Customer).ToListAsync();
                OrderPaginatedList = PaginatedList<Order>.ToPagedList(orderList, pageIndex ?? 1, pageSize);
            }
                return Page();
        }
    }
}
