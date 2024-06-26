const { sequelize, Car, Category, Rental, Sale, User } = require('../models');
const { Op } = require("sequelize");
const bcrypt = require("bcrypt");

let users = [
    {
        id:1,
        username:"user",
        name:"user",
        password:"123"
    }
];
let categories = [
    {
        id: 1,
        name: 'Hatchback'
    },
    {
        id: 2,
        name: 'Coupe'
    },
    {
        id: 3,
        name: 'SUV'
    }
];
let cars = [
    {
        id: 1,
        category: "Hatchback",
        brand: "Volkswagen",
        model: "Golf",
        dailyPrice: 10000,
        unavailableDates: [
            "2024-03-21"
        ]
    },
    {
        id: 2,
        category: "Coupe",
        brand: "BMW",
        model: "850i",
        dailyPrice: 20000,
        unavailableDates: [
            "2024-03-26",
            "2025-04-28"
        ]
    },
    {
        id: 3,
        category: "SUV",
        brand: "Barabus",
        model: "G63",
        dailyPrice: 25000,
        unavailableDates: [
            "2024-06-30",
            "2024-07-01",
            "2024-07-02"
        ]
    }
];
let rentals = [
    {
        car: {
            id: 1,
            category: "Hatchback",
            brand: "Volkswagen",
            model: "Golf",
            dailyPrice: 10000,
            unavailableDates: [
                "2024-03-21"
            ]
        },
        fromDate: "2024-03-21",
        toDate: "2024-03-21",
        price: 9990,
        rentalTimestamp: "1711042568"
    },
    {
        car: {
            id: 3,
            category: "SUV",
            brand: "Barabus",
            model: "G63",
            dailyPrice: 25000,
            unavailableDates: [
                "2024-06-30",
                "2024-07-01",
                "2024-07-02"
            ]
        },
        fromDate: "2024-06-30",
        toDate: "2024-07-02",
        price: 14990,
        rentalTimestamp: "1711042568"
    }
];

async function initDb(){
    try{
        await sequelize.authenticate();
        await sequelize.sync(/*{ alter: true }*/);
        console.log("SQLite DB Connected!");
    }
    catch(err){
        console.log(err);
    }


    //try{
    //    const user = await User.create({ username: "user", name: "user", password: "123" });
    //    console.log(user.toJSON());
    //    for(cname of [ "Hatchback", "Coupe", "SUV", "Sedan", "Wagon" ]){
    //        const category = await Category.create({ name: cname });
    //        console.log(category.toJSON());
    //    }
    //    for({ id, brand, model ,dailyPrice } of cars){
    //        const car = await Car.create({ category_id: id, brand: brand, model: model, daily_price: dailyPrice });
    //        console.log(car.toJSON());
    //    }
    //    for({ car, fromDate, toDate, price } of rentals){
    //        const rental = await Rental.create({ user_id: 1, car_id: car.id, from_date: fromDate, to_date: toDate, price: price });
    //        console.log(rental.toJSON());
    //    }
    //}
    //catch(err){
    //    console.log(err);
    //}
}

