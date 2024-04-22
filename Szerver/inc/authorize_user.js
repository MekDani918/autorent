const { getUserByUsername } = require("./db_connect.js");

module.exports = (req, res, next) => {
    getUserByUsername(req.userData.username).then(user => {
        if(user.is_admin){
            next();
            return;
        }
        else{
            return res.status(403).json({
                message:"Access denied"
            });
        }
    }).catch(error => {
        return res.status(500).json({
            message:"Internal server error"
        });
    });
};