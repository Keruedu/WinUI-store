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
      table.string('Email', 255).notNullable().unique();
      table.string('PhoneNumber', 50);
      table.integer('AddressID').unsigned().references('AddressID').inTable('Address');
    });
  
    await knex.schema.createTable('Category', (table) => {
      table.increments('CategoryID').primary();
      table.string('Name', 100).notNullable();
      table.string('Description', 255);
    });
  
    await knex.schema.createTable('Shoes', (table) => {
      table.increments('ShoeID').primary();
      table.integer('CategoryID').unsigned().references('CategoryID').inTable('Category').notNullable();
      table.string('Name', 100).notNullable();
      table.string('Size', 50).notNullable();
      table.string('Color', 50).notNullable();
      table.decimal('Price', 10, 2).notNullable();
      table.integer('Stock').notNullable();
    });
  
    await knex.schema.createTable('Order', (table) => {
      table.increments('OrderID').primary();
      table.integer('UserID').unsigned().references('UserID').inTable('User').notNullable();
      table.date('OrderDate').notNullable();
      table.string('Status', 50).notNullable();
      table.integer('AddressID').unsigned().references('AddressID').inTable('Address');
      table.decimal('TotalAmount', 10, 2).notNullable();
    });
  
    await knex.schema.createTable('Detail', (table) => {
      table.increments('DetailID').primary();
      table.integer('OrderID').unsigned().references('OrderID').inTable('Order').notNullable();
      table.integer('ShoeID').unsigned().references('ShoeID').inTable('Shoes').notNullable();
      table.integer('Quantity').notNullable();
      table.decimal('Price', 10, 2).notNullable();
    });
  };
  
  exports.down = async function(knex) {
    await knex.schema.dropTable('Detail');
    await knex.schema.dropTable('Order');
    await knex.schema.dropTable('Shoes');
    await knex.schema.dropTable('Category');
    await knex.schema.dropTable('Address');
    await knex.schema.dropTable('User');
  };
