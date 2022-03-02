namespace BredWeb.Models
{
    public class UserIdList
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string PersonId { get; set; }
        public Person Person { get; set; }
    }
}
