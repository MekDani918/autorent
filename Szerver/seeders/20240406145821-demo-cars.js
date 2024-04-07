'use strict';

/** @type {import('sequelize-cli').Migration} */
module.exports = {
  async up (queryInterface, Sequelize) {
    /**
     * Add seed commands here.
     *
     * Example:
     * await queryInterface.bulkInsert('People', [{
     *   name: 'John Doe',
     *   isBetaMember: false
     * }], {});
    */
	return queryInterface.bulkInsert('cars', [
		{
		  category_id: 1,
		  brand: 'Volkswagen',
		  model: 'Golf 3 GTI',
		  daily_price: 10000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 4,
		  brand: 'BMW',
		  model: '850i',
		  daily_price: 38000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 2,
		  brand: 'Mercedes',
		  model: 'E 220',
		  daily_price: 15000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 3,
		  brand: 'Rimac',
		  model: 'Nevera',
		  daily_price: 100000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 5,
		  brand: 'Rolls-Royce',
		  model: 'Phantom',
		  daily_price: 80000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 7,
		  brand: 'Dacia',
		  model: 'Duster',
		  daily_price: 5000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 6,
		  brand: 'Volvo',
		  model: 'XC60',
		  daily_price: 30000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 8,
		  brand: 'Ford',
		  model: 'Ranger',
		  daily_price: 20000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 9,
		  brand: 'Audi',
		  model: 'A4',
		  daily_price: 12000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 2,
		  brand: 'BMW',
		  model: 'M5',
		  daily_price: 50000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 1,
		  brand: 'Ford',
		  model: 'Focus RS',
		  daily_price: 45000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 6,
		  brand: 'Barabus',
		  model: 'G63',
		  daily_price: 53000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  category_id: 4,
		  brand: 'BMW',
		  model: 'M2',
		  daily_price: 40000,
		  createdAt: new Date(),
		  updatedAt: new Date()
		}
	]);
  },

  async down (queryInterface, Sequelize) {
    /**
     * Add commands to revert seed here.
     *
     * Example:
     * await queryInterface.bulkDelete('People', null, {});
     */
	 return queryInterface.bulkDelete('cars', null, {});
  }
};
