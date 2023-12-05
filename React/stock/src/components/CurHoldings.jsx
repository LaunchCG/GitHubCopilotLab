import BaseTable from './BaseTable';
import Utility from '../Utility/Utility'

function CurHoldings(props)
{
    const data = props.performance.stocks ?? [];

    const columns = [
        {
            accessorKey: 'symbol',
            header: 'Symbol',
            cell: info => info.getValue(),
            footer: info => info.column.id,
        },
        {
            accessorKey: 'currentShares',
            header: 'Shares',
            cell: info => info.getValue(),
            footer: info => info.column.id,
        },
        {
            accessorKey: 'currentValue',
            header: 'Value',
            cell: info => Utility.FormatCurrency(info.getValue()),
            footer: info => info.column.id,
        },
    ];
    
    console.log("CurHoldings render, data=", data)
    return (
        <>
            <p><b>Cash Balance {Utility.FormatCurrency(props.performance.cashBalance)}</b></p>
            <p>Stock Holdings</p>
            <BaseTable data={data} columns={columns}></BaseTable>
        </>
    );
}

export default CurHoldings;
