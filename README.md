# MembershipManagement
Follow these steps to get your development environment set up: (Before Running Start the Docker Desktop)
1. Clone the repository
. At the root directory which include **docker-compose.yml** files, run below command:
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
To run the front end change directory to test-managment-spa

```
cd membership-spa
npm install
npm start
```

React application should be running on port 3000. It will need the back end to run on port 7201.
