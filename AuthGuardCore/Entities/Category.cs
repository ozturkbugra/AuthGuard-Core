namespace AuthGuardCore.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public bool Status { get; set; }

        public List<Message> Messages { get; set; }
    }
}
