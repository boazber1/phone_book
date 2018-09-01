using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhoneBook.ViewModels.Contacts.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dapper;


namespace PhoneBook.ViewModels.Forum.ViewModel
{
    class ForumViewModel: ViewModelBase
    {
        public ICommand SaveEditCommand { get; set; }
        public ICommand CancelEditCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }
        public ICommand AddPhoneNumberCommand { get; set; }

        public ForumViewModel()
        {
            SaveEditCommand = new RelayCommand(SaveEdit);
            CancelEditCommand = new RelayCommand(CancelEdit);
            DeletePhoneCommand = new RelayCommand(DeletePhoneNumber);
            AddPhoneNumberCommand = new RelayCommand(AddPhoneNumber);
        }

        private void AddPhoneNumber()
        {
            throw new NotImplementedException();
        }

        private void DeletePhoneNumber()
        {
            throw new NotImplementedException();
        }

        private void CancelEdit()
        {
            throw new NotImplementedException();
        }

        private void SaveEdit()
        {
            throw new NotImplementedException();
        }
    }
}
