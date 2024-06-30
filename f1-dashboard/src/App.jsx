import React, { useState, useCallback, useEffect } from 'react'
import './App.css'
import { Container, Row, Col } from 'react-grid-system';
import useWebSocket, { ReadyState } from 'react-use-websocket';

function formatTime(timeMs, signed = false, decimalPlaces = 3) {
    const sign = signed ? timeMs < 0 ? "-" : "+" : "";
    const time = Math.abs(timeMs);
    const minutes = Math.floor(time / 60000);
    const seconds = Math.floor((time % 60000) / 1000);
    const milliseconds = Math.floor((time % 1000));
    const padAmount = (seconds > 10 || minutes > 0) ? 2 : 1;
    return `${sign}${minutes > 0 ? `${minutes}:` : ""}${seconds.toString().padStart(padAmount, '0')}.${milliseconds.toString().padStart(decimalPlaces, '0')}`;
}

function Dashboard({lapTime, lastLapTime, position, totalPositions, rpm, speed, speedUnit, gear, recommendedGear, drsState, s1, s2, ersCharge, isOvertake, shiftLights, currentLap, totalLaps}) {
    var drsClassName;
    if (drsState === "Active") {
        drsClassName = "drsIndicatorActive";
    } else if (drsState === "None") {
        drsClassName = "drsIndicatorNone";
    } else {
        drsClassName = "drsIndicatorReady";
    }
    return (
        <>
            <Container className={"centered"} style={{ margin: '0px', width: '100%', height: '100%' }} fluid>
                <Col>
                    <Row style={{ height: '3vh' }}>
                        {[...Array(Math.round(shiftLights / 5))].map((value, index) =>
                            <div className={"shiftLightRect"} style={{ background: 'green' }} />
                        )}
                    </Row>
                    <Row align="center" style={{ height: '100%', width: '100%' }}>
                        <Col>
                            <Row style={{ height: '20vh' }}>
                                <p className={"miscIndicator"}>{formatTime(lapTime)} : {formatTime(lastLapTime)}</p>
                            </Row>
                            <Row style={{ height: '10.3vh' }}>
                                <p className={"miscIndicator"}>{currentLap}/{totalLaps}</p>
                            </Row>
                            <Row justify="center" style={{ height: '66.6vh' }}>
                                <div className={"positionOuter"}>
                                    <p className={"miscIndicator"} style={{ fontSize: '6rem' }}>{position}</p>
                                    <p className={"miscIndicator smallFont"}>/{totalPositions}</p>
                                </div>
                            </Row>
                        </Col>
                        <Col>
                            <Row style={{ height: '32.3vh' }}>
                                <p className={"miscIndicator"}>{rpm} <p className={"smallFont"}>rpm</p></p>
                                <p className={"miscIndicator"}>{speed} <p className={"smallFont"}>{speedUnit}</p></p>
                            </Row>
                            <Row style={{ height: '32.3vh' }}>
                                <p className={"gearIndicator miscIndicator"}>{gear}</p>
                            </Row>
                            <Row style={{ height: '32.3vh' }}>
                                {recommendedGear === 0 ? "" :
                                    <div className={"miscIndicator recommendedGearOuter"}>
                                        <p className={"miscIndicator recommendedGear"}>{recommendedGear}</p>
                                    </div>
                                }
                            </Row>
                        </Col>
                        <Col>
                            <Col style={{ height: '32.2vh' }}>
                                <p className={`miscIndicator verticalCenter ${drsClassName}`}> DRS {drsClassName == "drsIndicatorReady" ? `${drsState}m` : ""} </p>
                            </Col>
                            <Col style={{ height: '32.2vh' }}>
                                <Row>
                                    <p className={"miscIndicator"}>S1: {formatTime(s1)}</p>
                                </Row>
                                <Row>
                                    <p className={"miscIndicator"}>S2: {formatTime(s2)}</p>
                                </Row>
                            </Col>
                            <Col style={{ height: '32.2vh' }}>
                                <p className={`miscIndicator verticalCenter ersIndicator ${isOvertake ? "ersIndicatorOvertake" : ""}`}> {((Math.round(ersCharge/4000000 * 1000))/10).toFixed(1)}% </p>
                            </Col>
                        </Col>
                    </Row >
                </Col>
            </Container >
        </>
    )
}

function App() {
    const [socketUrl, setSocketUrl] = useState("ws://" + window.location.hostname + ":5174");
    const { sendMessage, lastMessage, readyState } = useWebSocket(socketUrl);

    const [lapTime, setLapTime] = useState(0);
    const [lastLapTime, setLastLapTime] = useState(0);
    const [position, setPosition] = useState(1);
    const [totalPositions, setTotalPositions] = useState(20);
    const [rpm, setRpm] = useState(0);
    const [speed, setSpeed] = useState(0);
    const [speedUnit, setSpeedUnit] = useState("km/h");
    const [gear, setGear] = useState(0);
    const [recommendedGear, setRecommendedGear] = useState(0);
    const [drsState, setDrsState] = useState("None");
    const [s1, setS1] = useState(0.0);
    const [s2, setS2] = useState(0.0);
    const [ersCharge, setErsCharge] = useState(0);
    const [isOvertake, setIsOvertake] = useState(false);
    const [shiftLights, setShiftLights] = useState(0);
    const [currentLap, setCurrentLap] = useState(0);
    const [totalLaps, setTotalLaps] = useState(0);

    useEffect(() => {
        if (lastMessage) {
            console.log("Received message");
            const data = JSON.parse(lastMessage.data);
            setLapTime(data.lapTime);
            setLastLapTime(data.lastLapTime);
            setPosition(data.position);
            setTotalPositions(data.totalPositions);
            setRpm(data.rpm);
            setSpeed(data.speed);
            setSpeedUnit(data.speedUnit);
            setGear(data.gear);
            setRecommendedGear(data.recommendedGear);
            setDrsState(data.drsState);
            setS1(data.s1);
            setS2(data.s2);
            setErsCharge(data.ersCharge);
            setIsOvertake(data.isOvertake);
            setShiftLights(data.shiftLights);
            setCurrentLap(data.currentLap);
            setTotalLaps(data.totalLaps);
        }
    }, [lastMessage]);

    return (
        <>
            <Dashboard lapTime={lapTime} lastLapTime={lastLapTime} position={position} totalPositions={totalPositions} rpm={rpm} speed={speed} speedUnit={speedUnit} gear={gear} recommendedGear={recommendedGear} drsState={drsState} s1={s1} s2={s2} ersCharge={ersCharge} isOvertake={isOvertake} shiftLights={shiftLights} currentLap={currentLap} totalLaps={totalLaps} />
        </>
    )
}

export default App
