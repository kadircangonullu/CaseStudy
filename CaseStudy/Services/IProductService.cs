using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CaseStudy.Models;
using System.Linq;
using System.Web;

namespace CaseStudy.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Task<List<Product>> GetProductsAsync();
    }

}