using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhoneBook.ViewModels.Contacts.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PhoneBook.ViewModels.AddPhone.ViewModel
{
    public class PhoneViewModel : ViewModelBase
    {
        private Phone _phone;
        
        public PhoneViewModel(Phone phone, List<PhoneType> phoneTypes)
        {
            _phone = phone;
            PhoneTypes = phoneTypes;
            SaveCommand = new RelayCommand(Save);            
            PhoneSaved = false;
        }

        public event EventHandler OnSaved;
        
        public int Id
        {
            get
            {
                return _phone.Id;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phone.PhoneNumber;
            }
            set
            {
                _phone.PhoneNumber = value;
                RaisePropertyChanged();
            }
        }

        public List<PhoneType> PhoneTypes { get; }

        public PhoneType SelectedPhoneType
        {
            get
            {
                return _phone.PhoneType;
            }
            set
            {
                _phone.PhoneType = value;
                RaisePropertyChanged();
            }
        }

        public bool PhoneSaved { get; set; }

        public ICommand SaveCommand { get; }

        public Phone GetModel() { return _phone; }

        private void Save()
        {
            PhoneSaved = true;
            if(OnSaved != null)
                OnSaved(this, EventArgs.Empty);
        }

    }
}
