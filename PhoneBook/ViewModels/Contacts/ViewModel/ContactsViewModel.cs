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
using System.IO;
using Microsoft.Win32;
using System.Data;

namespace PhoneBook.ViewModels.Contacts.ViewModel
{
    class ContactsEventArgs : ContactEventArgs
    {
        public List<City> Cities { get; set; }        
    }

    class ContactsViewModel : ViewModelBase 
    {

        

        public ObservableCollection<ContactViewModel> ContactsList { get; set; }
        public ObservableCollection<String> _autoCompleteDbCollection;
        public ICommand SearchCommand { get; set; }        
        public ICommand ExportToExcelCommand { get; set; }        
        public ICommand ImportToDataBaseCommand { get; set; }        
        public ICommand EditContactCommand { get; set; }
        public ICommand AddNewContactCommand { get; set; }
        private string _searchString;
        private List<City> _cities;
        private List<Model.Contact> _contactsForDB;
        private List<PhoneType> _phoneTypes;

        public event EditContactClickedEventHandler OnNewContactClicked;
        public delegate void EditContactClickedEventHandler(object sender, ContactsEventArgs args);
        public event EditContactClickedEventHandler OnEdit;


        public ContactsViewModel()
        {
            _contactsForDB = new List<Model.Contact>();
            ContactsList = new ObservableCollection<ContactViewModel>();
            _autoCompleteDbCollection = new ObservableCollection<string>();
            SearchCommand = new RelayCommand(Search);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            ImportToDataBaseCommand = new RelayCommand(ImportToDB);
            AddNewContactCommand = new RelayCommand(AddContact);
            _cities = GetAllCities();
            _phoneTypes = GetAllPhoneType();
        }

