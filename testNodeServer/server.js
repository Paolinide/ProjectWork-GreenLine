const Influx = require('influx');
const express = require('express');
const http = require('http');
const os = require('os');
const app = express();
const port = 4000;
var child = require('child_process').exec;
var pathinflux = 'start \\DBinflux\\influxdb-1.7.4-1\\influxd.exe'
child(pathinflux, function (err, data) {
  if (err) {
    return console.log('something bad happened', err);
  }
})
app.use(express.json());
var contatore = 0;
app.listen(port, (err) => {
  if (err) {
    return console.log('something bad happened', err);
  }

  console.log(`Node server is listening on ${port}`);
});
const influx = new Influx.InfluxDB({
  host: 'localhost',
  database: 'express_3',
  schema: [
    {
      measurement: 'response_times',
      fields: {
        IdVeicolo: Influx.FieldType.INTEGER,
        StringaVeicolo: Influx.FieldType.STRING,
        TimeStamp: Influx.FieldType.STRING,
        Latitudine: Influx.FieldType.FLOAT,
        Longitudine: Influx.FieldType.FLOAT,
        Altitudine: Influx.FieldType.FLOAT,
        Passeggeri: Influx.FieldType.INTEGER,
        PorteAperte: Influx.FieldType.BOOLEAN
      },
      tags: [
        'host'
      ]
    }
  ]
});
influx.getDatabaseNames()
  .then(names => {
    if (!names.includes('express_3')) {
      return influx.createDatabase('express_3');
    }
  })
  .then(() => {
    http.createServer(app).listen(3000, function () {
      console.log('Influx Listening on port 3000')
    })
  })
  .catch(err => {
    console.error(`Error creating Influx database!`);
  })
/* var element; */
app.post('/', (request, response) => {
  dati = request.body[0];
  influx.writePoints([
    {
      measurement: 'response_times',
      tags: { host: os.hostname() },
      fields: {
        IdVeicolo: dati.idVehicle,
        StringaVeicolo: dati.description,
        TimeStamp: dati.timeDate,
        Latitudine: dati.latitude,
        Longitudine: dati.longitude,
        Altitudine: dati.altitude,
        Passeggeri: dati.passenger,
        PorteAperte: dati.theDoors
      },
    }
  ]).catch(err => {
    console.error(`Error saving data to InfluxDB! ${err.stack}`)
  })
  response.send('Ok');
  console.log("// " + (++contatore));
  console.log(request.body);
});

//parte in get


app.get('/get', function (req, res) {
  influx.query(`
    select * from response_times `)
    .then(result => {
      res.json(result)
    }).catch(err => {
      res.status(500).send(err.stack)
    })
})