<?php

	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		trigger_error(mysqli_error($con), E_USER_ERROR);
	}

	$expansionNumber= $_REQUEST["expansion"];
	$rankingcountquery = "SELECT COUNT(*) FROM `card_transactions`";
	if ($expansionNumber != "ALL") {
		$rankingcountquery = $rankingcountquery . " WHERE expansion=" . $expansionNumber . ";";
	}
	$rankingcount = mysqli_query($con, $rankingcountquery) or die("2 - ranking counter query failed" . $rankingcountquery);
	echo "count=" . array_values(mysqli_fetch_assoc($rankingcount))[0];
	
?>