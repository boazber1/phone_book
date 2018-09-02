using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.SqlClient;
using PhoneBook.ViewModels.Contacts.Model;
using System.Windows;
using PhoneBook.ViewModels.Contacts.ViewModel;

namespace PhoneBook.ViewModels.Contact
{
    public class ContactEventArgs : EventArgs
    {
        public PhoneBook.ViewModels.Contacts.Model.Contact Contact { get; set; }        
    }

    public class ContactViewModel : ViewModelBase
    {        

        private PhoneBook.ViewModels.Contacts.Model.Contact _contact;
        private Phone _selectedPhone;        
        private List<City> _cities;
        public ICommand DeleteContactCommand { get; set; }       
        public ICommand EditClickedCommand { get; set; }

        public event EventHandler OnCotactDeleted;
        //public event EventHandler OnCotactEditClicked;
        public delegate void EditContactClickedEventHandler(object sender, ContactEventArgs args);
        public event EditContactClickedEventHandler EditContactClicked;


        public ContactViewModel(PhoneBook.ViewModels.Contacts.Model.Contact contact, List<City> cities)
        {
            _contact = contact;
            _cities = cities;
            SelectedPhone = _contact.PhoneNumbers.First();
            DeleteContactCommand = new RelayCommand(DeleteContact);
            EditClickedCommand = new RelayCommand(OnEditClicked);
                        
        }

        protected virtual void OnEditContactClicked(PhoneBook.ViewModels.Contacts.Model.Contact contact)
        {
            if (EditContactClicked != null)
            {
                var args = new ContactEventArgs
                {
                    Contact = contact,
                };
                EditContactClicked(this, args);
            }

        }

        private void OnEditClicked()
        {
            OnEditContactClicked(_contact);
        }


        public string City
        {
            get { return _contact.City.Name; }
            set {
                _contact.City.Name = value;
                RaisePropertyChanged();
            }
        }

        public List<City> Cities
        {
            get { return _cities; }
        }

        public City SelectedCity {
            get { return _contact.City ?? new City() ; }
            set
            {
                _contact.City = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get { return _contact.Street; }
            set {
                _contact.Street = value;
                RaisePropertyChanged();
            }
        }

        public List<Phone> PhoneNumbers
        {
            get { return _contact.PhoneNumbers; }
            set {
                _contact.PhoneNumbers = value;
                RaisePropertyChanged();
            }
        }


        public string LastName
        {
            get { return _contact.LastName; }
            set {
                _contact.LastName = value;
                RaisePropertyChanged();
            }
        }


        public string FirstName
        {
            get { return _contact.FirstName; }
            set {
                _contact.FirstName = value;
                RaisePropertyChanged();
               }
        }


        public int Id
        {
            get { return _contact.Id; }
            set {
                _contact.Id = value;
                RaisePropertyChanged();
            }
        }

  
        public Phone SelectedPhone {
            get {
                return _selectedPhone;
            } set {
                _selectedPhone = value;
                RaisePropertyChanged();
            } }


        private void DeleteContact()
        {
            try
            {
                
                using (var sqlCon = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
                {
                    var idTODelete = _contact.Id;
                    sqlCon.Execute("deleteContact", new { ContactIdToDelete = idTODelete }, commandType: System.Data.CommandType.StoredProcedure);
                    MessageBox.Show("Contact Deleted");
                }

                if(OnCotactDeleted != null)
                {
                    OnCotactDeleted(this, EventArgs.Empty);
                }              

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }   
           
        }


    }
}

