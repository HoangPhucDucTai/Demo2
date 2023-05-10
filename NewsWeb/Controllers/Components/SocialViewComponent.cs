using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NewsWeb.Enums;
using NewsWeb.Models;
using NewsWeb.ModelViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Controllers.Components
{
    public class SocialViewComponent: ViewComponent
    {
        private readonly IConfiguration _config;
        private IMemoryCache _memoryCache;
        public SocialViewComponent(IMemoryCache memoryCache, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _config = config;
        }
        public IViewComponentResult Invoke()
        {
            var _social = _memoryCache.GetOrCreate(CacheKeys.Social, entry =>
            {
                entry.SlidingExpiration = TimeSpan.MaxValue;
                return GetlsSocials();
            });
            return View(_social);
        }
        public SocialVM GetlsSocials()
        {
            SocialVM socialVM = new SocialVM();
            socialVM.Facebook = _config.GetValue<string>("SocialLinks:facebook");
            socialVM.Twitter = _config.GetValue<string>("SocialLinks:twitter");
            socialVM.Instagram = _config.GetValue<string>("SocialLinks:instagram");
            socialVM.Pinterest = _config.GetValue<string>("SocialLinks:pinterest");
            socialVM.Youtube = _config.GetValue<string>("SocialLinks:youtube");

            return socialVM;
        }

    }
    
}
