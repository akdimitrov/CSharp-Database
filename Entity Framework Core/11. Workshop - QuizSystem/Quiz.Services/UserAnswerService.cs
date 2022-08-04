using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;

namespace Quiz.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext dbContext;

        public UserAnswerService(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public void AddUserAnswer(string userId, int quizid, int questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                IdentityUserId = userId,
                QuizId = quizid,
                QuestionId = questionId,
                AnswerId = answerId
            };

            dbContext.UserAnswers.Add(userAnswer);
            dbContext.SaveChanges();
        }

        public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        {
            var userAnswers = new List<UserAnswer>();

            foreach (var question in quizInputModel.Questions)
            {
                var userAnswer = new UserAnswer
                {
                    IdentityUserId = quizInputModel.UserId,
                    QuizId = quizInputModel.QuizId,
                    QuestionId = question.QuestionId,
                    AnswerId = question.AnswerId
                };

                userAnswers.Add(userAnswer);
            }

            dbContext.AddRange(userAnswers);
            dbContext.SaveChanges();
        }

        public int GetUserResult(string userId, int quizId)
        {
            var totalPoints = dbContext.Quizes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .ThenInclude(x => x.UserAnswers)
                .Where(x => x.Id == quizId &&
                    x.UserAnswers.Any(x => x.IdentityUserId == userId))
                .SelectMany(x => x.UserAnswers)
                .Where(x => x.Answer.IsCorrect)
                .Sum(x => x.Answer.Points);

            return totalPoints;
        }
    }
}
