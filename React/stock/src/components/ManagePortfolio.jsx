import React, {useEffect, useState} from 'react';
import GetService from '../services/ServiceFactory';
import { TradeStock } from './TradeStock';
import CurHoldings from './CurHoldings';

export const ManagePortfolio = ({activeAccount}) => {
    const [performance, setPerformance] = useState([]);

    var serviceApi = GetService();

    useEffect(() => {
        const fetchData = async() => {
            await loadPerformance(activeAccount.accountNumber);
        }
        
        fetchData();
// eslint-disable-next-line        
    }, [])

    const loadPerformance = async (acctNum) => {
        var tempPerformance = await serviceApi.getPerformance(acctNum);
        setPerformance(tempPerformance);
    }

    const buyStock = async (symbol, quantity) => {
        var acctNum = activeAccount.accountNumber;
        var buyResult = await serviceApi.buyStock(acctNum, symbol, quantity);
        await loadPerformance(acctNum);
        return buyResult;
    }

    const sellStock = async (symbol, quantity) => {
        var acctNum = activeAccount.accountNumber;
        var sellResult = await serviceApi.sellStock(acctNum, symbol, quantity);
        await loadPerformance(acctNum);
        return sellResult;
    }

    return (
        <>
            <div style={{marginLeft: "10px", width: "90%"}}>
                <h2>Manage Portfolio for {activeAccount.description}</h2>
                <CurHoldings performance={performance}></CurHoldings>
                <TradeStock buyStockFunc={buyStock} sellStockFunc={sellStock} performance={performance}></TradeStock>
            </div>
       </>
    )
}