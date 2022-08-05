using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Services.Models;

namespace Quiz.Services
{
    public interface IUserAnswerService
    {
        void AddUserAnswer(string userName, int questionId, int answerId);

        int GetUserResult(string userName, int quizId);
    }
}
