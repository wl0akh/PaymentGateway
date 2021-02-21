docker build --pull -t paymen_gateway .
docker-compose up -d
sleep 3
echo "Server is started at http://localhost:80"