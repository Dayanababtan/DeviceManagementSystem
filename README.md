# DeviceManagementSystem

Device management system ia a full-stack application which keeps track of all the mobile devices the company owns, all the details of the device, location, and who is using it at the moment. It uses LLMs (Large Language Models) to automatically generate professional device descriptions and a Weighted Scoring Algorithm for high-relevance search.

The AI-generated description is automatically generated when a new device is created. The dummy data from the database do not have that AI Summary because they aren't made using the POST API. 

Tech Stack

Frontend: Angular 17+ (Standalone Components, RxJS, Reactive Forms)

Backend: .NET 10 Web API

Database: MongoDB

AI Integration: Groq


Before you begin, ensure you have the following installed:
.NET 10 SDK
MongoDB Community Server (running locally on port 27017)
An API Key from Groq

1. Keys configuration:

    open the file DeviceManagement.Api/appsettings.Example.json, copy the code and paste it in appsettings.json. Change the place holders of the Database ConnectionString and 
    ApiKey with your own.

2. Database Configuration:

    in the terminal, open the DeviceManagement.Api/Database and run the following commands (put your username and password):

    mongosh "mongodb+srv://<username>:<password>@cluster0.ijua5jd.mongodb.net/?appName=Cluster0" init-db.js

    mongosh "mongodb+srv://<username>:<password>@cluster0.ijua5jd.mongodb.net/?appName=Cluster0" seed-data.js  

    After this, in MongoDb you will have a database named DeviceManagementDb with 3 tables: Devices, Users and Counters, all of them populated with dummy data.

3. Backend Setup

    cd DeviceManagement.Api

    dotnet run


    the server is now running on port 5131.

4. Frontend Setup

    cd client

    npm install

    npm run start


    The app will be available at http://localhost:4200.