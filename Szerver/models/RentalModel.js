'use strict';
const {
  Model
} = require('sequelize');
module.exports = (sequelize, DataTypes) => {
  class Rental extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      // define association here
      Rental.belongsTo(models.User, {
        foreignKey: 'user_id',
        as: 'user',
        onDelete: 'CASCADE'
      });
      Rental.belongsTo(models.Car, {
        foreignKey: 'car_id',
        onDelete: 'CASCADE'
      })
    }

    toJSON(){
      return { ...this.get(), createdAt: undefined, updatedAt: undefined }
    }
  }
  Rental.init({
    user_id: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    car_id: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    from_date: {
      type: DataTypes.DATE,
      allowNull: false
    },
    to_date: {
      type: DataTypes.DATE,
      allowNull: false
    },
    price: {
      type: DataTypes.INTEGER,
      allowNull: false
    }
  }, {
    sequelize,
    tableName: 'rentals',
    modelName: 'Rental',
  });
  return Rental;
};