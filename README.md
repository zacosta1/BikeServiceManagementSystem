# Bike Service Management System
This project is a business solution that allows bike mechanics to manage bike servicing, developed to refresh and showcase my ASP.NET and C# development skills.

The solution features CRUD (Create, Read, Update, and Delete) functions through:
- Creating a new service, service details, and service parts required by a service
- Reading customer, employee, service, service detail, service parts data
- Updating service detail comments and service parts quantity needed by services
- Deleting service, service detail, and service part records

## Project Scope
The main scope of this project is a Servicing subsystem, which was my individual deliverable for my Intermediate Application Development class final group project. In the said project, this Servicing subsystem would have worked with three other subsystems, each developed by other team members, to work as a full business solution. These other subsystems would've been:

- Purchasing - supports inventory item purchases
- Receiving and Returns - supports reception of products from suppliers
- Sales - supports sales for online customers

### Web Design
The solution uses [Bootstrap](https://getbootstrap.com/) for quicker design and layout. Web design was not a focus in this project, though the application's design should at least simulate a real one's appearance.

### Database
A database backup was already included in the original group project setup file. Database programming was not the focus in this project, but the application has to connect and interact with the supplied database through a web connection string.

## Security Implementation
The original group project application setup file already included security components. Each team member only needed to adjust the supplied security components to support the required and appropriate security roles.

 - **Application Administrators**
   - User(s) that have exclusive access to Security management, and can view all subsystems of the application:
     - Webmaster
 - **Mechanics**
   - User(s) that cannot access the Sales and Purchasing subsystems, and Security management:
     - Nole Body
     - Willie Work
     - Ken Fixit
     - Brad Brake
     - Brian Brown
     - Bob Brooks
 - **Shipping/Receiving Clerks**
   - User(s) that cannot access Servicing and Sales subsystems, and Security management:
     - Freda Flash
     - Sadie Star
 - **Salespeople**
   - User(s) that cannot access Servicing and Purchasing subsystems, and Security management:
     - Max Smart
     - Shelia Seller
### User Name
The default username for each user, except the Webmaster, is their first name's first letter, followed by their lastname. (i.e. `jdoe` for John Doe). The Webmaster can login as `webmaster`.
### Password
The default password for the users listed above is `Pa$$word1`
