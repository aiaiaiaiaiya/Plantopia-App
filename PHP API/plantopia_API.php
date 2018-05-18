<?php
	$servername = "localhost";
	$username = "plantopia_reg";
	$password = "plantopiapass";
	$dbname = "plantopiadb";

	//Make Connection
	$conn = new mysqli($servername,$username,$password,$dbname);

	//Check Connection
	if(!$conn){
		die("Connection Failed.".mysql_connect_error());
	}
	
	// //Read DB ----temporary fn
	// if($_REQUEST['action'] == 'read'){
	// 	$_id = $_REQUEST['id'];
	// 	$sql = "SELECT * FROM user WHERE userID = '$_id'";
	// 	$result = mysqli_query($conn,$sql);
	// 	if(mysqli_num_rows($result) > 0){
	// 	//show data for each row
	// 		while ($row = mysqli_fetch_assoc($result)) {
	// 			echo "ID:".$row['userID']."Username:".$row['username']." | Name:".$row['fName']." | Lastname:".$row['lName']." | Email:".$row['email'].";";
	// 		}
	// 	}
	// }

	//Read DB 'user' by FBuserID
	if($_REQUEST['action'] == 'readFBuserID'){
		$_id = $_REQUEST['FBid'];
		$sql = "SELECT * FROM user WHERE FBUserId = '$_id'";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
			$row = mysqli_fetch_assoc($result);
			echo $row['userID'].",".$row['username'];
		} else {
			echo "0";
		}
	}

	//Insert DB 'user' by FBuserID
	if($_REQUEST['action'] == 'insertFBuserID'){
		$_id = $_REQUEST['FBid'];
		$username = $_REQUEST['username'];
		$sql1 = "INSERT INTO user (FBUserId,username) VALUES('$_id','$username')";
		mysqli_query($conn,$sql1);
	}

	//Read DB 'user_potInput' By plantID --> userID and remove diameter
	if($_REQUEST['action'] == 'readPotInput'){
		$userID = $_REQUEST['userID'];
		$sql = "SELECT * FROM user_potInput WHERE userID = $userID ORDER BY timestamp DESC LIMIT 1";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		// echo $row['userID'].",".$row['timestamp'].",".$row['light'].",".$row['waterTemp'].",".$row['temperature'].",".$row['diameter'];
		echo $row['userID'].",".$row['timestamp'].",".$row['light'].",".$row['waterTemp'].",".$row['temperature'];
	}

	//Insert DB 'user_control'(Light) By plantID --> userID
	if($_REQUEST['action'] == 'insertControlLight'){
		$userID = $_REQUEST['userID'];
		$light = $_REQUEST['light'];
		$sql1 = "INSERT INTO user_control (userID,light) VALUES('$userID','$light')";
		// $sql2 = "INSERT INTO user_potInput (userID,light) VALUES('$userID','$light')";
		mysqli_query($conn,$sql1);
		// mysqli_query($conn,$sql2);
	}

	//Insert DB 'user_control'(Pump) By plantID --> userID
	if($_REQUEST['action'] == 'insertControlPump'){
		$userID = $_REQUEST['userID'];
		$pumpSpeed = $_REQUEST['pumpSpeed'];
		$sql1 = "INSERT INTO user_control (userID,pumpSpeed) VALUES('$userID','$pumpSpeed')";
		// $sql2 = "INSERT INTO user_potInput (userID,pumpSpeed) VALUES('$userID','$pumpSpeed')";
		mysqli_query($conn,$sql1);
		// mysqli_query($conn,$sql2);
	}

	//Read DB 'user_control' By plantID --> userID
	if($_REQUEST['action'] == 'readControl'){
		$userID = $_REQUEST['userID'];
		$sql = "SELECT * FROM user_control WHERE userID = $userID ORDER BY timestamp DESC LIMIT 1";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['userID'].",".$row['timestamp'].",".$row['light'].",".$row['pumpSpeed'].",".$row['nutrientValve'];
	}

	//Read DB 'user_plant' By plantID
	if($_REQUEST['action'] == 'readPlant'){
		$plantId = $_REQUEST['plantId'];
		$sql = "SELECT * FROM user_plant WHERE plantId = $plantId";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['userID'].",".$row['plantID'].",".$row['plantName'].",".$row['plantTypeNo'].",".$row['DOB'].",".$row['level'].",".$row['plantHealth'];
	}

	//Read DB 'user_plant' By userID
	if($_REQUEST['action'] == 'readPlantbyUserID'){
		$userId = $_REQUEST['userId'];
		$sql = "SELECT * FROM user_plant WHERE userID = $userId";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
		//show data for each row
			while ($row = mysqli_fetch_assoc($result)) {
				echo $row['userID'].",".$row['plantID'].",".$row['plantName'].",".$row['plantTypeNo'].",".$row['DOB'].",".$row['level'].";";
			}
		} else {
			echo "0";
		}
	}

	//Read DB 'user' By userID
	if($_REQUEST['action'] == 'readUser'){
		$userId = $_REQUEST['userId'];
		$sql = "SELECT * FROM user WHERE userID = $userId";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['userID'].",".$row['FBUserId'].",".$row['username'].",".$row['DOR'].",".$row['gender'];
	}

	//Read DB 'event_detector'
	if($_REQUEST['action'] == 'readEvent'){
		$sql = "SELECT * FROM event_detector ORDER BY timestamp DESC LIMIT 1";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['ID'].",".$row['date'].",".$row['event'];
	}

	//Insert DB 'event_detector'
	if($_REQUEST['action'] == 'resetEvent'){
		$sql = "INSERT INTO event_detector (event) VALUES('0')";
		mysqli_query($conn,$sql);
	}

	//Read DB 'level_info' by plantTypeNo and level
	if($_REQUEST['action'] == 'readLevelInfo'){
		$plantTypeNo = $_REQUEST['plantTypeNo'];
		$level = $_REQUEST['level'];
		$sql = "SELECT * FROM level_info WHERE plantTypeNo = $plantTypeNo AND level = $level";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['idealLight'].",".$row['idealWaterTemp'].",".$row['idealTemperature'].",".$row['idealDiameter'].",".$row['idealNutrient'];
	}

	//Read DB 'user_potInput' by timestamp (each hours) for graph
	if($_REQUEST['action'] == 'readPotInputForGraphDayHourly'){
		$date = $_REQUEST['date'];
		$sql = "SELECT AVG(light) as avgLight, AVG(waterTemp) as avgWtTemp, AVG(temperature) as avgTemp,
			HOUR(timestamp) as hour
			FROM user_potInput
			WHERE DATE_SUB(`timestamp`,INTERVAL 1 HOUR) AND 
			timestamp LIKE '$date%'
			GROUP BY HOUR(timestamp)";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
			while ($row = mysqli_fetch_assoc($result)) {
				echo $row['hour'].",".$row['avgLight'].",".$row['avgWtTemp'].",".$row['avgTemp'].";";
			}
		}
	}

	// //Insert DB
	// if($_REQUEST['action'] == 'insert'){
    //     $_username = $_REQUEST['username'];
	// 	$_fName = $_REQUEST['fName'];
	// 	$_lName = $_REQUEST['lName'];
    //     $_email = $_REQUEST['email'];
    //     $_password = md5($_REQUEST['password']);
	// 	$sql = "INSERT INTO user (username,fName,lName,email,password) VALUES('$_username','$_fName','$_lName','$_email','$_password')";
	// 	mysqli_query($conn,$sql);
	// }
	// // //Update DB
	// // if($_REQUEST['action'] == 'updatename'){
	// // 	$_id = $_REQUEST['id'];
	// // 	$_name = $_REQUEST['name'];
	// // 	$sql = "UPDATE Items SET Name ='$_name' WHERE ID='$_id'";
	// // 	mysqli_query($conn,$sql);
	// // }

	// //Delete DB
	// if($_REQUEST['action'] == 'delete'){
	// 	$_id = $_REQUEST['id'];
	// 	$sql = "DELETE FROM user WHERE ID='$_id'";
	// 	mysqli_query($conn,$sql);
	// }

	//Insert DB 'user_plant' By userID
	if($_REQUEST['action'] == 'insertPlant'){
		$userID = $_REQUEST['userID'];
		$plantname = $_REQUEST['plantname'];
		$planttype = $_REQUEST['planttype'];
		$sql = "INSERT INTO user_plant (userID,plantName,plantTypeNo) VALUES('$userID','$plantname','$planttype')";
		mysqli_query($conn,$sql);
	}

?>