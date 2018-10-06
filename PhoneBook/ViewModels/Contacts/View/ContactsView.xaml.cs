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
            AutoComplateBorder.Visibility = Visibility.Collapsed;
            _viewModel.AutoCompleteDbCollection.CollectionChanged += AutoCompleteDbCollection_CollectionChanged;
        }

        private void AutoCompleteDbCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs eventArgs)
        {

            switch (eventArgs.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    AutoComplateBorder.Visibility = System.Windows.Visibility.Visible;
                    var fullNames = eventArgs.NewItems.Cast<string>().ToList();
                    foreach (var fullName in fullNames)
                    {
                        var block = new TextBlock();

                        // Add the text   
                        block.Text = fullName;

                        // A little style...   
                        block.Margin = new Thickness(2, 3, 2, 3);
                        block.Cursor = Cursors.Hand;

                        // Mouse events   
                        block.MouseLeftButtonUp += (s, e) =>
                        {
                            SearchTextBox.Text = (s as TextBlock).Text;
                        };

                        block.MouseEnter += (s, e) =>
                        {
                            var b = s as TextBlock;
                            b.Background = Brushes.PeachPuff;
                        };

                        block.MouseLeave += (s, e) =>
                        {
                            var b = s as TextBlock;
                            b.Background = Brushes.Transparent;
                        };

                        // Add to the panel   
                        ResultStack.Children.Add(block);
                    }
                    
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    ResultStack.Children.Clear();
                    AutoComplateBorder.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }

           // throw new NotImplementedException();

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
