using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Data;
using Quiz.Models;

namespace Quiz.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext dbContext;

        public QuestionService(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public int Add(string title, int quizId)
        {
            var question = new Question
            {
                Title = title,
                QuizId = quizId
            };

            dbContext.Questions.Add(question);
            dbContext.SaveChanges();

            return question.Id;
        }
    }
}
