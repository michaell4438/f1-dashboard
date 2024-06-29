import React, { useState } from 'react'
import './App.css'
import { Container, Row, Col } from 'react-grid-system';

function Dashboard() {
    return (
        <>
            <Container className={"centered"} style={{ margin: '0px', width: '100%', height: '100%' }} fluid>
                <Col>
                    <Row>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                        <div className={"shiftLightRect"} style={{ background: 'green' }}/>
                    </Row>
                    <Row align="center" style={{ height: '100%', width: '100%' }}>
                        <Col>
                            <Row style={{ height: '20vh' }}>
                                <p className={"miscIndicator"}>1:00.000 : 1:30.000</p>
                            </Row>
                            <Row style={{ height: '10.3vh' }}>
                                <p className={"miscIndicator"} style={{ color: 'green' }}>-1.000</p>
                            </Row>
                            <Row justify="center" style={{ height: '66.6vh' }}>
                                <div className={"positionOuter"}>
                                    <p className={"miscIndicator"} style={{ fontSize: '6rem' }}>1 </p>
                                    <p className={"miscIndicator smallFont"}> /20</p>
                                </div>
                            </Row>
                        </Col>
                        <Col>
                            <Row style={{ height: '32.3vh' }}>
                                <p className={"miscIndicator"}>1350 <p className={"smallFont"}>rpm</p></p>
                                <p className={"miscIndicator"}>150 <p className={"smallFont"}>km/h</p></p>
                            </Row>
                            <Row style={{ height: '32.3vh' }}>
                                <p className={"gearIndicator miscIndicator"}>1</p>
                            </Row>
                            <Row style={{ height: '32.3vh' }}>
                                <div className={"miscIndicator recommendedGearOuter"}>
                                    <p className={"miscIndicator recommendedGear"}>2</p>
                                </div>
                            </Row>
                        </Col>
                        <Col>
                            <Col style={{ height: '32.2vh' }}>
                                <p className={"miscIndicator verticalCenter drsIndicatorNone"}> DRS </p>
                            </Col>
                            <Col style={{ height: '32.2vh' }}>
                                <Row>
                                    <p className={"miscIndicator"}>S1: 0.000</p>
                                </Row>
                                <Row>
                                    <p className={"miscIndicator"}>S2: 0.000</p>
                                </Row>
                                <Row>
                                    <p className={"miscIndicator"}>S3: 0.000</p>
                                </Row>
                            </Col>
                            <Col style={{ height: '32.2vh' }}>
                                <p className={"miscIndicator verticalCenter ersIndicator ersIndicatorOvertake"}> 50% </p>
                            </Col>
                        </Col>
                    </Row >
                </Col>
            </Container >
        </>
    )
}

function App() {
    return (
        <>
            <Dashboard />
        </>
    )
}

export default App
