using System.Collections.Generic;
using System.Threading.Tasks;
using SALearning.ApiModel;

namespace SALearning.Services
{
    public interface IPortfolioService
    {
        Task<List<LeaderboardEntry>> GetLeaderboard();
        UserProfile Login(UserProfile userProfile);
        UserProfile GetProfile(int profileId);
        UserProfile UpdateProfile(UserProfile profile);
        int DeleteProfile(int profileId);
        List<UserProfile> GetProfileList();
        Account GetAccount(int accountNumber);
        Task<AccountPerformance> GetAccountPerformance(int accountNumber);
        Account UpdateAccount(Account account);
        int DeleteAccount(int accountNumber);
        Account CreateAccount(Account account);
        Account GetProfileAccount(int profileId);
        List<Account> GetAccountList();
        List<Holding> GetHoldingList(int accountNumber);
        Holding GetHolding(int accountNumber, string symbol);
        Holding UpdateHolding(int accountNumber, Holding holding);
        int DeleteHolding(int accountNumber, string symbol);
        Holding CreateHolding(int accountNumber, Holding holding);
        List<Operation> GetOperationList(int accountNumber);
        Operation GetOperation(int accountNumber, int operationId);
        Operation UpdateOperation(int accountNumber, Operation operation);
        int DeleteOperation(int accountNumber, int operationId);
        Operation CreateOperation(int accountNumber, Operation operation);
        Task<OperationResult> TradeStock(int accountNumber, Operation operation);
        Task<OperationResult> TransferCash(int accountNumber, decimal amount);
        Task<OperationResult> GenerateRecommendations(int accountNumber);
    }
}
