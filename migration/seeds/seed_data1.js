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
    { ShoeID: 1, CategoryID: 1, Name: 'Nike Air Max', Size: '10', Color: 'Black', Price: 120.00, Stock: 50, Image: '\x89504e470d0a1a0a0000000d494844520000000c0000000c0806000000c48d8d7600000001735247420000aece1ce90000000467414d410000b18f0bfc6105000000097048597300000ec400000ec401952b0e1b0000001974455874536f6674776172650041646f626520496d616765526561647971c9653c00000049444154789cedd3310a803014c0f150d22e38d74e8201d88ae9ffbc102db17c6016bc2f66fb3b87720c0414a0d26bb01c83f8f989b10aee0a88b118f634382d98e9411512a20253ecdb4fc73a9d0a5464062036b2f7c9241101a1cb08a70000000049454e44ae426082' },
    { ShoeID: 2, CategoryID: 2, Name: 'Adidas Ultraboost', Size: '8', Color: 'White', Price: 150.00, Stock: 30, Image: '\x89504e470d0a1a0a0000000d494844520000000c0000000c0806000000c48d8d7600000001735247420000aece1ce90000000467414d410000b18f0bfc6105000000097048597300000ec400000ec401952b0e1b0000001974455874536f6674776172650041646f626520496d616765526561647971c9653c00000049444154789cedd3310a803014c0f150d22e38d74e8201d88ae9ffbc102db17c6016bc2f66fb3b87720c0414a0d26bb01c83f8f989b10aee0a88b118f634382d98e9411512a20253ecdb4fc73a9d0a5464062036b2f7c9241101a1cb08a70000000049454e44ae426082' },
    { ShoeID: 3, CategoryID: 3, Name: 'Puma Running', Size: '9', Color: 'Blue', Price: 100.00, Stock: 20, Image: '\x89504e470d0a1a0a0000000d494844520000000c0000000c0806000000c48d8d7600000001735247420000aece1ce90000000467414d410000b18f0bfc6105000000097048597300000ec400000ec401952b0e1b0000001974455874536f6674776172650041646f626520496d616765526561647971c9653c00000049444154789cedd3310a803014c0f150d22e38d74e8201d88ae9ffbc102db17c6016bc2f66fb3b87720c0414a0d26bb01c83f8f989b10aee0a88b118f634382d98e9411512a20253ecdb4fc73a9d0a5464062036b2f7c9241101a1cb08a70000000049454e44ae426082' }
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
