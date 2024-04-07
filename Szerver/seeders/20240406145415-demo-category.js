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
	return queryInterface.bulkInsert('category', [
		{
		  name: 'Hot hatch',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Sedan',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Supercar',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Coupe',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Luxury',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'SUV',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Crossover',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Pickup',
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  name: 'Wagon',
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
	 return queryInterface.bulkDelete('category', null, {});
  }
};
