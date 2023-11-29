using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.User_Services
{
    public class SofiaCallWebCrawlerService
    {
        private readonly IMapper _mapper;

        public SofiaCallWebCrawlerService(IMapper mapper)
        {
            _mapper = mapper;
        }


    }
}
