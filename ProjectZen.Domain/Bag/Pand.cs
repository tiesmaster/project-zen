namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class Pand
    {
        public Pand(string id, int constructionYear)
        {
            Id = id;
            ConstructionYear = constructionYear;
        }

        public string Id { get; }
        public int ConstructionYear { get; }
    }
}