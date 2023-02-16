# UserTicketsApp
Angular App to modify users and create incident tickets

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
