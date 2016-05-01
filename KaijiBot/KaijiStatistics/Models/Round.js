var path = require('path');
var sequelize = require(path.resolve(__dirname, '..','sequelize.js'));
module.exports = function (sequelize) {
	sequelize.define('Round', {
		roundNum: {
			type: sequelize.Sequelize.INTEGER,
		},
		dealId: {
			type: sequelize.Sequelize.INTEGER,
		},	
		bet: {
			type: sequelize.Sequelize.INTEGER,
		},
		result: {
			type: sequelize.Sequelize.INTEGER,
		},		
	}, {
		tableName: 'Round',
	});
}
