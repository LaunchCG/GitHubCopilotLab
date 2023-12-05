class MockApi {
    HoldingArray = [
        [{symbol : 'MSFT', shares : 10.1},
            {symbol : 'AMZN', shares : 2},
            {symbol : 'GOOG', shares : 3.75}
        ],
        [{symbol : 'F', shares : 50},
            {symbol : 'TM', shares : 75},
            {symbol : 'GM', shares : 20},
            {symbol : 'TSLA', shares : .75}
        ]
    ];

    Accounts = [
        {accountNumber : 1, profileId : 1, description : "Joe's Fine Account", balance : 12000.35, gain : 7000.35 },
        {accountNumber : 2, profileId : 2, description : "Alysha's Awesome Analysis", balance : 11000.45, gain : 6000.45 },
        {accountNumber : 3, profileId : 3, description : "Petru's Physics Picks", balance : 10000.55, gain : 5000.55 },
        {accountNumber : 4, profileId : 4, description : "Neha's Nice Notions", balance : 9000.65, gain : 4000.65 },
        {accountNumber : 5, profileId : 5, description : "Alfred's Amazing Artifacts", balance : 8000.75, gain : 3000.75 },
        {accountNumber : 6, profileId : 6, description : "Stu's Suprising Steals", balance : 7000.85, gain : 2000.85 },
        {accountNumber : 7, profileId : 1, description : "Joe's Terrible Backup Portfolio", balance : 2000.95, gain : -2999.05 }
    ]

    PeformanceArray = [
        [{symbol : 'MSFT', Gain : 2025.93 },
         {symbol : 'AMZN', Gain : 10000.25 },
         {symbol : 'GOOG', Gain : -2343.93 }
        ],
        [   {symbol : 'F', Gain : 4921.35 },
            {symbol : 'TM', Gain : -2339.94 },
            {symbol : 'GM', Gain : 4929.42 },
            {symbol : 'TSLA', Gain : -1932.95 }
        ]
    ];

    PricingInfo = [
           {symbol : 'MSFT', name: 'Microsoft Corporation', description: 'Microsoft Description...', price: 305.79},
            {symbol : 'AMZN', name: 'Amazon.com', description: 'Amazon Description...', price: 103.23},
            {symbol : 'GOOG', name: 'Alphabet Inc', description: 'Google Description...', price: 106.15},
            {symbol : 'NFLX', name: 'Netflix Inc', description: 'Netflix Description...', price: 318.91},
            {symbol : 'UBER', name: 'Uber Technologies', description: 'Uber Description...', price: 36.31},
            {symbol : 'INTC', name: 'Intel Corporation', description: 'Intel Description...', price: 29.84},
            {symbol : 'ALK', name: 'Alaska Airlines', description: 'Alaska Airlines Description...', price: 43.04}
    ];

    constructor() 
    {
        console.log("MockApi class constructed");
    }

    privGetIndex(acctNum)
    {
        if (acctNum % 2) {
            return 0;
        } else {
            return 1;
        }
    }

    async login(activeAccount)
    {
        return {
            aadId: "aaaad195-994f-4f7b-a8ab-c3400ac2b35f",
            description: "Sample description",
            email: "test@gmail.com",
            name: "Joe Welch",
            profileId: 19
        }
    }

    async getProfileAccount(profileId)
    {
        return this.Accounts[0];
    }

    async getLeaderboard()
    {
        var retValue = this.Accounts;
        console.log("getLeaderboard", retValue);
        return retValue;
    }

    async getAccounts(profileId)
    {
        var retValue = this.Accounts.filter(acct => acct.profileId === profileId);

        console.log("getAccounts, profileId=", profileId, retValue);
        return retValue;
    }

    async getHoldings(acctNum)
    {
        var index = this.privGetIndex(acctNum);
        var holdings = this.HoldingArray[index];
        console.log("getHoldings called with acctNum = ", acctNum, holdings);
        return holdings;
    }

    async getPerformance(acctNum)
    {
        console.log("getPerformance called, acctNum=", acctNum);

        const perfObject = 
        {
            "accountNumber": 4,
            "profileId": 3,
            "description": "Joe's Fine Account",
            "cashBalance": 2708.40,
            "stockBalance": 2238.450,
            "gain": -53.150,
            "stockGain": -53.150,
            "stocks": [
                {
                    "symbol": "MSFT",
                    "currentShares": 5.0,
                    "currentValue": 1643.00,
                    "gain": -21.45
                },
                {
                    "symbol": "GOOG",
                    "currentShares": 5.0,
                    "currentValue": 595.450,
                    "gain": -31.700
                }
            ]
        };

        console.log("getPerformance returned", perfObject);
        return perfObject;
    }

    async getPricing(symbol)
    {
        console.log("getPricing called, symbol = ", symbol);
        var retVal = this.PricingInfo.find(e => { return e.symbol === symbol.toUpperCase() });
        console.log("GetPricing returned", retVal);
        return retVal;
    }

    async buyStock(acctNum, symbol, quantity)
    {
        console.log("buyStock called", acctNum, symbol, quantity);
        var index = this.privGetIndex(acctNum);
        var holdings = this.HoldingArray[index];
        var holding = holdings.find(h => {return h.symbol === symbol.toUpperCase() });
        if (holding)
            holding.Shares += quantity;
        else
            holdings.push({symbol: symbol, shares: quantity});

        this.HoldingArray[index] = [...holdings];

        console.log("buyStock completed", holdings);
    }

    async sellStock(acctNum, symbol, quantity)
    {
        console.log("sellStock called", acctNum, symbol, quantity);
        var index = this.privGetIndex(acctNum);
        var holdings = this.HoldingArray[index];
        var holding = holdings.find(h => {return h.symbol === symbol.toUpperCase() });

        holding.Shares -= quantity;
        if (holding.Shares === 0)
        {
            // Return all holdings except current symbol
            holdings = holdings.filter(item => item.symbol !== symbol.toUpperCase() );
        }
        this.HoldingArray[index] = [...holdings];

        console.log("sellStock completed", holdings);
    }
}

const mockApi = new MockApi();
export default mockApi;