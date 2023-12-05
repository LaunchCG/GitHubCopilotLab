class ServiceApi {
    hostURL = "";
    
    ConfigureHostURL(hosturl)
    {
        console.log("hostURL set to ", hosturl);
        this.hostURL = hosturl;
    }

    async genericGet(getUrl) {
        const url = `${this.hostURL}${getUrl}`;

        return await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("HTTP error, status = " + response.status);
            }
            else if (response.status === 204)
                return null;
            else
                return response.json();
        })
    }

    async genericPost(postUrl, body) {
        const url = `${this.hostURL}${postUrl}`;

        return await fetch(url, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("HTTP error, status = " + response.status);
            }
            else if (response.status === 204)
                return null;
            else
                return response.json();
        })
    }

    async login(activeAccount)
    {
        var body = {
            aadId: activeAccount.localAccountId,
            name: activeAccount.name,
            email: activeAccount.localAccountId + '@gmail.com', 
            description: 'Sample description'
        }

        var retVal = this.genericPost(`/api/profile/login`, body);
        return retVal;
        
    }

    async getProfileAccount(profileId)
    {
        var retVal = this.genericGet(`/api/account/profile/${profileId}`);
        console.log("getProfileAccount called, returned:", retVal);
        return retVal;
    }

    async createProfileAccount(profileId, name, description, amount)
    {
        var body = {
            ProfileId: profileId,
            Description: description,
            Balance: amount
        }

        var retVal = this.genericPost(`/api/account`, body);
        console.log("createProfileAccount called, returned: ", retVal);
        return retVal;
    }

    async getLeaderboard()
    {
        var retVal = this.genericGet(`/api/leaderboard`);
        console.log("getLeaderboard called, returned:", retVal);
        return retVal;
    }

    async getAccounts(profileId)
    {
        var retVal = this.genericGet(`/api/leaderboard`);
        console.log(`getAccounts called with ${profileId}, returned:`, retVal);
        return retVal;
    }

    async getHoldings(acctNum)
    {
        var retVal = this.genericGet(`/api/account/${acctNum}/holding`);
        console.log(`getHoldings called with ${acctNum}, returned:`, retVal);
        return retVal;
    }

    async getPerformance(acctNum)
    {
        var retVal = this.genericGet(`/api/account/${acctNum}/performance`);
        console.log(`getPerformance called with ${acctNum}, returned:`, retVal);
        return retVal;
    }

    async getPricing(symbol)
    {
        var retVal = this.genericGet(`/api/pricing/getinfo/${symbol}`);
        console.log(`getPricing called with ${symbol}, returned:`, retVal);
        return retVal;
    }

    async buyStock(acctNum, symbol, quantity)
    {
        var body = {
            OperationType: 0,
            Symbol: symbol,
            Shares: quantity
        }

        var retVal = this.genericPost(`/api/account/${acctNum}/buystock`, body);
        console.log("buyStock called, returned: ", retVal);
        return retVal;
    }

    async sellStock(acctNum, symbol, quantity)
    {
        var body = {
            OperationType: 1,
            Symbol: symbol,
            Shares: quantity
        }

        var retVal = this.genericPost(`/api/account/${acctNum}/sellstock`, body);
        console.log("sellStock called, returned: ", retVal);
        return retVal;
    }
}

const serviceApi = new ServiceApi();
export default serviceApi;
