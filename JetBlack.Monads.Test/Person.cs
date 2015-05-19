using System;
using System.Collections.Generic;

namespace JetBlack.Monads.Test
{
    public class Person
    {
        public Person(string name, string email, DateTime dateOfBirth, IList<Address> addresses)
        {
            Addresses = addresses;
            DateOfBirth = dateOfBirth;
            Email = email;
            Name = name;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public IList<Address> Addresses { get; private set; }
    }
}