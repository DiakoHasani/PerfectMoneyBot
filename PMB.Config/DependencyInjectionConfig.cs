using Microsoft.Extensions.DependencyInjection;
using PMB.Business;
using PMB.Repository.Domain;
using PMB.Repository.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Config
{
    public class DependencyInjectionConfig
    {
        public void Set(IServiceCollection services)
        {
            SetRepository(services);
            SetBusiness(services);
        }

        private void SetRepository(IServiceCollection services)
        {
            services.AddScoped<DataContext, DataContext>();
            services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
            services.AddScoped<IErrorRepository, ErrorRepository>();
        }

        private void SetBusiness(IServiceCollection services)
        {
            services.AddScoped<IEx4IrBusiness, Ex4IrBusiness>();
            services.AddScoped<IPriceHistoryBusiness, PriceHistoryBusiness>();
            services.AddScoped<IErrorBusiness, ErrorBusiness>();
            services.AddScoped<IHdPayBusiness, HdPayBusiness>();
        }
    }
}
