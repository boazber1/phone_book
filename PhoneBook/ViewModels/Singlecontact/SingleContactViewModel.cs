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
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PhoneBook.ViewModels.Contact
{
    public class ContactEventArgs : EventArgs
    {
        public Contacts.Model.Contact Contact { get; set; }
        public List<PhoneType> PhoneTypes { get; set; }
    }

    public class ContactViewModel : ViewModelBase
    {        

        private PhoneBook.ViewModels.Contacts.Model.Contact _contact;
        private Phone _selectedPhone;        
        private List<City> _cities;
        private List<PhoneType> _phoneTypes;
        private List<int> _phoneIdsToDelete;
        public ICommand DeleteContactCommand { get; set; }       
        public ICommand EditClickedCommand { get; set; }
        public ICommand AddPhoneCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeletePhoneCommand { get; set; }

        public event EventHandler OnCotactDeleted;

        public delegate void ContactClickedEventHandler(object sender, ContactEventArgs args);
        public event ContactClickedEventHandler EditContactClicked;
        public event ContactClickedEventHandler AddPhoneContactClicked;
        public event EventHandler Saved;

        public ContactViewModel(Contacts.Model.Contact contact, List<City> cities, List<PhoneType> phoneTypes)
        {
           
            _contact = contact;
            _cities = cities;
            _phoneTypes = phoneTypes;
            _phoneIdsToDelete = new List<int>();
            SelectedPhone = _contact.PhoneNumbers.FirstOrDefault();
            DeleteContactCommand = new RelayCommand(DeleteContact);
            EditClickedCommand = new RelayCommand(OnEditClicked);
            AddPhoneCommand = new RelayCommand(AddPhone);
            SaveCommand = new RelayCommand(Save);
            DeletePhoneCommand = new RelayCommand(DeletePhone);
        }

        

        private void DeletePhone()
        {
            
            if (_contact.PhoneNumbers.Count > 1)
            {                
                SelectedPhone = _contact.PhoneNumbers.FirstOrDefault();
                int selectedId = _selectedPhone.Id;
                _phoneIdsToDelete.Add(selectedId);
                PhoneNumbers.Remove(_selectedPhone);
                SelectedPhone = _contact.PhoneNumbers.FirstOrDefault();
            }
        }

        private void Save()
        {
            using(var sqlConnection = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
            {
                var param = new DynamicParameters();
                var id = _contact.Id;
                var firstName = _contact.FirstName;
                var lastName = _contact.LastName;
                var street = _contact.Street;
                var cityId = _contact.City.Id;

                param.Add("Id"          , id);
                param.Add("FirstName"   , firstName);
                param.Add("LastName"    , lastName);
                param.Add("Street"      , street);
                param.Add("CityId"      , cityId);
                param.Add("Result"      , dbType: DbType.Int32, direction: ParameterDirection.Output);

                sqlConnection.Execute("Save"
                                     , param
                                     , commandType: System.Data.CommandType.StoredProcedure);

                var idToInsertPhones = param.Get<int>("Result");

                var newPhones = _contact.PhoneNumbers
                                        .Where(row => row.Id <= 0)
                                        .ToList();
                newPhones.ForEach(phone => 
                {
                    var phoneNubmer = phone.PhoneNumber;
                    var phoneTypeId = phone.PhoneType.Id;

                    var paramsForPhone = new DynamicParameters();
                    paramsForPhone.Add("ContactId", idToInsertPhones);
                    paramsForPhone.Add("PhoneNubmer", phoneNubmer);
                    paramsForPhone.Add("PhoneTypeId", phoneTypeId);

                    sqlConnection.Execute("AddPhone", paramsForPhone, commandType: CommandType.StoredProcedure);
                });


                //foreach (var phoneId in _phoneIdsToDelete)
                //{
                //    sqlConnection.Execute("deletePhoneFromContact", new { phonedIdToDelete = phoneId }, commandType: System.Data.CommandType.StoredProcedure);
                //}

            }
            if (Saved != null)
                Saved(this, EventArgs.Empty);
        }

        private void AddPhone()
        {
            if(AddPhoneContactClicked != null)
            {
                var args = new ContactEventArgs
                {
                    Contact = _contact,
                    PhoneTypes = _phoneTypes
                };

                AddPhoneContactClicked(this, args);
            }
        }

        public void AddPhoneToList(Phone phone)
        {
            _contact.PhoneNumbers.Add(phone);
        }

        protected virtual void OnEditContactClicked(PhoneBook.ViewModels.Contacts.Model.Contact contact)
        {
            if (EditContactClicked != null)
            {
                var args = new ContactEventArgs
                {
                    Contact = contact,
                    PhoneTypes = _phoneTypes,
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
     

        public List<int> PhoneIdsToDelete
        {
            get { return _phoneIdsToDelete; }
            set
            {
                _phoneIdsToDelete = value;
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

