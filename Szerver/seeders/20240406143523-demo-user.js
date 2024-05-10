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
		  password: '$2b$10$TWuGiYWFF6VcDaJUyzEwYOO3YJfX98DgAF8v/StdsyUINVUtAwBNm',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'admin2',
		  name: 'Auerbach Dávid',
		  password: '$2b$10$XyC.aTnIECdHjt1r9HPJ9OHH.C2oMPtXzMjeUxzFZzQJFWs5aRzQ.',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'admin3',
		  name: 'Koronczai Hont',
		  password: '$2b$10$zjncMXA8LakHOMQfJvmBAelsi6lFdaBetWgtMWN4llSrUwCFLgXgK',
		  is_admin: true,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'bercza',
		  name: 'Bercza Ferenc Dániel',
		  password: '$2b$10$k22462WduOobtU0pd.FApOor8Ze9R1Gs.qhfRdQ054dZfvM7x.oFC',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'harnos',
		  name: 'Harnos Adrián Dániel',
		  password: '$2b$10$lFY0OSKxNhCUD9WNShlCJ.ehmkbXbijkdRcsGhtNHUQsGoY4Q7YGm',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'dmartin',
		  name: 'Dömök Martin',
		  password: '$2b$10$NoMadvKMWOjOeMhg1V4TD.i.DMaviE6uw30bKTy8ijirTrCYY8o3C',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'heller',
		  name: 'Heller Benedek',
		  password: '$2b$10$VN17EQHoxpg5ZUy28e89xuyt47Y4slrn5XBYpSs3zUNHgcdtHWoJW',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'user',
		  name: 'User123',
		  password: '$2b$10$CaSORyotNunMqxtn9gZTT..r2Q52TFTsMc1s7TJzwVidZK7aVBMkS',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'fkrisi',
		  name: 'Ferencz Kristóf',
		  password: '$2b$10$SAnI6pA5qTpmXdtHGIsgk.zvHbONFGcoQWlqkeQqQj.ylRqINLQcu',
		  is_admin: false,
		  createdAt: new Date(),
		  updatedAt: new Date()
		},
		{
		  username: 'hadam',
		  name: 'A. Ádám',
		  password: '$2b$10$7xuWOd2ybkAyfwDqcyWClOstGYWTScDdh2yTvPsky057dUQbAt24e',
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
