using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Data;
using Quiz.Services;

namespace Quiz.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //var questionService = serviceProvider.GetService<IQuestionService>();
            //questionService.Add("1+1", 1);

            //var answerService = serviceProvider.GetService<IAnswerService>();
            //answerService.Add("2", 5, true, 2);

            //var userAnswerService = serviceProvider.GetService<IUserAnswerService>();
            //userAnswerService.AddUserAnswer("09cb8789-c2f6-45c3-a6fa-9615a0e7f460", 1, 2, 1);

            var userAnswerService = serviceProvider.GetService<IUserAnswerService>();
            var result = userAnswerService.GetUserResult("09cb8789-c2f6-45c3-a6fa-9615a0e7f460", 1);
            Console.WriteLine(result);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();
        }
    }
}
