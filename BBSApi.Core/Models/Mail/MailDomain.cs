namespace BBSApi.Core.Models.Mail
{
    public class MailDomain
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public bool Active { get; set; }
        public string Postmaster { get; set; }
    }
}