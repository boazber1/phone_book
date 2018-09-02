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
        public ContactsView()
        {
            InitializeComponent();
            var viewModel  = new ContactsViewModel();
            viewModel.OnNewContactClicked += OpenForumOnAddNewcontact;
            viewModel.OnEdit += openEditOrAddForm;
            //viewModel.OnEditContactClicked += OpenForumOnEditNewcontact;
            DataContext = viewModel;          
        }

        private void openEditOrAddForm(object sender, ContactsEventArgs args)
        {
            if(args != null)
            {
                var contact = args.Contact;
                var cities = args.Cities;
                if(contact != null && cities != null)
                {
                    ForumView forumView = new ForumView(contact, cities);
                    forumView.ShowDialog();
                }

            }
                
        }

        public void OpenForumOnAddNewcontact(object sender, EventArgs e)
        {
            ForumView forumView = new ForumView(null, null);
            forumView.ShowDialog();
           
        }

        public void OpenForumOnEditNewcontact(object sender, EventArgs e)
        {
            
        }
    }
}
