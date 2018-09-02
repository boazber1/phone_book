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

        public override string ToString()
        {
            return Type;
        }

        public override bool Equals(object obj)
        {
            if(obj is PhoneType)
            {
                var otherPhoneType = (PhoneType)obj;
                var id = this.Id;
                var otherId = otherPhoneType.Id;
                return id == otherId;
            }
            return false;
        }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if(obj is City)
            {
                var otherCity = (City)obj;
                var id = this.Id;
                var otherId = otherCity.Id;
                return id == otherId;
            }
            return false;
        }
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

