import React, {useState, useEffect} from 'react';
import { ManagePortfolio } from '../components/ManagePortfolio';
import { ConfigOptions } from '../components/ConfigOptions';
import { Tabs, Tab } from 'react-bootstrap';
import { LeaderboardFull } from '../components/LeaderboardFull';
import PromptCreate from '../components/PromptCreate'
import GetService from '../services/ServiceFactory';

export const MainPage = (props) => {
    // Attempt to get active profile and account
    const [activeProfile, setActiveProfile] = useState(null);
    const [activeAccount, setActiveAccount] = useState(null);
    const [loadingActiveAccount, setLoadingActiveAccount] = useState(false);
    const [reloadCounter, setReloadCounter] = useState(1);

    var serviceApi = GetService();

    useEffect(() => {
        const fetchData = async() => {
            setLoadingActiveAccount(true);
            var tempProfile = await serviceApi.login(props.activeAccount);
            console.log("Called Login with account = ", props.activeAccount);
            setActiveProfile(tempProfile);
            loadAccount(tempProfile.profileId);
        }
        
        fetchData();
// eslint-disable-next-line        
    }, [reloadCounter]);

    const loadAccount = async(profileId) => {
        var tempAccount = await serviceApi.getProfileAccount(profileId);
        setLoadingActiveAccount(false);
        setActiveAccount(tempAccount);
        console.log("Active account set to: ", tempAccount);
    }

    const createNewAccount = (name, description) => {
        console.log("CreateNewAccount called", name, description, activeProfile.profileId);
        serviceApi.createProfileAccount(activeProfile.profileId, name, description, 5000);
        setReloadCounter(reloadCounter + 1);
    };

    if (loadingActiveAccount)
        return (
            <>
                <h3>Finding account for profile...</h3>
            </>
        )

    if (!activeAccount) {
        return (
            <>
                <PromptCreate createNewAccount={createNewAccount}/>
            </>
        )
    }

    return (
        <>
            <Tabs>
                <Tab eventKey="Leaderboard" title="Leaderboard">
                    <LeaderboardFull/>
                </Tab>
                <Tab eventKey="MyPortfolio" title="My Portfolio">
                    <ManagePortfolio activeAccount={activeAccount}></ManagePortfolio>
                </Tab>
            </Tabs>
            <ConfigOptions forceRemountFunc={props.forceRemountFunc}></ConfigOptions>
        </>);
}