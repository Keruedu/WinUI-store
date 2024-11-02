To use this database you need to install docker golbal, knex and postgrec

npm init

npm install --save knex dotenv tedious

npm install pg




if you don't have container yet
comand to your terminal: 

docker run --name postgres -e POSTGRES_PASSWORD=<Your-password> -p <Your-port:Your-port> -d postgres

docker exec -it postgres psql -U postgres -c "CREATE DATABASE <Your-nameshop>" 

cp .env.example .env

npx knex migrate:latest

npx knex seed:run



if you have container and database empty

docker exec -i postgres psql -U postgres -d demoshoeshop < <your-file.sql>

npx knex migrate:latest

npx knex migrate:latest


database explain

Based on the diagram you provided, here’s a list of potential properties for each entity in this database:

User
UserID: Unique identifier for the user
Name: Full name of the user
Email: Contact email
PhoneNumber: Contact phone number
AddressID: Linked address for the user (if applicable)
Order
OrderID: Unique identifier for the order
UserID: Reference to the user who made the order
OrderDate: Date when the order was placed
Status: Current status of the order (e.g., pending, shipped, delivered)
AddressID: Reference to the address where the order will be delivered
TotalAmount: Total price of the order
Detail
DetailID: Unique identifier for the order detail
OrderID: Reference to the order this detail belongs to
ShoeID: Reference to the shoe being ordered
Quantity: Number of items ordered
Price: Price per item (could also be calculated as per quantity if stored individually)
Address
AddressID: Unique identifier for the address
Street: Street information
City: City name
State: State or region
ZipCode: Postal code
Country: Country of the address
Category
CategoryID: Unique identifier for the category
Name: Name of the category (e.g., men's, women's, sports)
Description: Description of the category
Shoes
ShoeID: Unique identifier for the shoe
CategoryID: Reference to the category it belongs to
Name: Name of the shoe
Size: Size of the shoe
Color: Color of the shoe
Price: Price of the shoe
Stock: Quantity available in stock