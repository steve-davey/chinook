﻿#region snippet_All
using Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Project.Pages.Albums
{
    public class DeleteModel : PageModel
    {
        private readonly Chinook _context;

        public DeleteModel(Chinook context)
        {
            _context = context;
        }

        [BindProperty]
        public Album Album { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, bool? saveChangesError = false)
        {

            Album = await _context.Albums
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AlbumId == id);

            if (Album == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            try
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Albums");
            }
            catch (DbUpdateException /* ex */)
            {
                //To do: log the error (uncomment ex variable name and write a log).
                return RedirectToAction("/Albums/Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}
#endregion