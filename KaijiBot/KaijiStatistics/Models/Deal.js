var path = require('path');
var sequelize = require(path.resolve(__dirname, '..', 'sequelize.js'));


module.exports = function (sequelize) {
	sequelize.define('Deal', {
		startCards: {
			type: sequelize.Sequelize.TEXT
		},
		endCards: {
			type: sequelize.Sequelize.ARRAY(sequelize.Sequelize.INTEGER),
		},	
		result: {
			type: sequelize.Sequelize.INTEGER,
		},
		win: {
			type: sequelize.Sequelize.BOOLEAN
		},
		chips: {
			type: sequelize.Sequelize.INTEGER,
		},
		chipsDiff: {
			type: sequelize.Sequelize.INTEGER,
		},
	}, {
		tableName: 'Deal',
	});
}
