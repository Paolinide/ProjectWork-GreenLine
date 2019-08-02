Per esporre porta di redis al ip del host e renderlo accessibile anche al difuori del container
sudo docker run -p 127.0.0.1:6379:6379 -d redis
comando per collegarsi a docker usando la redis-cli dalla bash di linux e non dal container docker
sudo redis-cli -h 127.0.0.1 -p 6379

