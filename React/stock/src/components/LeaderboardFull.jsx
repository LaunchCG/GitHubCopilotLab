import GetService from '../services/ServiceFactory';
import React, {useEffect, useState} from 'react';
import MyCard from '../components/MyCard';
import LeaderboardTable from './LeaderboardTable';
import PortfolioPerformance from '../components/PortfolioPerfomance';
import { ConfigContext } from '../ConfigContext';

export const LeaderboardFull = (props) => {
    const [curAccount, setCurAccount] = useState({});
    const [leaderboard, setLeaderboard] = useState([]);
    const [loadingLeaderboard, setLoadingLeaderboard] = useState(false);
    const [performance, setPerformance] = useState([]);
    const [loadingPerformance, setLoadingPerformance] = useState(false);

    var serviceApi = GetService();
    
    useEffect(() => {
        const fetchData = async() => {
            setLoadingLeaderboard(true);
            var tempLeaderboard = await serviceApi.getLeaderboard();
            if (tempLeaderboard.length > 0) {
                setCurAccount(tempLeaderboard[0]);
                fetchHoldings(tempLeaderboard[0].accountNumber)
            }
            setLoadingLeaderboard(false);
            setLeaderboard(tempLeaderboard);
        }
        
        fetchData();
// eslint-disable-next-line        
    }, [])

    const fetchHoldings = async(acctNum) => {
        setLoadingPerformance(true);
        var tempPerformance = await serviceApi.getPerformance(acctNum);
        setLoadingPerformance(false);
        setPerformance(tempPerformance);
    }

    const selectFromLeaderboard = (acctNum) => {
        var account = leaderboard.find(acct => acct.accountNumber === acctNum);
        setCurAccount(account);
        fetchHoldings(account.accountNumber);
    }

    const StockLeaderboard = () => {
        if (loadingLeaderboard)
            return (
                <>
                    <h3>Loading leaderboard...</h3>
                </>
            )
        return (
            <>
                <LeaderboardTable data={leaderboard} selectFunc={selectFromLeaderboard}></LeaderboardTable>
            </>
        )
    }

    const StockPortfolioPerformance = ({name}) => {
        if (loadingPerformance)
            return (
                <><h4>Loading detailed performance...</h4>
                </>
            )

        return (
            <>
                <PortfolioPerformance data={performance} name={name}></PortfolioPerformance>
            </>
        )
    }

    const { displayRows } = React.useContext(ConfigContext);
    const perfName = `Performance of ${curAccount.description}`;
    const displayWidth = displayRows ? "60%" : "100%";

    return (
        <>
                <div className='flex-child' style={{width: "100%"}}>
                    <MyCard CardTitle="Portfolio Leaderboard" IsClosed={false}>
                        <div className='flex-container' style={{display: 'flex', flexDirection: "row"}}>
                            <div className='flex-child' style={{width: displayWidth}}>
                                <StockLeaderboard></StockLeaderboard>
                            </div>
                            {displayRows &&
                                <div className='flex-child' style={{width: "40%"}}>
                                    <StockPortfolioPerformance name={perfName}></StockPortfolioPerformance>
                                </div>
                            }
                        </div>
                    </MyCard>
                </div>
                {!displayRows &&
                <MyCard CardTitle={perfName} IsClosed={true}>
                    <StockPortfolioPerformance></StockPortfolioPerformance>
                </MyCard>
                }
        </>
    )
}