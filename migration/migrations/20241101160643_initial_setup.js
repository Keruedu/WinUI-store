exports.up = async function(knex) {

  await knex.schema.createTable('Address', (table) => {
      table.increments('AddressID').primary();
      table.string('Street', 255).notNullable();
      table.string('City', 100).notNullable();
      table.string('State', 100).notNullable();
      table.string('ZipCode', 20).notNullable();
      table.string('Country', 100).notNullable();
  });

  await knex.schema.createTable('User', (table) => {
    table.increments('UserID').primary();
    table.string('Name', 255).notNullable();
    table.string('Password', 255).notNullable();
    table.string('Email', 255).notNullable().unique();
    table.string('PhoneNumber', 50);
    table.integer('AddressID').unsigned().references('AddressID').inTable('Address');
    table.enu('Role', ['Admin', 'Manager', 'User']).notNullable().defaultTo('User');
    table.enu('Status', ['Active', 'Banned']).notNullable().defaultTo('Active');
    table.string('Image');
  });


  await knex.schema.createTable('Category', (table) => {
    table.increments('CategoryID').primary();
    table.string('Name', 100).notNullable();
    table.string('Description', 255);
  });

  await knex.schema.createTable('Shoes', (table) => {
    table.increments('ShoesID').primary();
    table.integer('CategoryID').unsigned().references('CategoryID').inTable('Category').notNullable();
    table.string('Name', 100).notNullable();
    table.string('Brand', 100);
    table.string('Size', 50);
    table.string('Color', 50);
    table.decimal('Cost', 10, 2).defaultTo(0).notNullable();
    table.decimal('Price', 10, 2).notNullable();
    table.integer('Stock').notNullable(); 
    table.string('Image');
    table.string('Description', 255);
    table.enu('Status', ['Active', 'Inactive']).defaultTo('active');
});

  await knex.schema.createTable('Order', (table) => {
    table.increments('OrderID').primary();
    table.integer('UserID').unsigned().references('UserID').inTable('User').notNullable();
    table.date('OrderDate').notNullable();
    table.enu('Status', ['Pending', 'Shipped', 'Delivered', 'Cancelled']).notNullable().defaultTo('Pending');
    table.integer('AddressID').unsigned().references('AddressID').inTable('Address');
    table.decimal('TotalAmount', 10, 2).notNullable();
  });

  await knex.schema.createTable('Detail', (table) => {
    table.increments('DetailID').primary();
    table.integer('OrderID').unsigned().references('OrderID').inTable('Order').notNullable();
    table.integer('ShoesID').unsigned().references('ShoesID').inTable('Shoes').notNullable();
    table.integer('Quantity').notNullable();
    table.decimal('Price', 10, 2).notNullable();
  });
};

exports.down = async function(knex) {
  await knex.schema.dropTable('Detail');
  await knex.schema.dropTable('Order');
  await knex.schema.dropTable('Shoes');
  await knex.schema.dropTable('Category');
  await knex.schema.dropTable('User');
  await knex.schema.dropTable('Address');
  
};