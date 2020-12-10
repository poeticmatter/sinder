<!DOCTYPE html>
<html>
<head>
	<title>Card Ranking</title>
</head>
<body>
<?php

	function get_transactions($con, $expansionNumber) {
		$gettransactionsquery = "SELECT * FROM card_transactions WHERE expansion=" . $expansionNumber . ";";
		$gettransaction = mysqli_query($con, $gettransactionsquery) or trigger_error(mysqli_error($con), E_USER_ERROR);
		return $gettransaction;
	}

	function get_cards($con, $expansionNumber) {
		$getcardsquery = "SELECT id, card_title FROM cards WHERE expansion=" . $expansionNumber . ";";
		$getcards = mysqli_query ($con, $getcardsquery) or trigger_error(mysqli_error($con), E_USER_ERROR);
		return $getcards;
	}

	function elo_calculation($scorea, $scoreb, $result, $K) {
		$expectedScore = 1 / (1 + Pow(10, ($scoreB - $scoreA) / 400));
        return $scoreA + $K * ($result - $expectedScore);
	}

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		trigger_error(mysqli_error($con), E_USER_ERROR);
	}

	$expansionNumber = $_REQUEST["expansion"];

	
	$cardsresults = get_cards($con, $expansionNumber);
	$cards = array();
	$elotable = array();

	while($row = mysqli_fetch_assoc($cardsresults)) {
		$cards[$row['id']] = $row['card_title'];
		$elotable[$row['id']] = 1500;
	}

	$transactionsresults = get_transactions($con, $expansionNumber);
	
	while($row = mysqli_fetch_assoc($transactionsresults)) {
		$card_win_id = $row['card_win_id'];
		$card_lose_id = $row['card_lose_id'];
    	$elotable[$card_win_id] += elo_calculation($elotable[$card_win_id], $elotable[$card_lose_id], 1, 10);
    	$elotable[$card_lose_id] += elo_calculation($elotable[$card_lose_id], $elotable[$card_win_id], 0, 10);
	}

	arsort($elotable);
	foreach ($elotable as $key => $value) {
		echo $cards[$key] . " " . $value . "<br>";
	}
	

?>

</body>
</html>