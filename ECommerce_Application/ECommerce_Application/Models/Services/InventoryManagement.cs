﻿using ECommerce_Application.Data;
using ECommerce_Application.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_Application.Models.Services
{

    /// <summary>
    /// Referencing the Interface Product
    /// </summary>
    public class InventoryManagement : IProduct
    {
        private readonly StoreDbContext _context;
        private readonly IConfiguration _config;
        private readonly IImage _image;

        public InventoryManagement(StoreDbContext context, IImage image, IConfiguration config)
        {
            _context = context;
            _config = config;
            _image = image;
        }


        /// <summary>
        /// Create the product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> CreateProduct(Product product)
        {

            product.Image = await _image.GetBlob(product.Name, _config["ImageContainer"]);
            _context.Entry(product).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return product;
        }


        /// <summary>
        /// Delete the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProduct(int id)
        {
            Product dog = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            _context.Entry(dog).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Get the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetProduct(int id)
        {
            var dog = await _context.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return dog;
        }



        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetProducts()
        {
            List<Product> dogs = await _context.Products
                .ToListAsync();

            return dogs;
        }


        /// <summary>
        /// Update the product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> UpdateProduct(Product product)
        {
            var dog = await _context.Products.FindAsync(product.Id);

            dog.Name = product.Name;
            dog.Price = product.Price;
            dog.SKU = product.SKU;
            dog.SuperPower = product.SuperPower;
            dog.Description = product.Description;
            dog.Color = product.Color;
            dog.Breed = product.Breed;
            dog.Age = product.Age;
            dog.Quantity = product.Quantity;
            if(dog.Image == null)
            {
            dog.Image = await _image.GetBlob(product.Name, _config["ImageContainer"]);

            }
            else
            {
                dog.Image = product.Image;
            }



            _context.Entry(dog).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return dog;
        }
    }
}
