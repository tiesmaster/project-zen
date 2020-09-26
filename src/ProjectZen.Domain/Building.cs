namespace Tiesmaster.ProjectZen.Domain
{
    public class Building
    {
        public Building(string id, int constructionYear, Address address)
        {
            Id = id;
            ConstructionYear = constructionYear;
            Address = address;
        }

        public string Id { get; }
        public int ConstructionYear { get; }
        public Address Address { get; }
    }

    public class Address
    {
        public Address(
            string street,
            int houseNumber,
            char? houseLetter,
            string houseNumberAddition,
            string postalCode,
            string city)
        {
            Street = street;
            HouseNumber = houseNumber;
            HouseLetter = houseLetter;
            HouseNumberAddition = houseNumberAddition;
            PostalCode = postalCode;
            City = city;
        }

        public string Street { get; }
        public int HouseNumber { get; }
        public char? HouseLetter { get; }
        public string HouseNumberAddition { get; }
        public string PostalCode { get; }
        public string City { get; }
    }
}