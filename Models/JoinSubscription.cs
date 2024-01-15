namespace Health_Insurance.Models
{
    public class JoinSubscription
    {

        public Useraccount userAccounts { get; set; }
        public Subscription subscription { get; set; }
        public Beneficiary beneficiary { get; set;}
    }
}
