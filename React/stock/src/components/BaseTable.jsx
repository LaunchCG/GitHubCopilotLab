import { Table as BTable } from 'react-bootstrap'
import React from 'react';

import {
  flexRender,
  getCoreRowModel,
  useReactTable,
} from '@tanstack/react-table'

function BaseTable(props)
{
    const data = props.data;
    const columns = props.columns;
    
    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
    })

    return (
        <div className="p-2">
            <BTable striped bordered hover responsive size="sm">
            <thead>
                {table.getHeaderGroups().map(headerGroup => (
                <tr key={headerGroup.id}>
                    {headerGroup.headers.map(header => (
                    <th key={header.id}>
                        {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext()
                            )}
                    </th>
                    ))}
                </tr>
                ))}
            </thead>
            <tbody>
                {table.getRowModel().rows.map(row => (
                <tr key={row.id}>
                    {row.getVisibleCells().map(cell => (
                    <td key={cell.id}>
                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </td>
                    ))}
                </tr>
                ))}
            </tbody>
            </BTable>
        </div>
    );
}

export default BaseTable;
