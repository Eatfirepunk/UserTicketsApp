# UserTicketsApp
Angular App to modify users and create incident tickets

Design:
This app tries to be a demostration on how follow SOLID principles and ensure modularity.
 The goal is to create two microservices, one for handling tickets and the other one for users. Each microservice will handle its respective domain.

I will use the repository pattern to extract data from the database. This will allow me to abstract away the details of the database implementation from the business logic of the microservices. By doing so, I can ensure that my microservices are not tightly coupled to the database implementation, and can be easily swapped out if needed.

I will also ensure that my Angular application follows SOLID principles by creating separate components for each feature and ensuring that each component is responsible for a single responsibility. I will also create separate services for each microservice to ensure that the Angular application is not tightly coupled to the microservices. This will ensure that I can easily change the implementation of the microservices without affecting the Angular application.


How to run it locally:

1.- Clone the whole repository

2.- Locate the SQL folder, open it , run the DB Initialization script.sql file in your SQL Server environment

3.- Change the appsettings DefaultConnection to the db pointing to the newly created db do this for both microservices(Tickets microservice is still not implemented)

4.- In the startup.cs on the usersmicroservice check that the port that runs your angular app maps the one from the CORS policy

5.- Open the TicketApp folder and run npm install to restore the nuget packages

6.- To run the angular app run the following command: ng serve

7.- Run the user microservice on the debug platform select IIS Express

8.- To test the users microservice you will first have to login if you inserted the credentials in the script the login should be:

password:holaMundo

email:john.doe@example.com

(please not that in the event of the credentials not working, please feel free to add the AllowAnonymus decorator in to the     
 public async Task<IActionResult> CreateUser(LoginDto userDto) method , to create and admin user and use it to login and get the token to use the other endpoints)
 
9.- For step 8 use api/Login endpoint to login this will return the token for the request to this endpoint, use the credentials above or follow the before mentioned instructions.
 
10.- To login to the angular app use the credentials described, or create a new user using the before mentioned steps.


This is work done by Julio Lozada -- contact me at julio_lozada123@hotmail.com

Please review the Issues List to check for the remaining work for the whole app.


Requirements:

Build an Angular Application with following specifications,

1.       Web Application should have following pages that should be redirected according to the Roles (Admin, User)

a.     Design pages using Angular

b.      Created REST API methods for any transactions with Database

c.       Use No SQL  or RDMS for creating the required Tables.

              i.      Users – should hold User  and admin relationship (keep the mail id as unique field and should be used as username to login into the application, Username, password, user type, reporting person)

              ii.      Ticket – Ticket table should be created as per the specification given below.

2.       UserType

a.       User  – If no one is reporting to him/her(Employee- as a developer/tester), One or more employee reporting to him (Lead/Manger)

b.      Admin – One who maintain the system setting and providing access to the user

3.       Create a Admin Page with following options,

a.       User list - Should list the user information

b.      Provide option to add a user

c.       And assign role and reporting person(should be populated based on role)

4.       Create a User Page with following options,

a.       My Tickets - Should list the tickets created by him/her

b.      Create Ticket – Should be able to create the new ticket with following fields

                                                              i.      TicketId (unique Id)

                                                            ii.      Ticket Title

                                                          iii.      Ticket Description

                                                           iv.      TicketType- Issue type from lookup

                                                             v.      Ticket Created Datetime

                                                           vi.      Status (New, In progress, Approved and Rejected)

5.       Create a Manager/Lead Page with following Options,

a.       My Tickets – Should list the tickets created by him/her

b.      Team Tickets – Should list the tickets of the employees who are reporting to him/her

                                                              i.      Should be able to Approve/Reject Tickets

c.       Create Ticket – Should be able to create the new ticket with following fields

                                                              i.      TicketId (unique Id)

                                                            ii.      Ticket Title

                                                          iii.      Ticket Description

                                                           iv.      TicketType

                                                             v.      Ticket Created Datetime

                                                           vi.      Status (New, In progress, Approved and Rejected)

6.       Every listing page should have a following search filters (should be reused in all pages),

a.       Date Range by Created Datetime (start date and end date)

b.      TicketType

c.      Wild card search on Ticket Title and Description
