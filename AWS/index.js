var awsIot = require('aws-iot-device-sdk');

//
// Replace the values of '<YourUniqueClientIdentifier>' and '<YourCustomEndpoint>'
// with a unique client identifier and custom host endpoint provided in AWS IoT.
// NOTE: client identifiers must be unique within your AWS account; if a client attempts 
// to connect with a client identifier which is already in use, the existing 
// connection will be terminated.
//
var device = awsIot.device({
    keyPath: '/home/marco/Scrivania/ProjectWork/2.private.key',
    certPath: '/home/marco/Scrivania/ProjectWork/3.certificate.pem',
    caPath: '/home/marco/Scrivania/ProjectWork/ProjectWork-GreenLine/AWS/root-Ca.pem',
    clientId: 'poltest', //equivale alla nome della policy
    host: 'a15axy3tr9fuwq.iot.eu-west-1.amazonaws.com'
});

//
// Device is an instance returned by mqtt.Client(), see mqtt.js for full
// documentation.
//

device
    .on('connect', function () {
        console.log('connect');
        device.subscribe('topic1');
        device.publish('topic1', JSON.stringify({ test_data: 'NodeJS server connected...' }));
       //device.publish('topic a cui si vuole inviare il msg')
       //device.subscribe('topic da cui si vuole ricevere msg')
    });

//ricezione dal topic
device
    .on('message', function (topic, payload) {
        console.log('message', topic, payload.toString());
    }); 