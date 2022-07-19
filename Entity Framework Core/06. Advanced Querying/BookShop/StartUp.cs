namespace BookShop
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            // ---Task 02. Age Restriction
            //string ageGroup = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, ageGroup));
            
            // ---Task 03. Golden Books
            //Console.WriteLine(GetGoldenBooks(db));

            // ---Task 04. Books by Price
            //Console.WriteLine(GetBooksByPrice(db));

            // ---Task 05. Not Released In
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            // ---Task 06. Book Titles by Category
            //string categories = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, categories));

            // ---Task 07. Released Before Date
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            // ---Task 08. Author Search
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            // ---Task 09. Book Search
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            // ---Task 10. Book Search by Author
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, input));

            // ---Task 11. Count Books
            //int lengthCheck = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, lengthCheck));

            // ---Task 12. Total Book Copies
            //Console.WriteLine(CountCopiesByAuthor(db));

            // ---Task 13. Profit by Category
            //Console.WriteLine(GetTotalProfitByCategory(db));

            // ---Task 14. Most Recent Books
            //Console.WriteLine(GetMostRecentBooks(db));

            // ---Task 15. Increase Prices
            //IncreasePrices(db);

            // ---Task 16. Remove Books
            //Console.WriteLine(RemoveBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageGroup = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(x => x.AgeRestriction == ageGroup)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            decimal priceThreshold = 40;

            var books = context.Books
                .Where(x => x.Price > priceThreshold)
                .Select(x => new { x.Title, x.Price })
                .OrderByDescending(x => x.Price)
                .ToArray();

            return string.Join(Environment.NewLine,
                books.Select(x => $"{x.Title} - ${x.Price:f2}"));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split();

            var books = context.Books
                .Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var targetDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate < targetDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                })
                .ToArray();

            return string.Join(Environment.NewLine,
                books.Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}"));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName != null && x.FirstName.EndsWith(input))
                .Select(x => x.FirstName + " " + x.LastName)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    x.Title,
                    Authror = x.Author.FirstName + " " + x.Author.LastName
                })
                .ToArray();

            return string.Join(Environment.NewLine,
                books.Select(x => $"{x.Title} ({x.Authror})"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(x => x.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var result = context.Authors
                .Select(x => new
                {
                    Author = x.FirstName + " " + x.LastName,
                    TotalCopies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.TotalCopies)
                .ToArray();

            return string.Join(Environment.NewLine,
                result.Select(x => $"{x.Author} - {x.TotalCopies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result = context.Categories
                .Select(x => new
                {
                    x.Name,
                    Profit = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
                })
                .OrderByDescending(x => x.Profit)
                .ThenBy(x => x.Name)
                .ToArray();

            return string.Join(Environment.NewLine,
                result.Select(x => $"{x.Name} ${x.Profit:f2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(x => new
                {
                    x.Name,
                    Books = x.CategoryBooks
                    .OrderByDescending(x => x.Book.ReleaseDate)
                    .Select(x => new
                    {
                        BookTitle = x.Book.Title,
                        ReleaseYear = x.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                })
                .OrderBy(x => x.Name)
                .ToArray();

            var sb = new StringBuilder();
            foreach (var category in books)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToArray();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Length;
        }
    }
}
