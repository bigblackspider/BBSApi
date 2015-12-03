namespace BBSApi.Core.Models.Mail
{
    public class MailAccount
    {
        public long AccountId { get; set; }
        public bool Active { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public int MaxSize { get; set; }
        public string Names { get; set; }
    }
}