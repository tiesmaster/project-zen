namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagNummeraanduiding : BagBase
    {
        public BagNummeraanduiding(string id, BagVersion version) : base(id, version)
        {
        }
    }

    public class BagOpenbareRuimte : BagBase
    {
        public BagOpenbareRuimte(string id, BagVersion version, string name, string relatedWoonplaats) : base(id, version)
        {
            Name = name;
            RelatedWoonplaats = relatedWoonplaats;
        }

        public string Name { get; }
        public string RelatedWoonplaats { get; }
    }
}