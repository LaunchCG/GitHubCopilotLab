import { Tabs, Tab } from 'react-bootstrap';
import { BuyStock } from '../components/BuyStock';
import { SellStock } from './SellStock';
import { AIRecommendation } from './AIRecommendation';

export const TradeStock = (props) => {
    var stocks = props.performance.stocks ?? [];

    return (
        <Tabs>
            <Tab eventKey="BuyTab" title="Buy Stock">
                <BuyStock buyStockFunc={props.buyStockFunc}></BuyStock>
            </Tab>
            <Tab eventKey="SellTab" title="Sell Stock">
                <SellStock holdings={stocks} sellStockFunc={props.sellStockFunc}></SellStock>
            </Tab>
            <Tab eventKey="AITab" title="Generate AI Analysis">
                <AIRecommendation holdings={stocks}></AIRecommendation>
            </Tab>
        </Tabs>
    );
}

