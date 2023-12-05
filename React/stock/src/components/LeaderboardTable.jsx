import { Button } from 'react-bootstrap'
import React from 'react';
import BaseTable from './BaseTable';
import utility from '../Utility/Utility';

function LeaderboardTable(props)
{
    var selectFunc = props.selectFunc;
    const data = props.data;

    const columns = [
        {
            accessorKey: 'description',
            header: 'Description',
            cell: info => <Button variant="link" onClick={()=> {selectFunc(info.row.original.accountNumber); } }>{info.getValue()}</Button>,
            footer: info => info.column.id,
        },
        {
            accessorKey: 'balance',
            header: 'Balance',
            cell: info => utility.FormatCurrency(info.getValue()),
            footer: info => info.column.id,
        },
        {
            accessorKey: 'gain',
            header: 'Gain',
            cell: info => utility.FormatCurrency(info.getValue()),
            footer: info => info.column.id,
        },
    ];
    
    return (
        <BaseTable data={data} columns={columns}></BaseTable>
    );
}

export default LeaderboardTable;
