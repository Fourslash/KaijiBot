'use strict';
var path = require('path');
var Sequelize = require('sequelize');
var config = require(path.resolve(__dirname, 'config', 'config.json'));
var sequelize = new Sequelize(
	config.database, config.username, config.password, config
);

require('./Models/Deal.js')(sequelize);
require('./Models/Round.js')(sequelize);

sequelize.sync();
module.exports = sequelize;