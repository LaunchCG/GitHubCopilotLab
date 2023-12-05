import { NavigationBar } from './NavigationBar.jsx';

export const PageLayout = (props) => {
    return (
        <>
            <NavigationBar />
            <br />
            <h5>
                <center>Welcome to the Blue Team Stock App</center>
            </h5>
            <br />
                {props.children}
            <br />
        </>
    );
}