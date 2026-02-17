namespace AuthGuardCore.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
        public string Status { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
