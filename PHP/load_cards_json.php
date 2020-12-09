<?php
	
	$con = mysqli_connect('localhost', 'root', 'root', 'keyforgedb');

	if (mysqli_connect_errno())
	{
		echo "1 - connection failed";
		exit();
	}

	$string = file_get_contents("AllCards.json");
	$json_a = json_decode($string, true);

	$row_count = 0;
	foreach ($json_a as $block => $block_data) {
		$house = "";
		$card_title = "";
		$front_image = "";
		$card_type = "";
		$rarity = "";
		$expansion = "";
		$id = "";
		$is_maverick = "";
		$is_enhanced = "";
		$is_anomaly = "";
   		foreach ($block_data as $key => $value) {
   			if ($key == 'house') {
   				$house = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'card_title') {
   				$card_title = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'front_image') {
   				$front_image = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'card_type') {
   				$card_type = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'rarity') {
   				$rarity = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'expansion') {
   				$expansion = mysqli_real_escape_string($con, $value);
   			}
   			if ($key == 'id') {
   				$id = $value;
   			}
   			if ($key == 'is_maverick') {
   				$is_maverick = $value;
   			}
   			if ($key == 'is_enhanced') {
   				$is_enhanced = $value;
   			}
   			if ($key == 'is_anomaly') {
   				$is_anomaly = $value;
   			}
		}
		if ($is_anomaly) {
   			if ($house == 'Logos') {
   				$expansion = 452;
   				$house = 'Anomaly';
   			} else {
   				continue;
   			}
   		}
		if (!$is_maverick && !$is_enhanced) {
			$row_count++;
			echo $card_title . " " . $house . "<br>";
			$inseruserquery = "INSERT INTO cards (house, card_title, front_image, card_type, rarity, expansion, id) VALUES ('" . $house . "', '" . $card_title . "', '" . $front_image . "', '" . $card_type . "', '" . $rarity . "', '" . $expansion . "', '" . $id. "');";
			if(!mysqli_query($con, $inseruserquery)) {
				echo("Error description: " . mysqli_error($con) . "<br>");
				echo $row_count;
			}
		}
	}
	echo $row_count++ . "<br>";



?>