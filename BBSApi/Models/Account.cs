namespace BBSApi.Models
{
    public class Account
    {
        public int ID { get; set; }
        public bool Active { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public int MaxSize { get; set; }
        public string Address { get; set; }
    }
}