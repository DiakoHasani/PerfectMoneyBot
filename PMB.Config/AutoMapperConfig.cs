using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PMB.Business.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Config
{
    public class AutoMapperConfig
    {
        public void Config(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
