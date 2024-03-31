//const mysql = require('mysql');

//const conn = mysql.createConnection({
//    host: process.env.DB_HOST || "127.0.0.1",
//    user: process.env.DB_USER || "root",
//    password: process.env.DB_PASSWD || ""
//});

//conn.connect((error)=>{
//    if(error) throw error;
//    console.log("DB Connected!");
//});

function getUserByUsername(username){
    let users = [
        {
            id:1,
            username:"user",
            name:"user",
            password:"123"
        }
    ];

    return users.find((x)=>x.username == username);
}
function getCars(category = null){
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
    return cars.filter((x)=>(x.category == category || category == null));
}
function getCarById(carId){
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
    return cars.find((x)=>x.id==carId);
}


module.exports.getUserByUsername = getUserByUsername;
module.exports.getCars = getCars;
module.exports.getCarById = getCarById;