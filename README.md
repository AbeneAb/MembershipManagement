# MembershipManagement
Follow these steps to get your development environment set up: (Before Running Start the Docker Desktop)
1. Clone the repository
. At the root directory which include **docker-compose.yml** files, run below command:
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
 You can use:

* **HealthData.API -> http://host.docker.internal:8000/swagger/index.html**
* **membershipData.API -> http://host.docker.internal:8000/swagger/index.html**
* **Rabbit Management Dashboard -> http://host.docker.internal:15672**   -- guest/guest
* **SQL Server -> http://host.docker.internal:5433**   --[UserName] => sa  [Password] => Thynk1234
* **Health Data dashboard -> http://localhost:3000
```
cd membership-spa
npm install
npm start
```

React application should be running on port 3000. It will need the back end to run on port 7201.
