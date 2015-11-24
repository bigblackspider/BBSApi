namespace BBSApi.Core.Models.Mail
{
    public class Domain
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public bool Active { get; set; }
        public string Postmaster { get; set; }
    }
}