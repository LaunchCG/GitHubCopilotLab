import MockService from './MockApi';
import ServiceApi from './ServiceApi';
import { ConfigContext } from '../ConfigContext';
import React from 'react';

const GetService = () => {
    // console.log("Entered GetService...");
    const { useMockData, hostURL } = React.useContext(ConfigContext);
    // console.log("Getting service interface, mockData = ", useMockData);
    if (useMockData) 
        return MockService;
    else {
        ServiceApi.ConfigureHostURL(hostURL);
        return ServiceApi;
    }
}

export default GetService;