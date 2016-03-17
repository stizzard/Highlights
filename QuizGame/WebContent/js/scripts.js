var answers = [];

//-click event for quiz buttons
$('#colorBtn').click(function(){
	startQuiz('colors');
});
  
$('#sizeBtn').click(function(){
	startQuiz('sizes');
});
  
$('#numberBtn').click(function(){
	startQuiz('numbers');
});

//display questions
function startQuiz(file) {
	var questions;
	var url;
	
	//figure out which file to use
	switch(file) {
		case 'colors':
			url = '../assets/color.txt';
			break;
		case 'sizes':
			url = '../assets/size.txt';
			break;
		case 'numbers':
			url = '../assets/highest.txt'
			break;
		default:
			return;
	}
	
	//apply data from file
	$.get(url, function(data){
		questions = getQuestions(data);
		showQuestions(questions);
	});

}//end startQuiz(file)



//randomly choose 5 question sets to display
function getQuestions(file) {
	var questions = [];
	var lines = file.split("\n");
	
	$.each(lines, function(line, data){
		questions.push(data);
	});
	
	shuffle(questions);
	
	return questions;
};//end getQuestions

function showQuestions(questions){
	answers = [];
	
	$('#quizArea').empty();

	$.each(questions, function(i, question) {
		var line = question.split(":");
		var div = $('<div>');
		var p = $('<p>');
		
		p.append(line[1] + '<br>');
		
		//add options with radio buttons
		$.each(line, function(j, option){
			if(j > 1 && j < 6){
				p.append($('<input type="radio" name="answer' + i + '" value="' + option + '">' + option + '<br>'));
			}
		});
		
		//add hidden show answer button
		p.append($('<div class="hide" id="answer' + i + '"><button type="button" class="answerBtn">Show Answer</button>' +
			'<br><div class="answerDiv hide">The correct answer is: ' + line[6] + '</div></div>'));

		answers.push(line[6]);
		$('#quizArea').append(div.append(p));
	});

	$('button.answerBtn').click(function(){
		var answerDiv = $(this).parent().find($('.answerDiv'));
		if (answerDiv.hasClass("hide")){
			answerDiv.toggleClass("show");
		}else {
			answerDiv.toggleClass("hide");
		}
	});

//	console.log(questions);
//	console.log(answers);
};//end showQuestions


//-click event for refresh
$('#refreshBtn').click(function(){
	$('#quizArea input').prop('checked', false);
	$('#quizArea input').prop('disabled', false);
	$('#quizArea div p div').removeClass("show");
	$('#quizArea div').css({'background-color':'white'});
});//end refreshBtn

//-click event for submit
//	display message box with:
//		correct answer total
//		incorrect answer total
//		not answered total
//	display button next to each unanswered/incorrect answer (showAnswer)
//	unanswered/incorrect answers background set to red
//	correct answers background set to green
//	disable answers
$('#submitBtn').click(function(){
	checkAnswers();	
});//end submitBtn

function checkAnswers(){
	var correctAnswers = 0;
	var unanswered = 0;
	var incorrect = 0;
	
	$.each($('#quizArea > div'), function(i){
		//console.log(this);
		
		var checked = $(this).find('input[type="radio"]:checked');
		var value = checked.val();
		var answer = answers[i];
		
		//console.log(value);
		//console.log(answer);
		
		if(value == undefined){
			$(this).children().children().toggleClass("show");
			$(this).css({'background-color':'red'});
			unanswered ++;
			//console.log('no match');
		}else if(value.trim() === answer.trim()) {
			$(this).css({'background-color':'green'});
			correctAnswers++;
			//console.log('match');
		} else {
			$(this).children().children().toggleClass("show");
			$(this).css({'background-color':'red'});
			incorrect ++;
			//console.log('no match');
		}
		
		//console.log(correctAnswers);
		$(this).find('input[type="radio"]').attr('disabled', 'disabled');
		
	});
	
	//how  many  questions  were  successfully answered, how many were not answered, and how many were incorrect. 
	alert('Correct answers: ' + correctAnswers + '\n' +
			'Unanswered questions: ' + unanswered + '\n' + 
			'Incorrect answers: ' + incorrect);
	
}//end checkAnswers



/////////////////
//SHUFFLE////////
//http://stackoverflow.com/questions/2450954/how-to-randomize-shuffle-a-javascript-array
/////////////////
function shuffle(array) {
	  var currentIndex = array.length, temporaryValue, randomIndex;

	  // While there remain elements to shuffle...
	  while (0 !== currentIndex) {

	    // Pick a remaining element...
	    randomIndex = Math.floor(Math.random() * currentIndex);
	    currentIndex -= 1;

	    // And swap it with the current element.
	    temporaryValue = array[currentIndex];
	    array[currentIndex] = array[randomIndex];
	    array[randomIndex] = temporaryValue;
	  }

	  //remove last 3 questions
	  array.splice(1, 3);
	  
	  return array;
};//end shuffle
