
The project has been containerised into two containers launched from docker-compose. Docker must be installed to run it.
To launch the project, enter the command docker-compose up --build in the directory with the docker-compose file.

If the backend container shuts down, restart it. This is a problem with the container starting too slowly with the database.
Once the containers are running, you can open your browser at http://localhost:5000/swagger/index.html where you can manage the endpoints.

A seed with 3 parking spaces is loaded into the database when the application is launched.

The main idea was to physically represent each parking space as a row in the database.
When a car is added to a space, the space automatically changes its status to IsOccupied. Similarly, when a car leaves, the space becomes free again.
The vehicle is removed from the ParkedVehicle table when it leaves the car park. However, in my opinion, it should remain in the archive due to the bank rejecting the payment after a few hours.

Adding a car to the database and occupying a parking space should be atomic: either both conditions are met, or the changes should be rolled back due to potential inconsistencies in the database.

Questions
- Should I delete records from the database about cars that have used the parking system?
- Should car registration be validated in any way?
- How should fee values be rounded?
