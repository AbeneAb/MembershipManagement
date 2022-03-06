# build stage
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY membership-spa/package*.json ./
RUN npm install
COPY  membership-spa/. .
RUN npm run build

FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/build /usr/share/nginx/html
COPY membership-spa/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]