async function registerUser(username, name, password){
    try{
        let salt = await bcrypt.genSalt();
        let password_hash = await bcrypt.hash(password, salt);

        let user = await User.create({
            username:username,
            name:name,
            password:password_hash
        });

        return user;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getUserByUsername(username){
    try{
        return await User.findOne({
            where: { username: username }
        });
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
    return null;
}
async function createCategory(nameIn){
    try{
        let s_cat = await Category.findOne({
            where: { name: nameIn }
        });

        if(s_cat){
            return null;
        }

        const c_category = await Category.create({
            name: nameIn
        });

        return c_category;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getCategoryById(catId){
    try{
        const resCat = await Category.findOne({
            where: { id: catId }
        });
        return resCat;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getCategories(){
    try{
        return await Category.findAll();
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
    return [];
}
async function createCar(brand, model, categoryId, dailyPrice){
    try{
        const c_car = await Car.create({
            category_id: categoryId,
            brand: brand,
            model: model,
            daily_price: dailyPrice
        });

        let car = await getCarById(c_car.id);

        return car;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getCars(category = null){
    try{
        let optionsObj = {
            attributes: [
                'id',
                'brand',
                'model',
                'daily_price'
            ],
            include: {
                model: Category
            }
        };
        if(category){
            const cat = await Category.findOne({
                where: { name: category }
            });

            if(!cat) return [];
            
            optionsObj.where = { category_id: cat.id };
        }
        const cars = await Car.findAll(optionsObj);

        let out = [];
        for(car of cars){
            out.push(await getCustomCarObject(car));
        }

        return out;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
    return [];
}
async function getCarById(carId){
    try{
        const car = await Car.findOne({
            where: { id: carId },
            attributes: [
                'id',
                'brand',
                'model',
                'daily_price'
            ],
            include: {
                model: Category
            }
        });
        return car;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
    return null;
}
async function createSale(carIdIn, descriptionIn, percentIn){
    try{
        let s_sale = await Sale.findOne({
            where: { car_id: carIdIn }
        });

        if(s_sale){
            return null;
        }

        const c_sale = await Sale.create({
            car_id: carIdIn,
            description: descriptionIn,
            percent: percentIn
        });

        return await getCustomSaleObject(c_sale);
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getSaleByCarId(carIdIn){
    try{
        const resSale = await Sale.findOne({
            where: { car_id: carIdIn }
        });
        return resSale;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
}
async function getSales(){
    try{
        let s_sales = []
        for( s_sale of await Sale.findAll()){
            s_sales.push(await getCustomSaleObject(s_sale));
        }
        return s_sales;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }
    return [];
}
async function inserRental(userId, carId, fromTimestamp, toTimestamp){
    try{
        const car = await Car.findOne({
            where: { id: carId }
        });

        if(!car){
            const err = new Error("Car not found!");
            err.status = 404;
            throw err;
        }

        const sale = await Sale.findOne({
            where: { car_id: carId }
        });
        percent = 0;
        if(sale)
            percent = sale.percent;

        const fromDate = new Date(fromTimestamp * 1000);
        const toDate = new Date(toTimestamp * 1000);

        if(!(await isCarValidForPeriod(car, fromDate, toDate))){
            const err = new Error("Car is not available for selected period!");
            err.status = 400;
            throw err;
        }

        const days = Math.floor((toDate - fromDate) / (1000*60*60*24)) + 1
        const price = car.daily_price * days * (100 - percent) / 100;
        
        return await Rental.create({ user_id: userId, car_id: carId, from_date: fromDate, to_date: toDate, price: price });
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        switch (e.status) {
            case 404:
            case 400:
                throw e;
                break;
            default:
                throw err;
                break;
        }
    }
}
async function getRentalsByUserId(userId){
    try{
        const rentals = await Rental.findAll({
            attributes: [
                'from_date',
                'to_date',
                'price',
                'createdAt'
            ],
            include: {
                model: Car,
                include: { model: Category }
            },
            where: { user_id: userId }
        });
        
        out = [];
        for(rent of rentals){
            let s = {}
            s.car = await getCustomCarObject(rent.Car);
            s.fromDate = formatDate(rent.from_date);
            s.toDate = formatDate(rent.to_date);
            s.price = rent.price;
            s.rentalTimestamp = '' + Math.floor(rent.createdAt.getTime() / 1000);

            out.push(s);
        }
        
        return out;
    }
    catch(e){
        const err = new Error("Internal Database Error!");
        err.status = 500;
        throw err;
    }

    return [];
}

async function isCarValidForPeriod(car, fromDate, toDate){
    if(!fromDate || !toDate)
        return false;

    const now = new Date()
    if(fromDate > toDate)
        return false;
    if(fromDate < now && !isSameDay(fromDate, now))
        return false;
    if(toDate < now && !isSameDay(toDate, now))
        return false;
    const rentals = await Rental.findAndCountAll({
        where: {
            [Op.and]: [
                {
                    from_date:{ [Op.gte]: fromDate }
                },
                {
                    from_date:{ [Op.lte]: toDate }
                },
                {
                    to_date:{ [Op.gte]: fromDate }
                },
                {
                    to_date:{ [Op.lte]: toDate }
                },
                {
                    car_id:{ [Op.eq]: car.id }
                }
            ]
            
        }
    })
    if(!rentals || rentals.count > 0)
        return false;


    return true;
}
function isSameDay (date1, date2) {
    return date1.toDateString() == date2.toDateString();
}
function formatDate(date_in) {
    return date_in.toISOString().split('T')[0];
}
async function getCustomCarObject(car_in){
    let sobj = {};

    sobj.id = car_in.id;
    sobj.category = car_in.Category.name;
    sobj.brand = car_in.brand;
    sobj.model = car_in.model;
    sobj.dailyPrice = car_in.daily_price;
    sobj.discountPercentage = 0;
    sobj.unavailableDates = [];

    let today = new Date();
    let c_rents = await Rental.findAll({
        where: {
            car_id: car_in.id,
            to_date: { [Op.gte]: today }
        }
    });

    if(c_rents){
        for(c_rent of c_rents){
            for(let z=c_rent.from_date; z<=c_rent.to_date; z.setDate(z.getDate()+1)){
                let sdate = formatDate(z);
                if(!sobj.unavailableDates.includes(sdate)){
                    sobj.unavailableDates.push(sdate);
                }
            }
        }
    }

    let c_sale = await Sale.findOne({
        where: { car_id: car_in.id },
        attributes: ['percent']
    });
    if(c_sale && c_sale.percent){
        sobj.discountPercentage = c_sale.percent;
    }

    return sobj;
}
async function getCustomSaleObject(sale_in){
    csObj = {};
    csObj.car = await getCustomCarObject(await getCarById(sale_in.car_id));
    csObj.description = sale_in.description;
    csObj.percent = sale_in.percent;
    return csObj;
}

initDb();

module.exports.registerUser = registerUser;
module.exports.getUserByUsername = getUserByUsername;
module.exports.createCategory = createCategory;
module.exports.getCategoryById = getCategoryById;
module.exports.getCategories = getCategories;
module.exports.createCar = createCar;
module.exports.getCars = getCars;
module.exports.getCarById = getCarById;
module.exports.inserRental = inserRental;
module.exports.getRentalsByUserId = getRentalsByUserId;
module.exports.getSales = getSales;
module.exports.getSaleByCarId = getSaleByCarId;
module.exports.createSale = createSale;
module.exports.getCustomCarObject = getCustomCarObject;