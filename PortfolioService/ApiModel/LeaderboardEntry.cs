namespace SALearning.ApiModel;

public class LeaderboardEntry {
    public int AccountNumber { get; set; }
    public int ProfileId { get; set; }
    public string Description { get; set; }
    public decimal Balance { get; set; }
    public decimal Gain { get; set; }
}
