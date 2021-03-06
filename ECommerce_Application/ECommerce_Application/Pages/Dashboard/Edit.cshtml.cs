﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce_Application.Data;
using ECommerce_Application.Models;
using Microsoft.AspNetCore.Authorization;
using ECommerce_Application.Models.Interfaces;

namespace ECommerce_Application.Pages.Dashboard
{
    /// <summary>
    /// Only admin can edit
    /// </summary>
    [Authorize(Policy = "Administrator")]

    public class EditModel : PageModel
    {
        private readonly StoreDbContext _context;
        private readonly IImage _image;

        public EditModel(StoreDbContext context, IImage image)
        {
            _context = context;
            _image = image;
        }

        [BindProperty]
        public Product Product { get; set; }
        [BindProperty]
        public List<string> Blobs { get; set; }


        /// <summary>
        /// Get the product for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            Blobs = await _image.GetAllBlobs();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
