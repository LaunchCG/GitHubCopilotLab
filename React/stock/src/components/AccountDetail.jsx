import BaseTable from './BaseTable';
import { Button } from 'react-bootstrap'

function AccountDetail(props)
{
    var sellHoldingFunc = props.sellHoldingFunc;
    var accountNumber = props.accountNumber;
    const data = props.data;

    const columns = [
        {
            accessorKey: 'symbol',
            header: 'Symbol',
            cell: info => info.getValue(),
            footer: info => info.column.id,
        },
        {
            accessorKey: 'shares',
            header: 'Shares',
            cell: info => info.getValue(),
            footer: info => info.column.id,
        },
        {
            id: 'Sell',
            cell: info => <Button variant="link" onClick={()=> {sellHoldingFunc(accountNumber, info.row.original.symbol, info.row.original.shares); } }>Sell Holding</Button>,
        },
    ];
    
    console.log("AccountDetail render, data=", data)
    return (
        <BaseTable data={data} columns={columns}></BaseTable>
    );
}

export default AccountDetail;
