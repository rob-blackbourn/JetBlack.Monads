namespace JetBlack.Monads.Test
{
    public class Address
    {
        public Address(string street, string town, string postCode, string country)
        {
            Country = country;
            PostCode = postCode;
            Town = town;
            Street = street;
        }

        public string Street { get; private set; }
        public string Town { get; private set; }
        public string PostCode { get; private set; }
        public string Country { get; private set; }
    }
}