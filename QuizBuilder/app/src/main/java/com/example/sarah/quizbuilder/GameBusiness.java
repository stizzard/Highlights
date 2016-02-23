package com.example.sarah.quizbuilder;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

public class GameBusiness {
    private static final String TAG = " MyActivity";

    private Map<String,String> hashMap = new HashMap<String,String>(); //map
    private String definition; //correct definition
    private String correctWord; //correct word
    private ArrayList<String> answers = new ArrayList<String>(); //correct word + 3 incorrect words


    public void readFile(InputStream file) {

        try{

            BufferedReader buf = new BufferedReader(new InputStreamReader(file));

            String lastLine = null;
            String[] arrayWords;

            while((lastLine = buf.readLine())!=null) {
                arrayWords = lastLine.split(":");
                hashMap.put(arrayWords[0], arrayWords[1]); //add each to map as a set
            }

            buf.close();

        } catch(Exception e){
            e.printStackTrace();
        }
    }//end readFile

    //getters start
    public String getCorrectWord() {
        return correctWord;
    }

    public String getDefinition() {
        return definition;
    }

    public ArrayList<String> getAnswers() {
        return answers;
    }
    //getters end

    //set the question/word
    public boolean setWord(){
        ArrayList<String> availableWords = new ArrayList<String>(); //words with definitions
        ArrayList<String> allWords = new ArrayList<String>(); //all words (with definitions and without)

        for(String key : hashMap.keySet()){
            String value = hashMap.get(key);//gets the VALUE of the key
            Log.d("MyActivity", key + " = " + value);

            //if word hasn't been used yet (definitions hasn't been nulled) add to available words arrayList
            if(value != null){
                availableWords.add(key);
            }

            //arrayList of all words (null definition or not)
            allWords.add(key);

        }

        Log.d("MyActivity", "length = " + allWords.size());

        if(availableWords.size()==0){
            return false;
        }

        Collections.shuffle(availableWords);
        Collections.shuffle(allWords);
        Log.d("MyActivity", "allWords=" + allWords.toString());

        //reset answers for next question
        answers.clear();

        //fill answers
        correctWord = availableWords.get(0);//correct answer
        answers.add(correctWord);//add correct answer to answers

        Log.d("MyActivity", "correct word=" + correctWord + ", definition=" + hashMap.get(correctWord));
        Log.d("MyActivity", "answers = " + answers.size());

        //add 3 other words that are incorrect
        for(int i = 0; i < 4; i++){
            if(!allWords.get(i).equals(correctWord)) {
                answers.add(allWords.get(i));
            }
        }

        //in case i != correctWord every time, delete last added word
        if(answers.size() == 5){
            answers.remove(4);
        }

        Log.d("MyActivity", "answers = " + answers.size());

        Collections.shuffle(answers);

        definition = hashMap.get(correctWord);//gets VALUE of correctWord

        //null the definition of the used word
        hashMap.put(correctWord, null);

        Log.d("MyActivity", "definition=" + hashMap.get(correctWord));

        return true;
    }//end setWord

}
