docker build --pull -t paymen-gateway .
docker-compose up -d
sleep 3
docker logs -f payment-gateway