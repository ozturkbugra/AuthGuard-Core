namespace AuthGuardCore.Entities
{
    public class Message
    {
        public int MessageID { get; set; }

        // Gönderen maili
        public string SenderEmail { get; set; }

        // Alıcı maili
        public string RecieverEmail { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }

        public bool IsRead { get; set; }

        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
