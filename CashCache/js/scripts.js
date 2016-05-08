$('#dataDisplay').click(function(){
    $(this).html() == "Hide All Data" ? $(this).html('Show All Data') : $(this).html('Hide All Data');

	$.ajax({
        url : "assets/data-file2-2.json",
        dataType: "json",
        success : function (data) {
        	
        	$('#dataTable').toggleClass('hide', 'show');
        	
        	
        	$.each(data, function(i, val){        		
            	$('#dataTable').append("<tr>" + "<td>" + val.name + "</td>" + 
            			"<td>" + val.phone + "</td>" +
            			"<td>" + val.email + "</td>" +
            			"<td>" + val.zip + "</td>" +
            			"<td>" + val.income + "</td>" +
            			"<td>" + val.age + "</td>" +
            			"</tr>");
        	});        	        
        }
    });
});

//Find & display average income of a zip code
$('#zipAverage').click(function(){
    $(this).html() == "Hide Avg Income in Zip" ? $(this).html('Show Avg Income in Zip') : $(this).html('Hide Avg Income in Zip');

	$.ajax({
        url : "assets/data-file2-2.json",
        dataType: "json",
        success : function (data) {
        	$.each(data, function(i, val){

        		//convert income strings to ints
        		val.income = val.income.replace("$", "");
        		val.income = val.income.replace(",", "");
        		val.income = parseInt(val.income);
        		
        	});     
        	
        	//toggle table
        	$('#zipAvgTable').toggleClass('hide', 'show');

        	
        	//count repeated zips, add incomes together
        	var counter = {};
        	var income = {};
        	for (var i = 0; i < data.length; i += 1) {
        	    counter[data[i].zip] = (counter[data[i].zip] || 0) + 1;
        	    income[data[i].zip] = income[data[i].zip] || 0;
        	    income[data[i].zip] += data[i].income;
        	}
        	var avgIncomes = [];
        	var incomeZips = [];
        	//get average income for each zip, display in table
        	for (var key in counter) {
    	    	//console.log(key, " is duplicated ", counter[key], " times");
    	    	var avgIncome = (income[key] / counter[key]);
    	    	var incomeZip = key;
            	var max = Math.max.apply(null, avgIncomes);
            	var min = Math.min.apply(null, avgIncomes); 
            	var x = avgIncomes.indexOf(max);
            	var y = avgIncomes.indexOf(min);
            	//console.log(x);
    	    	avgIncomes.push(avgIncome);
	    		incomeZips.push(incomeZip);
    	    	avgIncome = avgIncome.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    	    	max = max.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    	    	min = min.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
            	var maxKey = incomeZips[x];//
            	var minKey = incomeZips[y];//

    	    	//show average income in zip
    	    	$('#zipAvgTable').append("<tr>" + 
            			"<td>" + key + "</td>" + 
            			"<td>$" + avgIncome + "</td>" +
            			"</tr>");
        	}

            $('#highestAvgIncome').html("Zip with highest average income: " + maxKey + " | Average income: $" + max).toggleClass('hide', 'show');
        	$('#lowestAvgIncome').html("Zip with lowest average income: " + minKey + " | Average income: $" + min).toggleClass('hide', 'show');
        	
        }
    });	 

});


$('#incomeAgeRange').click(function(){
    $(this).html() == "Hide Avg Income in Age Range" ? $(this).html('Show Avg Income in Age Range') : $(this).html('Hide Avg Income in Age Range');

    //calculate avg income in age range
	$.ajax({
        url : "assets/data-file2-2.json",
        dataType: "json",
        success : function (data) {
        	$.each(data, function(i, val){

        		//convert income strings to ints
        		val.income = val.income.replace("$", "");
        		val.income = val.income.replace(",", "");
        		val.income = parseInt(val.income);
        		
        	});       
        	
        	//count repeated age ranges, add incomes together
        	var counter = {};
        	var income = {};
        	for (var i = 0; i < data.length; i += 1) {
        	    counter[data[i].age] = (counter[data[i].age] || 0) + 1;
        	    income[data[i].age] = income[data[i].age] || 0;
        	    income[data[i].age] += data[i].income;
        	}
        	var avgIncomes = [];
        	var incomeAges = [];

        	//get average income for each zip, display in table
        	for (var key in counter) {
    	    	//console.log(key, " is duplicated ", counter[key], " times");
    	    	var avgIncome = (income[key] / counter[key]);
    	    	var incomeAge = key;
    	    	
    	    	avgIncomes.push(avgIncome);
    	    	incomeAges.push(incomeAge);
    	    	avgIncome = avgIncome.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    	    	
        	}

        	var x = 0;
        	var len = avgIncomes.length;
        	while(x < len){ 
        		avgIncomes[x] = parseInt(avgIncomes[x].toFixed(2)); 
        	    x++
        	}
        	
        	$('#chartContainer').toggleClass('show');       

        	var chart = new CanvasJS.Chart("chartContainer", {
        		title:{
        			text: "Average Income per Age Range"              
        		},
				animationEnabled: false,
        		data: [              
        		{
        			type: "column",
        			dataPoints: [
        				{ label: "18-24",  y: avgIncomes[0]  },
        				{ label: "25-34", y: avgIncomes[1]  },
        				{ label: "35-44", y: avgIncomes[2]  },
        				{ label: "45-54",  y: avgIncomes[3]  },
        				{ label: "55-65",  y: avgIncomes[4]  }
        			]
        		}
        		]
        	});
        	chart.render();
        	//console.log("income ages:" +incomeAges[0]);
        	//console.log("avg incomes:" +avgIncomes[0] );
        	//console.log("avg incomes:" +avgIncomes[1] );
        	//console.log("avg incomes:" +avgIncomes[2] );
        	//console.log("avg incomes:" +avgIncomes[3] );
        	//console.log("avg incomes:" +avgIncomes[4] );

        }
    });	 	
});//end incomeRange click


$('#addEntry').click(function(){
    $(this).html() == "Cancel" ? $(this).html('Add Entry') : $(this).html('Cancel');
    $('#addForm').toggleClass('hide', 'show');

});
$('#addForm').submit(function(e){

    var name = $("#name").val();
    var phone = $("#phone").val();
    var email = $("#email").val();
    var zip = $("#zip").val();
    var income = $("#income").val();
    var age = $("#age").val();
    income = parseInt(income);
    income = income.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    //console.log(income);
    income = '$' + income;
    //console.log(income);
    $.ajax({
        type: "POST",
        url: "save.php",
        data: {
            name: name,
            phone: phone,
            email: email,
            zip: zip,
            income: income,
            age: age
        },

        success: function() {
            $('#addForm').html("<h1>Success!</h1>").fadeOut(1500);
            $('#addEntry').html('Add Entry');
			//console.log("success");
        }
    });
    return false;

});

