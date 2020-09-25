namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagNummeraanduiding : BagBase
    {
        public BagNummeraanduiding(
            string id,
            BagVersion version,
            int houseNumber,
            char houseLetter,
            string houseNumberAddition,
            string postalCode,
            string relatedOpenbareRuimte) : base(id, version)
        {
            HouseNumber = houseNumber;
            HouseLetter = houseLetter;
            HouseNumberAddition = houseNumberAddition;
            PostalCode = postalCode;
            RelatedOpenbareRuimte = relatedOpenbareRuimte;
        }

        public int HouseNumber { get; }
        public char HouseLetter { get; }
        public string HouseNumberAddition { get; }
        public string PostalCode { get; }
        public string RelatedOpenbareRuimte { get; }
    }
}