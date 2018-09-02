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
using System.ComponentModel;
using PhoneBook.ViewModels.Forum.View;
using PhoneBook.ViewModels.Contact;
using static PhoneBook.ViewModels.Contact.ContactViewModel;
using System.Windows;

namespace PhoneBook.ViewModels.Contacts.ViewModel
{
    class ContactsEventArgs : ContactEventArgs
    {
        public List<City> Cities { get; set; }
    }

    class ContactsViewModel : ViewModelBase 
    {

        

        public ObservableCollection<ContactViewModel> ContactsList { get; set; }
        
        public ICommand SearchCommand { get; set; }        
        public ICommand EditContactCommand { get; set; }
        public ICommand AddNewContactCommand { get; set; }
        private string _searchString;
        private List<City> _cities;

        public event EventHandler OnNewContactClicked;
        public delegate void EditContactClickedEventHandler(object sender, ContactsEventArgs args);
        public event EditContactClickedEventHandler OnEdit;
        


        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged();
            }
        }

        public ContactsViewModel()
        {
            ContactsList = new ObservableCollection<ContactViewModel>();          
            SearchCommand = new RelayCommand(Search);           
            //EditContactCommand = new RelayCommand(EditContact);
            AddNewContactCommand = new RelayCommand(AddContact);
            _cities = GetAllCities();
            
        }



        private void AddContact()
        {
           if(OnNewContactClicked != null)
            {
                OnNewContactClicked(this, EventArgs.Empty);
            } 
            
        }

    
        private void Search()
        {

            ContactsList.Clear();
             var contacts = new List<Model.Contact>();
            using (var sqlCon = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
            {
                sqlCon.Query<Model.Contact, City, Phone, PhoneType, Model.Contact>("searchContact",
                    (contact ,city, phone, phoneType) => {
                        var contactForList = contacts.FirstOrDefault(con => con.Id == contact.Id);
                        
                        if(contactForList == null)
                        {
                            contactForList = contact;
                            contactForList.City = new City();
                            contactForList.PhoneNumbers = new List<Phone>();                            
                            contacts.Add(contactForList);
                        }
                        contactForList.City = city;
                        phone.PhoneType = phoneType;                                             
                        contactForList.PhoneNumbers.Add(phone);                     
                        return contact;
                    }, new { SearchString = SearchString}, commandType: System.Data.CommandType.StoredProcedure).ToList();
               
            }

              foreach(Model.Contact con in contacts)
            {                

                var contactViewModel = new ContactViewModel(con, _cities);            
                contactViewModel.OnCotactDeleted += SingleContactDeleted;
                contactViewModel.EditContactClicked += SingleContactEdit;
                ContactsList.Add(contactViewModel);
            }
        }

        private List<City> GetAllCities()
        {
            var cities = new List<City>();
          
             using(var sqlCon = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
            {
               cities = sqlCon.Query<City>("getAllciteis", commandType: System.Data.CommandType.StoredProcedure).ToList();

            }

            
            return cities;
          
        }

        private void SingleContactEdit(object sender, ContactEventArgs args)
        {
            if(OnEdit != null)
            {
                var argsForEvent = new ContactsEventArgs()
                {
                    Contact = args.Contact,
                    Cities = _cities,
                };
                OnEdit(sender, argsForEvent);
            }
                
        }

        private void SingleContactDeleted(object sender, EventArgs e)
        {
            Search();
        }
    }
}
