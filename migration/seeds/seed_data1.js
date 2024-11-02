exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('Detail').del();
  await knex('Order').del();
  await knex('User').del();
  await knex('Address').del();
  await knex('Shoes').del();
  await knex('Category').del();

  // Insert seed entries for Address
  await knex('Address').insert([
    { AddressID: 1, Street: '123 Main St', City: 'Anytown', State: 'CA', ZipCode: '12345', Country: 'USA' },
    { AddressID: 2, Street: '456 Elm St', City: 'Othertown', State: 'NY', ZipCode: '67890', Country: 'USA' }
  ]);

  // Insert seed entries for User
  await knex('User').insert([
    { UserID: 1, Name: 'John Doe', Email: 'john.doe@example.com', PhoneNumber: '123-456-7890', AddressID: 1 },
    { UserID: 2, Name: 'Jane Smith', Email: 'jane.smith@example.com', PhoneNumber: '987-654-3210', AddressID: 2 }
  ]);

  // Insert seed entries for Category
  await knex('Category').insert([
    { CategoryID: 1, Name: 'Men', Description: 'Men\'s shoes' },
    { CategoryID: 2, Name: 'Women', Description: 'Women\'s shoes' },
    { CategoryID: 3, Name: 'Sports', Description: 'Sports shoes' }
  ]);

  // Insert seed entries for Shoes
  await knex('Shoes').insert([
    { ShoeID: 1, CategoryID: 1, Name: 'Nike Air Max', Size: '10', Color: 'Black', Price: 120.00, Stock: 50 },
    { ShoeID: 2, CategoryID: 2, Name: 'Adidas Ultraboost', Size: '8', Color: 'White', Price: 150.00, Stock: 30 },
    { ShoeID: 3, CategoryID: 3, Name: 'Puma Running', Size: '9', Color: 'Blue', Price: 100.00, Stock: 20 }
  ]);

  // Insert seed entries for Order
  await knex('Order').insert([
    { OrderID: 1, UserID: 1, OrderDate: '2023-10-31', Status: 'pending', AddressID: 1, TotalAmount: 100.00 },
    { OrderID: 2, UserID: 2, OrderDate: '2023-11-01', Status: 'shipped', AddressID: 2, TotalAmount: 200.00 }
  ]);

  // Insert seed entries for Detail
  await knex('Detail').insert([
    { DetailID: 1, OrderID: 1, ShoeID: 1, Quantity: 2, Price: 50.00 },
    { DetailID: 2, OrderID: 2, ShoeID: 2, Quantity: 1, Price: 200.00 }
  ]);
};
