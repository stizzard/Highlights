/**
 * Created by inet2005 on 10/15/15.
 */
"use strict";

//determines row & col of td
function findClass(ele, regex) {
    var classes = ele.className.split(/\s+/); //split class names on one or more spaces
    for(var i=0; i < classes.length; i++){
        var matches = classes[i].match(regex);
        if(matches){
            return matches[0];
        }
    }
    return false;
}


function keyUp(e){
    var entry = this.innerHTML;

    this.style.color = 'green';

    if(entry == ""){
        return;
    }

    //validate entry (numerical value)
    if(!entry.match(/^[0-9]$/)){
        alert("not a single digit");
        this.innerHTML = "";
    }


    //validate entry (Sudoku rules)

    //check subTable
    var trs = this.parentNode.parentNode.children;
    for(var tr=0; tr < trs.length; tr++){
        var tds = trs[tr].children;
        for(var td=0; td < tds.length; td++){
            if (entry == tds[td].innerHTML && this != tds[td]) {
                alert("repeat subtable entry");
            }
        }
    }


    //check row
    var rowClass = findClass(this,/row.?/);
    var rowtds = document.getElementsByClassName(rowClass);
    for(var rowtd=0; rowtd < rowtds.length; rowtd++){
        if(entry == rowtds[rowtd].innerHTML && this != rowtds[rowtd]){
            alert("repeat row entry");
            console.log(this.innerHTML);
            this.style.color = 'red';
        }
    }
    //console.log(findClass(this,/row.?/));


    //check column
    var colClass = findClass(this,/col.?/);
    var coltds = document.getElementsByClassName(colClass);
    for(var coltd=0; coltd < coltds.length; coltd++){
        if(entry == coltds[coltd].innerHTML && this != coltds[coltd]){
            alert("repeat column entry");
            this.className = 'error';
        }
    }
    //console.log(findClass(this,/col.?/));
    //console.log(this.innerHTML);



    //check for completion
    var entries = document.getElementsByClassName('editable');
    //console.log(entries.length);

    var complete = true;
    for(var es=0; es < entries.length; es++) {
        console.log(entries[es].innerHTML);
        if (entries[es].innerHTML == "" || entries[es].className == 'error') {
            complete = false;
            console.log("colClass: " + colClass + " rowClass:" + rowClass + " TF: " + complete + " es: " + es);
            console.log(entries[es]);
            break;
        }
    }

    if(complete == true){
        alert("You finished the puzzle!");
    }
}


//reset puzzle
function clearPuzzle() {
    editables = document.getElementsByClassName("editable");
    for(var c=0; c > editables.length; c++){
        editables[c].innerHTML = "";
    }
}


//solve puzzle
function showSolution(e) {
    e.preventDefault();

    var solutionImg = document.getElementById('solution');
    var buttonText = document.getElementById('peek');
    console.log(buttonText);

    if(solutionImg.className == "solution"){
        solutionImg.className = "solutionDisplay";
        buttonText.value = "Hide";
    }else{
        solutionImg.className = "solution";
        buttonText.value = "Peek";
    }
}


//auto-fill
function autoSolve(e){
    e.preventDefault();

    var cells = document.getElementsByClassName("editable");
    console.log(cells);

    //final entry left blank to demonstrate final keyup function with ease
    // array should be complete in a live situation
    //final number for array is 4
    var answerKey = [1,2,8,9,6,7, 6,2,4,7,1,8,9, 8,9,5,4,6,
        5,1,8,3, 4,6,7,1,5,3,8, 2,5,6,1,
        7,8,9,2,3, 9,3,4,6,7,2,5, 6,1,8,3,9,""];
    for(var i = 0; i < cells.length; i++){
        cells[i].innerHTML = answerKey[i];
    }

    keyUp.apply(cells[0],[e]);
}




//make tds editable...must loop to assign different attribute to each td in editables array
var editables = document.getElementsByClassName("editable");
for (var e=0; e < editables.length; e++) {
    editables[e].setAttribute("contenteditable",true);
    editables[e].addEventListener("keyup", keyUp);
}

document.getElementById('formReset').addEventListener("submit", clearPuzzle);
document.getElementById('formPeek').addEventListener("submit",showSolution);
document.getElementById('formSolve').addEventListener("submit",autoSolve);
