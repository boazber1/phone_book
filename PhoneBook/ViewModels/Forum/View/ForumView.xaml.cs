using PhoneBook.ViewModels.AddPhone.View;
using PhoneBook.ViewModels.AddPhone.ViewModel;
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
            _viewModel.Saved += OnSaved;
        }

        private void OnSaved(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnAddPhoneClicked(object sender, ContactEventArgs args)
        {
            var phoneTypes = args.PhoneTypes;
            var firstType = phoneTypes.First();
            var model = new Phone
            {
                Id = -1,
                PhoneNumber = string.Empty,
                PhoneType = firstType
            };
            var viewModel = new PhoneViewModel(model, phoneTypes);
            var window = new PhoneView(viewModel);
            window.ShowDialog();
            if (window.DialogResult.HasValue &&
                window.DialogResult.Value)
            {
                if(viewModel.PhoneSaved)
                {
                    var phone = viewModel.GetModel();
                    _viewModel.AddPhoneToList(model);
                }
            }

        }


    }
}
