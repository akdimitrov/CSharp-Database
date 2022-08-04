using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Services.Models;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext dbContext;

        public QuizService(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public int Add(string title)
        {
            var quiz = new Quiz.Models.Quiz
            {
                Title = title
            };

            dbContext.Quizes.Add(quiz);
            dbContext.SaveChanges();

            return quiz.Id;
        }

        public QuizViewModel GetQuizById(int quizId)
        {
            var quiz = dbContext.Quizes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var quizViewModel = new QuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                    .ToList()
                })
                .ToList()
            };

            return quizViewModel;
        }
    }
}
