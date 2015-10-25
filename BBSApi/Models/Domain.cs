namespace BBSApi.Models
{
    public class Domain
    {
        public int ID { get; set; }
        public string DomainName { get; set; }
        public bool Active { get; set; }
        public string Postmaster { get; set; }
    }
}