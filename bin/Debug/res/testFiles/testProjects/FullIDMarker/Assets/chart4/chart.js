arel.Plugin.Chart4 = 
{
	visible : false,
	id : "chart4",
	coordinateSystemID : 4,
	options : {},
	translation : new arel.Vector3D(-161.0,143.0,0.0),
	div : document.createElement("div"),
	create : function()
	{
		this.div.setAttribute("id", this.id);
		this.div.style.position = "absolute";
		this.div.style.width = "200px";
		this.div.style.height = "200px";
		document.documentElement.appendChild(this.div);
		$.getScript("Assets/chart4/options.js", function()
		{
			arel.Plugin.Chart4.options = init();
			$('#' + arel.Plugin.Chart4.id).highcharts(arel.Plugin.Chart4.options);
		})
		.fail(function() { console.log("Failed to load options for chart4")})
		.done(function() { console.log("Loaded options for chart4 successfully")});
		$.getScript("Assets/chart4/query.js", function()
		{
			var dataPath = "Assets/chart4/data.json";
			query(dataPath, arel.Plugin.Chart4);
		})
		.fail(function() { console.log("Failed to load query")})
		.done(function() { console.log("Loaded query successfully")});
	},
	show : function()
	{
		$('#' + this.id).show();
		this.visible = true;
	},
	hide : function()
	{
		$('#' + this.id).hide();
		this.visible = false;
	},
	getCoordinateSystemID : function()
	{
		return this.coordinateSystemID;
	}
};
