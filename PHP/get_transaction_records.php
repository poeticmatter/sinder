<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 - connection failed";
		exit();
	}

	$expansionNumber= $_POST["expansion"];

	$gettransactionsquery = "SELECT * FROM card_transactions WHERE";
	if ($expansionNumber == 452) {
		$gettransactionsquery = $getcardsquery . "(expansion=452 OR expansion=453);";
	} else {
		$gettransactionsquery = $getcardsquery . "expansion=" . $expansionNumber . ";";
	}

	$gettractions = mysqli_query($con, $gettransactionsquery) or die("2 - get transactions query failed");
	$data = array();
	while($row = mysqli_fetch_assoc($gettractions)) {
    	$data[] = $row;
	}
	$root = array('transactions' => $data );
	header('Content-Type: application/json');
	echo json_encode($root);
	
?>