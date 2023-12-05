namespace SALearning.DBModel
{
    public class DBProfile {
        public int ProfileId { get; set; }
        public string Email { get; set; }
        public string AadId { get; set; }
        public string Name { get; set; }
        public ApiModel.IdentityType AccountType { get; set; }
        public string Description { get; set; }
    }
}
