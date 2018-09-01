using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.ViewModels.Contacts.Model
{
    public class PhoneType
    {
        public int Id { get; set; }
        public string Type { get; set; }

    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType { get; set; }
        public override string ToString()
        {
            return PhoneNumber + " - " + PhoneType.Type;
        }
    }

    public class Contact 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Phone> PhoneNumbers { get; set; }
        public string Street { get; set; }
        public City City { get; set; }
    }


}

