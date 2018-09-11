using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.WebServices.Services;
using API.WebServices.Data;
using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace API.WebServices.Services
{
    public class TimeJob: Job
    {
        private static IServiceProvider _services;
        public TimeJob(IServiceProvider services)
        {
            _services = services;
        }

        [Invoke(Begin = "2018-2-1 0:0", Interval = 1000 * 3600 * 1, SkipWhileExecuting = true)]//1000 * 3600 * 12
        public static void Run()
        {
            ApplicationDbContext context = new ApplicationDbContext(_services.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            WebCrawler.GetAndSaveBlog(context);
            WebCrawler.GetAndSaveJobs(context);
        }
    }
}
