using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using DataAccess.Enum;
using DataAccess.DTO;

namespace PRM_FlowerShop.Pages.FlowerShop.OrderPage
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public DetailsModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;
        public IList<OrderDetail> OrderDetail { get; set; } = default!;
        public PaginatedList<OrderDetail> OrderDetailPaginatedList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, int? pageIndex)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            List<string> statusList = Enum.GetValues(typeof(StatusOrder))
                                              .Cast<StatusOrder>()
                                              .Select(v => v.ToString())
                                              .ToList();

            ViewData["StatusList"] = statusList;

            if (id == null || _context.Orders == null || _context.OrderDetails == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
                OrderDetail = await _context.OrderDetails
                    .Where(o => o.OrderId == id)
                    .Include(o => o.Flower)
                    .Include(o => o.Order)
                    .ToListAsync();
                IQueryable<OrderDetail> orderDetailList = from orderDetail
                     in _context.OrderDetails
                    .Where(o => o.OrderId == id)
                    .Include(o => o.Flower)
                    .Include(o => o.Order)
                                                              //.Include(flower => flower.OrderDetails)
                                                              //where flower.Fullname.Contains(_name)
                                                          select orderDetail;
                int pageSize = 1;
                OrderDetailPaginatedList = PaginatedList<OrderDetail>.ToPagedList(orderDetailList, pageIndex ?? 1, pageSize); ;
            }
            return Page();
        }
    }
}
