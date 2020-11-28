<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 - connection failed";
		exit();
	}

	$expansionNumber= $_POST["expansion"];

	$getcardsquery = "SELECT id, card_title, front_image, rarity FROM cards WHERE ";
	if ($expansionNumber == 452) {
		$getcardsquery = $getcardsquery . "(expansion=452 OR expansion=453);";
	} else {
		$getcardsquery = $getcardsquery . "expansion=" . $expansionNumber . ";";
	}
	$getcards = mysqli_query($con, $getcardsquery) or die("2 - name check query failed");
	$data = array();
	while($row = mysqli_fetch_assoc($getcards)) {
    	$data[] = $row;
	}
	$root = array('cardsArray' => $data );
	header('Content-Type: application/json');
	echo json_encode($root);
	
?>