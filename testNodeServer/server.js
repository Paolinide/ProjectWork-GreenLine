const express = require('express')
const app = express()
const port = 3000
app.use(express.json())

app.get('/', (request, response) => {
  response.send('Hello from Express!')
})

app.post('/', (request, response) => {
    response.send('Arrivato tutto bene!')
    console.log(request.body);
  })

app.listen(port, (err) => {
  if (err) {
    return console.log('something bad happened', err)
  }

  console.log(`server is listening on ${port}`)
})
