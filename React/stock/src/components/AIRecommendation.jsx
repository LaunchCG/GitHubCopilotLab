import React, {useState} from 'react';
import { Button } from 'react-bootstrap';

export const AIRecommendation = () => 
{
    const [showRecommendations, setShowRecommendations] = useState(false);
    const [loading, setLoading] = useState(false);

    const generateRecommendations = () => {
        setLoading(true);
        setTimeout( () => { setShowRecommendations(true); setLoading(false); }, 3000 )
    }

    return (
        <>
            <div style={{margin: "10px", width: "90%"}}>
                {!showRecommendations && 
                    <Button onClick={generateRecommendations}>Generate AI Analysis</Button>
                }
                {loading &&
                    <p>Analyzing portfolio and generating AI recommendations...</p>
                }
                {showRecommendations &&
                <p>
                    <b>Your stock portfolio is graded as a B</b>
                    <p>Your stock portfolio is graded as a B because it consists of well-established tech companies like Microsoft (MSFT) and Google (GOOG), 
                        which are known for their growth potential. Additionally, you hold some moderately risky stocks such as SoFi (SOFI) and TMDX. 
                        However, a diversified portfolio might help reduce risk further and improve the grade.</p>
                    <b>Buy Recommendations:</b> 
                        <ul>
                            <li>Buy more Microsoft (MSFT): Purchase an additional $300
                    worth of MSFT shares. 
                            </li>
                            <li>
                                Buy more Google (GOOG): Purchase an additional $200
                    worth of GOOG shares. 
                            </li>
                            <li>
                                Diversify with an S&amp;P 500 ETF (e.g., SPY): Invest
                            $200 in an S&amp;P 500 ETF for diversification.
                            </li>
                        </ul>
                    
                    <b>Sell Recommendations:</b>
                    <ul>
                        <li>
                            Sell all SoFi (SOFI) holdings: Liquidate your $854 worth of SOFI shares. 
                        </li>
                        <li>
                        Sell all TMDX holdings: Liquidate your $780 worth of TMDX shares.                        
                        </li>
                    </ul>
                    With these
                    recommendations, you'll allocate more funds to established tech giants (MSFT
                    and GOOG) for growth potential while diversifying a portion of your
                    portfolio with an S&amp;P 500 ETF. You'll also reduce risk by selling your
                    positions in SOFI and TMDX. Keep in mind that these are specific
                    recommendations based on the information provided, and you should consider
                    your own financial goals and risk tolerance before executing any trades.
                    Additionally, consult with a financial advisor or do further research if
                    needed.
                </p>        
                }
            </div>            
        </>
    );
}
