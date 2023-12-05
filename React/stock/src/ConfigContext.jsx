import React, {useState} from 'react';

const ConfigContext = React.createContext();

function ConfigProvider({children, forceRemountFunc}) {
    const [useMockData, setUseMockData] = useState(false);
    const [displayRows, setDisplayRows] = useState(true);
    const [hostURL, setHostURL] = useState('https://bluestockapi-staging.azurewebsites.net');
    const value = {
        useMockData, setUseMockData, displayRows, setDisplayRows, hostURL, setHostURL, forceRemountFunc
    }

    return <ConfigContext.Provider value={value}>{children}</ConfigContext.Provider>
}

export {ConfigProvider, ConfigContext};

