using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using DataAccess.Enum;

namespace PRM_FlowerShop.Pages.FlowerShop.OrderPage
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;

        public EditModel(DataAccess.Models.FlowerShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        [BindProperty]
        public string status { get; set; }
        public enum Status { Waiting, Complete }
        public List<String> strings { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order =  await _context.Orders.Include(o => o.Customer).Include(o => o.OrderDetails).FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
           ViewData["CustomerId"] = new SelectList(_context.Users, "UserId", "PhoneNumber");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int orderId , string newOrderStatus)
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            Order foundOrder = await _context.Orders.Include(o => o.Customer).Include(o => o.OrderDetails).FirstOrDefaultAsync(m => m.OrderId == orderId);
            if (foundOrder == null)
            {
                return NotFound();
            }
            Order newOrder = foundOrder;
            newOrder.Status = (int) Enum.Parse<StatusOrder>(newOrderStatus);
            _context.Attach(newOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("/FlowerShop/OrderPage/Index");
        }



        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(Order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(Order.OrderId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        //private bool OrderExists(int id)
        //{
        //  return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        //}
    }
}
