import {Accordion} from 'react-bootstrap';

function MyCard(props) {
    return (
        <Accordion defaultActiveKey={props.IsClosed ? "" : "0"}>
            <Accordion.Item eventKey="0">
                <Accordion.Header>{props.CardTitle}</Accordion.Header>
                <Accordion.Body>
                    {props.children}
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    );
}

export default MyCard;