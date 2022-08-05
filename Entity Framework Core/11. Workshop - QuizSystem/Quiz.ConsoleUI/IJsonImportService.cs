using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ConsoleUI
{
    public interface IJsonImportService
    {
        void Import(string fileName, string quizName);
    }
}
