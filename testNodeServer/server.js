const Influx = require('influx');
const express = require('express');
const http = require('http');
const os = require('os');
const app = express();
const port = 4000;
app.use(express.json());

var contatore = 0;
/* app.get('/', (request, response) => {
  response.send('Hello from Express!');
});

app.post('/', (request, response) => {
  response.send('Arrivato tutto bene!');
  console.log("// " + (++contatore));
  console.log(request.body);
}); */

app.listen(port, (err) => {
  if (err) {
    return console.log('something bad happened', err);
  }

  console.log(`server is listening on ${port}`);
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
      console.log('Listening on port 3000')
    })
  })
  .catch(err => {
    console.error(`Error creating Influx database!`);
  })
// influx.writePoints([
//   {
//     measurement: 'response_times',
//     tags: { host: os.hostname() },
//     fields: {
//       IdVeicolo: 2,
//       StringaVeicolo: "Pordenone-Talmassons",
//       TimeStamp: "2019-05-03 22:33:00",
//       Latitudine: 45.95,
//       Longitudine: 12.65,
//       Altitudine: 30.0,
//       Passeggeri: 0,
//       PorteAperte: false
//     },
//   }
// ]).catch(err => {
//   console.error(`Error saving data to InfluxDB! ${err.stack}`)
// })
var element;
app.post('/', (request, response) => {
  dati = request.body[0];
 influx.writePoints([
  {
    measurement: 'response_times',
    tags: { host: os.hostname() },
    fields: { IdVeicolo: dati.idVeicolo,
            StringaVeicolo: dati.stringaVeicolo,
            TimeStamp: dati.timeStamp,
            Latitudine: dati.latitudine,
            Longitudine: dati.longitudine,
            Altitudine: dati.altitudine,
            Passeggeri: dati.passeggeri,
            PorteAperte: dati.porteAperte
    },
  }
]).catch(err => {
      console.error(`Error saving data to InfluxDB! ${err.stack}`)
    })
  response.send('Arrivato tutto bene!');
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