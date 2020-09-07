namespace Tiesmaster.ProjectZen.Domain
{
    public class Building
    {
        public Building(string id, int constructionYear)
        {
            Id = id;
            ConstructionYear = constructionYear;
        }

        public string Id { get; }
        public int ConstructionYear { get; }
    }
}