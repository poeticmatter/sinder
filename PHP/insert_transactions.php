<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 " . mysqli_error($con);
		exit();
	}

	$transactions_data_json = $_POST["transactions_root"];
	$transactions_data = json_decode($transactions_data_json,true);

	$insert_query = "INSERT INTO card_transactions (card_win_id, card_lose_id, expansion) VALUES ";
	$first = true;
	foreach($transactions_data['transactions'] as $record) {
		if ($first) {
			$first = false;
		} else {
			$insert_query = $insert_query . ",";
		}
		$insert_query = $insert_query . "('" . $record['card_win_id'] . "','" . $record['card_lose_id'] . "','" . $record['expansion']. "')";
	}
	$insert_query = $insert_query . ";";

	mysqli_query($con, $insert_query) or die("2 " . mysqli_error($con));

	echo "0";

?>