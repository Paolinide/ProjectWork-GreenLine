
const redis = require("redis");
const bluebird = require("bluebird");
bluebird.promisifyAll(redis);
const client = redis.createClient(6379, '127.0.0.1');
client.setAsync('key','prova').then(function(res) {
    console.log(res); 
});

client.getAsync('key').then(function(res) {
    console.log(res);
});

client.quitAsync();







