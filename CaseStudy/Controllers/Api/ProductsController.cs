using CaseStudy.Services;
using System.Linq;
using System.Web.Http;

namespace CaseStudy.Controllers.Api
{
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IGoldPriceService _goldService;

        public ProductsController()
        {
            _goldService = new GoldPriceService();
            _productService = new ProductService(_goldService);
        }

        [HttpGet]
        public IHttpActionResult Get(double? minPrice = null, double? maxPrice = null,
                                     double? minPop = null, double? maxPop = null,
                                     string sort = null)
        {
            var list = _productService.GetProducts();

            if (minPrice.HasValue) list = list.Where(x => x.PriceUSD >= minPrice.Value).ToList();
            if (maxPrice.HasValue) list = list.Where(x => x.PriceUSD <= maxPrice.Value).ToList();
            if (minPop.HasValue) list = list.Where(x => x.PopularityScore >= minPop.Value).ToList();
            if (maxPop.HasValue) list = list.Where(x => x.PopularityScore <= maxPop.Value).ToList();

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "price_asc") list = list.OrderBy(x => x.PriceUSD).ToList();
                if (sort == "price_desc") list = list.OrderByDescending(x => x.PriceUSD).ToList();
            }

            var gold = _goldService.GetGoldPricePerGramAsync().Result;
            return Ok(new { goldPricePerGram = gold, products = list });
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var list = _productService.GetProducts();
            var p = list.FirstOrDefault(x => x.Id == id);
            if (p == null) return NotFound();
            return Ok(p);
        }
    }
}
