using DbFirstModelInEntity.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;

namespace DbFirstModelInEntity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<Book> allBooks;

        public ObservableCollection<Book> AllBooks
        {
            get { return allBooks; }
            set { allBooks = value; OnPropertyChanged(); }
        }

        public async void CallAdd()
        {
           string a=await AddAsync();
            string b = "";
        }
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            AllBooks = new ObservableCollection<Book>();
            Remove();
            GetAsync();
            // Update();
            CallAdd();
        }

        private async void GetAsync()
        {
            using (var context=new LibraryEntities())
            {

                //var book = await context
                //    .Books
                //    .Include(nameof(Book.Author))
                //    .Include(nameof(Book.Category))
                //    .FirstOrDefaultAsync(b => b.Id == 2);
                //var list= new ObservableCollection<Book>();
                //list.Add(book);
                //myGrid.ItemsSource = list;

                var books = context.Books.Include(nameof(Book.Author)).Include(nameof(Book.Category));
                AllBooks = new ObservableCollection<Book>(books);
            }
        }


        public async Task<string> AddAsync()
        {
            using (var context = new LibraryEntities())
            {
                var book = new Book
                {
                    Name = "My New Book",
                    AuthorId = 1,
                    CategoryId = 1,
                    Pages = 1111
                };


                context.Entry(book).State = EntityState.Added;
                GetAsync();
                return (await context.SaveChangesAsync()).ToString();

            }


        }


        public async void Update()
        {
            using (var context = new LibraryEntities())
            {

                var book = await context
                    .Books
                    .Include(nameof(Book.Author))
                    .Include(nameof(Book.Category))
                    .FirstOrDefaultAsync(b => b.Id == 2);
                if (book != null)
                {

                book.Name = "My Updated Book";
                context.Entry(book).State = EntityState.Modified;
                var result=await context.SaveChangesAsync();


                GetAsync();
                }
            }


            
        }

        public async void Remove()
        {
            using (var context=new LibraryEntities())
            {
                var book = await context
                   .Books
                   .Include(nameof(Book.Author))
                   .Include(nameof(Book.Category))
                   .FirstOrDefaultAsync(b => b.Id == 1);

                if (book != null)
                {
                    //context.Books.Remove(book);
                    //await context.SaveChangesAsync();


                    context.Entry(book).State= EntityState.Deleted;

                    await context.SaveChangesAsync();


                }
                GetAsync();
            }
        } 

    }
}
