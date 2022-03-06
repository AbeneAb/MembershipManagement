# build stage
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY membership-spa/package*.json ./
RUN npm install
COPY . .
RUN npm run start
EXPOSE 3000