using System.Collections.Generic;
using SALearning.ApiModel;
using SALearning.DBModel;
using SALearning.Database;
using System.Linq;
using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SALearning.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly PortfolioContext _context;
        private readonly IMapper _mapper;
        private readonly IPricingService _pricingSvc;
        public PortfolioService(PortfolioContext ctx, IMapper mapper, IPricingService pricingService)
        {
            _context = ctx;
            _mapper = mapper;
            _pricingSvc = pricingService;
        }

        // How much to charge buy and sell transactions (in dollars)
        private readonly decimal TRANSACTION_COST = 10.0M;

        private decimal GetAccountStartingBalance(List<DBOperation> operations)
        {
            decimal startCash = 0.0M;
            startCash += operations.Where(op => op.OperationType == (int) OperationType.Deposit).Sum(op => op.Amount);
            startCash -= operations.Where(op => op.OperationType == (int) OperationType.Withdrawal).Sum(op => op.Amount);

            return startCash;
        }

        public async Task<List<LeaderboardEntry>> GetLeaderboard()
        {
            // Compute current balance for all accounts
            var accountList = _context.Accounts.ToList();
            var leaderboardList = new List<LeaderboardEntry>();

            foreach (var account in accountList)
            {
                // Get holdings and current prices for each holding
                var holdingList = _context.Holdings.Where(h => h.AccountNumber == account.AccountNumber).ToList();
                var symbolList = holdingList.Select(h => h.Symbol).ToList();
                var priceList = await _pricingSvc.GetPricingList(symbolList);

                // Get values of holdings
                var stockValue = 0.0M;
                for (int i = 0; i < holdingList.Count; i++)
                {
                    stockValue += holdingList[i].Shares * priceList[i];
                }
                
                // Get starting value of account
                var operations = _context.Operations.Where(op => op.AccountNumber == account.AccountNumber).ToList();
                var startBalance = GetAccountStartingBalance(operations);

                // Add leaderboard entry
                leaderboardList.Add( new LeaderboardEntry{
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance + stockValue,
                    Description = account.Description,
                    ProfileId = account.ProfileId,
                    Gain = account.Balance + stockValue - startBalance
                });
            }

            return leaderboardList;
        }

        public UserProfile Login(UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.AadId))
                throw new Exception("User Profile must contain Aad Id");

            var dbProfile = _context.Profiles.FirstOrDefault(profile => profile.AadId == userProfile.AadId);
            if (dbProfile == null)
            {
                // Add new profile if none exists
                dbProfile = _mapper.Map<DBProfile>(userProfile);
                _context.Profiles.Add(dbProfile);
            }
            _context.SaveChanges();
            return _mapper.Map<UserProfile>(dbProfile);
        }

        public UserProfile GetProfile(int profileId)
        {
            var dbProfile = _context.Profiles.AsNoTracking()
                                        .FirstOrDefault(profile => profile.ProfileId == profileId);

            if (dbProfile == null)
                throw new Exception("email not found");

            return _mapper.Map<UserProfile>(dbProfile);
        }

        public UserProfile UpdateProfile(UserProfile profile)
        {
            profile.Email = profile.Email.ToLower();
            var dbProfile = _mapper.Map<DBProfile>(profile);
            _context.Profiles.Update(dbProfile);
            _context.SaveChanges();
            return profile;
        }

        public int DeleteProfile(int profileId)
        {
            int recordsModified;
            try {
                _context.Profiles.Remove(new DBProfile { ProfileId = profileId });
                recordsModified = _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                recordsModified = 0;
            }
            return recordsModified;
        }

        public List<UserProfile> GetProfileList()
        {
            List<UserProfile> profileList = _context.Profiles.Select(profile => _mapper.Map<UserProfile>(profile)).ToList();
            return profileList;
        }

        public Account GetAccount(int accountNumber)
        {
            var dbAccount = _context.Accounts.AsNoTracking()
                                        .FirstOrDefault(acct => acct.AccountNumber == accountNumber);
            if (dbAccount == null)
                throw new Exception("accountNumber not found");

            var account = _mapper.Map<Account>(dbAccount);

            // Get name for account
            // TODO: we could perform join to prevent multiple DB calls
            var dbProfile = _context.Profiles.AsNoTracking()
                                        .FirstOrDefault(profile => profile.ProfileId == dbAccount.ProfileId);

            return account;
        }

        public async Task<AccountPerformance> GetAccountPerformance(int accountNumber)
        {
            var account = _context.Find<DBAccount>(accountNumber);

            // Get all operations
            var operations = _context.Operations.Where(op => op.AccountNumber == accountNumber).ToList();

            // Get starting balance
            decimal startCash = GetAccountStartingBalance(operations);

            // Get list of symbols which have been traded
            var symbols = operations.Where(op => op.OperationType == (int) OperationType.BuyStock).Select(op => op.Symbol).Distinct().ToList();
            var stockPerformanceList = new List<StockPerformance>();
            var accountStockGain = 0.0M;
            var accountStockBalance = 0.0M;
            foreach (var symbol in symbols)
            {
                // For each symbol, get current amount and overall gain/loss
                var stockOperations = operations.Where(op => op.Symbol == symbol);
                var curQuantity = 0.0M;
                var curGain = 0.0M;
                stockOperations.ToList().ForEach(op => {
                    if (op.OperationType == (int) OperationType.BuyStock) {
                        curQuantity += op.Shares;
                        curGain -= op.Amount;
                    }
                    else
                    {
                        curQuantity -= op.Shares;
                        curGain += op.Amount;
                    }
                });
                var curValue = 0.0M;
                if (curQuantity > 0)
                {
                    // If quantity is greater than 0, get stock price and add to stock list
                    var pricingInfo = await _pricingSvc.GetPricingInfo(symbol);
                    curValue = (decimal) pricingInfo.Price * curQuantity;

                    curGain += curValue;

                    stockPerformanceList.Add( new StockPerformance{
                        CurrentShares = curQuantity,
                        CurrentValue = curValue,
                        Gain = curGain, 
                        Symbol = symbol
                    });
                }

                // Keep running total of stock gains, balance
                accountStockGain += curGain;
                accountStockBalance += curValue;
            }

            return new AccountPerformance {
                AccountNumber = accountNumber, 
                Description = account.Description,
                ProfileId = account.ProfileId,
                CashBalance = account.Balance,
                StockBalance = accountStockBalance,
                Gain = account.Balance + accountStockBalance - startCash,
                StockGain = accountStockGain,
                Stocks = stockPerformanceList
            };
        }

        public Account UpdateAccount(Account account)
        {
            var dbAccount = _mapper.Map<DBAccount>(account);
            _context.Accounts.Update(dbAccount);
            _context.SaveChanges();
            return account;
        }

        public int DeleteAccount(int accountNumber)
        {
            int recordsModified;
            try {
                _context.Accounts.Remove(new DBAccount { AccountNumber = accountNumber });
                recordsModified = _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                recordsModified = 0;
            }
            return recordsModified;            
        }

        public Account CreateAccount(Account account)
        {
            // Create Account and add initial deposit as an operation
            var dbAccount = _mapper.Map<DBAccount>(account);
            _context.Accounts.Add(dbAccount);
            // Call this to properly get the new account number
            _context.SaveChanges();

            // Add the initial deposit
            _context.Operations.Add(new DBOperation 
                { AccountNumber = dbAccount.AccountNumber,
                  OperationType = (int) OperationType.Deposit,
                  Shares = 0,
                  TransactDate = DateOnly.FromDateTime(DateTime.UtcNow),
                  Amount = dbAccount.Balance
                });
            _context.SaveChanges();

            return _mapper.Map<Account>(dbAccount);
        }

        public Account GetProfileAccount(int profileId)
        {
            var dbAccount = _context.Accounts.FirstOrDefault(account => account.ProfileId == profileId);
            return _mapper.Map<Account>(dbAccount);
        }

        public List<Account> GetAccountList()
        {
            List<Account> acctList = _context.Accounts.AsNoTracking()
                                                    .Select(acct => _mapper.Map<Account>(acct)).ToList();
            return acctList;
        }

        public List<Holding> GetHoldingList(int accountNumber)
        {
            List<Holding> holdingList = _context.Holdings
                                            .AsNoTracking()
                                            .Where(h => h.AccountNumber == accountNumber)
                                            .Select(h => _mapper.Map<Holding>(h)).ToList();
            return holdingList;
        }

        public Holding GetHolding(int accountNumber, string symbol)
        {
            symbol = symbol.ToUpper();
            var dbHolding = _context.Holdings.AsNoTracking()
                                        .FirstOrDefault(holding => (holding.AccountNumber == accountNumber) && 
                                                                    (holding.Symbol == symbol));

            return _mapper.Map<Holding>(dbHolding);            
        }

        public Holding UpdateHolding(int accountNumber, Holding holding)
        {
            var dbHolding = _mapper.Map<DBHolding>(holding);
            dbHolding.AccountNumber = accountNumber;
            dbHolding.Symbol = dbHolding.Symbol.ToUpper();
            _context.Holdings.Update(dbHolding);
            _context.SaveChanges();
            return holding;
        }

        public int DeleteHolding(int accountNumber, string symbol)
        {
            int recordsModified;
            try {
                _context.Holdings.Remove(new DBHolding { AccountNumber = accountNumber, Symbol = symbol.ToUpper() });
                recordsModified = _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                recordsModified = 0;
            }
            return recordsModified;
        }

        public Holding CreateHolding(int accountNumber, Holding holding)
        {
            var dbHolding = _mapper.Map<DBHolding>(holding);
            dbHolding.AccountNumber = accountNumber;
            dbHolding.Symbol = dbHolding.Symbol.ToUpper();
            _context.Holdings.Add(dbHolding);
            _context.SaveChanges();

            return _mapper.Map<Holding>(dbHolding);
        }

        public List<Operation> GetOperationList(int accountNumber)
        {
            List<Operation> opList = _context.Operations.AsNoTracking()
                                                .Where(o => o.AccountNumber == accountNumber)
                                                .Select(o => _mapper.Map<Operation>(o)).ToList();
                                            
            return opList;
        }
        
        public Operation GetOperation(int accountNumber, int operationId)
        {
             var dbOp = _context.Operations.AsNoTracking()
                                    .FirstOrDefault(op => (op.AccountNumber == accountNumber) && 
                                                            (op.OperationId == operationId));

            return _mapper.Map<Operation>(dbOp);            
       }
        public Operation UpdateOperation(int accountNumber, Operation operation)
        {
            var dbOp = _mapper.Map<DBOperation>(operation);
            dbOp.AccountNumber = accountNumber;
            _context.Operations.Update(dbOp);
            _context.SaveChanges();
            return operation;
        }

        public int DeleteOperation(int accountNumber, int operationId)
        {
            int recordsModified;
            try {
                _context.Operations.Remove(new DBOperation { OperationId = operationId });
                recordsModified = _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                recordsModified = 0;
            }
            return recordsModified;
        }

        public Operation CreateOperation(int accountNumber, Operation operation)
        {
            var dbOp = _mapper.Map<DBOperation>(operation);
            dbOp.AccountNumber = accountNumber;
            _context.Operations.Add(dbOp);
            _context.SaveChanges();

            return _mapper.Map<Operation>(dbOp);
        }

        public async Task<OperationResult> TradeStock(int accountId, Operation operation)
        {
            bool buyStock = true;

            if (operation.OperationType == OperationType.SellStock)
                buyStock = false;
            else if (operation.OperationType != OperationType.BuyStock)
            {
                return new OperationResult{
                    fSuccess = false,
                    StatusMessage = $"Internal error: Trade stock expected an operation of either Buy or Sell Stock.  Received OperationType={operation.OperationType}"
                };
            }

            // First, get account, price, and holding (if present)
            var account = _context.Accounts.Find(accountId);
            var symbol = operation.Symbol.ToUpper();
            var priceInfo = await _pricingSvc.GetPricingInfo(symbol);
            var shares = operation.Shares;
            var dealAmount = shares * Convert.ToDecimal(priceInfo.Price);
            var dbHolding = _context.Holdings.Find(accountId, symbol);

            // Perform business checks and transactions
            if (buyStock)
            {
                dealAmount += TRANSACTION_COST;
                if (dealAmount > account.Balance)
                {
                    return new OperationResult{
                        fSuccess = false,
                        StatusMessage = $"Not enough funds to buy {shares} shares of {symbol}.  Required amount is {dealAmount}, available amount is {account.Balance}"
                    };
                }
                account.Balance -= dealAmount;
                if (dbHolding != null)
                    dbHolding.Shares += shares;
                else 
                    _context.Holdings.Add(new DBHolding{
                        AccountNumber = accountId,
                        Shares = shares,
                        Symbol = symbol});
            }
            else
            {
                dealAmount -= TRANSACTION_COST;
                if ((dbHolding == null) || (dbHolding.Shares < shares))
                {
                    return new OperationResult {
                        fSuccess = false,
                        StatusMessage = $"The account doesn't currently hold {shares} shares of {symbol}."
                    };
                }
                account.Balance += dealAmount;
                if (dbHolding.Shares == shares)
                    _context.Remove<DBHolding>(dbHolding);
                else
                    dbHolding.Shares -= shares;
            }

            // Update operation
            var dbOperation = new DBOperation{
                AccountNumber = accountId,
                OperationType = (int)operation.OperationType,
                Amount = dealAmount,
                Shares = operation.Shares,
                Symbol = symbol,
                TransactDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Operations.Add(dbOperation);
            _context.SaveChanges();

            return new OperationResult {
                fSuccess = true,
                StatusMessage = (buyStock ? "Bought" : "Sold") + $" {operation.Shares} share(s) of {symbol} for the amount: {dealAmount}"
            };
        }

        public async Task<OperationResult> TransferCash(int acctNum, decimal amount)
        {
            // Allow negative balances - no business checks currently
            var operationType = (amount < 0) ? OperationType.Withdrawal : OperationType.Deposit;

            // Add operation
            _context.Operations.Add(new DBOperation {
                AccountNumber = acctNum,
                Amount = Math.Abs(amount),
                OperationType = (int) operationType,
                TransactDate = DateOnly.FromDateTime(DateTime.Now)
            });

            // Adjust cash balance in account
            var account = _context.Accounts.Find(acctNum);
            account.Balance += amount;

            await _context.SaveChangesAsync();

            return new OperationResult {
                fSuccess = true,
                StatusMessage = "Cash transaction successful for account"
            };
        }

        public async Task<OperationResult> GenerateRecommendations(int accountNumber)
        {
            return await Task.FromResult(new OperationResult {
                fSuccess = true,
                StatusMessage = "Sample Recommendations"
            });
        }

    }
}
