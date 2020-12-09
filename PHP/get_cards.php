<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 - connection failed";
		exit();
	}

	$expansionNumber= $_POST["expansion"];
	$getcardsquery = "SELECT id, card_title, front_image, rarity, house FROM cards";
	if ($expansionNumber != "ALL") {
		$getcardsquery = $getcardsquery . " WHERE expansion=" . $expansionNumber . ";";
	}
	
	$getcards = mysqli_query($con, $getcardsquery) or die("2 - get cards query failed");
	$data = array();
	while($row = mysqli_fetch_assoc($getcards)) {
    	$data[] = $row;
	}
	$root = array('cardsArray' => $data );
	header('Content-Type: application/json');
	echo json_encode($root);
	
?>