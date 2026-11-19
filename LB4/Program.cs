using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystemOOP
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; private set; }
        public int? BorrowedByUserId { get; private set; }

        public Book(int id, string title, string author)
        {
            if (id <= 0) throw new ArgumentException("ID книги має бути більшим за 0.");
            Id = id;
            Title = string.IsNullOrWhiteSpace(title) ? "Без назви" : title;
            Author = string.IsNullOrWhiteSpace(author) ? "Невідомий автор" : author;
            IsAvailable = true;
            BorrowedByUserId = null;
        }

        internal bool MarkAsBorrowed(int userId)
        {
            if (!IsAvailable) return false;
            IsAvailable = false;
            BorrowedByUserId = userId;
            return true;
        }

        internal bool MarkAsReturned()
        {
            if (IsAvailable) return false;
            IsAvailable = true;
            BorrowedByUserId = null;
            return true;
        }

        public override string ToString()
        {
            string status = IsAvailable ? "Доступна" : $"В оренді (ID читача: {BorrowedByUserId})";
            return $"[ID: {Id}] «{Title}» — {Author} | Статус: {status}";
        }
    }

    public class User
    {
        private readonly List<Book> _borrowedBooks = new List<Book>();

        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks.AsReadOnly();

        public User(int id, string name)
        {
            if (id <= 0) throw new ArgumentException("ID користувача має бути більшим за 0.");
            Id = id;
            Name = string.IsNullOrWhiteSpace(name) ? "Безіменний читач" : name;
        }

        internal void AddBorrowedBook(Book book)
        {
            if (!_borrowedBooks.Contains(book))
            {
                _borrowedBooks.Add(book);
            }
        }

        internal bool RemoveBorrowedBook(Book book)
        {
            return _borrowedBooks.Remove(book);
        }

        public override string ToString()
        {
            string booksList = _borrowedBooks.Count == 0
                ? "немає орендованих книг"
                : string.Join(", ", _borrowedBooks.Select(b => $"«{b.Title}» (ID: {b.Id})"));

            return $"[Читач ID: {Id}] {Name} | Книги на руках: {booksList}";
        }
    }

    public class Library
    {
        private readonly List<Book> _books = new List<Book>();
        private readonly List<User> _users = new List<User>();

        public void AddBook(Book book)
        {
            if (book == null) return;
            if (_books.Any(b => b.Id == book.Id))
            {
                Console.WriteLine($"[Помилка] Книга з ID {book.Id} вже існує в системі.");
                return;
            }
            _books.Add(book);
        }

        public void AddUser(User user)
        {
            if (user == null) return;
            if (_users.Any(u => u.Id == user.Id))
            {
                Console.WriteLine($"[Помилка] Користувач з ID {user.Id} вже зареєстрований.");
                return;
            }
            _users.Add(user);
        }

        public void LendBook(int userId, int bookId)
        {
            User? user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"[Помилка видачі] Користувача з ID {userId} не знайдено.");
                return;
            }

            Book? book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine($"[Помилка видачі] Книгу з ID {bookId} не знайдено в бібліотеці.");
                return;
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine($"[Відмова] Книга «{book.Title}» (ID: {book.Id}) вже видана іншому читачеві (ID: {book.BorrowedByUserId}).");
                return;
            }

            if (book.MarkAsBorrowed(user.Id))
            {
                user.AddBorrowedBook(book);
                Console.WriteLine($"[Успіх] Книгу «{book.Title}» видано читачеві {user.Name} (ID: {user.Id}).");
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            User? user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"[Помилка повернення] Користувача з ID {userId} не знайдено.");
                return;
            }

            Book? book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine($"[Помилка повернення] Книгу з ID {bookId} не знайдено.");
                return;
            }

            if (book.IsAvailable || book.BorrowedByUserId != user.Id)
            {
                Console.WriteLine($"[Відмова] Читач {user.Name} не орендував книгу «{book.Title}» (ID: {book.Id}).");
                return;
            }

            if (book.MarkAsReturned())
            {
                user.RemoveBorrowedBook(book);
                Console.WriteLine($"[Успіх] Читач {user.Name} повернув книгу «{book.Title}» до бібліотеки.");
            }
        }

        public void ShowAllBooks()
        {
            Console.WriteLine("\n=== УСІ КНИГИ В БІБЛІОТЕЦІ ===");
            if (_books.Count == 0)
            {
                Console.WriteLine("Бібліотека порожня.");
                return;
            }
            foreach (var book in _books)
            {
                Console.WriteLine(book);
            }
        }

        public void ShowAvailableBooks()
        {
            Console.WriteLine("\n=== ДОСТУПНІ ДЛЯ ОРЕНДИ КНИГИ ===");
            var available = _books.Where(b => b.IsAvailable).ToList();
            if (available.Count == 0)
            {
                Console.WriteLine("Немає доступних книг.");
                return;
            }
            foreach (var book in available)
            {
                Console.WriteLine(book);
            }
        }

        public void ShowBorrowedBooks()
        {
            Console.WriteLine("\n=== КНИГИ В ОРЕНДІ ===");
            var borrowed = _books.Where(b => !b.IsAvailable).ToList();
            if (borrowed.Count == 0)
            {
                Console.WriteLine("Усі книги зараз у бібліотеці.");
                return;
            }
            foreach (var book in borrowed)
            {
                Console.WriteLine(book);
            }
        }

        public void ShowAllUsers()
        {
            Console.WriteLine("\n=== ЗАРЕЄСТРОВАНІ КОРИСТУВАЧІ ===");
            if (_users.Count == 0)
            {
                Console.WriteLine("Користувачів немає.");
                return;
            }
            foreach (var user in _users)
            {
                Console.WriteLine(user);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Library library = new Library();

            library.AddBook(new Book(1, "Кобзар", "Тарас Шевченко"));
            library.AddBook(new Book(2, "Тіні забутих предків", "Михайло Коцюбинський"));
            library.AddBook(new Book(3, "1984", "Джордж Орвелл"));
            library.AddBook(new Book(4, "Чистий код", "Роберт Мартін"));

            library.AddUser(new User(101, "Олена Коваленко"));
            library.AddUser(new User(102, "Максим Бондаренко"));

            library.ShowAllBooks();
            library.ShowAllUsers();

            Console.WriteLine("\n--- ДЕМОНСТРАЦІЯ РОБОТИ СИСТЕМИ ---");

            library.ShowAvailableBooks();

            Console.WriteLine("\n1. Видача книг:");
            library.LendBook(userId: 101, bookId: 3);
            library.LendBook(userId: 102, bookId: 4);

            Console.WriteLine("\n2. Спроба видати вже орендовану або неіснуючу книгу:");
            library.LendBook(userId: 102, bookId: 3);
            library.LendBook(userId: 101, bookId: 99);

            library.ShowBorrowedBooks();
            library.ShowAllUsers();

            Console.WriteLine("\n3. Спроба некоректного повернення:");
            library.ReturnBook(userId: 102, bookId: 3);
            library.ReturnBook(userId: 101, bookId: 1);

            Console.WriteLine("\n4. Коректне повернення книги:");
            library.ReturnBook(userId: 101, bookId: 3);

            Console.WriteLine("\n5. Повторна перевірка доступності та видача іншому читачеві:");
            library.ShowAvailableBooks();
            library.LendBook(userId: 102, bookId: 3);

            library.ShowAllBooks();
            library.ShowAllUsers();

            Console.ReadLine();
        }
    }
}