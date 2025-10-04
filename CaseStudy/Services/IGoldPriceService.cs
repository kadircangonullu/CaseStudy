using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CaseStudy.Services
{
    public interface IGoldPriceService
    {
        Task<double> GetGoldPricePerGramAsync();
    }
}