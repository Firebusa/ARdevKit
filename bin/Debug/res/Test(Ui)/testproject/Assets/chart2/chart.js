arel.Plugin.Chart2 = 
{
	visible : false,
	id : "chart2",
	coordinateSystemID : 2,
	options : {},
	translation : new arel.Vector3D(198.0,143.0,0.0),
	div : document.createElement("div"),
	create : function()
	{
		this.div.setAttribute("id", this.id);
		this.div.style.position = "absolute";
		this.div.style.width = "200px";
		this.div.style.height = "200px";
		document.documentElement.appendChild(this.div);
		$.getScript("Assets/chart2/options.js", function()
		{
			arel.Plugin.Chart2.options = init();
			$('#' + arel.Plugin.Chart2.id).highcharts(arel.Plugin.Chart2.options);
		})
		.fail(function() { console.log("Failed to load options for chart2")})
		.done(function() { console.log("Loaded options for chart2 successfully")});
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
