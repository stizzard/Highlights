package com.example.sarah.quizbuilder;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.io.InputStream;

public class GameActivity extends Activity {
    TextView number, definition, playerName, score;
    Button wOne, wTwo, wThree, wFour;

    final GameBusiness game = new GameBusiness();

    //user notification: correct/incorrect response
    public void toaster(Button buttonNum) {
        if (buttonNum.getText() == game.getCorrectWord()) {
            Context context = getApplicationContext();
            CharSequence text = "That's right!";
            int duration = Toast.LENGTH_SHORT;
            Toast toast = Toast.makeText(context, text, duration);
            toast.setGravity(Gravity.CENTER, 0, 150);
            toast.show();
        } else {
            Context context = getApplicationContext();
            CharSequence text = "Nope";
            int duration = Toast.LENGTH_SHORT;
            Toast toast = Toast.makeText(context, text, duration);
            toast.setGravity(Gravity.CENTER, 0, 150);
            toast.show();
        }
    }//end toaster

    //scoring
    public void addScore(Button buttonNum, TextView score){
        if (buttonNum.getText() == game.getCorrectWord()) {
            String scoreStr = score.getText().toString();
            int scoreNum;
            scoreNum = Integer.parseInt(scoreStr);
            score.setText(String.valueOf(scoreNum + 1));
        }
    }//end addScore

    //sets word, def, incorrect words
    private void nextQuestion(){
        String scoreStr = score.getText().toString();
        int scoreNum = Integer.parseInt(scoreStr);

        //prompt restart if all definitions are null
        if(!game.setWord()){
            Context context = getApplicationContext();
            CharSequence text = "Your total score is " + scoreNum + ". To play again, press restart.";
            int duration = Toast.LENGTH_LONG;
            Toast toast = Toast.makeText(context, text, duration);
            toast.setGravity(Gravity.CENTER, 0, 150);
            toast.show();
        } else {
            wOne.setText(game.getAnswers().get(0));
            wTwo.setText(game.getAnswers().get(1));
            wThree.setText(game.getAnswers().get(2));
            wFour.setText(game.getAnswers().get(3));
            definition.setText(game.getDefinition());
        }
    }//end nextQuestion

    //Called on btnReset touch
    public void restart(View view) {
        Log.d("MyActivity", "restart clicked");

        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }//end restart

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //receive Intent from http://developer.android.com/training/basics/firstapp/starting-activity.html
        // Get the name from the intent on prev page (MainActivity.java)
        Intent intent = getIntent();
        String name = intent.getStringExtra(MainActivity.EXTRA_MESSAGE);
        Log.d("MyActivity", name);

        // set the view
        setContentView(R.layout.activity_game);

        //set the controls
        definition=(TextView)findViewById(R.id.tvDefinition);
        playerName=(TextView)findViewById(R.id.tvName);
        wOne=(Button)findViewById(R.id.btnWordOne);
        wTwo=(Button)findViewById(R.id.btnWordTwo);
        wThree=(Button)findViewById(R.id.btnWordThree);
        wFour=(Button)findViewById(R.id.btnWordFour);
        score=(TextView)findViewById(R.id.tvScore);

        //read the file
        InputStream quizFile;
        try {
            quizFile = getAssets().open("quiz.txt");
            game.readFile(quizFile);
        } catch(Exception e) {
            e.printStackTrace();
        }

        //add string from etInputName in tvName
        name += "'s Score: ";
        playerName.setText(name);
        Log.d("MyActivity", playerName.toString());

        //word buttons listeners
        //can they be combined? they all do the same thing...
        wOne.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                toaster(wOne);
                addScore(wOne, score);
                nextQuestion();

            }
        });

        wTwo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                toaster(wTwo);
                addScore(wTwo, score);
                nextQuestion();

            }
        });

        wThree.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                toaster(wThree);
                addScore(wThree, score);
                nextQuestion();

            }
        });

        wFour.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                toaster(wFour);
                addScore(wFour, score);
                nextQuestion();

            }
        });

        //sets first question
        nextQuestion();
    }//end onCreate

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
