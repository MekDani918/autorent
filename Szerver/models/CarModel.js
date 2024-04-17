'use strict';
const {
  Model
} = require('sequelize');
module.exports = (sequelize, DataTypes) => {
  class Car extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      // define association here
      Car.belongsTo(models.Category, {
        foreignKey: 'category_id',
        onDelete: 'CASCADE'
      });
      Car.hasMany(models.Rental, {
        foreignKey: 'car_id'
      });
      Car.hasMany(models.Sale, {
        foreignKey: 'car_id'
      });
    }

    toJSON(){
      return { ...this.get(), createdAt: undefined, updatedAt: undefined }
    }
  }
  Car.init({
    category_id: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    brand: {
      type: DataTypes.STRING,
      allowNull: false
    },
    model: {
      type: DataTypes.STRING,
      allowNull: false
    },
    daily_price: {
      type: DataTypes.INTEGER,
      allowNull: false
    }
  }, {
    sequelize,
    tableName: 'cars',
    modelName: 'Car',
  });
  return Car;
};