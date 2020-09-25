namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagWoonplaats : BagBase
    {
        public BagWoonplaats(string id, BagVersion version, string name) : base(id, version)
        {
            Name = name;
        }

        public string Name { get; }
    }
}