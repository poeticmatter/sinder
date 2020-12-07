<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 - connection failed";
		exit();
	}

	$expansionNumber= $_POST["expansion"];
	$rankingcountquery = "SELECT COUNT(*) FROM `card_transactions`";
	if ($expansionNumber != "ALL") {
		$rankingcountquery = $rankingcountquery . " WHERE ";
		if ($expansionNumber == 452) {
			$rankingcountquery = $rankingcountquery . "(expansion=452 OR expansion=453);";
		} else {
			$rankingcountquery = $rankingcountquery . "expansion=" . $expansionNumber . ";";
		}
	}
	$rankingcount = mysqli_query($con, $rankingcountquery) or die("2 - ranking counter query failed" . $rankingcountquery);
	echo "count=" . array_values(mysqli_fetch_assoc($rankingcount))[0];
	
?>