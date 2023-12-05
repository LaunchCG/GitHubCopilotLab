import './styles/App.css';
import 'react-widgets/styles.css';

import { MsalProvider, useMsal, } from '@azure/msal-react';
import { PageLayout } from './components/PageLayout';
import { MainPage } from './pages/MainPage';
import { WelcomePage } from './pages/WelcomePage';
import { ConfigProvider } from './ConfigContext';
import React from 'react';

const MainContent = (props) => {
  const { instance } = useMsal();
  const activeAccount = instance.getActiveAccount();
  console.log("MainContent", activeAccount);

  return (
    <div>
      {activeAccount && <MainPage activeAccount={activeAccount}></MainPage>
      }
      {!activeAccount && <WelcomePage></WelcomePage>
      }
    </div>
  );
};

const App = ({ instance }) => {
  const [redrawCount, setRedrawCount] = React.useState(1);

  const forceRemount = () => {
    setRedrawCount(redrawCount + 1);
  }

  return (
    <MsalProvider instance = {instance}>
      <ConfigProvider forceRemountFunc={forceRemount}>
        <PageLayout>
          <MainContent key={redrawCount}></MainContent>
        </PageLayout>
      </ConfigProvider>
    </MsalProvider>
  );
}

export default App;
