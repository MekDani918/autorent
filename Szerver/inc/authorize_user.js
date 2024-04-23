const { getUserByUsername } = require("./db_connect.js");

module.exports = (req, res, next) => {
    if(req.userData.role == 'admin'){
        next();
        return;
    }
    return res.status(403).json({
        message:"Access denied"
    });
};