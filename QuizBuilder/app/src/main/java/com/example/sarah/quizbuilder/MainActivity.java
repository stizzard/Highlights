package com.example.sarah.quizbuilder;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends Activity {
    EditText playerName;
    public final static String EXTRA_MESSAGE = "com.example.sarah.quizbuilder.MESSAGE";

//http://developer.android.com/training/basics/firstapp/starting-activity.html

    //Called on btnStart touch
    public void sendMessage(View view) {
        Log.d("MyActivity", "start clicked");

        playerName=(EditText)findViewById(R.id.etInputName);

        Intent intent = new Intent(this, GameActivity.class);
        EditText editText = (playerName);
        String name = editText.getText().toString();

        if(!name.equals("")){
            intent.putExtra(EXTRA_MESSAGE, name);
            startActivity(intent);
        }else{
            Context context = getApplicationContext();
            CharSequence text = "Please enter your name";
            int duration = Toast.LENGTH_SHORT;
            Toast toast = Toast.makeText(context, text, duration);
            toast.setGravity(Gravity.CENTER, 0, 200);
            toast.show();
            setContentView(R.layout.activity_main);
            playerName.setText("");

        }
    }//end sendMessage

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        playerName=(EditText)findViewById(R.id.etInputName);
        playerName.setText("");
    }//end onCreate

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

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
