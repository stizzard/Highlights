<?php
//
//$tasktitle = $_POST['tasktitle'];
//setlocale(LC_TIME, "fi_FI");
//date_default_timezone_set("Europe/Helsinki");
//$date = strftime("%Y-%m-%d-%A");
//$timesaved = strftime("%H:%M:%S");
//$elapsedtime = $_POST['current_time'];
//$file = "saved/".$date.".txt";
//$cont = 'time finished: '.$timesaved.' - time elapsed: '.$elapsedtime.' - task name: '.$tasktitle.''. "n";
//
//$f = fopen ($file, 'a+');
//fwrite($f, $cont);
//fclose($f);
//
//?>
<?php

function debug_to_console( $data ) {

    if ( is_array( $data ) )
        $output = "<script>console.log( 'Debug Objects: " . implode( ',', $data) . "' );</script>";
    else
        $output = "<script>console.log( 'Debug Objects: " . $data . "' );</script>";

    echo $output;
}

//$dataString = $_POST['dataString'];
print_r( $_POST );

$dataString = json_encode($_POST);

//print_r( $dataString );
//$dataString = json_encode($dataString);
print_r( $dataString);

$file = file_get_contents('assets/data-file2-2.json');
$tempArray = json_decode($file);
array_push($tempArray, $_POST);
$jsonData = json_encode($tempArray);
file_put_contents('assets/data-file2-2.json', $jsonData);


//
//$name = $_POST['name'];
//$file = "assets/data-file2-2.json";
//$cont = $dataString;
//$f = fopen ($file, 'a+');
//fwrite($f, $cont);
//fclose($f);