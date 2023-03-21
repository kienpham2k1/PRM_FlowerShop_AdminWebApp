using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.IService;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
namespace PRM_FlowerShop.Pages.FlowerShop.FlowerPage
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Models.FlowerShopContext _context;
        private readonly IUploadFileService _fileUploadFileService;
        private readonly IFireBaseStorage _fireBaseStorage;
        private readonly IHostingEnvironment _env;
        public EditModel(DataAccess.Models.FlowerShopContext context, IUploadFileService fileUploadFileService,
                           IFireBaseStorage fireBaseStorage,
                           IHostingEnvironment environment)
        {
            _context = context;
            _fileUploadFileService = fileUploadFileService;
            _fireBaseStorage = fireBaseStorage;
            _env = environment;
        }

        [BindProperty]
        public Flower Flower { get; set; } = default!;
        [BindProperty]
        public IFormFile FormFile { get; set; }

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
            Flower = flower;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            FileStream stream;

            if (FormFile != null)
            {
                string path = await _fileUploadFileService.UploadFile(FormFile);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                string link = await Task.Run(() => _fireBaseStorage.Upload(stream, FormFile.FileName));
                Flower.FlowerImage = link;

                stream.Close();
            }
            var folderPath = Path.Combine(_env.ContentRootPath, @"wwwroot\images");
            _fileUploadFileService.DeleteFolder(folderPath);
            _context.Attach(Flower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerExists(Flower.FlowerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FlowerExists(int id)
        {
            return (_context.Flowers?.Any(e => e.FlowerId == id)).GetValueOrDefault();
        }
    }
}
