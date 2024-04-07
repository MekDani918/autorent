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
	return queryInterface.bulkInsert('rentals', [
		{
		  user_id: 1,
		  car_id: 2,
		  from_date: new Date("2024-04-02"),
		  to_date: new Date("2024-04-06"),
		  price: 123,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  user_id: 2,
		  car_id: 3,
		  from_date: new Date("2024-03-27"),
		  to_date: new Date("2024-03-30"),
		  price: 312,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  user_id: 3,
		  car_id: 6,
		  from_date: new Date("2024-05-06"),
		  to_date: new Date("2024-05-10"),
		  price: 41231,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  user_id: 4,
		  car_id: 4,
		  from_date: new Date("2024-04-23"),
		  to_date: new Date("2024-04-30"),
		  price: 42355,
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
	 return queryInterface.bulkDelete('rentals', null, {});
  }
};
