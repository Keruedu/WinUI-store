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
    { AddressID: 2, Street: '456 Elm St', City: 'Othertown', State: 'NY', ZipCode: '67890', Country: 'USA' },
    { AddressID: 3, Street: '789 Oak St', City: 'Smalltown', State: 'TX', ZipCode: '23456', Country: 'USA' },
    { AddressID: 4, Street: '101 Pine St', City: 'Bigcity', State: 'FL', ZipCode: '34567', Country: 'USA' },
    { AddressID: 5, Street: '202 Maple St', City: 'Littletown', State: 'CO', ZipCode: '45678', Country: 'USA' },
    { AddressID: 6, Street: '303 Birch St', City: 'Oldtown', State: 'WA', ZipCode: '56789', Country: 'USA' },
    { AddressID: 7, Street: '404 Cedar St', City: 'Neoncity', State: 'NV', ZipCode: '67801', Country: 'USA' },
    { AddressID: 8, Street: '505 Cherry St', City: 'Riverdale', State: 'OR', ZipCode: '78901', Country: 'USA' },
    { AddressID: 9, Street: '606 Walnut St', City: 'Sunnytown', State: 'CA', ZipCode: '89012', Country: 'USA' },
    { AddressID: 10, Street: '707 Cedar Ave', City: 'Hilltown', State: 'ID', ZipCode: '90123', Country: 'USA' },
    { AddressID: 11, Street: '123 Main St', City: 'Anytown', State: 'CA', ZipCode: '12345', Country: 'USA' },
    { AddressID: 12, Street: '456 Elm St', City: 'Othertown', State: 'NY', ZipCode: '67890', Country: 'USA' },
    { AddressID: 13, Street: '789 Oak St', City: 'Smalltown', State: 'TX', ZipCode: '23456', Country: 'USA' },
    { AddressID: 14, Street: '101 Pine St', City: 'Bigcity', State: 'FL', ZipCode: '34567', Country: 'USA' },
    { AddressID: 15, Street: '202 Maple St', City: 'Littletown', State: 'CO', ZipCode: '45678', Country: 'USA' },
    { AddressID: 16, Street: '303 Birch St', City: 'Oldtown', State: 'WA', ZipCode: '56789', Country: 'USA' },
    { AddressID: 17, Street: '404 Cedar St', City: 'Neoncity', State: 'NV', ZipCode: '67801', Country: 'USA' },
    { AddressID: 18, Street: '505 Cherry St', City: 'Riverdale', State: 'OR', ZipCode: '78901', Country: 'USA' },
    { AddressID: 19, Street: '606 Walnut St', City: 'Sunnytown', State: 'CA', ZipCode: '89012', Country: 'USA' },
  ]);
  await knex.raw('SELECT setval(\'"Address_AddressID_seq"\', (SELECT MAX("AddressID") FROM "Address"))');

  // Insert seed entries for User
  await knex('User').insert([
    { UserID: 1, Name: 'Viet Le', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'vietle@gmail.com', PhoneNumber: '123-456-7890', AddressID: 1, Role: 'Admin', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Viet+Le&background=random' },
    { UserID: 2, Name: 'Quoc Vinh', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'quocvinh@example.com', PhoneNumber: '987-654-3210', AddressID: 2, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Quoc+Vinh&background=random' },
    { UserID: 3, Name: 'Quang Vinh', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'quangvinh@example.com', PhoneNumber: '111-222-3333', AddressID: 1, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Quang+Vinh&background=random' },
    { UserID: 4, Name: 'Bob Brown', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'bob.brown@example.com', PhoneNumber: '444-555-6666', AddressID: 2, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Bob+Brown&background=random' },
    { UserID: 5, Name: 'Charlie Davis', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'charlie.davis@example.com', PhoneNumber: '777-888-9999', AddressID: 1, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Charlie+Davis&background=random' },
    { UserID: 6, Name: 'Diana Evans', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'diana.evans@example.com', PhoneNumber: '000-111-2222', AddressID: 2, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Diana+Evans&background=random' },
    { UserID: 7, Name: 'Ethan Foster', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'ethan.foster@example.com', PhoneNumber: '333-444-5555', AddressID: 1, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Ethan+Foster&background=random' },
    { UserID: 8, Name: 'Fiona Green', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'fiona.green@example.com', PhoneNumber: '666-777-8888', AddressID: 2, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Fiona+Green&background=random' },
    { UserID: 9, Name: 'George Harris', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'george.harris@example.com', PhoneNumber: '999-000-1111', AddressID: 1, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=George+Harris&background=random' },
    { UserID: 10, Name: 'Hannah White', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'hannah.white@example.com', PhoneNumber: '222-333-4444', AddressID: 2, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Hannah+White&background=random' },
    { UserID: 11, Name: 'Ivy King', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'ivy.king@example.com', PhoneNumber: '555-666-7777', AddressID: 3, Role: 'Admin', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Ivy+King&background=random' },
    { UserID: 12, Name: 'Jack Smith', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'jack.smith@example.com', PhoneNumber: '888-999-0000', AddressID: 3, Role: 'User', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Jack+Smith&background=random' },
    { UserID: 13, Name: 'Kelly Adams', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'kelly.adams@example.com', PhoneNumber: '222-444-5555', AddressID: 4, Role: 'User', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Kelly+Adams&background=random' },
    { UserID: 14, Name: 'Leo Walker', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'leo.walker@example.com', PhoneNumber: '999-333-2222', AddressID: 3, Role: 'Admin', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Leo+Walker&background=random' },
    { UserID: 15, Name: 'Mia Scott', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'mia.scott@example.com', PhoneNumber: '111-555-6666', AddressID: 4, Role: 'User', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Mia+Scott&background=random' },
    { UserID: 16, Name: 'Nathan Taylor', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'nathan.taylor@example.com', PhoneNumber: '444-888-1111', AddressID: 5, Role: 'User', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Nathan+Taylor&background=random' },
    { UserID: 17, Name: 'Olivia Martin', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'olivia.martin@example.com', PhoneNumber: '333-777-4444', AddressID: 5, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Olivia+Martin&background=random' },
    { UserID: 18, Name: 'Paul Jones', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'paul.jones@example.com', PhoneNumber: '222-000-5555', AddressID: 6, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Paul+Jones&background=random' },
    { UserID: 19, Name: 'Quinn Brown', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'quinn.brown@example.com', PhoneNumber: '555-222-1111', AddressID: 6, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Quinn+Brown&background=random' },
    { UserID: 20, Name: 'Ruby Young', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'ruby.young@example.com', PhoneNumber: '666-555-4444', AddressID: 7, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Ruby+Young&background=random' },
    { UserID: 21, Name: 'Sophia Allen', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'sophia.allen@example.com', PhoneNumber: '777-666-3333', AddressID: 7, Role: 'User', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Sophia+Allen&background=random' },
    { UserID: 22, Name: 'Thomas Carter', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'thomas.carter@example.com', PhoneNumber: '888-999-4444', AddressID: 8, Role: 'User', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Thomas+Carter&background=random' },
    { UserID: 23, Name: 'Uma Bell', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'uma.bell@example.com', PhoneNumber: '999-111-2222', AddressID: 8, Role: 'Manager', Status: 'Banned', Image: 'https://ui-avatars.com/api/?name=Uma+Bell&background=random' },
    { UserID: 24, Name: 'Victor Green', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'victor.green@example.com', PhoneNumber: '000-555-3333', AddressID: 9, Role: 'Admin', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Victor+Green&background=random' },
    { UserID: 25, Name: 'Wendy Gray', Password: '$2y$10$.KitBFmC87zTU.AqC5ZsieaJSJ/CLbldgZ1uh/EUxHjSE4mQUIF.m', Email: 'wendy.gray@example.com', PhoneNumber: '444-333-6666', AddressID: 9, Role: 'Manager', Status: 'Active', Image: 'https://ui-avatars.com/api/?name=Wendy+Gray&background=random' }
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
    { ShoesID: 13, CategoryID: 1, Name: 'Saucony Jazz', Brand: 'Saucony', Size: '10', Color: 'Blue', Price: 80.00, Stock: 40, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196146/nike4_wwbkfs.jpg', Description: 'Classic Saucony Jazz shoes with a retro design.' },
    { ShoesID: 14, CategoryID: 1, Name: 'Nike ZoomX', Brand: 'Nike', Size: '9', Color: 'Green', Price: 180.00, Stock: 20, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse5_tp0p8j.jpg', Description: 'High-performance Nike ZoomX shoes for running.' },
    { ShoesID: 15, CategoryID: 2, Name: 'Adidas NMD_R1', Brand: 'Adidas', Size: '10', Color: 'Black', Price: 160.00, Stock: 35, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/nike1_v2vfy2.jpgg', Description: 'Modern Adidas NMD_R1 sneakers with Boost technology.' },
    { ShoesID: 16, CategoryID: 3, Name: 'Puma Ignite', Brand: 'Puma', Size: '8', Color: 'Orange', Price: 140.00, Stock: 25, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196148/adidas5_uliobk.jpg', Description: 'Responsive and stylish Puma Ignite shoes for running.' },
    { ShoesID: 17, CategoryID: 1, Name: 'Reebok Nano X', Brand: 'Reebok', Size: '11', Color: 'White', Price: 120.00, Stock: 50, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/nike3_fchusm.jpg', Description: 'Reebok Nano X shoes for cross-training and fitness.' },
    { ShoesID: 18, CategoryID: 2, Name: 'New Balance Fresh Foam', Brand: 'New Balance', Size: '7', Color: 'Yellow', Price: 140.00, Stock: 30, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196148/adidas5_uliobk.jpg', Description: 'New Balance Fresh Foam shoes for ultimate comfort.' },
    { ShoesID: 19, CategoryID: 3, Name: 'Asics Metaspeed', Brand: 'Asics', Size: '9', Color: 'Blue', Price: 200.00, Stock: 15, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse2_popcku.jpg', Description: 'Asics Metaspeed racing shoes for professional runners.' },
    { ShoesID: 20, CategoryID: 1, Name: 'Jordan Delta', Brand: 'Nike', Size: '12', Color: 'Black', Price: 170.00, Stock: 40, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196146/nike4_wwbkfs.jpg', Description: 'Stylish and versatile Jordan Delta shoes for casual wear.' },
    { ShoesID: 21, CategoryID: 2, Name: 'Adidas Supernova', Brand: 'Adidas', Size: '9', Color: 'Pink', Price: 130.00, Stock: 20, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/adidas1_tbkhwo.jpg', Description: 'Adidas Supernova shoes for long-distance running.' },
    { ShoesID: 22, CategoryID: 3, Name: 'Hoka One Clifton', Brand: 'Hoka', Size: '8', Color: 'Grey', Price: 160.00, Stock: 25, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/adidas1_tbkhwo.jpg', Description: 'Hoka One Clifton shoes with superior cushioning.' },
    { ShoesID: 23, CategoryID: 1, Name: 'Nike Pegasus Trail', Brand: 'Nike', Size: '10', Color: 'Brown', Price: 150.00, Stock: 30, Image: 'https://res.cloudinary.com/dyocg3k6j/image/upload/v1732196147/converse1_tnygsh.jpg', Description: 'Trail running shoes from Nike for all terrains.' }
  ]);
  await knex.raw('SELECT setval(\'"Shoes_ShoesID_seq"\', (SELECT MAX("ShoesID") FROM "Shoes"))');

  function getRandomDate(start, end) {
    return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
  }
  
  const startDate = new Date(2022, 0, 1); // January 1, 2022
  const endDate = new Date(); 

  // Insert seed entries for Orders
  await knex('Order').insert([
    { OrderID: 1, UserID: 1, OrderDate: '2023-10-31', Status: 'Pending', AddressID: 1, TotalAmount: 270.00 },
    { OrderID: 2, UserID: 2, OrderDate: '2023-11-01', Status: 'Shipped', AddressID: 2, TotalAmount: 730.00 },
    { OrderID: 3, UserID: 3, OrderDate: '2023-11-02', Status: 'Delivered', AddressID: 1, TotalAmount: 480.00 },
    { OrderID: 4, UserID: 4, OrderDate: '2023-11-03', Status: 'Pending', AddressID: 2, TotalAmount: 1460.00 },
    { OrderID: 5, UserID: 5, OrderDate: '2023-11-04', Status: 'Pending', AddressID: 3, TotalAmount: 330.00 },

    // Generate 21 additional orders
    ...Array.from({ length: 21 }, (_, i) => {
      const id = i + 6;
      const userId = (id % 15) + 1; // Rotate UserID from 1 to 15
      const addressId = (id % 8) + 1; // Rotate AddressID from 1 to 8
      const totalAmount = Math.floor(Math.random() * 1500) + 100; // Random total amount between 100 and 1500
      const orderDate = getRandomDate(startDate, endDate).toISOString().split('T')[0]; // Random date in November
      const status = ['Pending', 'Shipped', 'Delivered', 'Cancelled'][Math.floor(Math.random() * 4)]; // Random status

      return {
        OrderID: id,
        UserID: userId,
        OrderDate: orderDate,
        Status: status,
        AddressID: addressId,
        TotalAmount: totalAmount,
      };
    })
  ]);

  await knex.raw('SELECT setval(\'"Order_OrderID_seq"\', (SELECT MAX("OrderID") FROM "Order"))');

  // Insert seed entries for Details
  await knex('Detail').insert([
    { DetailID: 1, OrderID: 1, ShoesID: 1, Quantity: 1, Price: 120.00 }, // 120.00
    { DetailID: 2, OrderID: 1, ShoesID: 2, Quantity: 1, Price: 150.00 }, // 150.00
    // Total for OrderID 1 = 270.00

    { DetailID: 3, OrderID: 2, ShoesID: 3, Quantity: 2, Price: 100.00 }, // 200.00
    { DetailID: 4, OrderID: 2, ShoesID: 4, Quantity: 3, Price: 110.00 }, // 330.00
    { DetailID: 5, OrderID: 2, ShoesID: 5, Quantity: 2, Price: 100.00 }, // 200.00
    // Total for OrderID 2 = 730.00

    { DetailID: 6, OrderID: 3, ShoesID: 6, Quantity: 3, Price: 140.00 }, // 420.00
    { DetailID: 7, OrderID: 3, ShoesID: 7, Quantity: 1, Price: 60.00 },  // 60.00
    // Total for OrderID 3 = 480.00

    { DetailID: 8, OrderID: 4, ShoesID: 8, Quantity: 4, Price: 150.00 }, // 600.00
    { DetailID: 9, OrderID: 4, ShoesID: 9, Quantity: 2, Price: 430.00 }, // 860.00
    // Total for OrderID 4 = 1460.00

    { DetailID: 10, OrderID: 5, ShoesID: 10, Quantity: 3, Price: 110.00 }, // 330.00
    // Total for OrderID 5 = 330.00

    // Generate details for 21 additional orders
    ...Array.from({ length: 21 }, (_, i) => {
      const detailIdStart = i * 3 + 11; // Increment DetailID for each order
      const orderId = i + 6;

      // Generate 3 random details for each order
      return [
        {
          DetailID: detailIdStart,
          OrderID: orderId,
          ShoesID: (orderId % 18) + 1,
          Quantity: Math.floor(Math.random() * 5) + 1, // Random quantity between 1 and 5
          Price: Math.floor(Math.random() * 300) + 50, // Random price between 50 and 300
        },
        {
          DetailID: detailIdStart + 1,
          OrderID: orderId,
          ShoesID: ((orderId + 1) % 10) + 1,
          Quantity: Math.floor(Math.random() * 5) + 1,
          Price: Math.floor(Math.random() * 300) + 50,
        },
        {
          DetailID: detailIdStart + 2,
          OrderID: orderId,
          ShoesID: ((orderId + 2) % 10) + 1,
          Quantity: Math.floor(Math.random() * 5) + 1,
          Price: Math.floor(Math.random() * 300) + 50,
        },
      ];
    }).flat()
  ]);

  await knex.raw('SELECT setval(\'"Detail_DetailID_seq"\', (SELECT MAX("DetailID") FROM "Detail"))');
}