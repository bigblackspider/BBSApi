namespace BBSApi.Core.Models.Mail
{
    public class MailDomain
    {
        public long DomainId { get; set; }
        public string DomainName { get; set; }
        public bool Active { get; set; }
        public string Postmaster { get; set; }
        public long CustomerId { get; set; }
    }
}