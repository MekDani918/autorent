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
	return queryInterface.bulkInsert('users', [
		{
		  username: 'admin',
		  name: 'Mátics Dániel',
		  password: '123',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'admin2',
		  name: 'Auerbach Dávid',
		  password: 'admin2',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'admin3',
		  name: 'Koronczai Hont',
		  password: 'admin3',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'bercza',
		  name: 'Bercza Ferenc Dániel',
		  password: 'bercza',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'harnos',
		  name: 'Harnos Adrián Dániel',
		  password: 'harnos',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'dmartin',
		  name: 'Dömök Martin',
		  password: 'dmartin',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'heller',
		  name: 'Heller Benedek',
		  password: 'hbeni',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'user',
		  name: 'User123',
		  password: '123',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'fkrisi',
		  name: 'Ferencz Kristóf',
		  password: 'krisi',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'hadam',
		  name: 'A. Ádám',
		  password: 'aharcos',
		  is_admin: false,
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
	 return queryInterface.bulkDelete('users', null, {});
  }
};
