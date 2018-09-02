using PhoneBook.ViewModels.AddPhone.ViewModel;
using PhoneBook.ViewModels.Contacts.Model;
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

namespace PhoneBook.ViewModels.AddPhone.View
{
    /// <summary>
    /// Interaction logic for PhoneView.xaml
    /// </summary>
    public partial class PhoneView : Window
    {
        private readonly PhoneViewModel _viewModel;        

        public PhoneView(PhoneViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.OnSaved += OnSaved;
            DataContext = _viewModel;
            
        }

        private void OnSaved(object sender, EventArgs e)
        {            
            DialogResult = true;            
            Close();
        }

        private void OnCanceled(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
