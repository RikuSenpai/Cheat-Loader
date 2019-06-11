<?php
   define('DB_SERVER', 'localhost');
   define('DB_USERNAME', 'mysql');
   define('DB_PASSWORD', 'mysql');
   define('DB_DATABASE', 'test');
   define('DB_PORT', '3306');
   $db = mysqli_connect(DB_SERVER,DB_USERNAME,DB_PASSWORD,DB_DATABASE, DB_PORT);
?>