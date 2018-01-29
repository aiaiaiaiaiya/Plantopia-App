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
	
	//Read DB ----temporary fn
	if($_REQUEST['action'] == 'read'){
		$_id = $_REQUEST['id'];
		$sql = "SELECT * FROM user WHERE userID = '$_id'";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
		//show data for each row
			while ($row = mysqli_fetch_assoc($result)) {
				echo "ID:".$row['userID']."Username:".$row['username']." | Name:".$row['fName']." | Lastname:".$row['lName']." | Email:".$row['email'].";";
			}
		}
	}

	//Read DB 'user' by FBuserID
	if($_REQUEST['action'] == 'readFBuserID'){
		$_id = $_REQUEST['FBid'];
		$sql = "SELECT * FROM user WHERE FBUserId = '$_id'";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
		//show data for each row
			while ($row = mysqli_fetch_assoc($result)) {
				echo $row['userID'].",".$row['username'];
			}
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

	//Read DB 'user_potInput' By plantID
	if($_REQUEST['action'] == 'readPotInput'){
		$plantId = $_REQUEST['plantId'];
		$sql = "SELECT * FROM user_potInput WHERE plantId = $plantId ORDER BY timestamp DESC LIMIT 1";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['plantID'].",".$row['timestamp'].",".$row['light'].",".$row['waterTemp'].",".$row['temperature'].",".$row['diameter'];
	}

	//Insert DB 'user_control'(Light) By plantID
	if($_REQUEST['action'] == 'insertControlLight'){
		$plantId = $_REQUEST['plantId'];
		$light = $_REQUEST['light'];
		$sql1 = "INSERT INTO user_control (plantID,light) VALUES('$plantId','$light')";
		// $sql2 = "INSERT INTO user_potInput (plantID,light) VALUES('$plantId','$light')";
		mysqli_query($conn,$sql1);
		// mysqli_query($conn,$sql2);
	}

	//Insert DB 'user_control'(Pump) By plantID
	if($_REQUEST['action'] == 'insertControlPump'){
		$plantId = $_REQUEST['plantId'];
		$pumpSpeed = $_REQUEST['pumpSpeed'];
		$sql1 = "INSERT INTO user_control (plantID,pumpSpeed) VALUES('$plantId','$pumpSpeed')";
		// $sql2 = "INSERT INTO user_potInput (plantID,pumpSpeed) VALUES('$plantId','$pumpSpeed')";
		mysqli_query($conn,$sql1);
		// mysqli_query($conn,$sql2);
	}

	//Read DB 'user_control' By plantID
	if($_REQUEST['action'] == 'readControl'){
		$plantId = $_REQUEST['plantId'];
		$sql = "SELECT * FROM user_control WHERE plantId = $plantId ORDER BY timestamp DESC LIMIT 1";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['plantID'].",".$row['timestamp'].",".$row['light'].",".$row['pumpSpeed'].",".$row['nutrientValve'];
	}

	//Read DB 'user_plant' By plantID
	if($_REQUEST['action'] == 'readPlant'){
		$plantId = $_REQUEST['plantId'];
		$sql = "SELECT * FROM user_plant WHERE plantId = $plantId";
		$result = mysqli_query($conn,$sql);
		$row = mysqli_fetch_assoc($result);
		echo $row['userID'].",".$row['plantID'].",".$row['gender'].",".$row['plantName'].",".$row['plantTypeNo'].",".$row['DOB'].",".$row['level'].",".$row['plantHealth'];
	}

	//Read DB 'user_plant' By userID
	if($_REQUEST['action'] == 'readPlantbyUserID'){
		$userId = $_REQUEST['userId'];
		$sql = "SELECT * FROM user_plant WHERE UserID = $userId";
		$result = mysqli_query($conn,$sql);
		if(mysqli_num_rows($result) > 0){
		//show data for each row
			while ($row = mysqli_fetch_assoc($result)) {
				echo $row['userID'].",".$row['plantID'].",".$row['gender'].",".$row['plantName'].",".$row['plantTypeNo'].",".$row['DOB'].",".$row['level'].",".$row['plantHealth'];
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

?>