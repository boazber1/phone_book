using PhoneBook.ViewModels.Contact;
using PhoneBook.ViewModels.Contacts.Model;
using PhoneBook.ViewModels.Forum.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook.ViewModels.Forum.View
{
    /// <summary>
    /// Interaction logic for ForumView.xaml
    /// </summary>
    public partial class ForumView : Window
    {
        public ForumView(Contacts.Model.Contact contact, List<City> cities)
        {
            InitializeComponent();
            DataContext = new ContactViewModel(contact, cities);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
