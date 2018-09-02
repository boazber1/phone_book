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
        private ContactViewModel _viewModel;
        
        public ForumView(Contacts.Model.Contact contact, List<City> cities, List<PhoneType> phoneTypes)
        {
            InitializeComponent();

            _viewModel = new ContactViewModel(contact, cities, phoneTypes);

            DataContext = _viewModel;
            _viewModel.AddPhoneContactClicked += OnAddPhoneClicked;
        }

        private void OnAddPhoneClicked(object sender, ContactEventArgs args)
        {
            //TODO Open Add Phone Window

        }


    }
}
