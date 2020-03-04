docker build -t electric-city-image .

docker tag electric-city-image registry.heroku.com/electric-city-server/web

docker push registry.heroku.com/electric-city-server/web

heroku container:release web -a electric-city-server