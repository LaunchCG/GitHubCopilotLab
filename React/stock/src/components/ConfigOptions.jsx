import React from 'react';
import MyCard from './MyCard';
import Form from 'react-bootstrap/Form';
import { ConfigContext } from '../ConfigContext';

export const ConfigOptions = (props) => 
{
    var context = React.useContext(ConfigContext)
    const {useMockData, setUseMockData, displayRows, setDisplayRows, forceRemountFunc} = context;

    const toggleMockData = () => {
        setUseMockData(!useMockData);
        if (forceRemountFunc)
            forceRemountFunc();
    }

    const toggleDisplayRows = () => {
        setDisplayRows(!displayRows);
        if (forceRemountFunc)
            forceRemountFunc();
    }

    return (
        <>
           <MyCard CardTitle='Configuration Options' IsClosed={true}>
                <Form>
                    <Form.Switch 
                        id='UseMockData'
                        label='Use Mock Data'
                        onChange={toggleMockData}
                        checked={useMockData}
                    />
                    <Form.Switch 
                        id='DisplayRows'
                        label='Display Leaderboard in Columns'
                        onChange={toggleDisplayRows}
                        checked={displayRows}
                    />
                </Form>

            </MyCard>
        </>
    );
}

export default ConfigOptions;