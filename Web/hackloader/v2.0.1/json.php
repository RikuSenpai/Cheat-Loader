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
      "vac" => 0,
      "untrusted" => 0	
	),
	array(
	 "name" => "Interception",
      "vac" => 0,
      "untrusted" => 1	
	)
	));
 
echo json_encode( $cart );
$file="base.log";
$col_zap=100000;

function getRealIpAddr() {
  if (!empty($_SERVER['HTTP_CLIENT_IP']))
  { $ip=$_SERVER['HTTP_CLIENT_IP']; }
  elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR']))
  { $ip=$_SERVER['HTTP_X_FORWARDED_FOR']; }
  else { $ip=$_SERVER['REMOTE_ADDR']; }
  return $ip;
}

$ip = getRealIpAddr();
$date = date("H:i:s d.m.Y");
$home = json_decode(file_get_contents("http://ip-api.com/json/".$ip), true);
$lines = file($file);
$ip = substr_replace($ip,'X',-2)."X";
while(count($lines) > $col_zap) array_shift($lines);
$lines[] = $date."|".$ip."|".$home["country"]."|\r\n";
file_put_contents($file, $lines);
}
else if($_REQUEST["mode"] == "ip"){
	include("config.php");
	$db->query("TRUNCATE TABLE test");
	foreach(file('base.log') as $str){
		$count = 1;
		$arr = explode("|", $str);
		$date = $arr[0];
		$ip = substr_replace($arr[1],'X',-2)."X";
		$country = $arr[2];
		mysqli_query($db, "INSERT INTO test SET ip='$ip', date='$date', country='$country'");

	}
	
	$result = $db->query("SELECT * FROM test");
	$cart = array();
	
	while($row = $result->fetch_assoc()){
		$ip = $row['ip'];
		$count = $cart[$ip];
		$cart[$ip] = $count+1;
		
	}
	echo "<table><tbody>";
	arsort($cart);
	foreach($cart as $ip => $count){
		echo "<tr><td>".$ip."</td><td>".$count."</td></tr>";
		
	}
	echo "</tbody></table>";
	$sum = 0;
	foreach($cart as $ip => $count){
		$sum += $count;
	}
	echo "USERS ".count($cart)." / COUNT ".$sum;
	
}
else if($_REQUEST['mode'] == "cheat"){
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