import BaseTable from './BaseTable';
import utility from '../Utility/Utility';

function PortfolioPerformance({data, name})
{
    console.log("PorfolioPerformance", data)
    const stockData = data.stocks;
    console.log("stockData", stockData);
    return (
        <>
            <div><b>{name}</b></div>
            <div>
                <b>Cash Position: </b>{utility.FormatCurrency(data.cashBalance)}
            </div>
            <div>
                <b>Stock Balance: </b>{utility.FormatCurrency(data.stockBalance)}
            </div>
            <div>
                <b>Overall Gain: </b>{utility.FormatCurrency(data.gain)}
            </div>
            <p></p>
            
            {stockData && <PortfolioPerformanceTable data={stockData}></PortfolioPerformanceTable>}
        </>
    )
}

function PortfolioPerformanceTable(props)
{
    const data = props.data;

    const columns = [
        {
            accessorKey: 'symbol',
            header: 'Symbol',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'gain',
            header: 'Gain',
            cell: info => utility.FormatCurrency(info.getValue()),
        },
        {
            accessorKey: 'currentShares',
            header: 'Shares',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'currentValue',
            header: 'Value',
            cell: info => utility.FormatCurrency(info.getValue()),
        }
    ];    
    return (
        <BaseTable data={data} columns={columns}></BaseTable>
    );
}

export default PortfolioPerformance;
