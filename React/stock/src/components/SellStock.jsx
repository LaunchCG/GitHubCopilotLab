// Implement Form for selling stock

import React, {useState} from 'react';
import { Button, Form } from 'react-bootstrap';
import { DropdownList } from 'react-widgets/cjs';
import GetService from '../services/ServiceFactory';

export const SellStock = ({holdings, sellStockFunc}) => {
    var serviceApi = GetService();

    const [form, setForm] = useState({
        symbol : '',
        quantity : ''
    });
    const [pricingInfo, setPricingInfo] = useState(null);
    const [pricingError, setPricingError] = useState(null);
    const [transactionStatus, setTransactionStatus] = useState("");

    const handleEditChange = (e) => {
        setForm({...form, [e.target.name]:e.target.value})
    }

    const handleSymbolChange = (symName) => {
        setForm({...form, symbol: symName});
        getPricingInfo(symName);
        console.log("handleSymbolChange", symName);
        setTransactionStatus('');
    }

    const submitButton = async (e) => {
        e.preventDefault();
        console.log(form);
        try {
            const sellStatus = await sellStockFunc(form.symbol, Number(form.quantity));
            setTransactionStatus(sellStatus.statusMessage);
            resetData();
        }
        catch (error) {
            alert(`An error was encountered during the sell transaction: ${error}`)
        }
    }

    const resetData = () => {
        setPricingInfo(null);
        setForm({
            symbol : '',
            quantity : ''
        });
    }

    const getPricingInfo = async(lookupSymbol) => {
        try {
            var tempPriceInfo = await serviceApi.getPricing(lookupSymbol);
            if (!tempPriceInfo)
                setPricingError(`Symbol ${lookupSymbol} not found`)
            else
                setPricingError(null);

            setPricingInfo(tempPriceInfo);
        }
        catch (error)
        {
            console.log("Caught error in getPricingInfo", error);
            setPricingError(`Error in getting information for ${lookupSymbol}`);
        }
    }

    var pricingStatus;
    var pricingDescription;
    var enableBuy = !pricingError && pricingInfo;

    if (pricingError)
    {
        pricingStatus = pricingError;
        pricingDescription = '';
    }
    else if (pricingInfo)
    {
        // Get current holding
        const curHolding = holdings.find(h => h.symbol === form.symbol);
        console.log("Current holding: ", curHolding);

        pricingStatus = `Portfolio has ${curHolding.currentShares} shares of ${curHolding.symbol} with a price of ${pricingInfo.price} and total value of ${pricingInfo.price * curHolding.currentShares}`;
        pricingDescription = pricingInfo.description;
    }
    else
    {
        pricingStatus = 'No company selected';
        pricingDescription = '';
    }

    const holdingSymbols = holdings.map(h => h.symbol);

    return (
        <>
            <Form>
                <Form.Group controlId='SymbolGroup'>
                    <Form.Label>Stock to Sell</Form.Label>
                    <DropdownList 
                        data={holdingSymbols}
                        onChange={handleSymbolChange}
                        value={form.symbol}
                    />

                </Form.Group>
                <Form.Group controlId='QuantityGroup'>
                    <Form.Label>Quantity</Form.Label>
                    <Form.Control type="text" name="quantity" value={form.quantity} onChange={handleEditChange}/>
                </Form.Group>
                <Form.Label>{pricingStatus}</Form.Label>
                <Form.Label>{pricingDescription}</Form.Label>
                <p></p>
                <Button onClick={submitButton} disabled={!enableBuy}>Sell Stock</Button>
                {transactionStatus}
            </Form>
        </>
    );
}
