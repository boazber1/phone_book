using PhoneBook.ViewModels.Contact;
using PhoneBook.ViewModels.Contacts.ViewModel;
using PhoneBook.ViewModels.Forum.View;
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

namespace PhoneBook.ViewModels.Contacts.View
{
    /// <summary>
    /// Interaction logic for ContactsView.xaml
    /// </summary>
    public partial class ContactsView : Window
    {
        private ContactsViewModel _viewModel;

        public ContactsView()
        {
            InitializeComponent();
            _viewModel  = new ContactsViewModel();
            _viewModel.OnNewContactClicked += OpenForumOnAddNewcontact;
            _viewModel.OnEdit += openEditOrAddForm;
            //viewModel.OnEditContactClicked += OpenForumOnEditNewcontact;
            DataContext = _viewModel;          
        }

        private void openEditOrAddForm(object sender, ContactsEventArgs args)
        {
            if(args != null)
            {
                var contact = args.Contact;
                var cities = args.Cities;
                var phoneTypes = args.PhoneTypes;
                if (contact != null && cities != null && phoneTypes != null)
                {
                    ForumView forumView = new ForumView(contact, cities, phoneTypes);
                    forumView.ShowDialog();
                    _viewModel.SearchCommand.Execute(null);
                }

            }
                
        }

        private void OpenForumOnAddNewcontact(object sender, ContactsEventArgs args)
        {

            if (args != null)
            {
                var contact = args.Contact;
                var cities = args.Cities;
                var phoneTypes = args.PhoneTypes;
                if (contact != null && cities != null && phoneTypes != null)
                {
                    ForumView forumView = new ForumView(contact, cities, phoneTypes);
                    forumView.ShowDialog();
                    _viewModel.SearchCommand.Execute(null);
                }

            }

        }

        public void OpenForumOnEditNewcontact(object sender, EventArgs e)
        {
            
        }
    }
}
