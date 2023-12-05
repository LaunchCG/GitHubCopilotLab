// Implement Form for buying stock

import React, {useCallback, useState} from 'react';
import { Button, Form } from 'react-bootstrap';
import GetService from '../services/ServiceFactory';
import debounce from "lodash.debounce";

export const BuyStock = (props) => {
    const SYMBOL_LOOKUP_TIMING = 750;

    var serviceApi = GetService();

    const [symbol, setSymbol] = useState("");
    const [quantity, setQuantity] = useState("");

    const [pricingInfo, setPricingInfo] = useState(null);
    const [pricingError, setPricingError] = useState(null);

    const [transactionStatus, setTransactionStatus] = useState("");

    const handleQuantityChange = (e) => {
        setQuantity(e.target.value);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
    const handleSymbolChange = useCallback(
        debounce(
            async (e) => {
                setTransactionStatus("");
                var curSymbol = e.target.value;

                console.log("Setting symbol and calling getPricingInfo", curSymbol);
                setSymbol(curSymbol);

                setPricingError(null);
                if (curSymbol !== "")
                {
                    try 
                    {
                        console.log("Getting pricing, current symbol = ", curSymbol);
                        var tempPriceInfo = await serviceApi.getPricing(curSymbol);
                        setPricingInfo(tempPriceInfo);
                    }
                    catch {
                        console.log(`Exception hit trying to get information for ${curSymbol}`)
                    }
            
                    if (!tempPriceInfo)
                        setPricingError(`Unable to retrieve information for ${curSymbol}`);
                }
                else
                    setPricingInfo(null);
            }, SYMBOL_LOOKUP_TIMING), []);

    const submitButton = async (e) => {
        e.preventDefault();
        const buyResult = await props.buyStockFunc(symbol, Number(quantity));
        console.log("Buy Submit, result = ", buyResult);
        setTransactionStatus(buyResult.statusMessage);
        resetData();
    }

    const resetData = () => {
        setQuantity("");
    }

    var pricingStatus;
    var pricingDescription;
    var enableBuy = !pricingError && pricingInfo && (Number(quantity) > 0);

    if (pricingError)
    {
        pricingStatus = pricingError;
        pricingDescription = '';
    }
    else if (pricingInfo)
    {
        pricingStatus = `${pricingInfo.name} - Current Price = ${pricingInfo.price}`;
        pricingDescription = pricingInfo.description;
    }
    else
    {
        pricingStatus = 'No company selected';
        pricingDescription = '';
    }

    return (
        <>
            <Form>
                <Form.Group controlId='SymbolGroup'>
                    <Form.Label>Symbol</Form.Label>
                    <Form.Control type="text" name="symbol" onChange={handleSymbolChange}/>
                </Form.Group>
                <Form.Group controlId='QuantityGroup'>
                    <Form.Label>Quantity</Form.Label>
                    <Form.Control type="text" name="quantity" value={quantity} onChange={handleQuantityChange}/>
                </Form.Group>
                <Form.Label>{pricingStatus}</Form.Label>
                <Form.Label>{pricingDescription}</Form.Label>
                <p></p>
                <Button onClick={submitButton} disabled={!enableBuy}>Buy Stock</Button>
                <div style={{marginLeft:5}}><Form.Label>{transactionStatus}</Form.Label></div>
            </Form>
        </>
    );
}
