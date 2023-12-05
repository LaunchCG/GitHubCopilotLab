// Ask for new account

import React, {useState} from 'react';
import { Button, Form } from 'react-bootstrap';

function PromptCreate(props) {
    const [form, setForm] = useState({
        accountDescription : '',
        name : ''
    });

    const handleChange = (e) => {
        setForm({...form, [e.target.name]:e.target.value})
    }

    const submitButton = (e) => {
        e.preventDefault();
        props.createNewAccount(form.name, form.accountDescription);
        setForm({accountDescription: '', name: ''});
    }

    var enableSubmit = form.accountDescription.length > 0 && form.name.length > 0;

    return (
        <>
            <div style={{marginLeft: "10px", width: "90%"}}>
                <h5><center>Enter information to join the Stock Picking Contest!</center></h5>
                <p>Participants start with an account of $5,000 and can buy and sell stocks.
                    A leaderboard will available to show the top stock-pickers.  The contest will run
                    every quarter, e.g. the next open contest will run from Oct 1st through Dec 21.
                </p>    
                    The primary differences from a real broker are:
                    <ul>
                        <li>Only stocks from the NYSE or Nasdaq are support</li>
                        <li>Stocks are bought/sold based on their closing prices</li>
                        <li>A flat transaction fee of $10 is added to every trade</li>
                        <li>Gains or losses are virtual  (This may be a good thing!) </li>
                    </ul>
                <Form>
                <Form.Group controlId='SymbolGroup'>
                        <Form.Label>Your Name</Form.Label>
                        <Form.Control type="text" name="name" value={form.name} onChange={handleChange}/>
                    </Form.Group>
                    <Form.Group controlId='SymbolGroup'>
                        <Form.Label>Account Description</Form.Label>
                        <Form.Control type="text" name="accountDescription" value={form.accountDescription} onChange={handleChange}/>
                    </Form.Group>
                    <p></p>
                    <Button onClick={submitButton} disabled={!enableSubmit}>Enter Contest</Button>
                </Form>

            </div>
            <div>
                {props.statusLine}
            </div>
        </>
    );
}

export default PromptCreate;