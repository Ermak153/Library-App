using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp1;
public partial class MainWindow : Window
{
    private ObservableCollection<User> users = new ObservableCollection<User>();
    private ObservableCollection<Book> books = new ObservableCollection<Book>();
    private ObservableCollection<Loan> loans = new ObservableCollection<Loan>();

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Year { get; set; }
        public int Count { get; set; }
    }

    public class Loan
    {
        public User User { get; set; }
        public Book Book { get; set; }
    }

    public MainWindow()
    {
        InitializeComponent();
        userListView.ItemsSource = users;
        bookListView.ItemsSource = books;
        issuedBooksListView.ItemsSource = loans;

        // Пример добавления пользователей и книг
        users.Add(new User { Id = 1, Name = "Тамерлан", Family = "Сырат" });
        users.Add(new User { Id = 2, Name = "Глеб", Family = "Гааг" });
        users.Add(new User { Id = 3, Name = "Анастасия", Family = "Иванова" });
        users.Add(new User { Id = 4, Name = "Андрей", Family = "Воркунов" });
        users.Add(new User { Id = 5, Name = "Полина", Family = "Красных" });
        users.Add(new User { Id = 6, Name = "Дмитрий", Family = "Емельянов" });
        users.Add(new User { Id = 7, Name = "Григорий", Family = "Белявцев" });
        users.Add(new User { Id = 8, Name = "Александр", Family = "Монич" });

        books.Add(new Book { Title = "Блэкаут", Author = "Александр Левченко", Year = DateTime.Now, Count = 5 });
        books.Add(new Book { Title = "Жизнь на продажу", Author = "Юкио Мисима", Year = DateTime.Now, Count = 3 });
        books.Add(new Book { Title = "1984", Author = "Джордж Оруэлл", Year = DateTime.Now, Count = 4 });
        books.Add(new Book { Title = "Преступление и наказание", Author = "Фёдор Достоевский", Year = DateTime.Now, Count = 7 });
        books.Add(new Book { Title = "Мартин Иден", Author = "Джек Лондон", Year = DateTime.Now, Count = 2 });
        books.Add(new Book { Title = "Маленький принц", Author = "Антуан де Сент-Экзюпери", Year = DateTime.Now, Count = 1 });
        books.Add(new Book { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Year = DateTime.Now, Count = 8 });
        books.Add(new Book { Title = "Отцы и дети", Author = "Иван Тургенев", Year = DateTime.Now, Count = 22 });


        // Пример добавления выданных книг
        loans.Add(new Loan { User = users[0], Book = books[0] });
    }

    private void AddUserButton_Click(object sender, RoutedEventArgs e)
    {
        string name = userNameTextBox.Text.Trim();
        string family = userFamilyTextBox.Text.Trim();

        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(family))
        {
            User newUser = new User { Id = users.Count + 1, Name = name, Family = family };
            users.Add(newUser);
            userNameTextBox.Clear();
            userFamilyTextBox.Clear();
        }
        else
        {
            MessageBox.Show("Пожалуйста, введите имя и фамилию пользователя.");
        }
    }

    private void IssueBookButton_Click(object sender, RoutedEventArgs e)
    {
        User selectedUser = (User)userListView.SelectedItem;
        Book selectedBook = (Book)bookListView.SelectedItem;

        if (selectedUser != null && selectedBook != null && selectedBook.Count > 0)
        {
            Loan newLoan = new Loan { User = selectedUser, Book = selectedBook };
            loans.Add(newLoan);

            // Уменьшение количества книг
            selectedBook.Count--;

            // Обновление отображения количества книг в списке книг
            CollectionViewSource.GetDefaultView(books).Refresh();
        }
        else
        {
            MessageBox.Show("Выберите пользователя и доступную книгу для выдачи.");
        }
    }

    private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
    {
        Loan selectedLoan = (Loan)issuedBooksListView.SelectedItem;

        if (selectedLoan != null)
        {
            selectedLoan.Book.Count++;

            // Удаление записи о возврате из коллекции loans
            loans.Remove(selectedLoan);

            // Обновление отображения количества книг в списке книг
            CollectionViewSource.GetDefaultView(books).Refresh();
        }
        else
        {
            MessageBox.Show("Выберите книгу для возврата.");
        }
    }

    private void FindUserButton_Click(object sender, RoutedEventArgs e)
    {
        string searchText = userSearchBox.Text.Trim();

        // Проверка на пустой запрос
        if (string.IsNullOrEmpty(searchText))
        {
            MessageBox.Show("Пожалуйста, введите имя или фамилию пользователя для поиска.");
            return;
        }

        // Очистка выделения в списке пользователей
        userListView.SelectedItem = null;

        // Проход по списку пользователей и поиск
        foreach (User user in users)
        {
            if (user.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                user.Family.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                // Найден пользователь, выделение в списке
                userListView.SelectedItem = user;
                break; // Выход из цикла после первого совпадения
            }
        }

        // Если пользователь не найден, выведите сообщение
        if (userListView.SelectedItem == null)
        {
            MessageBox.Show("Пользователь не найден.");
        }
    }
    private void FindBookButton_Click(object sender, RoutedEventArgs e)
    {
        string searchText = bookSearchBox.Text.Trim();

        // Проверка на пустой запрос
        if (string.IsNullOrEmpty(searchText))
        {
            MessageBox.Show("Пожалуйста, введите название книги или автора для поиска.");
            return;
        }

        // Очистка выделения в списке книг
        bookListView.SelectedItem = null;

        // Проход по списку книг и поиск
        foreach (Book book in books)
        {
            if (book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                // Найдена книга, выделение в списке
                bookListView.SelectedItem = book;
                break; // Выход из цикла после первого совпадения
            }
        }

        // Если книга не найдена, выведите сообщение
        if (bookListView.SelectedItem == null)
        {
            MessageBox.Show("Книга не найдена.");
        }
    }

}
