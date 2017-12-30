﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsApi.Model;

namespace ProductsApi.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        private readonly ProductsApiContext _context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ProductsApiContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public List<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Tüm Ürünler Listeleniyor");
                return _context?.Products?.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm ürünler listelenirken hata alındı");
                return null;
            }
        }

        //[HttpGet("/title_{title}")]
        //public Product GetTitle(string title)
        //{
        //    try
        //    {
        //        var product = _context.Products.Where(k => k.Title.Contains(title));
        //        return product?.FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Kayıt getirme işleminde hata oluştu.");
        //        return null;
        //    }
        //}

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            try
            {
                var product = _context?.Products?.Where(k => k.Id == id);
                return product?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt getirme işleminde hata oluştu.");
                return null;
            }
        }

        [HttpPost]
        public void AddProduct([FromBody] Product value)
        {
            var product = new Product
            {
                Title = value.Title,
                Price = value.Price,
                CreatedDate = DateTime.Now
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public void UpdateProduct(int id, [FromBody] Product value)
        {
            if (id > 0)
            {
                var product = new Product
                {
                    Id = id,
                    Title = value.Title,
                    Price = value.Price,
                    ModifiedDate = DateTime.Now
                };
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                _context.Products.Remove(new Product() { Id = id });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, !_context.Products.Any(i => i.Id == id) ? "Kayıt bulunamadı" : "Silme işleminde hata oluştu");
            }
        }
    }
}