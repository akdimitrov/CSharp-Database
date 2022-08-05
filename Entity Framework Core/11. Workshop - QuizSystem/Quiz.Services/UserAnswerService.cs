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

        public void AddUserAnswer(string userName, int questionId, int answerId)
        {
            var userId = dbContext.Users
               .Where(x => x.UserName == userName)
               .Select(x => x.Id)
               .FirstOrDefault();

            var userAnswer = dbContext.UserAnswers
                .FirstOrDefault(x => x.IdentityUserId == userId
                                && x.QuestionId == questionId);

            userAnswer.AnswerId = answerId;
            dbContext.SaveChanges();
        }

        public int GetUserResult(string userName, int quizId)
        {
            var userId = dbContext.Users
               .Where(x => x.UserName == userName)
               .Select(x => x.Id)
               .FirstOrDefault();

            var totalPoints = dbContext.UserAnswers
                .Where(x => x.IdentityUserId == userId 
                        && x.Question.QuizId == quizId)
                .Sum(x => x.Answer.Points);

            return totalPoints;
        }
    }
}
