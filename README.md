# SofomoRecruitmentTask

Summary
The aim of this task is to build a simple API (backed by any kind of database). The application should be able to store weather forecast data in the database, based on provided latitude and longitude - you can use https://open-meteo.com/ to get a weather forecast. The API should be able to add (new longitude and latitude), delete or provide weather forecast. It should also provide an endpoint to list previously used longitudes and latitudes from the database, and choose them to provide the newest weather forecast.  

Application specification
 - It should be a RESTful API
 - You can use https://open-meteo.com/  for the geolocation of IP addresses and URLs
 - The application can be built in .net (preferable the newest one)
 - Usage of any free library which will help implement solution is acceptable (e.g. swagger)
 - It is preferable that the API operates using JSON (for both input and output)
 - The solution should also include base specs/tests coverage

How to submit
 - Create a public Git repository and share the link with us

Notes:
 - We will run the application on our local machines for testing purposes. This implies that the solution should provide a quick and easy way to get the system up and running, including test data (hint: you can add Docker support so we can run it easily)
 - We will test the behavior of the system under various "unfortunate" conditions (hint: How will the app behave when we take down the DB? How about the IPStack API?)
 - After we finish reviewing the solution, we'll invite you to Sofomo's office (or to a Zoom call) for a short discussion about the provided solution. We may also use that as an opportunity to ask questions and drill into the details of your implementation.

Bonus points:
 - Setup Web App (free version) on Microsoft Azure with your solution. DB can be mocked.

