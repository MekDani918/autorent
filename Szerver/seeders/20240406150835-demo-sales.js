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
	return queryInterface.bulkInsert('sales', [
		{
		  car_id: 2,
		  description: 'Kifutó autó',
		  percent: 30,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  car_id: 4,
		  description: 'Kifutó autó',
		  percent: 50,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  car_id: 8,
		  description: 'Kifutó autó',
		  percent: 20,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  car_id: 11,
		  description: 'Kifutó autó',
		  percent: 10,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  car_id: 13,
		  description: 'Kifutó autó',
		  percent: 5,
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
	 return queryInterface.bulkDelete('sales', null, {});
  }
};
