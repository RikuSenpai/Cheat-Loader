<?php
include("config.php");
// UD - 0, Unknown - 1, Detected - 2
if($_REQUEST["mode"] == null){
$cart = array(
    "md5" => md5_file("dlls.zip"),
	"version" => "2.0.3",
	"count" => "9",
	"rage" =>array(
     array(
	 "name" => "xy0",
      "vac" => 0,
      "untrusted" => 0	
	),
	array(
	 "name" => "stickrpg",
      "vac" => 0,
      "untrusted" => 0	
	),
	array(
	 "name" => "samoware",
      "vac" => 0,
      "untrusted" => 1	
	),
	array(
	 "name" => "Eternity.cc",
      "vac" => 1,
      "untrusted" => 1	
	),
	array(
	 "name" => "1tapgang",
      "vac" => 0,
      "untrusted" => 0	
	)),
	"legit" => array(
	array(
	 "name" => "pphud",
      "vac" => 0,
      "untrusted" => 0	
	),
	array(
	 "name" => "Interium",
      "vac" => 1,
      "untrusted" => 0	
	),
	array(
	 "name" => "M0ne0N",
      "vac" => 2,
      "untrusted" => 0	
	),
	array(
	 "name" => "Interception",
      "vac" => 0,
      "untrusted" => 1	
	)
	));
 
echo json_encode( $cart ); //отправить json

include("config.php");
//collect ip
function getRealIpAddr() {
  if (!empty($_SERVER['HTTP_CLIENT_IP']))
  { $ip=$_SERVER['HTTP_CLIENT_IP']; }
  elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR']))
  { $ip=$_SERVER['HTTP_X_FORWARDED_FOR']; }
  else { $ip=$_SERVER['REMOTE_ADDR']; }
  return $ip;
}

$ip = getRealIpAddr();
$ip = "X".substr($ip, 1, -1)."X";
//insert to db
$res = mysqli_query($db, "SELECT count(*) FROM test WHERE ip = '$ip'") or die();
$row = mysqli_fetch_row($res);
if ($row[0] > 0)
{
    // не 1 раз зашел
	mysqli_query($db, "UPDATE test SET count=count+1 WHERE ip='$ip'");
}
else
{
	//1 раз
    mysqli_query($db, "INSERT INTO test SET ip='$ip', count = '1'");
}



}
else if($_REQUEST["mode"] == "ip"){
	include("config.php");
	
	$result = mysqli_query($db, "SELECT * FROM test");
	$count = 0;
	echo "<table><tbody>";
	while($row = $result->fetch_assoc()){
		echo "<tr><td>".$row['ip']."</td><td>".$row['count']."</td></tr>";
		$count += $row['count'];
		
	}
	echo "</tbody></table><hr/>";
	echo "TOTAL ".$count;
	
}
else if($_REQUEST['mode'] == "cheat"){
	echo"
	<style>
   td {
    font-size: 150%;
   } 
  </style>";
	include("config.php");
	if($_REQUEST['data'] == null){
	$result = mysqli_query($db, "SELECT * FROM cheats");
	echo "<table><tbody>";
	while($row = $result->fetch_assoc()){
		echo "<tr><td>".$row['cheat']."</td><td>".$row['count']."</td></tr>";
		
	}
	echo "</tbody></table>";
	}
	else{
		
		$cheat = mysqli_real_escape_string($db, $_REQUEST["data"]);
		mysqli_query($db, "UPDATE cheats SET count=count+1 WHERE cheat='$cheat'");
	}
	
}
else{
	echo "mode not found";
}

?>