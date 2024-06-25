import React, { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { Container, Row, Col } from 'react-grid-system';

function Dashboard() {
    return (
        <>
            <Container fluid>
                <Row>
                    <Col>
                        <p className={"miscIndicator"}>1:00.00</p>
                    </Col>
                    <Col>
                        <Row>
                            <p className={"miscIndicator"}>150 km/h</p>
                        </Row>
                        <Row>
                            <p className={"gearIndicator"}>1</p>
                            <div className={"recommendedGearOuter"}>
                                <p className={"recommendedGear"}>2</p>
                            </div>
                        </Row>
                    </Col>
                    <Col>
                        <Row>
                            <p className={"miscIndicator smallFont"}>S1: 0.00</p>
                        </Row>
                        <Row>
                            <p className={"miscIndicator smallFont"}>S2: 0.00</p>
                        </Row>
                        <Row>
                            <p className={"miscIndicator smallFont"}>S3: 0.00</p>
                        </Row>
                    </Col>
                </Row >
            </Container >
        </>
    )
}

function App() {
    const [connected, setConnected] = useState(false)

    return (
        <>
            <Dashboard />
        </>
    )
}

export default App
