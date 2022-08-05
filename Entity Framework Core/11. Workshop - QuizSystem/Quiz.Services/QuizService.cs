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
                Id = quizId,
                Title = quiz.Title,
                Questions = quiz.Questions
                .OrderBy(r => Guid.NewGuid())
                .Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Answers = q.Answers
                    .OrderBy(r => Guid.NewGuid())
                    .Select(a => new AnswerViewModel
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

        public IEnumerable<UserQuizViewModel> GetQuizesByUserName(string userName)
        {
            var quizes = dbContext.Quizes
                .Select(x => new UserQuizViewModel
                {
                    QuizId = x.Id,
                    Title = x.Title
                })
                .ToList();

            foreach (var quiz in quizes)
            {
                var questionsCount = dbContext.UserAnswers
                        .Count(ua => ua.IdentityUser.UserName == userName
                                && ua.Question.QuizId == quiz.QuizId);

                if (questionsCount == 0)
                {
                    quiz.Status = QuizStatus.NotStarted;
                    continue;
                }

                var answeredQuestions = dbContext.UserAnswers
                        .Count(ua => ua.IdentityUser.UserName == userName
                                && ua.Question.QuizId == quiz.QuizId
                                && ua.AnswerId.HasValue);


                quiz.Status = answeredQuestions == questionsCount
                    ? QuizStatus.Finished
                    : QuizStatus.InProgress;
            }

            return quizes;
        }

        public void StartQuiz(string userName, int quizId)
        {
            if (dbContext.UserAnswers.Any(x => x.IdentityUser.UserName == userName
                && x.Question.QuizId == quizId))
            {
                return;
            }

            var userId = dbContext.Users
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .FirstOrDefault();

            var questions = dbContext.Questions
                .Where(x => x.QuizId == quizId)
                .Select(x => new { x.Id })
                .ToList();

            foreach (var question in questions)
            {
                dbContext.UserAnswers.Add(new UserAnswer
                {
                    AnswerId = null,
                    IdentityUserId = userId,
                    QuestionId = question.Id
                });
            }

            dbContext.SaveChanges();
        }
    }
}