        private void ImportToDB()
        {
            string filePath;            
            string firstNameTocompare = string.Empty;//compare whether i'm in the same contact or it's new one , or end of data
            string LastNameTocompare = string.Empty;
            string streetTocompare = string.Empty;            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Cvs Files(*.csv files| *.csv; )";

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                using (var reader = new StreamReader(filePath))
                {
                    string headeLine = reader.ReadLine();//read and ignor first line
                    string secondline = reader.ReadLine();//read and restor second line(first contact)
                    List<string> lineOfValues = secondline.Split(',').ToList<string>();
                    firstNameTocompare = lineOfValues[0];
                    LastNameTocompare = lineOfValues[1];
                    streetTocompare = lineOfValues[4];                    
                    var contactCity = _cities.First(item => item.Name == lineOfValues[5]);
                    var phoneType = _phoneTypes.First(item => item.Type == lineOfValues[3]);

                    var newModel = new Model.Contact()
                    {
                        Id = -1,
                        FirstName = lineOfValues[0],
                        LastName = lineOfValues[1],
                        Street = lineOfValues[4],
                        City = contactCity,
                        PhoneNumbers = new List<Phone>()
                    };



                    var contactPhone = new Phone
                    {
                        PhoneNumber = lineOfValues[2],
                        PhoneType = phoneType
                    };
                    newModel.PhoneNumbers.Add(contactPhone);
                    _contactsForDB.Add(newModel);


                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        List<string> listOfValues = line.Split(',').ToList<string>();
                        if (firstNameTocompare == listOfValues[0] && LastNameTocompare == listOfValues[1] && streetTocompare == listOfValues[4])
                        {
                            var newContactPhoneType = new PhoneType
                            {
                                Type = listOfValues[3],
                            };

                            var newContactPhone = new Phone
                            {
                                PhoneNumber = listOfValues[2],
                                PhoneType = phoneType,
                            };
                            
                            newModel.PhoneNumbers.Add(newContactPhone);

                        } else {
                            
                            firstNameTocompare = listOfValues[0];
                            LastNameTocompare = listOfValues[1];
                            streetTocompare = listOfValues[4];

                            var newContactCity = _cities.First(item => item.Name == listOfValues[5]);
                            var newPhoneType = _phoneTypes.Find(item => item.Type == listOfValues[3]);
                            var newContactModel = new Model.Contact()
                            {
                                Id = -1,
                                FirstName = listOfValues[0],
                                LastName = listOfValues[1],
                                Street = listOfValues[4],
                                City = newContactCity,
                                PhoneNumbers = new List<Phone>()
                            };



                            var newContactPhone = new Phone
                            {
                                PhoneNumber = lineOfValues[2],
                                PhoneType = newPhoneType,
                            };
                            newContactModel.PhoneNumbers.Add(newContactPhone);
                            _contactsForDB.Add(newContactModel);
                            newModel = newContactModel;

                        }

                        //call sql query to add the contacListForDB one by one
                       

                        
                        
                    }
                    UploadContacsForDB();
                }
                
            }

           
        }

        private void UploadContacsForDB()
        {
            foreach (var contact in _contactsForDB)
            {
                using (var sqlConnection = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
                {
                    var param = new DynamicParameters();
                    var id = contact.Id;
                    var firstName = contact.FirstName;
                    var lastName = contact.LastName;
                    var street = contact.Street;
                    var cityId = contact.City.Id;

                    param.Add("Id", id);
                    param.Add("FirstName", firstName);
                    param.Add("LastName", lastName);
                    param.Add("Street", street);
                    param.Add("CityId", cityId);
                    param.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    sqlConnection.Execute("Save"
                                         , param
                                         , commandType: System.Data.CommandType.StoredProcedure);

                    var idToInsertPhones = param.Get<int>("Result");

                    var newPhones = contact.PhoneNumbers
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

                }
            }
        }


        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                AutoCompleteFromDB();
                RaisePropertyChanged();
            }
        }

        private void ExportToExcel()
        {
            StringBuilder csvContent = new StringBuilder();
            string currDate = DateTime.Now.ToString("yyyy_MM_ddHH_mm");
            string csvPath = "D:\\cvs_exports\\" + currDate + ".csv";


            List<ContactViewModel> contactsToExcel = new List<ContactViewModel>();
            foreach (var item in ContactsList)
            {
                contactsToExcel.Add(item);
            }

            string firstNameHeadline = "First Name";
            string lastNameHeadline = "Last Name";
            string phonesHeadline = "Phone(s)";
            string streetHeadline = "Street";
            string cityHeadline = "City";

            var excelHeadLines = string.Format("{0},{1},{2},{3},{4}", firstNameHeadline, lastNameHeadline, phonesHeadline, streetHeadline, cityHeadline);
            csvContent.AppendLine(excelHeadLines);

            File.WriteAllText(csvPath, csvContent.ToString());
            foreach (var con in contactsToExcel)
            {
                foreach (var phoneNumber in con.PhoneNumbers)
                {
                    var contactDetails = string.Format("{0},{1},{2},{3},{4}", con.FirstName, con.LastName, phoneNumber, con.Street, con.City);
                    csvContent.AppendLine(contactDetails);
                    File.WriteAllText(csvPath, csvContent.ToString());
                }
            }



        }

        public ObservableCollection<String> AutoCompleteDbCollection
        {
            get { return _autoCompleteDbCollection; }
            set
            {
                _autoCompleteDbCollection = value;
            }
        }

        private void AutoCompleteFromDB()
        {
            _autoCompleteDbCollection.Clear();
            var listForAutoComplete = new List<string>();

            using (var sqlCon = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
            {
                listForAutoComplete = sqlCon.Query<string>("autoCompleteForSearch", new { searchStringforAutoComplete = SearchString }, commandType: System.Data.CommandType.StoredProcedure).ToList();

            }

            foreach(var item in listForAutoComplete)
            {
                _autoCompleteDbCollection.Add(item);
            }
            
        }

        private void AddContact()
        {
           if(OnNewContactClicked != null)
            {
                var firstCity = _cities.First();
                
                var newModel = new Model.Contact()
                {
                    Id = -1,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Street = string.Empty,
                    City = firstCity,
                    PhoneNumbers = new List<Phone>()
                };
                var args = new ContactsEventArgs
                {
                    Contact = newModel,
                    Cities = _cities,
                    PhoneTypes = _phoneTypes
                };
                OnNewContactClicked(this, args);
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

                var contactViewModel = new ContactViewModel(con, _cities, _phoneTypes);            
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

        private List<PhoneType> GetAllPhoneType()
        {
            var phoneTypes = new List<PhoneType>();

            using (var sqlCon = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;"))
            {
                phoneTypes = sqlCon.Query<PhoneType>("getAllPhoneType", commandType: System.Data.CommandType.StoredProcedure).ToList();

            }


            return phoneTypes;

        }

        private void SingleContactEdit(object sender, ContactEventArgs args)
        {
            if(OnEdit != null)
            {
                var argsForEvent = new ContactsEventArgs()
                {
                    Contact = args.Contact,
                    Cities = _cities,
                    PhoneTypes = _phoneTypes
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
