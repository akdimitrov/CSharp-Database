using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Data;
using Quiz.Models;

namespace Quiz.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext dbContext;

        public AnswerService(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public int Add(string title, int points, bool isCorrect, int questionId)
        {
            var answer = new Answer
            {
                Title = title,
                Points = points,
                IsCorrect = isCorrect,
                QuestionId = questionId
            };

            dbContext.Answers.Add(answer);
            dbContext.SaveChanges();

            return answer.Id;
        }
    }
}
