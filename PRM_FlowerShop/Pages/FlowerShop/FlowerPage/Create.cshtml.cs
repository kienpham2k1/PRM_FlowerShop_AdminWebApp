using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Models;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using BusinessObject.IService;

namespace PRM_FlowerShop.Pages.FlowerShop.FlowerPage
{
    public class CreateModel : PageModel
    {
        private readonly FlowerShopContext _context;
        private readonly IUploadFileService _fileUploadFileService;
        private readonly IFireBaseStorage _fireBaseStorage;
        private readonly IHostingEnvironment _env;

        public CreateModel(FlowerShopContext context,
                           IUploadFileService fileUploadFileService,
                           IFireBaseStorage fireBaseStorage,
                           IHostingEnvironment environment)
        {
            _context = context;
            _fileUploadFileService = fileUploadFileService;
            _fireBaseStorage = fireBaseStorage;
            _env = environment;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            return Page();
        }

        [BindProperty]
        public Flower Flower { get; set; } = default!;
        [BindProperty]
        public IFormFile FormFile { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetInt32("role") == null) { return RedirectToPage("/Login"); }
            if (!ModelState.IsValid || _context.Flowers == null || Flower == null)
            {
                return Page();
            }

            FileStream stream;
            var folderPath = Path.Combine(_env.ContentRootPath, @"wwwroot\images");

            _fileUploadFileService.DeleteFolder(folderPath);
            if (FormFile != null)
            {
                string path = await _fileUploadFileService.UploadFile(FormFile);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                string link = await Task.Run(() => _fireBaseStorage.Upload(stream, FormFile.FileName));
                Flower.FlowerImage = link;
                stream.Close();
            }
            _context.Flowers.Add(Flower);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
