FROM node:16 AS build

WORKDIR /app

RUN ls

COPY ../../react-frontend/package.json ../../react-frontend/package-lock.json ./

# Install dependencies
RUN npm install

COPY ../../react-frontend/ ./

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
