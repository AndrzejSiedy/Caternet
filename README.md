# Caternet
Test

We want to start a monthly developer meetup in Eastleigh to discuss the latest frameworks and technologies. With only a limited number of spaces available, we need a system to be created which will let people book seats for the next meetup. Assuming a separate team are working on the UI we would like you to build the API and backend systems including persisting the data.

In our current meetup space we have 100 seats available, 10 rows of 10 seats, labelled A1 to J10, although in the future this may change. In addition, for now the meetup is free, so there is no need to build in support for taking payment, but we would like it to be easy to add the ability to charge for some meetups in the future.

Requirements

At the point of booking a person will select which seats they want, seats are not automatically allocated.
Two people must not be able to book the same seat.
A single person may book up to four seats in one transaction.
A unique name and email address are required for each seat that is booked.

Little bit about how does it work;
- project created from MVC Core + EF Core VS template
- created for localDB MS SQL Server Express
- implements standard Core Identity (as per standard VS template)

Pre-requisits:
- database need to be created before migrations can be loaded
- conection string can be set in appsettings.json

- to install db tables, from project directory execute: dotnet ef database update
this should load all required tables

there are two main classes/tables: Events & Seats
users are stored in AspNetUsers

logic behind: 
Events - defines date, name, number of rows & columns (as every event will contain own set of seats)
Seats - describes seat

All logic is drectly implemented in Controllers.

TODO: 
1. separate logic from view
2. use interfaces, services & DTO classes
3. create nicer front end.
