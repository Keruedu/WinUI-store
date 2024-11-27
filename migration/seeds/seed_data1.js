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
  await knex.raw('SELECT setval(\'"Address_AddressID_seq"\', (SELECT MAX("AddressID") FROM "Address"))');

  // Insert seed entries for User
  await knex('User').insert([
    { UserID: 1, Name: 'John Doe', Password: '123', Email: 'admin@gmail.com', PhoneNumber: '123-456-7890', AddressID: 1, Role: 'admin' },
    { UserID: 2, Name: 'Jane Smith', Password: '123', Email: 'jane.smith@example.com', PhoneNumber: '987-654-3210', AddressID: 2, Role: 'manager' },
    { UserID: 3, Name: 'Alice Johnson', Password: '123', Email: 'alice.johnson@example.com', PhoneNumber: '111-222-3333', AddressID: 1, Role: 'manager' },
    { UserID: 4, Name: 'Bob Brown', Password: '123', Email: 'bob.brown@example.com', PhoneNumber: '444-555-6666', AddressID: 2, Role: 'manager' },
    { UserID: 5, Name: 'Charlie Davis', Password: '123', Email: 'charlie.davis@example.com', PhoneNumber: '777-888-9999', AddressID: 1, Role: 'manager' },
    { UserID: 6, Name: 'Diana Evans', Password: '123', Email: 'diana.evans@example.com', PhoneNumber: '000-111-2222', AddressID: 2, Role: 'manager' },
    { UserID: 7, Name: 'Ethan Foster', Password: '123', Email: 'ethan.foster@example.com', PhoneNumber: '333-444-5555', AddressID: 1, Role: 'manager' },
    { UserID: 8, Name: 'Fiona Green', Password: '123', Email: 'fiona.green@example.com', PhoneNumber: '666-777-8888', AddressID: 2, Role: 'manager' },
    { UserID: 9, Name: 'George Harris', Password: '123', Email: 'george.harris@example.com', PhoneNumber: '999-000-1111', AddressID: 1, Role: 'manager' },
    { UserID: 10, Name: 'Hannah White', Password: '123', Email: 'hannah.white@example.com', PhoneNumber: '222-333-4444', AddressID: 2, Role: 'manager' }
  ]);
  // Update the sequence value to match the highest
  await knex.raw('SELECT setval(\'"User_UserID_seq"\', (SELECT MAX("UserID") FROM "User"))');

  // Insert seed entries for Category
  await knex('Category').insert([
    { CategoryID: 1, Name: 'Men', Description: 'Men\'s shoes' },
    { CategoryID: 2, Name: 'Women', Description: 'Women\'s shoes' },
    { CategoryID: 3, Name: 'Sports', Description: 'Sports shoes' }
  ]);
  await knex.raw('SELECT setval(\'"Category_CategoryID_seq"\', (SELECT MAX("CategoryID") FROM "Category"))');

  // Insert seed entries for Shoes
  await knex('Shoes').insert([
    { ShoesID: 1, CategoryID: 1, Name: 'Nike Air Max', Brand: 'Nike', Size: '10', Color: 'Black', Price: 120.00, Stock: 50, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196148/adidas3_yls1aa.jpg', Description: 'Comfortable and stylish Nike Air Max shoes.' },
    { ShoesID: 2, CategoryID: 2, Name: 'Adidas Ultraboost', Brand: 'Adidas', Size: '8', Color: 'White', Price: 150.00, Stock: 30, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196148/adidas5_uliobk.jpg', Description: 'High-performance Adidas Ultraboost running shoes.' },
    { ShoesID: 3, CategoryID: 3, Name: 'Puma Running', Brand: 'Puma',  Size: '9', Color: 'Blue', Price: 100.00, Stock: 20, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196148/adidas4_x0go0s.jpg', Description: 'Lightweight and durable Puma running shoes.' },
    { ShoesID: 4, CategoryID: 1, Name: 'Reebok Classic', Brand: 'Reebok', Size: '11', Color: 'Red', Price: 110.00, Stock: 40, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/adidas1_tbkhwo.jpg', Description: 'Classic Reebok shoes with a retro design.' },
    { ShoesID: 5, CategoryID: 2, Name: 'New Balance 574', Brand: 'New Balance', Size: '7', Color: 'Green', Price: 130.00, Stock: 25, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse1_tnygsh.jpg', Description: 'Versatile and comfortable New Balance 574 shoes.' },
    { ShoesID: 6, CategoryID: 3, Name: 'Asics Gel-Kayano', Brand: 'Asics', Size: '9', Color: 'Yellow', Price: 140.00, Stock: 35, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196146/nike4_wwbkfs.jpg', Description: 'Supportive and cushioned Asics Gel-Kayano shoes.' },
    { ShoesID: 7, CategoryID: 1, Name: 'Converse All Star', Brand: 'Converse', Size: '10', Color: 'Black', Price: 60.00, Stock: 60, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse2_popcku.jpg', Description: 'Iconic Converse All Star shoes with a timeless design.' },
    { ShoesID: 8, CategoryID: 2, Name: 'Vans Old Skool', Brand: 'Vans', Size: '8', Color: 'White', Price: 70.00, Stock: 45, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/nike3_fchusm.jpg', Description: 'Classic Vans Old Skool shoes with a retro design.' },
    { ShoesID: 9, CategoryID: 3, Name: 'Under Armour HOVR', Brand: 'Under Armour', Size: '9', Color: 'Blue', Price: 150.00, Stock: 20, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse5_tp0p8j.jpg', Description: 'High-performance Under Armour HOVR running shoes.' },
    { ShoesID: 10, CategoryID: 1, Name: 'Jordan Air', Brand: 'Nike', Size: '11', Color: 'Red', Price: 200.00, Stock: 30, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/nike3_fchusm.jpg', Description: 'Premium Jordan Air shoes with superior comfort.' },
    { ShoesID: 11, CategoryID: 2, Name: 'Fila Disruptor', Brand: 'Fila', Size: '7', Color: 'Pink', Price: 90.00, Stock: 50, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse4_eqyc6x.jpg', Description: 'Chunky and stylish Fila Disruptor shoes.' },
    { ShoesID: 12, CategoryID: 3, Name: 'Brooks Ghost', Brand: 'Brooks', Size: '9', Color: 'Grey', Price: 130.00, Stock: 25, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/nike1_v2vfy2.jpg', Description: 'Comfortable and supportive Brooks Ghost running shoes.' },
    { ShoesID: 13, CategoryID: 1, Name: 'Saucony Jazz', Brand: 'Saucony', Size: '10', Color: 'Blue', Price: 80.00, Stock: 40, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196146/nike4_wwbkfs.jpg', Description: 'Classic Saucony Jazz shoes with a retro design.' }
  ]);
  await knex.raw('SELECT setval(\'"Shoes_ShoesID_seq"\', (SELECT MAX("ShoesID") FROM "Shoes"))');

  // Insert seed entries for Order
  await knex('Order').insert([
    { OrderID: 1, UserID: 1, OrderDate: '2023-10-31', Status: 'pending', AddressID: 1, TotalAmount: 270.00 },
    { OrderID: 2, UserID: 2, OrderDate: '2023-11-01', Status: 'shipped', AddressID: 2, TotalAmount: 600.00 },
    { OrderID: 3, UserID: 3, OrderDate: '2023-11-02', Status: 'delivered', AddressID: 1, TotalAmount: 450.00 },
    { OrderID: 4, UserID: 4, OrderDate: '2023-11-03', Status: 'pending', AddressID: 2, TotalAmount: 1000.00 }
  ]);
  await knex.raw('SELECT setval(\'"Order_OrderID_seq"\', (SELECT MAX("OrderID") FROM "Order"))');

  // Insert seed entries for Detail
  await knex('Detail').insert([
    { DetailID: 1, OrderID: 1, ShoesID: 1, Quantity: 1, Price: 120.00 },
    { DetailID: 2, OrderID: 1, ShoesID: 2, Quantity: 1, Price: 150.00 },
    { DetailID: 3, OrderID: 2, ShoesID: 3, Quantity: 2, Price: 200.00 },
    { DetailID: 4, OrderID: 2, ShoesID: 4, Quantity: 2, Price: 200.00 },
    { DetailID: 5, OrderID: 2, ShoesID: 5, Quantity: 1, Price: 130.00 },
    { DetailID: 6, OrderID: 3, ShoesID: 6, Quantity: 3, Price: 140.00 },
    { DetailID: 7, OrderID: 3, ShoesID: 7, Quantity: 1, Price: 60.00 },
    { DetailID: 8, OrderID: 4, ShoesID: 8, Quantity: 4, Price: 70.00 },
    { DetailID: 9, OrderID: 4, ShoesID: 9, Quantity: 4, Price: 150.00 },
    { DetailID: 10, OrderID: 4, ShoesID: 10, Quantity: 2, Price: 200.00 }
  ]);
  await knex.raw('SELECT setval(\'"Detail_DetailID_seq"\', (SELECT MAX("DetailID") FROM "Detail"))');
};